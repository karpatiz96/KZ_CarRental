using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CarRental.Web.Dialogs
{
    public class PickUpDateResolverDialog : CancelAndHelpDialog
    {
        private const string PickUpMsgText = "When would you like to pickup the vehicle please enter a pickup date.";
        private const string RepromptMsgText = "I'm sorry, to answer your question please enter a full date after today including Day Month and Year.";
        private const string ValidDateMsgText = "I'm sorry, to answer your question please enter a full date after today.";

        public PickUpDateResolverDialog(string id = null)
            : base(id ?? nameof(PickUpDateResolverDialog))
        {
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt), DateTimePromptValidator));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reservationDetails = (CarReservationDetails)stepContext.Options;

            var promptMessage = MessageFactory.Text(PickUpMsgText, PickUpMsgText, InputHints.ExpectingInput);
            var repromptMessage = MessageFactory.Text(RepromptMsgText, RepromptMsgText, InputHints.ExpectingInput);
            var validDateMessage = MessageFactory.Text(ValidDateMsgText, ValidDateMsgText, InputHints.ExpectingInput);

            if (reservationDetails.PickUpTime == null)
            {
                // We were not given any date at all so prompt the user.
                return await stepContext.PromptAsync(nameof(DateTimePrompt),
                    new PromptOptions
                    {
                        Prompt = promptMessage,
                        RetryPrompt = repromptMessage
                    }, cancellationToken);
            }

            // We have a Date we just need to check it is unambiguous.
            var timexProperty = new TimexProperty(reservationDetails.PickUpTime);
            if (!timexProperty.Types.Contains(Constants.TimexTypes.Definite))
            {
                // This is essentially a "reprompt" of the data we were given up front.
                return await stepContext.PromptAsync(nameof(DateTimePrompt),
                    new PromptOptions
                    {
                        Prompt = repromptMessage,
                        RetryPrompt = repromptMessage
                    }, cancellationToken);
            }

            var date = TimexHelpers.DateFromTimex(timexProperty);
            if(date.Date < DateTime.Now.AddDays(1).Date)
            {
                return await stepContext.PromptAsync(nameof(DateTimePrompt),
                    new PromptOptions
                    {
                        Prompt = validDateMessage,
                        RetryPrompt = repromptMessage
                    }, cancellationToken);
            }

            return await stepContext.NextAsync(new List<DateTimeResolution> { new DateTimeResolution { Timex = reservationDetails.PickUpTime } }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var timex = ((List<DateTimeResolution>)stepContext.Result)[0].Timex;

            return await stepContext.EndDialogAsync(timex, cancellationToken);
        }

        private static Task<bool> DateTimePromptValidator(PromptValidatorContext<IList<DateTimeResolution>> promptContext, CancellationToken cancellationToken)
        {
            if (promptContext.Recognized.Succeeded)
            {
                // This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
                // TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
                var timex = promptContext.Recognized.Value[0].Timex.Split('T')[0];

                // If this is a definite Date including year, month and day we are good otherwise reprompt.
                // A better solution might be to let the user know what part is actually missing.
                var isDefinite = new TimexProperty(timex).Types.Contains(Constants.TimexTypes.Definite);

                if (isDefinite)
                {
                    var date = TimexHelpers.DateFromTimex(new TimexProperty(timex));

                    if (date.Date < DateTime.Now.AddDays(1).Date)
                    {
                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(isDefinite);
            }

            return Task.FromResult(false);
        }
    }
}
