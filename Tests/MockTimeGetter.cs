using System;
using Kontur.GameStats.Server;

namespace Tests
{
    public class MockTimeGetter : ICurrentTimeGetter
    {
        public MockTimeGetter(DateTime time)
        {
            Time = time;
        }

        public DateTime Time { get; set; }

        public DateTime GetCurrentTime()
        {
            return Time;
        }
    }
}