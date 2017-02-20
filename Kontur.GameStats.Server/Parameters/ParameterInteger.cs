namespace Kontur.GameStats.Server
{
    public class ParameterInteger : ParameterAttribute
    {
        public int MinValue = 0;
        public int MaxValue = int.MaxValue;
        public bool Required = true;
        public int DefaultValue;

        public override object ParseFromString(string s)
        {
            if (s == null)
            {
                if (!Required) return DefaultValue;

                throw new BadRequestException("Missing requred parameter");
            }

            int result = int.Parse(s);

            if (result > MaxValue) result = MaxValue;
            else if (result < MinValue) result = MinValue;

            return result;
        }
    }
}
