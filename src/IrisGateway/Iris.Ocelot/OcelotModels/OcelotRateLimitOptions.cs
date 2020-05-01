namespace Iris.Ocelot.OcelotModels
{
    public class OcelotRateLimitOptions
    {
        public string ClientIdHeader { get; set; } = "ClientId";


        public string QuotaExceededMessage { get; set; }

        public string RateLimitCounterPrefix { get; set; } = "ocelot";


        public bool DisableRateLimitHeaders { get; set; }

        public int HttpStatusCode { get; set; } = 429;
    }
}