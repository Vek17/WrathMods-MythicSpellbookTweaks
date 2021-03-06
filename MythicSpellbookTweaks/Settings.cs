using UnityModManagerNet;
using Kingmaker.EntitySystem.Stats;

namespace MythicSpellbookTweaks {
    public class Settings : UnityModManager.ModSettings {
        public StatType aeonStat        = StatType.Charisma;
        public StatType angelStat       = StatType.Wisdom;
        public StatType azataStat       = StatType.Charisma;
        public StatType demonStat       = StatType.Charisma;
        public StatType lichStat        = StatType.Intelligence;
        public StatType tricksterStat   = StatType.Charisma;

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
