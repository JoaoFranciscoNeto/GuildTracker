// <copyright file="IDatabaseConnection.cs" company="Mercedes-Benz Grand Prix Limited">
// Copyright (c) Mercedes-Benz Grand Prix Limited. All rights reserved.
// </copyright>

namespace GuildTracker.Common.Database
{
    using GuildTracker.Common.Models;

    public interface IDatabaseConnection
    {
        void StoreGuild(GuildRecord guild);
    }
}