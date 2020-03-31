using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Iris.FrameCore.Resolvers
{
    public class NullToEmptyStringValueProvider : IValueProvider
    {
        private readonly PropertyInfo _MemberInfo;
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            _MemberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object result = _MemberInfo.GetValue(target);
            if (result == null)
            {
                if (!_MemberInfo.PropertyType.Name.Contains("Nullable`1"))
                {
                    switch (_MemberInfo.PropertyType.Name)
                    {
                        case "String":
                            return result = "";
                        case "List`1":
                            return result = Array.Empty<int>();
                    }
                    if (_MemberInfo.PropertyType.IsArray)
                    {
                        return result = Array.Empty<int>();
                    }

                    if (_MemberInfo.PropertyType.IsClass)
                    {
                        return new object();
                    }
                }
                else
                {
                    var fullName = _MemberInfo.PropertyType.FullName;
                    if (fullName.Contains("System.Int16"))
                    {
                        Int16 i = 0;
                        return result = i;
                    }
                    else if (fullName.Contains("System.Int32"))
                    {
                        Int32 i = 0;
                        return result = i;
                    }
                    else if (fullName.Contains("System.Int64"))
                    {
                        Int64 i = 0;
                        return result = i;
                    }
                    else if (fullName.Contains("System.DateTime"))
                    {
                        //return result = "";
                    }
                    else if (fullName.Contains("System.Decimal"))
                    {
                        return result = 0.00m;
                    }
                    else if (fullName.Contains("System.Single"))
                    {
                        return result = 0.00f;
                    }
                    else if (fullName.Contains("System.Double"))
                    {
                        return result = 0.00d;
                    }
                }
            }
            return result;

        }

        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value);
        }
    }
}
