namespace Kontur.GameStats.Server
{
    public static class MyMath
    {
        public static float UpdateAverage(float oldAverage, int oldCount, float newValue)
        {
            int newCount = oldCount + 1;
            return (oldAverage * oldCount + newValue) / newCount;
        }
    }
}
