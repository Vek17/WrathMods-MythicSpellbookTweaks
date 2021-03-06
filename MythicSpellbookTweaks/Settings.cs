using UnityModManagerNet;
using Kingmaker.EntitySystem.Stats;

namespace MythicSpellbookTweaks {
    public class Settings : UnityModManager.ModSettings {
        public StatType Stat = StatType.Intelligence;

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
