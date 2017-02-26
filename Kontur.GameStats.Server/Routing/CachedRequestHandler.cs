using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public abstract class CachedRequestHandler<TElements> : RequestHandler<CountParameters>
    {
        private TElements[] elementsCache;
        private DateTime lasTimeRecached;

        public override sealed object Process(CountParameters parameters, object data)
        {
            if (elementsCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                elementsCache = Recache();

                lasTimeRecached = DateTime.Now;
            }

            return elementsCache.Take(parameters.Count);
        }

        public abstract TElements[] Recache();
    }
}
