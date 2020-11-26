// <copyright file="ArgentPonyConnection.cs" company="Mercedes-Benz Grand Prix Limited">
// Copyright (c) Mercedes-Benz Grand Prix Limited. All rights reserved.
// </copyright>

namespace GuildTracker.Common.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ArgentPonyWarcraftClient;
    using GuildTracker.Common.Models;
    using Guild = GuildTracker.Common.Models.Guild;
    using Member = GuildTracker.Common.Models.Member;

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

        public Guild GetGuild(GuildRequest request)
        {
            var rosterRequest = this.client.GetGuildRosterAsync(
                request.Realm,
                request.Name,
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
                Members = this.GetMembers(guildRoster.Members),
            };
        }

        private IEnumerable<Member> GetMembers(IEnumerable<GuildMember> members)
        {
            return members
                .Select(guildMember => 
                    this.GetMember(
                        guildMember.Character.Name,
                        guildMember.Character.Realm.Slug))
                .Where(member => member != null);
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

            Equipment equipment = null;

            if (characterProfile.Level >= 50)
            {
                equipment = this.GetEquipment(characterProfile.Name,realmName);
            }

            return new Member
            {
                Class = characterProfile.CharacterClass.Name,
                Equipment = equipment,
                Name = characterProfile.Name,
                Race = characterProfile.Race.Name,
                Level = characterProfile.Level,
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
                EquippedItems = equipment.EquippedItems.Select(i=>new EquipmentItem{ItemLevel = (int) i.Level.Value,Name = i.Name,Slot = i.Slot.Name})
            };
        }
    }
}