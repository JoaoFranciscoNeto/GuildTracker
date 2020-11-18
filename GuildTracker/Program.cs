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

                var guild = connection.GetGuild(
                    guildArray[0],
                    guildArray[1]);
                Console.WriteLine($"{guildArray[0]} ({guildArray[1]})");

                db.StoreGuild(new GuildRecord(guild,DateTime.Now));

                foreach (var guildMember in guild.Members)
                {
                    if (guildMember == null)
                    {
                        Console.WriteLine();
                        continue;
                    }

                    Console.WriteLine($"{guildMember.Name,30} | {guildMember.Race,20} | {guildMember.Class,20} | {guildMember.ItemLevel,4}");
                }

            }
            /*
            var guild = connection.GetGuild("sevenwave");

            Console.WriteLine($"{guild.Name} ({guild.Faction})");

            foreach (var guildMember in guild.Members)
            {
                if (guildMember == null)
                {
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine($"{guildMember.Name,30} | {guildMember.Race,20} | {guildMember.Class,20} | {guildMember.ItemLevel,4}");
            }
            */
        }
    }
}
