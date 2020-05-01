namespace Iris.Ocelot.OcelotModels
{
    public class OcelotQoSOptions
    {
        public int ExceptionsAllowedBeforeBreaking { get; set; }

        public int DurationOfBreak { get; set; }

        public int TimeoutValue { get; set; }
    }
}