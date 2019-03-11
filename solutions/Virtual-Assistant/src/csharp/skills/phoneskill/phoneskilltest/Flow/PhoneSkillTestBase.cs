using Autofac;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Solutions.Authentication;
using Microsoft.Bot.Solutions.Proactive;
using Microsoft.Bot.Solutions.Responses;
using Microsoft.Bot.Solutions.Skills;
using Microsoft.Bot.Solutions.TaskExtensions;
using Microsoft.Bot.Solutions.Telemetry;
using Microsoft.Bot.Solutions.Testing;
using Microsoft.Bot.Solutions.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneSkill.Dialogs.Main.Resources;
using PhoneSkill.Dialogs.OutgoingCall.Resources;
using PhoneSkill.Dialogs.Shared.Resources;
using PhoneSkillTest.Flow.LuisTestUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PhoneSkillTest.Flow
{
    public class PhoneSkillTestBase : BotTestBase
    {
        public SkillConfigurationBase Services { get; set; }

        public EndpointService EndpointService { get; set; }

        public ConversationState ConversationState { get; set; }

        public UserState UserState { get; set; }

        public ProactiveState ProactiveState { get; set; }

        public IBotTelemetryClient TelemetryClient { get; set; }

        public IBackgroundTaskQueue BackgroundTaskQueue { get; set; }

        [TestInitialize]
        public override void Initialize()
        {
            var builder = new ContainerBuilder();

            ConversationState = new ConversationState(new MemoryStorage());
            UserState = new UserState(new MemoryStorage());
            TelemetryClient = new NullBotTelemetryClient();
            Services = new MockSkillConfiguration();

            Services.LocaleConfigurations.Add("en", new LocaleConfiguration()
            {
                Locale = "en-us",
                LuisServices = new Dictionary<string, ITelemetryLuisRecognizer>
                {
                    { "general", GeneralTestUtil.CreateRecognizer() },
                    { "phone", PhoneSkillTestUtil.CreateRecognizer() }
                }
            });

            Services.AuthenticationConnections.Add("Google", "Google");

            builder.RegisterInstance(new BotStateSet(UserState, ConversationState));
            Container = builder.Build();

            ResponseManager = new ResponseManager(
                Services.LocaleConfigurations.Keys.ToArray(),
                new MainResponses(),
                new SharedResponses(),
                new OutgoingCallResponses());
        }

        public TestFlow GetTestFlow()
        {
            var adapter = new TestAdapter()
                .Use(new AutoSaveStateMiddleware(ConversationState));

            var testFlow = new TestFlow(adapter, async (context, token) =>
            {
                var bot = BuildBot() as PhoneSkill.PhoneSkill;
                await bot.OnTurnAsync(context, CancellationToken.None);
            });

            return testFlow;
        }

        public override IBot BuildBot()
        {
            return new PhoneSkill.PhoneSkill(Services, EndpointService, ConversationState, UserState, ProactiveState, TelemetryClient, BackgroundTaskQueue, true, ResponseManager, null);
        }

        protected Action<IActivity> ShowAuth()
        {
            return activity =>
            {
                var eventActivity = activity.AsEventActivity();
                Assert.AreEqual(eventActivity.Name, "tokens/request");
            };
        }

        protected Activity GetAuthResponse()
        {
            var providerTokenResponse = new ProviderTokenResponse
            {
                TokenResponse = new TokenResponse(token: "test"),
                AuthenticationProvider = OAuthProvider.AzureAD
            };
            return new Activity(ActivityTypes.Event, name: "tokens/response", value: providerTokenResponse);
        }
    }
}