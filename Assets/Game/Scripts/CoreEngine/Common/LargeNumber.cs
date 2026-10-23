namespace Common
{
    public static class LargeNumber
    {
        public static string FormatNumber(float value)
        {
            if (value >= 1000000)
            {
                return (value / 1000000f).ToString("0.#") + "M";
            }
            else if (value >= 1000)
            {
                return (value / 1000f).ToString("0.#") + "K";
            }
            else
            {
                return value.ToString("0");
            }
        }
    }
}