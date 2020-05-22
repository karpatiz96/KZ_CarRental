// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.6.2

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
    public class DateResolverDialog : CancelAndHelpDialog
    {
        private const string PickUpMsgText = "When would you like to pickup the vehicle please enter a pickup date.";
        private const string DropOffMsgText = "When would you like to drop off the vehicle please enter a drop off date.";
        private const string PromptMsgText = "When would you like to travel?";
        private const string RepromptMsgText = "I'm sorry, to answer your question please enter a full date after today including Day Month and Year.";

        public DateResolverDialog(string id = null)
            : base(id ?? nameof(DateResolverDialog))
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
            String timex = null;
            if(reservationDetails.PickUpTime == null)
            {

            }

            if(reservationDetails.DropOffTime == null)
            {

            }

            var promptMessage = MessageFactory.Text(PromptMsgText, PromptMsgText, InputHints.ExpectingInput);
            var repromptMessage = MessageFactory.Text(RepromptMsgText, RepromptMsgText, InputHints.ExpectingInput);
//            var validDateMessage = MessageFactory.Text(ValidDateMsgText, ValidDateMsgText, InputHints.ExpectingInput);

            if (timex == null)
            {
                // We were not given any date at all so prompt the user.
                return await stepContext.PromptAsync(nameof(DateTimePrompt),
                    new PromptOptions
                    {
                        Prompt = promptMessage,
                        RetryPrompt = repromptMessage,
                        Validations = reservationDetails
                    }, cancellationToken);
            }

            // We have a Date we just need to check it is unambiguous.
            var timexProperty = new TimexProperty(timex);
            if (!timexProperty.Types.Contains(Constants.TimexTypes.Definite))
            {
                // This is essentially a "reprompt" of the data we were given up front.
                return await stepContext.PromptAsync(nameof(DateTimePrompt),
                    new PromptOptions
                    {
                        Prompt = repromptMessage,
                    }, cancellationToken);
            }

            return await stepContext.NextAsync(new List<DateTimeResolution> { new DateTimeResolution { Timex = timex } }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var timex = ((List<DateTimeResolution>)stepContext.Result)[0].Timex;
            var time = new TimexProperty(timex);

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

                var carReservationDetails = (CarReservationDetails)promptContext.Options.Validations;

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
