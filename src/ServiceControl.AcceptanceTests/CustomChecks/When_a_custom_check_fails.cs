﻿namespace ServiceBus.Management.AcceptanceTests.CustomChecks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using EndpointTemplates;
    using Infrastructure.Settings;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.CustomChecks;
    using NUnit.Framework;
    using ServiceControl.Contracts.CustomChecks;
    using ServiceControl.EventLog;

    [TestFixture]
    class When_a_custom_check_fails : AcceptanceTest
    {
        [Test]
        public async Task Should_result_in_a_custom_check_failed_event()
        {
            EventLogItem entry = null;

            await Define<MyContext>()
                .WithEndpoint<EndpointWithFailingCustomCheck>()
                .Done(async c =>
                {
                    var result = await this.TryGetSingle<EventLogItem>("/api/eventlogitems/", e => e.EventType == typeof(CustomCheckFailed).Name);
                    entry = result;
                    return result;
                })
                .Run();

            Assert.AreEqual(Severity.Error, entry.Severity, "Failed custom checks should be treated as error");
            Assert.IsTrue(entry.RelatedTo.Any(item => item == "/customcheck/MyCustomCheckId"));
            Assert.IsTrue(entry.RelatedTo.Any(item => item.StartsWith("/endpoint/CustomChecks.EndpointWithFailingCustomCheck")));
        }

        public class MyContext : ScenarioContext
        {
        }

        public class EndpointWithFailingCustomCheck : EndpointConfigurationBuilder
        {
            public EndpointWithFailingCustomCheck()
            {
                EndpointSetup<DefaultServerWithoutAudit>(c => { c.ReportCustomChecksTo(Settings.DEFAULT_SERVICE_NAME, TimeSpan.FromSeconds(1)); });
            }

            class FailingCustomCheck : CustomCheck
            {
                public FailingCustomCheck()
                    : base("MyCustomCheckId", "MyCategory")
                {
                }

                public override Task<CheckResult> PerformCheck()
                {
                    return Task.FromResult(CheckResult.Failed("Some reason"));
                }
            }
        }
    }
}