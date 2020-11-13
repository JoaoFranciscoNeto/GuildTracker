using System;

namespace GuildTracker
{
    using System.Threading.Tasks;
    using ArgentPonyWarcraftClient;
    using GuildTracker.Common.Connection;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var clientId = "0fb0c38b57264472bc1d22d1f7ee02fb";
            var secret = "nj6PVzGcmypAPIJPhtOvss6KCsSPKFTv";


            var client = new WarcraftClient(clientId,secret, Region.Europe, Locale.en_GB);

            var realmSlug = "aggra-português";

            var connection = new ArgentPonyConnection(clientId,secret,"Europe","en_GB");

            connection.SetRealm(realmSlug);
            connection.SetProfile("profile-eu");

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

            Console.ReadLine();
        }
    }
}
