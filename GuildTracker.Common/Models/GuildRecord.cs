// <copyright file="GuildRecord.cs" company="Mercedes-Benz Grand Prix Limited">
// Copyright (c) Mercedes-Benz Grand Prix Limited. All rights reserved.
// </copyright>

namespace GuildTracker.Common.Models
{
    using System;

    public class GuildRecord
    {
        public GuildRecord(Guild guild, DateTime date)
        {
            this.Guild = guild;
            this.Date = date;
        }

        public DateTime Date { get; set; }
        public Guild Guild { get; set; }
    }
}