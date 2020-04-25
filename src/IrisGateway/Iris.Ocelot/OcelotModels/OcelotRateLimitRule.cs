using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Ocelot.OcelotModels
{
    public class OcelotRateLimitRule
    {
        public List<string> ClientWhitelist
        {
            get;
            set;
        } = new List<string>();

        public bool EnableRateLimiting
        {
            get;
            set;
        }

        public string Period
        {
            get;
            set;
        }

        public double PeriodTimespan
        {
            get;
            set;
        }

        public long Limit
        {
            get;
            set;
        }

        public override string ToString()
        {
            if (!EnableRateLimiting)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("{0}:{1},{2}:{3:F},{4}:{5},{6}:[", "Period", Period, "PeriodTimespan", PeriodTimespan, "Limit", Limit, "ClientWhitelist"));
            stringBuilder.AppendJoin(',', ClientWhitelist);
            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }
    }
}