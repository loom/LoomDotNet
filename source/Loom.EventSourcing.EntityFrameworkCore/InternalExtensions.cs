﻿namespace Loom.EventSourcing.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Loom.Json;
    using Loom.Messaging;

    internal static class InternalExtensions
    {
        public static IQueryable<PendingEvent> GetPendingEventsQuery(
            this EventStoreContext context,
            string stateType,
            Guid streamId)
        {
            return from e in context.PendingEvents
                   where e.StateType == stateType && e.StreamId == streamId
                   orderby e.Version
                   select e;
        }

        public static Message GenerateMessage(
            this IEvent entity,
            TypeResolver typeResolver,
            IJsonProcessor jsonProcessor)
        {
            Type type = typeResolver.TryResolveType(entity.EventType);

            ConstructorInfo constructor = typeof(StreamEvent<>)
                .MakeGenericType(type)
                .GetTypeInfo()
                .GetConstructor(new[]
                {
                    typeof(Guid),
                    typeof(long),
                    typeof(DateTime),
                    type,
                });

            object data = constructor.Invoke(parameters: new object[]
            {
                entity.StreamId,
                entity.Version,
                new DateTime(entity.RaisedTimeUtc.Ticks, DateTimeKind.Utc),
                jsonProcessor.FromJson(entity.Payload, type),
            });

            var tracingProperties = new TracingProperties(
                entity.OperationId,
                entity.Contributor,
                entity.ParentId);

            return new Message(id: entity.MessageId, data, tracingProperties);
        }

        public static void Deconstruct<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            out TKey key,
            out TValue value)
        {
            (key, value) = (pair.Key, pair.Value);
        }
    }
}
