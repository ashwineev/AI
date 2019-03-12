using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Solutions.Responses;
using Microsoft.Bot.Solutions.Skills;
using PhoneSkill.Common;
using PhoneSkill.Dialogs.OutgoingCall.Resources;
using PhoneSkill.Dialogs.Shared;
using PhoneSkill.ServiceClients;

namespace PhoneSkill.Dialogs.OutgoingCall
{
    public class OutgoingCallDialog : SkillDialogBase
    {
        private ContactFilter contactFilter;

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

            contactFilter = new ContactFilter();
        }

        private async Task<DialogTurnResult> PromptForRecipient(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await ConversationStateAccessor.GetAsync(stepContext.Context);
            var contactProvider = GetContactProvider(state);
            contactFilter.Filter(state, contactProvider);

            if (state.ContactResult.Matches.Count != 0)
            {
                return await stepContext.NextAsync();
            }

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

        private IContactProvider GetContactProvider(SkillConversationState state)
        {
            if (state.SourceOfContacts == null)
            {
                // TODO Better error message to tell the bot developer where to specify the source.
                throw new Exception("Cannot retrieve contact list because no contact source specified.");
            }

            return ServiceManager.GetContactProvider(state.Token, state.SourceOfContacts.Value);
        }

        private class DialogIds
        {
            public const string RecipientPrompt = "RecipientPrompt";
            public const string ContactSelection = "ContactSelection";
            public const string PhoneNumberSelection = "PhoneNumberSelection";
        }
    }
}
