using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneSkill.Dialogs.Main.Resources;
using PhoneSkill.Dialogs.Shared.Resources;
using PhoneSkillTest.Flow.Utterances;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace PhoneSkillTest.Flow
{
    [TestClass]
    public class MainDialogTests : PhoneSkillTestBase
    {
        [TestMethod]
        public async Task Test_Help_Intent()
        {
            await GetTestFlow()
                .Send(GeneralUtterances.Help)
                .AssertReply(HelpMessage())
                .StartTestAsync();
        }

        [TestMethod]
        public async Task Test_Unhandled_Message()
        {
            await GetTestFlow()
                .Send(string.Empty)
                .AssertReply(DidntUnderstandMessage())
                .StartTestAsync();
        }

        private Action<IActivity> HelpMessage()
        {
            return activity =>
            {
                var messageActivity = activity.AsMessageActivity();
                CollectionAssert.Contains(ParseReplies(MainResponses.HelpMessage, new StringDictionary()), messageActivity.Text);
            };
        }

        private Action<IActivity> DidntUnderstandMessage()
        {
            return activity =>
            {
                var messageActivity = activity.AsMessageActivity();
                CollectionAssert.Contains(ParseReplies(SharedResponses.DidntUnderstandMessage, new StringDictionary()), messageActivity.Text);
            };
        }
    }
}
