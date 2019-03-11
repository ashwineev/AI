using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneSkill.Dialogs.OutgoingCall.Resources;
using PhoneSkillTest.Flow.Utterances;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace PhoneSkillTest.Flow
{
    [TestClass]
    public class OutgoingCallDialogTests : PhoneSkillTestBase
    {
        [TestMethod]
        public async Task Test_OutgoingCall_Dialog()
        {
            await GetTestFlow()
               .Send(OutgoingCallDialogUtterances.Trigger)
               .AssertReply(ShowAuth())
               .Send(GetAuthResponse())
               .AssertReply(RecipientPrompt())
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
    }
}
