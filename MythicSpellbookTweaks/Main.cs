using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace MythicSpellbookTweaks {
    static class Main {

        public static Settings Settings;
        public static bool Enabled;
        public static ModEntry Mod;
        public static readonly BlueprintGuid AeonSpellbook = BlueprintGuid.Parse("6091d66a2a9876b4891b989804cfbcb6");
        public static readonly BlueprintGuid AngelSpellbook = BlueprintGuid.Parse("015658ac45811b843b036e4ccc96c772");
        public static readonly BlueprintGuid AzataSpellbook = BlueprintGuid.Parse("b21b9f5e2831c2549a782d8128fb905b");
        public static readonly BlueprintGuid DemonSpellbook = BlueprintGuid.Parse("e3daa889c72982e45a026f62cc84937d");
        public static readonly BlueprintGuid LichSpellbook = BlueprintGuid.Parse("08a80074263809c4b9616aac05af90ae");
        public static readonly BlueprintGuid TricksterSpellbook = BlueprintGuid.Parse("2ff51e0531ed8e545ab4cb35c32d40f4");
        public static readonly string[] MythicSpellbooks = new string[] { 
            "Aeon",
            "Angel",
            "Azata",
            "Demon",
            "Lich",
            "Trickster"
        };
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg) {
            Mod.Logger.Log(msg);
        }

        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();
            Mod = modEntry;
            Settings = Settings.Load<Settings>(modEntry);
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {

            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry) {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Abundant Casting", GUILayout.ExpandWidth(false))) {
                Settings.enableAbundantCasting = !Settings.enableAbundantCasting;
            }
            GUILayout.Label(Settings.enableAbundantCasting ? "Enabled" : "Disabled", GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Arcane Failure", GUILayout.ExpandWidth(false))) {
                Settings.disableArcaneFailure = !Settings.disableArcaneFailure;
            }
            GUILayout.Label(Settings.disableArcaneFailure ? "Disabled" : "Enabled", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(
                    string.Format("Casting Type: {0}", Settings.castingType.ToString()), GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Fixed Stat", GUILayout.ExpandWidth(false))) {
                Settings.castingType = Settings.CastingType.FixedStat;
            }
            if (GUILayout.Button("Highest Mental", GUILayout.ExpandWidth(false))) {
                Settings.castingType = Settings.CastingType.HighestMental;
            }
            if (GUILayout.Button("Highest Physical", GUILayout.ExpandWidth(false))) {
                Settings.castingType = Settings.CastingType.HighestPhysical;
            }
            if (GUILayout.Button("Highest Stat", GUILayout.ExpandWidth(false))) {
                Settings.castingType = Settings.CastingType.HighestStat;
            }
            if (GUILayout.Button("Mythic Rank", GUILayout.ExpandWidth(false))) {
                Settings.castingType = Settings.CastingType.MythicRank;
            }
            if (GUILayout.Button("Dobule Mythic Rank", GUILayout.ExpandWidth(false)))
            {
                Settings.castingType = Settings.CastingType.DoubleMythicRank;
            }
            if (GUILayout.Button("Half Mythic Rank Plus Highest Stat", GUILayout.ExpandWidth(false)))
            {
                Settings.castingType = Settings.CastingType.HalfMythicRankPlusHighestStat;
            }
            if (GUILayout.Button("Mythic Rank Plus Highest Stat", GUILayout.ExpandWidth(false)))
            {
                Settings.castingType = Settings.CastingType.MythicRankPlusHighestStat;
            }
            GUILayout.EndHorizontal();
            foreach (var mythic in MythicSpellbooks) {
                GUILayout.Label(
                    string.Format("{0}: {1}",
                        mythic,
                        Settings.castingType == Settings.CastingType.FixedStat ? Settings.GetMythicBookStat(mythic).ToString() : String.Join(" ", Settings.castingType.ToString().SplitOnCapitals())),
                    GUILayout.ExpandWidth(false));

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Strength", GUILayout.ExpandWidth(false))) {
                    Settings.SetMythicBookStat(mythic, StatType.Strength);
                }
                if (GUILayout.Button("Dexterity", GUILayout.ExpandWidth(false))) {
                    Settings.SetMythicBookStat(mythic, StatType.Dexterity);
                }
                if (GUILayout.Button("Constitution", GUILayout.ExpandWidth(false))) {
                    Settings.SetMythicBookStat(mythic, StatType.Constitution);
                }
                if (GUILayout.Button("Intelligence", GUILayout.ExpandWidth(false))) {
                    Settings.SetMythicBookStat(mythic, StatType.Intelligence);
                }
                if (GUILayout.Button("Wisdom", GUILayout.ExpandWidth(false))) {
                    Settings.SetMythicBookStat(mythic, StatType.Wisdom);
                }
                if (GUILayout.Button("Charisma", GUILayout.ExpandWidth(false))) {
                    Settings.SetMythicBookStat(mythic, StatType.Charisma);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            Settings.Save(modEntry);
        }

        public static IEnumerable<string> SplitOnCapitals(this string text) {
            Regex regex = new Regex(@"[\p{Lu}\d-/]+[^\p{Lu}\s\d]*");
            foreach (Match match in regex.Matches(text))
            {
                yield return match.Value;
            }
        }

        [HarmonyPatch(typeof(RuleCalculateAbilityParams), "OnTrigger", new Type[] { typeof(RulebookEventContext) })]
        static class RuleCalculateAbilityParams_OnTrigger {

            static void Postfix(RuleCalculateAbilityParams __instance) {
                if (Settings.castingType == Settings.CastingType.MythicRank) { return; }
                bool isMythic = false;
                Spellbook spellbook = __instance.Spellbook;
#if false
                if (spellbook == null) {
                    Log(__instance.AbilityData.Name);
                    var AbilityParams = __instance.AbilityData.Blueprint.ComponentsArray.OfType<ContextCalculateAbilityParams>().First();
                    if (AbilityParams.StatTypeFromCustomProperty) {
                        BlueprintCharacterClass characterClass = AbilityParams.m_CustomProperty.Get()
                                .ComponentsArray.OfType<CastingAttributeGetter>().First()
                                .m_Class;
                        isMythic = characterClass.IsMythic;
                        Log($"Class: {characterClass.Name}");
                        Log($"isMythic: {isMythic}");
                    }
                }
#endif
                if (spellbook != null) {
                    Log($"{__instance.AbilityData.Name}");
                    isMythic = (spellbook != null) ? spellbook.IsMythic : false;
                    Log($"isMythic: {isMythic}");
                }

                if (isMythic) {
                    Log($"Using: {Settings.castingType}");
                    switch (Settings.castingType) {
                        case Settings.CastingType.HighestMental: {
                            updateDC(__instance, getHighestStat(__instance, new StatType[] {
                                StatType.Intelligence,
                                StatType.Wisdom,
                                StatType.Charisma
                            }));
                            return;
                        }
                        case Settings.CastingType.HighestPhysical: {
                            updateDC(__instance, getHighestStat(__instance, new StatType[] {
                                StatType.Strength,
                                StatType.Dexterity,
                                StatType.Constitution,
                            }));
                            return;
                        }
                        case Settings.CastingType.HighestStat: {
                            updateDC(__instance, getHighestStat(__instance, new StatType[] {
                                StatType.Strength,
                                StatType.Dexterity,
                                StatType.Constitution,
                                StatType.Intelligence,
                                StatType.Wisdom,
                                StatType.Charisma
                            }));
                            return;
                        }
                        case Settings.CastingType.FixedStat: {
                            var bookID = __instance.AbilityData.Spellbook.Blueprint.AssetGuid;
                            if (bookID.Equals(AeonSpellbook)) {
                                updateDC(__instance, Settings.AeonStat);
                                return;
                            }
                            if (bookID.Equals(AngelSpellbook)) {
                                updateDC(__instance, Settings.AngelStat);
                                return;
                            }
                            if (bookID.Equals(AzataSpellbook)) {
                                updateDC(__instance, Settings.AzataStat);
                                return;
                            }
                            if (bookID.Equals(DemonSpellbook)) {
                                updateDC(__instance, Settings.DemonStat);
                                return;
                            }
                            if (bookID.Equals(LichSpellbook)) {
                                updateDC(__instance, Settings.LichStat);
                                return;
                            }
                            if (bookID.Equals(TricksterSpellbook)) {
                                updateDC(__instance, Settings.TricksterStat);
                                return;
                            }
                            return;
                        }
                        case Settings.CastingType.MythicRank: {
                            //updateDC(__instance, __instance.Initiator.Progression.MythicLevel);
                            return;
                        }
                        case Settings.CastingType.DoubleMythicRank:
                            {
                                updateDC(__instance, __instance.Initiator.Progression.MythicLevel * 2);
                                return;
                            }
                        case Settings.CastingType.HalfMythicRankPlusHighestStat:
                            {
                                var highestStat = getHighestStat(__instance, new StatType[] {
                                    StatType.Strength,
                                    StatType.Dexterity,
                                    StatType.Constitution,
                                    StatType.Intelligence,
                                    StatType.Wisdom,
                                    StatType.Charisma
                                });
                                var bonus = (__instance.Initiator.Progression.MythicLevel / 2) + __instance.Initiator.Stats.GetAttribute(highestStat).Bonus;
                                updateDC(__instance, bonus);
                                return;
                            }
                        case Settings.CastingType.MythicRankPlusHighestStat:
                            {
                                var highestStat = getHighestStat(__instance, new StatType[] {
                                    StatType.Strength,
                                    StatType.Dexterity,
                                    StatType.Constitution,
                                    StatType.Intelligence,
                                    StatType.Wisdom,
                                    StatType.Charisma
                                });
                                var bonus = __instance.Initiator.Progression.MythicLevel + __instance.Initiator.Stats.GetAttribute(highestStat).Bonus;
                                updateDC(__instance, bonus);
                                return;
                            }
                        default: {
                            return;
                        }
                    }
                }
            }
            static private void updateDC(RuleCalculateAbilityParams abilityParams, StatType newStat) {
                var newMod = abilityParams.Initiator.Stats.GetStat<ModifiableValueAttributeStat>(newStat).Bonus;
                updateDC(abilityParams, newMod);
            }
            static private void updateDC(RuleCalculateAbilityParams abilityParams, int newMod) {
                var oldMod = abilityParams.Initiator.Stats.GetStat<ModifiableValueAttributeStat>(abilityParams.Spellbook.Blueprint.CastingAttribute).Bonus;
                Log($"Starting DC: {abilityParams.Result.DC}");
                abilityParams.Result.DC -= abilityParams.Initiator.Progression.MythicLevel;
                abilityParams.Result.DC += newMod;
                Log($"Ending DC: {abilityParams.Result.DC}");
            }
            static private StatType getHighestStat(RuleCalculateAbilityParams abilityParams, StatType[] stats) {
                StatType highestStat = StatType.Unknown;
                int highestValue = -1;
                foreach (StatType stat in stats) {
                    var value = abilityParams.Initiator.Stats.GetStat(stat).ModifiedValue;
                    if (value > highestValue) {
                        highestStat = stat;
                        highestValue = value;
                    }
                }
                Log($"Highest Stat: {highestStat} - {abilityParams.Initiator.Stats.GetStat(highestStat).ModifiedValue}");
                return highestStat;
            }
        }
        [HarmonyPatch(typeof(AbilityData), "IsAffectedByArcaneSpellFailure", MethodType.Getter)]
        static class AbilityData_MythicArcaneFailure_Patch {
            static void Postfix(AbilityData __instance, ref bool __result) {
                if (!Settings.disableArcaneFailure) { return; }
                if (__instance.Spellbook?.IsStandaloneMythic ?? false) { __result = false; }
            }
        }

        [HarmonyPatch(typeof(Spellbook), "GetSpellsPerDay", new Type[] { typeof(int) })]
        static class Spellbook_AbundantSpells_Patch {
            static void Postfix(Spellbook __instance, int spellLevel, ref int __result) {
                if (__instance.IsStandaloneMythic) {
                    if (Settings.enableAbundantCasting) {
                        UnitPartExtraSpellsPerDay unitPartExtraSpellsPerDay = __instance.Owner.Get<UnitPartExtraSpellsPerDay>();
                        if (unitPartExtraSpellsPerDay != null && unitPartExtraSpellsPerDay.BonusSpells.Length > spellLevel) {
                            __result += unitPartExtraSpellsPerDay.BonusSpells[spellLevel];
                        }
                    }
                }
            }
        }
    }
}