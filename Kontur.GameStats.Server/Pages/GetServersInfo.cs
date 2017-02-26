﻿namespace Kontur.GameStats.Server
{
    public class GetServersInfo : RequestHandler<EmptyParameters>
    {
        private readonly IRepository<Model.Server> serversTable;

        public GetServersInfo(IRepository<Model.Server> serversTable)
        {
            this.serversTable = serversTable;
        }

        public override object Process(EmptyParameters parameters, object data)
        {
            return serversTable.GetAll();
        }
    }
}
