﻿using System;
using Fclp;

namespace Kontur.GameStats.Server
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            var commandLineParser = new FluentCommandLineParser<Options>();

            commandLineParser
                .Setup(options => options.Prefix)
                .As("prefix")
                .SetDefault("http://+:8080/")
                .WithDescription("HTTP prefix to listen on");

            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader(AppDomain.CurrentDomain.FriendlyName + " [--prefix <prefix>]")
                .Callback(text => Console.WriteLine(text));

            if (commandLineParser.Parse(args).HelpCalled)
                return;

            RunServer(commandLineParser.Object);
        }

        private static void RunServer(Options options)
        {
            using (var server = new StatServer())
            {
                server.Start(options.Prefix);

                Console.WriteLine("Server started. Press q to terminate.");

                ConsoleKeyInfo k;
                do
                {
                    k = Console.ReadKey(true);
                } while (k.Key != ConsoleKey.Q);
            }
        }

        private class Options
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Prefix { get; set; }
        }
    }
}
