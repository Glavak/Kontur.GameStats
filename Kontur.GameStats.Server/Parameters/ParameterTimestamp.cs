using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class ParameterTimestamp : ParameterAttribute
    {
        public bool Required = true;
        public DateTime DefaultValue;

        public override object ParseFromString(string s)
        {
            if (s == null)
            {
                if (!Required) return DefaultValue;

                throw new BadRequestException("Missing requred parameter");
            }


            DateTime result = DateTime.ParseExact(s.Substring(0, s.Length - 1), @"s", CultureInfo.InvariantCulture);

            return result;
        }
    }
}
