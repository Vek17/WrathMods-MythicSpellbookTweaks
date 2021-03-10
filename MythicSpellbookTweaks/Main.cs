using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using UnityEngine;
using System.Collections.Generic;
using static UnityModManagerNet.UnityModManager;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using System;

namespace MythicSpellbookTweaks {
    static class Main {

        public static Settings Settings;
        public static bool Enabled;
        public static ModEntry Mod;
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
            if (GUILayout.Button("Toggle Mythic Casting", GUILayout.ExpandWidth(false))) {
                Settings.toggleMythicCasting();
            }
            foreach (var mythic in mythicSpellbooks.Keys) {
                GUILayout.Label(
                    string.Format("{0}: {1}",
                        mythic,
                        Settings.mythicCasting ? "Mythic Rank" : Settings.GetMythicBookStat(mythic).ToString(),
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

        /// <summary>
        /// We cannot modify blueprints until after the game has loaded them, we patch 
        /// LibraryScriptableObject.LoadDictionary to be able to make our modifications as
        /// soon as the blueprints have loaded.
        /// </summary>
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                    // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                    // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                    // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                    // to prevent blueprints from being garbage collected.
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                Main.Mod.Logger.Log("Resource Library ENTER");
                if (Initialized) return;
                Initialized = true;
                patchMythicSpellbookAttributes();
            }
            static void patchMythicSpellbookAttributes() {
                // Update Azata to be Int based
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
                Main.Mod.Logger.Log("Mythic Calc ENTER");
                if (!Settings.mythicCasting) {
                    Main.Mod.Logger.Log("Mtythic Calc EXIT");
                    return;
                }
                Main.Mod.Logger.Log("Mythic Calc Trigger");
                Spellbook spellbook = __instance.Spellbook;
                bool isMythic = (spellbook != null) ? spellbook.IsMythic : false;
                Main.Mod.Logger.Log($"Starting DC: {__instance.Result.DC}");
                if (isMythic) {
                    Main.Mod.Logger.Log($"isMythic: {isMythic}");
                    var oldMod = __instance.Initiator.Stats.GetStat<ModifiableValueAttributeStat>(__instance.Spellbook.Blueprint.CastingAttribute).Bonus;
                    var newMod = __instance.Initiator.Progression.MythicExperience;
                    __instance.Result.DC -= oldMod;
                    __instance.Result.DC += newMod;
                }
                Main.Mod.Logger.Log($"Ending DC: {__instance.Result.DC}");
            }
        }
    }
}