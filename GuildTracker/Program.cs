using System;

namespace GuildTracker
{
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

            foreach (var guildConfig in guildsConfig)
            {
                var guildArray = guildConfig.Value.Split(",");

                var request = new GuildRequest
                {
                    Name = guildArray[0], Realm = guildArray[1]
                };

                var guild = connection.GetGuild(request);

                if (guild == null)
                {
                    continue;
                }

                db.StoreGuild(new GuildRecord(guild,DateTime.Now));

            }
        }
    }
}
