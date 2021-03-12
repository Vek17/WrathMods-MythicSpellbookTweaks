using static UnityModManagerNet.UnityModManager;
using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using UnityEngine;
using System.Collections.Generic;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using System;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints.Classes;

namespace MythicSpellbookTweaks {
    static class Main {

        public static Settings Settings;
        public static bool Enabled;
        public static ModEntry Mod;
        public static void Log(string msg) {
            if (Settings.Debug) {
                Mod.Logger.Log(msg);
            }
        }
        private static Dictionary<string, string> mythicSpellbooks = new Dictionary<string, string>() {
            {"Aeon",        "6091d66a2a9876b4891b989804cfbcb6"},
            {"Angel",       "015658ac45811b843b036e4ccc96c772"},
            {"Azata",       "b21b9f5e2831c2549a782d8128fb905b"},
            {"Demon",       "e3daa889c72982e45a026f62cc84937d"},
            {"Lich",        "08a80074263809c4b9616aac05af90ae"},
            {"Trickster",   "2ff51e0531ed8e545ab4cb35c32d40f4"},

        };

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
            GUILayout.Label(
                    string.Format("Casting Type: {0}", Settings.castingType.ToString(),
                    GUILayout.ExpandWidth(false)));
            if (GUILayout.Button("Fixed Stat")) {
                Settings.castingType = Settings.CastingType.FixedStat;
            }
            if (GUILayout.Button("Highest Mental")) {
                Settings.castingType = Settings.CastingType.HighestMental;
            }
            if (GUILayout.Button("Highest Physical")) {
                Settings.castingType = Settings.CastingType.HighestPhysical;
            }
            if (GUILayout.Button("Highest Stat")) {
                Settings.castingType = Settings.CastingType.HighestStat;
            }
            if (GUILayout.Button("Mythic Rank")) {
                Settings.castingType = Settings.CastingType.MythicRank;
            }
            GUILayout.EndHorizontal();
            foreach (var mythic in mythicSpellbooks.Keys) {
                GUILayout.Label(
                    string.Format("{0}: {1}",
                        mythic,
                        Settings.castingType == Settings.CastingType.FixedStat ? Settings.GetMythicBookStat(mythic).ToString() : Settings.castingType.ToString(),
                    GUILayout.ExpandWidth(false)));

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Strength")) {
                    Settings.SetMythicBookStat(mythic, StatType.Strength);
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    mythicSpellbook.CastingAttribute = Settings.GetMythicBookStat(mythic);
                }
                if (GUILayout.Button("Dexterity")) {
                    Settings.SetMythicBookStat(mythic, StatType.Dexterity);
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    mythicSpellbook.CastingAttribute = Settings.GetMythicBookStat(mythic);
                }
                if (GUILayout.Button("Constitution")) {
                    Settings.SetMythicBookStat(mythic, StatType.Constitution);
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    mythicSpellbook.CastingAttribute = Settings.GetMythicBookStat(mythic);
                }
                if (GUILayout.Button("Intelligence")) {
                    Settings.SetMythicBookStat(mythic, StatType.Intelligence);
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    mythicSpellbook.CastingAttribute = Settings.GetMythicBookStat(mythic);
                }
                if (GUILayout.Button("Wisdom")) {
                    Settings.SetMythicBookStat(mythic, StatType.Wisdom);
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    mythicSpellbook.CastingAttribute = Settings.GetMythicBookStat(mythic);
                }
                if (GUILayout.Button("Charisma")) {
                    Settings.SetMythicBookStat(mythic, StatType.Charisma);
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    mythicSpellbook.CastingAttribute = Settings.GetMythicBookStat(mythic);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            Settings.Save(modEntry);
        }

        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Log("Patching MythicSpellBookTweaks");
                patchMythicSpellbookAttributes();
            }
            static void patchMythicSpellbookAttributes() {
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Aeon"]).CastingAttribute = Settings.GetMythicBookStat("Aeon");
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Angel"]).CastingAttribute = Settings.GetMythicBookStat("Angel");
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Azata"]).CastingAttribute = Settings.GetMythicBookStat("Azata");
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Demon"]).CastingAttribute = Settings.GetMythicBookStat("Demon");
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Lich"]).CastingAttribute = Settings.GetMythicBookStat("Lich");
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Trickster"]).CastingAttribute = Settings.GetMythicBookStat("Trickster");
            }
        }

        [HarmonyPatch(typeof(RuleCalculateAbilityParams), "OnTrigger", new Type[] { typeof(RulebookEventContext) })]
        static class RuleCalculateAbilityParams_OnTrigger {
            
            static void Postfix(RuleCalculateAbilityParams __instance) {
                bool isMythic = false;
                Spellbook spellbook = __instance.Spellbook;
                
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
                else {
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
                        case Settings.CastingType.MythicRank: {
                                updateDC(__instance, __instance.Initiator.Progression.MythicExperience);
                                return;
                            }
                        default: {
                                return;
                            }
                    }
                }
            }
            static private void updateDC(RuleCalculateAbilityParams abilityParams, StatType newStat) {
                var newMod = abilityParams.Initiator.Stats.GetStat< ModifiableValueAttributeStat>(newStat).Bonus;
                updateDC(abilityParams, newMod);
            }
            static private void updateDC(RuleCalculateAbilityParams abilityParams, int newMod) {
                var oldMod = abilityParams.Initiator.Stats.GetStat<ModifiableValueAttributeStat>(abilityParams.Spellbook.Blueprint.CastingAttribute).Bonus;
                Log($"Starting DC: {abilityParams.Result.DC}");
                abilityParams.Result.DC -= oldMod;
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
    }
}