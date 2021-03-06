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

namespace MythicSpellbookTweaks {
    static class Main {

        public static Settings Settings;
        public static bool Enabled;

        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();
            /*
            Settings = Settings.Load<Settings>(modEntry);
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            */
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }
        /*
        static void OnGUI(UnityModManager.ModEntry modEntry) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("MyButton", GUILayout.ExpandWidth(false));
            GUILayout.Button("test", GUILayout.Width(300f));
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }
        
        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            Settings.Save(modEntry);
        }
        */
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
                var azataSpellbook = ResourcesLibrary.TryGetBlueprint<BlueprintSpellbook>("b21b9f5e2831c2549a782d8128fb905b");
                azataSpellbook.CastingAttribute = StatType.Intelligence;
            }
        }
    }
}