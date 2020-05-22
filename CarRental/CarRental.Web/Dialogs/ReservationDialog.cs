using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CarRental.Web.Dialogs
{
    public class ReservationDialog : CancelAndHelpDialog
    {
        private const string VehicleModelNameStepMsgText = "What kind of vehicle model would you like? Please choose a vehicle model type.";
        private const string AddressStepMsgText = "Where would you like to pickup and drop off the vehicle? Please choose a Car Rental Place.";

        public ReservationDialog()
            : base(nameof(ReservationDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DropOffDateResolverDialog());
            AddDialog(new PickUpDateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                VehicleModelStepAsync,
                PickUpDateStepAsync,
                DropOffDateStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> VehicleModelStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reservationDetails = (CarReservationDetails)stepContext.Options;

            if (reservationDetails.VehicleModelName == null)
            {
                var promptMessage = MessageFactory.Text(VehicleModelNameStepMsgText, VehicleModelNameStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(reservationDetails.VehicleModelName, cancellationToken);
        }

        private async Task<DialogTurnResult> PickUpDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reservationDetails = (CarReservationDetails)stepContext.Options;

            reservationDetails.VehicleModelName = (string)stepContext.Result;

            if (reservationDetails.PickUpTime == null || IsAmbiguous(reservationDetails.PickUpTime))
            {
                return await stepContext.BeginDialogAsync(nameof(PickUpDateResolverDialog), reservationDetails, cancellationToken);
            }

            var pickUp = TimexHelpers.DateFromTimex(new TimexProperty(reservationDetails.PickUpTime));

            if (pickUp.Date < DateTime.Now.AddDays(1).Date)
            {
                return await stepContext.BeginDialogAsync(nameof(PickUpDateResolverDialog), reservationDetails, cancellationToken);
            }

            return await stepContext.NextAsync(reservationDetails.PickUpTime, cancellationToken);
        }

        private async Task<DialogTurnResult> DropOffDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reservationDetails = (CarReservationDetails)stepContext.Options;

            reservationDetails.PickUpTime = (string)stepContext.Result;
            reservationDetails.PickUpTimeValue = TimexHelpers.DateFromTimex(new TimexProperty(reservationDetails.PickUpTime));

            if (reservationDetails.DropOffTime == null || IsAmbiguous(reservationDetails.DropOffTime))
            {
                return await stepContext.BeginDialogAsync(nameof(DropOffDateResolverDialog), reservationDetails, cancellationToken);
            }

            var dropOff = TimexHelpers.DateFromTimex(new TimexProperty(reservationDetails.PickUpTime));

            if (dropOff.Date < DateTime.Now.AddDays(1).Date)
            {
                return await stepContext.BeginDialogAsync(nameof(DropOffDateResolverDialog), reservationDetails, cancellationToken);
            }

            int days = (dropOff.Date - reservationDetails.PickUpTimeValue.Date).Days;

            if (days < 1)
            {
                return await stepContext.BeginDialogAsync(nameof(DropOffDateResolverDialog), reservationDetails, cancellationToken);
            }

            return await stepContext.NextAsync(reservationDetails.DropOffTime, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reservationDetails = (CarReservationDetails)stepContext.Options;

            reservationDetails.DropOffTime = (string)stepContext.Result;
            reservationDetails.DropOffTimeValue = TimexHelpers.DateFromTimex(new TimexProperty(reservationDetails.DropOffTime));

            var messageText = $"Please confirm, I have your reservation question details pick-up: {reservationDetails.PickUpTime} dropoff: {reservationDetails.DropOffTime} vehicle model: {reservationDetails.VehicleModelName}. Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var reservationDetails = (CarReservationDetails)stepContext.Options;

                return await stepContext.EndDialogAsync(reservationDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }
    }
}
