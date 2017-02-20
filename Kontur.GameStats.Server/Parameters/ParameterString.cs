namespace Kontur.GameStats.Server
{
    public class ParameterString : ParameterAttribute
    {
        public bool Required = true;
        public string DefaultValue;

        public override object ParseFromString(string s)
        {
            if (s == null)
            {
                if (!Required) return DefaultValue;

                throw new BadRequestException("Missing requred parameter");
            }

            return s;
        }
    }
}
