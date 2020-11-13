using System;

namespace GuildTracker
{
    using System.Threading.Tasks;
    using ArgentPonyWarcraftClient;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var clientId = "0fb0c38b57264472bc1d22d1f7ee02fb";
            var secret = "nj6PVzGcmypAPIJPhtOvss6KCsSPKFTv";

            var client = new WarcraftClient(clientId,secret, Region.Europe, Locale.en_GB);

            var realmSlug = "aggra-português";


            var guildRoster = await client.GetGuildRosterAsync(
                realmSlug,
                "darksoul",
                "profile-eu");

            foreach (var member in guildRoster.Value.Members)
            {
                Console.WriteLine($"{member.Character.Name,40} | {member.Character.Level,5}");
            }

            Console.ReadLine();
        }
    }
}
