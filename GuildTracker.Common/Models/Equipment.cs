namespace GuildTracker.Common.Models
{
    using System.Collections.Generic;

    public class Equipment
    {
        public float ItemLevel { get; set; }
        public IEnumerable<EquipmentItem> EquippedItems { get; set; }
    }
}