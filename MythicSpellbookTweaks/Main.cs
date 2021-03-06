using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using System;
using UnityEngine;
using UnityEngine.UI;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using System.Collections.Generic;

namespace MythicSpellbookTweaks {
    static class Main {

        public static Settings Settings;
        public static bool Enabled;

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

            foreach (var mythic in mythicSpellbooks.Keys) {
                GUILayout.BeginVertical();
                switch (mythic) {
                    case "Aeon":
                        GUILayout.Label(string.Format("{0}: {1}", mythic, Settings.aeonStat.ToString(), GUILayout.ExpandWidth(false)));
                        break;
                    case "Angel":
                        GUILayout.Label(string.Format("{0}: {1}", mythic, Settings.angelStat.ToString(), GUILayout.ExpandWidth(false)));
                        break;
                    case "Azata":
                        GUILayout.Label(string.Format("{0}: {1}", mythic, Settings.azataStat.ToString(), GUILayout.ExpandWidth(false)));
                        break;
                    case "Demon":
                        GUILayout.Label(string.Format("{0}: {1}", mythic, Settings.demonStat.ToString(), GUILayout.ExpandWidth(false)));
                        break;
                    case "Lich":
                        GUILayout.Label(string.Format("{0}: {1}", mythic, Settings.lichStat.ToString(), GUILayout.ExpandWidth(false)));
                        break;
                    case "Trickster":
                        GUILayout.Label(string.Format("{0}: {1}", mythic, Settings.tricksterStat.ToString(), GUILayout.ExpandWidth(false)));
                        break;
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("STR")) {
                    var stat = StatType.Strength;
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    switch (mythic) {
                        case "Aeon":
                            Settings.aeonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.aeonStat;
                            break;
                        case "Angel":
                            Settings.angelStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.angelStat;
                            break;
                        case "Azata":
                            Settings.azataStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.azataStat;
                            break;
                        case "Demon":
                            Settings.demonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.demonStat;
                            break;
                        case "Lich":
                            Settings.lichStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.lichStat;
                            break;
                        case "Trickster":
                            Settings.tricksterStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.tricksterStat;
                            break;
                    }
                }
                if (GUILayout.Button("DEX")) {
                    var stat = StatType.Dexterity;
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    switch (mythic) {
                        case "Aeon":
                            Settings.aeonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.aeonStat;
                            break;
                        case "Angel":
                            Settings.angelStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.angelStat;
                            break;
                        case "Azata":
                            Settings.azataStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.azataStat;
                            break;
                        case "Demon":
                            Settings.demonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.demonStat;
                            break;
                        case "Lich":
                            Settings.lichStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.lichStat;
                            break;
                        case "Trickster":
                            Settings.tricksterStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.tricksterStat;
                            break;
                    }
                }
                if (GUILayout.Button("CON")) {
                    var stat = StatType.Constitution;
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    switch (mythic) {
                        case "Aeon":
                            Settings.aeonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.aeonStat;
                            break;
                        case "Angel":
                            Settings.angelStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.angelStat;
                            break;
                        case "Azata":
                            Settings.azataStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.azataStat;
                            break;
                        case "Demon":
                            Settings.demonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.demonStat;
                            break;
                        case "Lich":
                            Settings.lichStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.lichStat;
                            break;
                        case "Trickster":
                            Settings.tricksterStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.tricksterStat;
                            break;
                    }
                }
                if (GUILayout.Button("INT")) {
                    var stat = StatType.Intelligence;
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    switch (mythic) {
                        case "Aeon":
                            Settings.aeonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.aeonStat;
                            break;
                        case "Angel":
                            Settings.angelStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.angelStat;
                            break;
                        case "Azata":
                            Settings.azataStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.azataStat;
                            break;
                        case "Demon":
                            Settings.demonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.demonStat;
                            break;
                        case "Lich":
                            Settings.lichStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.lichStat;
                            break;
                        case "Trickster":
                            Settings.tricksterStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.tricksterStat;
                            break;
                    }
                }
                if (GUILayout.Button("WIS")) {
                    var stat = StatType.Wisdom;
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    switch (mythic) {
                        case "Aeon":
                            Settings.aeonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.aeonStat;
                            break;
                        case "Angel":
                            Settings.angelStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.angelStat;
                            break;
                        case "Azata":
                            Settings.azataStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.azataStat;
                            break;
                        case "Demon":
                            Settings.demonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.demonStat;
                            break;
                        case "Lich":
                            Settings.lichStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.lichStat;
                            break;
                        case "Trickster":
                            Settings.tricksterStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.tricksterStat;
                            break;
                    }
                }
                if (GUILayout.Button("CHA")) {
                    var stat = StatType.Charisma;
                    var mythicSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks[mythic]);
                    switch (mythic) {
                        case "Aeon":
                            Settings.aeonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.aeonStat;
                            break;
                        case "Angel":
                            Settings.angelStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.angelStat;
                            break;
                        case "Azata":
                            Settings.azataStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.azataStat;
                            break;
                        case "Demon":
                            Settings.demonStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.demonStat;
                            break;
                        case "Lich":
                            Settings.lichStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.lichStat;
                            break;
                        case "Trickster":
                            Settings.tricksterStat = stat;
                            mythicSpellbook.CastingAttribute = Settings.tricksterStat;
                            break;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
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
                if (Initialized) return;
                Initialized = true;
                patchAzataAttribute();
            }
            static void patchAzataAttribute() {
                // Update Azata to be Int based
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Aeon"]).CastingAttribute         = Settings.aeonStat;
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Angel"]).CastingAttribute        = Settings.angelStat;
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Azata"]).CastingAttribute        = Settings.azataStat;
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Demon"]).CastingAttribute        = Settings.demonStat;
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Lich"]).CastingAttribute         = Settings.lichStat;
                ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>(mythicSpellbooks["Trickster"]).CastingAttribute    = Settings.tricksterStat;
            }
        }
    }
}