using System;

namespace GuildTracker
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ArgentPonyWarcraftClient;
    using GuildTracker.Common.Connection;
    using GuildTracker.Common.Database;
    using GuildTracker.Common.Models;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Bson.Serialization.Serializers;

    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var blizzardConfig = configuration.GetSection("Blizzard");


            var connection = new ArgentPonyConnection(blizzardConfig["ClientId"], blizzardConfig["ClientSecret"], "Europe","en_GB");

            connection.SetProfile("profile-eu");

            var applicationConfig = configuration.GetSection("Application");
            var guildsConfig = applicationConfig.GetSection("Guilds").GetChildren();

            var db = new MongoConnection(configuration);
            var request = guildsConfig.Select( g=>
            {
                var guildArray = g.Value.Split(",");
                return new GuildRequest
                {
                    Name = guildArray[0], Realm = guildArray[1]
                };
            });

            var guilds = GetGuilds(request, connection);

            db.StoreGuilds(guilds.ToArray());
        }

        private static IEnumerable<GuildRecord> GetGuilds(IEnumerable<GuildRequest> guildRequests,ArgentPonyConnection connection)
        {
            foreach (var guildRequest in guildRequests)
            {
                var guild = connection.GetGuild(guildRequest);

                if (guild != null)
                {
                    Console.WriteLine($"{guild.Name,40} | {guild.Members.Count()}");
                    yield return new GuildRecord(guild,DateTime.Now);
                }
            }
        }
    }
}
