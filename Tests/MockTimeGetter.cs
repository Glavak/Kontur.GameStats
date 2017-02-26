using System;
using Kontur.GameStats.Server;

namespace Tests
{
    public class MockTimeGetter : ICurrentTimeGetter
    {
        public DateTime Time { get; set; }

        public MockTimeGetter(DateTime time)
        {
            Time = time;
        }

        public DateTime GetCurrentTime()
        {
            return Time;
        }
    }
}
