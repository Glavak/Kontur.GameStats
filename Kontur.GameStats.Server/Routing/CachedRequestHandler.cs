using System;
using System.Collections.Generic;
using System.Linq;

namespace Kontur.GameStats.Server
{
    /// <summary>
    /// Abstract class, which extends functionality of RequestHandler by adding
    /// caching layer. Child requestsHandler should override method Recache() for
    /// it to return collection of element to return to client. If Recache() was
    /// called during last minute, previously returned collection will be used
    /// </summary>
    /// <typeparam name="TElements">Type of elements in collection</typeparam>
    public abstract class CachedRequestHandler<TElements> : RequestHandler<CountParameters>
    {
        private IEnumerable<TElements> elementsCache;
        private DateTime lasTimeRecached;

        private readonly ICurrentTimeGetter timeGetter;

        protected CachedRequestHandler(ICurrentTimeGetter timeGetter)
        {
            this.timeGetter = timeGetter;
        }

        public sealed override object Process(CountParameters parameters, object data)
        {
            if (elementsCache == null || (timeGetter.GetCurrentTime() - lasTimeRecached).TotalMinutes > 1)
            {
                elementsCache = Recache().ToArray(); // Call ToArray to evaluate linq expression

                lasTimeRecached = timeGetter.GetCurrentTime();
            }

            return elementsCache.Take(parameters.Count);
        }

        public abstract IEnumerable<TElements> Recache();
    }
}
