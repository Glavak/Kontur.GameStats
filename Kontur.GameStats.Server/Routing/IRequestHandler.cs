﻿using LiteDB;

namespace Kontur.GameStats.Server
{
    public interface IRequestHandler
    {
        object Handle(string[] parameters, object data, LiteDatabase database);
    }
}
