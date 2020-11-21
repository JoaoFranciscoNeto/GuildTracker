namespace GuildTracker.Common.Connection
{
    using System.Collections.Generic;
    using GuildTracker.Common.Models;
    using Guild = Models.Guild;

    public interface IConnection
    {
        Guild GetGuild(GuildRequest request);
    }
}