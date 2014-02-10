﻿namespace ServiceControl.MessageAuditing.Handlers
{
    using System;
    using Contracts.Operations;
    using NServiceBus;
    using Raven.Client;
    using Raven.Json.Linq;

    class AuditMessageHandler : IHandleMessages<ImportSuccessfullyProcessedMessage>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(ImportSuccessfullyProcessedMessage message)
        {
            var auditMessage = new ProcessedMessage(message);
            Session.Advanced.GetMetadataFor(auditMessage)["Raven-Expiration-Date"] = new RavenJValue(DateTime.UtcNow.AddDays(5));
            Session.Store(auditMessage); // TODO bulks
        }

    }
}
