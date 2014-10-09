﻿#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using NEventStore;
namespace System.Abstract.EventSourcing
{
    /// <summary>
    /// ESAggregateRootSnapshotStore
    /// </summary>
    public class ESAggregateRootSnapshotStore : IAggregateRootSnapshotStore
    {
        private readonly IStoreEvents _store;

        /// <summary>
        /// Initializes a new instance of the <see cref="ESAggregateRootSnapshotStore"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public ESAggregateRootSnapshotStore(IStoreEvents store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            _store = store;
        }

        /// <summary>
        /// Gets the latest snapshot.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="aggregateID">The aggregate ID.</param>
        /// <returns></returns>
        public AggregateRootSnapshot GetLatestSnapshot<TAggregateRoot>(object aggregateID)
            where TAggregateRoot : AggregateRoot
        {
            var streamID = (Guid)aggregateID;
            var latestSnapshot = _store.Advanced.GetSnapshot(streamID, int.MaxValue);
            return (latestSnapshot == null ? null : latestSnapshot.Payload as AggregateRootSnapshot);
        }

        /// <summary>
        /// Saves the snapshot.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="snapshot">The snapshot.</param>
        public void SaveSnapshot(Type aggregateType, AggregateRootSnapshot snapshot)
        {
            var streamID = (Guid)snapshot.AggregateID;
            _store.Advanced.AddSnapshot(new Snapshot(streamID, snapshot.LastEventSequence, snapshot));
        }

        /// <summary>
        /// Gets the inline snapshot predicate.
        /// </summary>
        public Func<IAggregateRootRepository, AggregateRoot, bool> InlineSnapshotPredicate { get; set; }
    }
}