
using CarRental.Bll.IServices;
using CarRental.Web.CognitiveModels;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRental.Web.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly CarRentalRecognizer _luisRecognizer;
        protected readonly ILogger Logger;
        private readonly IBotService _botService;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(CarRentalRecognizer luisRecognizer, ReservationDialog reservationDialog, ILogger<MainDialog> logger, IBotService botService)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;
            _botService = botService;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(reservationDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with today?\nSay something like \"Is threre any free citroen c3 models between 2020.05.15 and 2020.05.18 ?\"";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                // LUIS is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
                return await stepContext.BeginDialogAsync(nameof(ReservationDialog), new ReservationDialog(), cancellationToken);
            }

            // Call LUIS and gather any potential booking details. (Note the TurnContext has the response to the prompt.)
            var luisResult = await _luisRecognizer.RecognizeAsync<CarReservation>(stepContext.Context, cancellationToken);

            switch (luisResult.TopIntent().intent)
            {
                case CarReservation.Intent.CarReservation:

                    var carReservationDetails = new CarReservationDetails
                    {
                        VehicleModelName = luisResult.Model,
                        PickUpTime = luisResult.PickUp,
                        DropOffTime = luisResult.DropOff
                    };

                    if(luisResult.PickUp != null)
                    {
                        var timexPickUp = new TimexProperty(luisResult.PickUp);
                        var isDefinite = timexPickUp.Types.Contains(Constants.TimexTypes.Definite);
                        if (isDefinite)
                        {
                            var start = TimexHelpers.DateFromTimex(timexPickUp);
                            carReservationDetails.PickUpTimeValue = start;
                        }
                    }

                    if (luisResult.DropOff != null)
                    {
                        var timexDropOff = new TimexProperty(luisResult.DropOff);
                        var isDefinite = timexDropOff.Types.Contains(Constants.TimexTypes.Definite);
                        if (isDefinite)
                        {
                            var end = TimexHelpers.DateFromTimex(timexDropOff);
                            carReservationDetails.DropOffTimeValue = end;
                        }
                    }

                    // Run the BookingDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
                    return await stepContext.BeginDialogAsync(nameof(ReservationDialog), carReservationDetails, cancellationToken);

                case CarReservation.Intent.GetAddresses:
                    var addresses = await _botService.GetAddresses();

                    var addressesMessageText = $"The vehicles can be picked up and dropped off at the following rental places:";
                    foreach (var address in addresses)
                    {
                        addressesMessageText += $" {address.FullAddress},";
                    }
                    addressesMessageText += ".";
                    var addressesMessage = MessageFactory.Text(addressesMessageText, addressesMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(addressesMessage, cancellationToken);
                    break;
                case CarReservation.Intent.GetFreeVehicle:
                    var vehicles = await _botService.GetFreeVehicles();

                    var freeVehicleMessageText = $"The currently available models are: ";
                    foreach(var vehicle in vehicles)
                    {
                        freeVehicleMessageText += $" {vehicle.VehicleType},";
                    }
                    var freeVehicleMessage = MessageFactory.Text(freeVehicleMessageText, freeVehicleMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(freeVehicleMessage, cancellationToken);
                    break;
                case CarReservation.Intent.None:
                    var noneMessageText = $"Sorry, I didn't get that. Please try asking in a different way.";
                    var noneMessage = MessageFactory.Text(noneMessageText, noneMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(noneMessage, cancellationToken);
                    break;
                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way.";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is CarReservationDetails result)
            {
                var start = result.PickUpTimeValue;
                var end = result.DropOffTimeValue;
                var model = result.VehicleModelName;
                var cars = await _botService.GetCars(start, end, model);

                var count = cars.Count();
                if(count > 0)
                {
                    var messageText = $"There are {count} available {model} models from: {result.PickUpTime} to {result.DropOffTime} .";
                    var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(message, cancellationToken);
                }
                else
                {
                    var messageText = $"There are no available {model} models from: {result.PickUpTime} to {result.DropOffTime} .";
                    var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(message, cancellationToken);
                }
            }

            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
