using Kingmaker.EntitySystem.Stats;
using UnityModManagerNet;
using static MythicSpellbookTweaks.Settings.CastingType;

namespace MythicSpellbookTweaks {
    public class Settings : UnityModManager.ModSettings {
        public StatType AeonStat = StatType.Charisma;
        public StatType AngelStat = StatType.Wisdom;
        public StatType AzataStat = StatType.Charisma;
        public StatType DemonStat = StatType.Charisma;
        public StatType LichStat = StatType.Intelligence;
        public StatType TricksterStat = StatType.Charisma;
        public CastingType castingType = MythicRank;
        public bool disableArcaneFailure = true;
        public bool enableAbundantCasting = true;

        public StatType GetMythicBookStat(string mythic) {
            mythic = mythic.ToLower().Trim();
            switch (mythic) {
                case "aeon":
                    return AeonStat;
                case "angel":
                    return AngelStat;
                case "azata":
                    return AzataStat;
                case "demon":
                    return DemonStat;
                case "lich":
                    return LichStat;
                case "trickster":
                    return TricksterStat;
                default:
                    return StatType.Unknown;
            }
        }

        public void SetMythicBookStat(string mythic, StatType stat) {
            mythic = mythic.ToLower().Trim();
            switch (mythic) {
                case "aeon":
                    AeonStat = stat;
                    break;
                case "angel":
                    AngelStat = stat;
                    break;
                case "azata":
                    AzataStat = stat;
                    break;
                case "demon":
                    DemonStat = stat;
                    break;
                case "lich":
                    LichStat = stat;
                    break;
                case "trickster":
                    TricksterStat = stat;
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
            MythicRank,
            DoubleMythicRank,
            MythicRankPlusHighestStat
        }
    }
}
