namespace Kontur.GameStats.Server
{
    public static class RoutingRules
    {
        public const string EndpointRegexp = @"(([a-zA-Z]+|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})-(\d+))";
        public const string TimestampRegexp = @"(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z)";
        public const string PlayerNameRegexp = @"([A-Za-z0-9\+%]+)";
        public const string CountRegexp = @"(\d+)";

        public static void BindRules(Router router)
        {
            router.Bind<AdvertiseServerInfo>("^/servers/" + EndpointRegexp + "/info/?$", "PUT");
            router.Bind<AdvertiseMatchResult>("^/servers/" + EndpointRegexp + "/matches/" + TimestampRegexp + "/?$", "PUT");

            router.Bind<GetServerInfo>("^/servers/" + EndpointRegexp + "/info/?$", "GET");
            router.Bind<GetServersInfo>("^/servers/info/?$", "GET");
            router.Bind<GetServersMatches>("^/servers/" + EndpointRegexp + "/matches/" + TimestampRegexp + "/?$", "GET");

            router.Bind<GetPlayerStats>("^/players/" + PlayerNameRegexp + "/stats/?$", "GET");

            router.Bind<ReportBestPlayers>("^/reports/best-players/?$", "GET");
            router.Bind<ReportBestPlayers>("^/reports/best-players/" + CountRegexp + "/?$", "GET");
            router.Bind<ReportRecentMatches>("^/reports/recent-matches/?$", "GET");
            router.Bind<ReportRecentMatches>("^/reports/recent-matches/" + CountRegexp + "/?$", "GET");
            router.Bind<ReportPopularServers>("^/reports/popular-servers/?$", "GET");
            router.Bind<ReportPopularServers>("^/reports/popular-servers/" + CountRegexp + "/?$", "GET");
        }
    }
}
