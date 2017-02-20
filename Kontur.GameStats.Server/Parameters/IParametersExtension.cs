using System.Linq;

namespace Kontur.GameStats.Server
{
    public static class IParametersExtension
    {
        public static void SetValues(this IParameters parameters, string[] adressParameters)
        {
            var properties = parameters.GetType()
                .GetProperties()
                .Where(z => z.GetCustomAttributes(typeof(ParameterAttribute), false).Length > 0)
                .ToArray();

            for (int i = 0; i < properties.Length; i++)
            {
                ParameterAttribute propertyAttribute = (ParameterAttribute)properties[i].GetCustomAttributes(typeof(ParameterAttribute), false)[0];

                string stringParameterValue = i < adressParameters.Length ? adressParameters[i] : null;
                object targetParameterValue = propertyAttribute.ParseFromString(stringParameterValue);

                properties[i].SetValue(parameters, targetParameterValue);
            }
        }
    }
}
