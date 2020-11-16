namespace GuildTracker.Common.Connection
{
    using System;
    using System.Linq;
    using ArgentPonyWarcraftClient;
    using GuildTracker.Common.Models;
    using Guild = Models.Guild;
    using Member = Models.Member;

    public interface IConnection
    {
        Guild GetGuild(string name, string realm);
    }

    public class ArgentPonyConnection : IConnection
    {
        private readonly IWarcraftClient client;

        private string profile;

        public ArgentPonyConnection(string clientId, string clientSecret, string regionString, string localeString)
        {
            if (!Enum.TryParse<Region>(
                regionString,
                out var region))
            {
                throw new ArgumentException($"Region value {regionString} could not be parsed.");
            }

            if (!Enum.TryParse<Locale>(
                localeString,
                out var locale))
            {
                throw new ArgumentException($"Locale value {localeString} could not be parsed.");
            }

            this.client = new WarcraftClient(clientId,clientSecret,region,locale);
        }
        public void SetProfile(string profileString)
        {
            this.profile = profileString;
        }

        public Guild GetGuild(string name, string realm)
        {
            var rosterRequest = this.client.GetGuildRosterAsync(
                realm,
                name,
                this.profile);

            rosterRequest.Wait();

            if (!rosterRequest.IsCompletedSuccessfully || !rosterRequest.Result.Success)
            {
                return null;
            }

            var guildRoster = rosterRequest.Result.Value;
            
            return new Guild
            {
                Faction = guildRoster.Guild.Faction.Name,
                Name = guildRoster.Guild.Name,
                Members = guildRoster.Members.Select(m => this.GetMember(m.Character.Name,m.Character.Realm.Slug)),
            };
        }

        private Member GetMember(string memberName, string realmName)
        {
            var profileRequest = this.client.GetCharacterProfileSummaryAsync(
                realmName,
                memberName,
                this.profile);

            profileRequest.Wait();

            if (!profileRequest.IsCompletedSuccessfully || !profileRequest.Result.Success)
            {
                return null;
            }

            var characterProfile = profileRequest.Result.Value;


            return new Member
            {
                Class = characterProfile.CharacterClass.Name,
                ItemLevel = this.GetEquipment(characterProfile.Name,realmName).ItemLevel,
                Name = characterProfile.Name,
                Race = characterProfile.Race.Name
            };
        }

        private Equipment GetEquipment(string characterName, string realmName)
        {
            var equipmentRequest = this.client.GetCharacterEquipmentSummaryAsync(
                realmName,
                characterName,
                this.profile);

            equipmentRequest.Wait();

            if (!equipmentRequest.IsCompletedSuccessfully || !equipmentRequest.Result.Success)
            {
                return null;
            }

            var equipment = equipmentRequest.Result.Value;

            return new Equipment
            {
                ItemLevel = equipment.EquippedItems.Average(e => e.Level.Value),
            };
        }
    }
}