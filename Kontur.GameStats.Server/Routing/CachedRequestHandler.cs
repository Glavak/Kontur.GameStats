using System;
using System.Linq;

namespace Kontur.GameStats.Server
{
    public abstract class CachedRequestHandler<TElements> : RequestHandler<CountParameters>
    {
        private TElements[] elementsCache;
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
                elementsCache = Recache();

                lasTimeRecached = timeGetter.GetCurrentTime();
            }

            return elementsCache.Take(parameters.Count);
        }

        public abstract TElements[] Recache();
    }
}
