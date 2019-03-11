using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneSkillTest.Flow.Utterances;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using PhoneSkill.Dialogs.Main.Resources;
using PhoneSkill.Dialogs.OutgoingCall.Resources;

namespace PhoneSkillTest.Flow
{
    [TestClass]
    public class InterruptionTests : PhoneSkillTestBase
    {
        [TestMethod]
        public async Task Test_Help_Interruption()
        {
            await GetTestFlow()
               .Send(OutgoingCallDialogUtterances.Trigger)
               .AssertReply(ShowAuth())
               .Send(GetAuthResponse())
               .AssertReply(RecipientPrompt())
               .Send(GeneralUtterances.Help)
               .AssertReply(HelpResponse())
               .AssertReply(RecipientPrompt())
               .StartTestAsync();
        }

        [TestMethod]
        public async Task Test_Cancel_Interruption()
        {
            await GetTestFlow()
               .Send(OutgoingCallDialogUtterances.Trigger)
               .AssertReply(ShowAuth())
               .Send(GetAuthResponse())
               .AssertReply(RecipientPrompt())
               .Send(GeneralUtterances.Cancel)
               .AssertReply(CancelResponse())
               .StartTestAsync();
        }

        private Action<IActivity> RecipientPrompt()
        {
            return activity =>
            {
                Assert.AreEqual("message", activity.Type);
                var messageActivity = activity.AsMessageActivity();
                CollectionAssert.Contains(ParseReplies(OutgoingCallResponses.RecipientPrompt, new StringDictionary()), messageActivity.Text);
            };
        }

        private Action<IActivity> HelpResponse()
        {
            return activity =>
            {
                Assert.AreEqual("message", activity.Type);
                var messageActivity = activity.AsMessageActivity();
                CollectionAssert.Contains(ParseReplies(MainResponses.HelpMessage, new StringDictionary()), messageActivity.Text);
            };
        }

        private Action<IActivity> CancelResponse()
        {
            return activity =>
            {
                Assert.AreEqual("message", activity.Type);
                var messageActivity = activity.AsMessageActivity();
                CollectionAssert.Contains(ParseReplies(MainResponses.CancelMessage, new StringDictionary()), messageActivity.Text);
            };
        }
    }
}
