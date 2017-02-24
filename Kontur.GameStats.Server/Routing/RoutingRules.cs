namespace Kontur.GameStats.Server
{
    public static class RoutingRules
    {
        public static void BindRules(Router router)
        {
            const string endpointRegexp = @"(([a-zA-Z]+|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})-(\d+))";
            const string timestampRegexp = @"(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z)";
            const string playerNameRegexp = @"([A-Za-z0-9\+%]+)";
            const string countRegexp = @"(\d+)";

            router.Bind("^/servers/" + endpointRegexp + "/info/?$", "PUT", new AdvertiseServerInfo());
            router.Bind("^/servers/" + endpointRegexp + "/matches/" + timestampRegexp + "/?$", "PUT", new AdvertiseMatchResult());

            router.Bind("^/servers/" + endpointRegexp + "/info/?$", "GET", new GetServerInfo());
            router.Bind("^/servers/info/?$", "GET", new GetServersInfo());
            router.Bind("^/servers/" + endpointRegexp + "/matches/" + timestampRegexp + "/?$", "GET", new GetServersMatches());

            router.Bind("^/players/"+playerNameRegexp+"/stats/?$", "GET", new GetPlayerStats());

            router.Bind("^/reports/best-players/?$", "GET", new ReportBestPlayers());
            router.Bind("^/reports/best-players/" + countRegexp + "/?$", "GET", new ReportBestPlayers());
            router.Bind("^/reports/recent-matches/?$", "GET", new ReportRecentMatches());
            router.Bind("^/reports/recent-matches/" + countRegexp + "/?$", "GET", new ReportRecentMatches());
            router.Bind("^/reports/popular-servers/?$", "GET", new ReportPopularServers());
            router.Bind("^/reports/popular-servers/" + countRegexp + "/?$", "GET", new ReportPopularServers());
        }
    }
}
