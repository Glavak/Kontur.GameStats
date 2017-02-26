namespace Kontur.GameStats.Server
{
    public static class MyMath
    {
        /// <summary>
        /// Recalculates average of multiple float values, adding newValue to set of
        /// oldCount values with oldAverage average
        /// </summary>
        /// <param name="oldAverage">Previous average value</param>
        /// <param name="oldCount">Count of values before adding new</param>
        /// <param name="newValue">New value to add</param>
        /// <returns>New average value</returns>
        public static float UpdateAverage(float oldAverage, int oldCount, float newValue)
        {
            int newCount = oldCount + 1;
            return (oldAverage * oldCount + newValue) / newCount;
        }
    }
}
