namespace Kontur.GameStats.Server
{
    public static class RoutingRules
    {
        public const string endpointRegexp = @"(([a-zA-Z]+|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})-(\d+))";
        public const string timestampRegexp = @"(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z)";
        public const string playerNameRegexp = @"([A-Za-z0-9\+%]+)";
        public const string countRegexp = @"(\d+)";

        public static void BindRules(Router router)
        {
            router.Bind<AdvertiseServerInfo>("^/servers/" + endpointRegexp + "/info/?$", "PUT");
            router.Bind<AdvertiseMatchResult>("^/servers/" + endpointRegexp + "/matches/" + timestampRegexp + "/?$", "PUT");

            router.Bind<GetServerInfo>("^/servers/" + endpointRegexp + "/info/?$", "GET");
            router.Bind<GetServersInfo>("^/servers/info/?$", "GET");
            router.Bind<GetServersMatches>("^/servers/" + endpointRegexp + "/matches/" + timestampRegexp + "/?$", "GET");

            router.Bind<GetPlayerStats>("^/players/" + playerNameRegexp + "/stats/?$", "GET");

            router.Bind<ReportBestPlayers>("^/reports/best-players/?$", "GET");
            router.Bind<ReportBestPlayers>("^/reports/best-players/" + countRegexp + "/?$", "GET");
            router.Bind<ReportRecentMatches>("^/reports/recent-matches/?$", "GET");
            router.Bind<ReportRecentMatches>("^/reports/recent-matches/" + countRegexp + "/?$", "GET");
            router.Bind<ReportPopularServers>("^/reports/popular-servers/?$", "GET");
            router.Bind<ReportPopularServers>("^/reports/popular-servers/" + countRegexp + "/?$", "GET");
        }
    }
}
