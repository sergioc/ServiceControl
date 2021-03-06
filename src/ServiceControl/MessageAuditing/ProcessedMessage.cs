﻿namespace ServiceControl.MessageAuditing
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;

    public class ProcessedMessage
    {
        public ProcessedMessage()
        {
            MessageMetadata = new Dictionary<string, object>();
            Headers = new Dictionary<string, string>();
        }

        public ProcessedMessage(Dictionary<string, string> headers, Dictionary<string, object> metadata)
        {
            UniqueMessageId = headers.UniqueId();
            MessageMetadata = metadata;
            Headers = headers;

            if (Headers.TryGetValue(NServiceBus.Headers.ProcessingEnded, out var processedAt))
            {
                ProcessedAt = DateTimeExtensions.ToUtcDateTime(processedAt);
            }
            else
            {
                ProcessedAt = DateTime.UtcNow; // best guess
            }
        }

        public string Id { get; set; }

        public string UniqueMessageId { get; set; }

        public Dictionary<string, object> MessageMetadata { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public DateTime ProcessedAt { get; set; }
    }
}