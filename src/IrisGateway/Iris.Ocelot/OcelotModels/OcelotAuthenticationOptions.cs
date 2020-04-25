using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Ocelot.OcelotModels
{
    public class OcelotAuthenticationOptions
    {
        public OcelotAuthenticationOptions()
        {
            AllowedScopes = new List<string>();
        }
        public string AuthenticationProviderKey
        {
            get;
            set;
        }

        public List<string> AllowedScopes
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder val = new StringBuilder();
            val.Append("AuthenticationProviderKey:" + AuthenticationProviderKey + ",AllowedScopes:[");
            val.AppendJoin(',', (IEnumerable<string>)AllowedScopes);
            val.Append("]");
            return ((object)val).ToString();
        }
    }
}