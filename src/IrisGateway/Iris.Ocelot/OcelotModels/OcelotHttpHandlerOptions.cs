namespace Iris.Ocelot.OcelotModels
{
    public class OcelotHttpHandlerOptions
    {
        public bool AllowAutoRedirect
        {
            get;
            set;
        } = false;

        public bool UseCookieContainer
        {
            get;
            set;
        } = false;

        public bool UseTracing
        {
            get;
            set;
        }

        public bool UseProxy
        {
            get;
            set;
        } = true;
    }
}