using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace PanSlotMod
{
    public class ModEntry : Mod
    {
        public static Texture2D PanSlotTexture;
        public static IModHelper ModHelper;
        public static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            Config = helper.ReadConfig<ModConfig>();
            PanSlotTexture = helper.ModContent.Load<Texture2D>("assets/UI_Empty_PanSlot.png");

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.PatchAll();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += (s, e) => PanSlotState.Load();
            helper.Events.GameLoop.Saving += (s, e) => PanSlotState.Save();
            helper.Events.GameLoop.Saved += (s, e) => PanSlotState.AfterSave();
            helper.Events.Display.MenuChanged += ClintUpgradeSwap.OnMenuChanged;

        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm == null) return;

            gmcm.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            gmcm.AddTextOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.slot-position.name"),
                tooltip: () => Helper.Translation.Get("config.slot-position.tooltip"),
                getValue: () => Config.SlotPosition.ToString(),
                setValue: value =>
                {
                    if (System.Enum.TryParse<SlotPosition>(value, out var parsed))
                        Config.SlotPosition = parsed;
                },
                allowedValues: new[] { "BelowBoots", "RightOfHat", "BelowTrashCan" },
                formatAllowedValue: value => value switch
                {
                    "BelowBoots" => Helper.Translation.Get("slot-position.below-boots"),
                    "RightOfHat" => Helper.Translation.Get("slot-position.right-of-hat"),
                    "BelowTrashCan" => Helper.Translation.Get("slot-position.below-trash-can"),
                    _ => value
                });
        }
    }
}