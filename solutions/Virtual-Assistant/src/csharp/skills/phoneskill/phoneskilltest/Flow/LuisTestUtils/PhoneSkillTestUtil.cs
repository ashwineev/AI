using Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Solutions.Testing.Mocks;
using PhoneSkillTest.Flow.Utterances;
using System.Collections.Generic;

namespace PhoneSkillTest.Flow.LuisTestUtils
{
    public class PhoneSkillTestUtil
    {
        private static Dictionary<string, IRecognizerConvert> _utterances = new Dictionary<string, IRecognizerConvert>
        {
            { OutgoingCallDialogUtterances.Trigger, CreateIntent(OutgoingCallDialogUtterances.Trigger, PhoneLU.Intent.OutgoingCall) },
        };

        public static MockLuisRecognizer CreateRecognizer()
        {
            var recognizer = new MockLuisRecognizer(defaultIntent: CreateIntent(string.Empty, PhoneLU.Intent.None));
            recognizer.RegisterUtterances(_utterances);
            return recognizer;
        }

        public static PhoneLU CreateIntent(string userInput, PhoneLU.Intent intent)
        {
            var result = new PhoneLU
            {
                Text = userInput,
                Intents = new Dictionary<PhoneLU.Intent, IntentScore>()
            };

            result.Intents.Add(intent, new IntentScore() { Score = 0.9 });

            result.Entities = new PhoneLU._Entities
            {
                _instance = new PhoneLU._Entities._Instance()
            };

            return result;
        }
    }
}
