﻿namespace Loom.EventSourcing.EntityFrameworkCore
{
    using System;

    public class PendingEvent : IEvent
    {
        private PendingEvent()
        {
        }

        public string StateType { get; private set; }

        public Guid StreamId { get; private set; }

        public long Version { get; private set; }

        public DateTime RaisedTimeUtc { get; private set; }

        public string EventType { get; private set; }

        public string Payload { get; private set; }

        public string MessageId { get; private set; }

        public string OperationId { get; private set; }

        public string Contributor { get; private set; }

        public string ParentId { get; private set; }

        public Guid Transaction { get; private set; }

        internal static PendingEvent Create(StreamEvent source) => new PendingEvent
        {
            StateType = source.StateType,
            StreamId = source.StreamId,
            Version = source.Version,
            RaisedTimeUtc = source.RaisedTimeUtc,
            EventType = source.EventType,
            Payload = source.Payload,
            MessageId = source.MessageId,
            OperationId = source.OperationId,
            Contributor = source.Contributor,
            ParentId = source.ParentId,
            Transaction = source.Transaction,
        };
    }
}
