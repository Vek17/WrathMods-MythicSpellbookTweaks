using UnityModManagerNet;
using Kingmaker.EntitySystem.Stats;
using static MythicSpellbookTweaks.Settings.CastingType;

namespace MythicSpellbookTweaks {
    public class Settings : UnityModManager.ModSettings {
        public bool Debug = false;
        public StatType aeonStat = StatType.Charisma;
        public StatType angelStat = StatType.Wisdom;
        public StatType azataStat = StatType.Charisma;
        public StatType demonStat = StatType.Charisma;
        public StatType lichStat = StatType.Intelligence;
        public StatType tricksterStat = StatType.Charisma;
        public CastingType castingType = FixedStat;

        public StatType GetMythicBookStat(string mythic) {
            mythic = mythic.ToLower().Trim();
            switch (mythic) {
                case "aeon":
                    return aeonStat;
                case "angel":
                    return angelStat;
                case "azata":
                    return azataStat;
                case "demon":
                    return demonStat;
                case "lich":
                    return lichStat;
                case "trickster":
                    return tricksterStat;
                default:
                    return StatType.Unknown;
            }
        }

        public void SetMythicBookStat(string mythic, StatType stat) {
            mythic = mythic.ToLower().Trim();
            switch (mythic) {
                case "aeon":
                    aeonStat = stat;
                    break;
                case "angel":
                    angelStat = stat;
                    break;
                case "azata":
                    azataStat = stat;
                    break;
                case "demon":
                    demonStat = stat;
                    break;
                case "lich":
                    lichStat = stat;
                    break;
                case "trickster":
                    tricksterStat = stat;
                    break;
                default:
                    break;
            }
        }
        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
        public enum CastingType {
              FixedStat,
              HighestMental,
              HighestPhysical,
              HighestStat,
              MythicRank
        }
    }
}
