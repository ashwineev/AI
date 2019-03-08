using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Solutions.Responses;
using Microsoft.Bot.Solutions.Skills;
using PhoneSkill.Dialogs.OutgoingCall.Resources;
using PhoneSkill.Dialogs.Shared;
using PhoneSkill.ServiceClients;

namespace PhoneSkill.Dialogs.OutgoingCall
{
    public class OutgoingCallDialog : SkillDialogBase
    {
        public OutgoingCallDialog(
            SkillConfigurationBase services,
            ResponseManager responseManager,
            IStatePropertyAccessor<SkillConversationState> conversationStateAccessor,
            IStatePropertyAccessor<SkillUserState> userStateAccessor,
            IServiceManager serviceManager,
            IBotTelemetryClient telemetryClient)
            : base(nameof(OutgoingCallDialog), services, responseManager, conversationStateAccessor, userStateAccessor, serviceManager, telemetryClient)
        {
            var outgoingCall = new WaterfallStep[]
            {
                GetAuthToken,
                AfterGetAuthToken,
                PromptForRecipient,
                AskToSelectContact,
                AskToSelectPhoneNumber,
                ExecuteCall,
            };

            AddDialog(new WaterfallDialog(nameof(OutgoingCallDialog), outgoingCall));
            AddDialog(new TextPrompt(DialogIds.RecipientPrompt));
            AddDialog(new ChoicePrompt(DialogIds.ContactSelection, ValidateContactChoice));
            AddDialog(new ChoicePrompt(DialogIds.PhoneNumberSelection, ValidatePhoneNumberChoice));

            InitialDialogId = nameof(OutgoingCallDialog);
        }

        private async Task<DialogTurnResult> PromptForRecipient(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await ConversationStateAccessor.GetAsync(stepContext.Context);
            var intent = state.LuisResult.TopIntent().intent;
            var entities = state.LuisResult.Entities;

            var prompt = ResponseManager.GetResponse(OutgoingCallResponses.RecipientPrompt);
            return await stepContext.PromptAsync(DialogIds.RecipientPrompt, new PromptOptions { Prompt = prompt });
        }

        private async Task<DialogTurnResult> AskToSelectContact(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync();
        }

        private async Task<bool> ValidateContactChoice(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            return false;
        }

        private async Task<DialogTurnResult> AskToSelectPhoneNumber(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync();
        }

        private async Task<bool> ValidatePhoneNumberChoice(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            return false;
        }

        private async Task<DialogTurnResult> ExecuteCall(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await ConversationStateAccessor.GetAsync(stepContext.Context);

            var tokens = new StringDictionary
            {
                { "contactOrPhoneNumber", stepContext.Result.ToString() },
            };

            var response = ResponseManager.GetResponse(OutgoingCallResponses.ExecuteCall, tokens);
            await stepContext.Context.SendActivityAsync(response);

            state.Clear();

            return await stepContext.EndDialogAsync();
        }

        private class DialogIds
        {
            public const string RecipientPrompt = "RecipientPrompt";
            public const string ContactSelection = "ContactSelection";
            public const string PhoneNumberSelection = "PhoneNumberSelection";
        }
    }
}
