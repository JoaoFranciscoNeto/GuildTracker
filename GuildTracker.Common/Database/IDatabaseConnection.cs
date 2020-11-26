// <copyright file="IDatabaseConnection.cs" company="Mercedes-Benz Grand Prix Limited">
// Copyright (c) Mercedes-Benz Grand Prix Limited. All rights reserved.
// </copyright>

namespace GuildTracker.Common.Database
{
    using System.Collections.Generic;
    using GuildTracker.Common.Models;

    public interface IDatabaseConnection
    {
        public void StoreGuilds(IEnumerable<GuildRecord> guilds);
    }
}