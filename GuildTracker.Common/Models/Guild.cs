namespace GuildTracker.Common.Models
{
    using System.Collections.Generic;

    public class Guild
    {
        public string Name { get; set; }
        public string Faction { get; set; }
        public IEnumerable<Member> Members { get; set; }
    }
}