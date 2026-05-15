using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;

namespace PanSlotMod
{
    [HarmonyPatch(typeof(Game1), nameof(Game1.pressUseToolButton))]
    public class PressUseToolButtonPatch
    {
        public static bool Prefix()
        {
            Pan pan = PanSlotState.GetPan();

            if (pan == null) return true;
            if (!Context.IsWorldReady || !Context.CanPlayerMove) return true;

            var player = Game1.player;
            var location = player.currentLocation;

            Vector2 position = Game1.wasMouseVisibleThisFrame
                ? new Vector2(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y)
                : player.GetToolLocation(false);

            var cursorTile = new Vector2((int)(position.X / 64), (int)(position.Y / 64));

            if (location.orePanPoint.Value == Point.Zero) return true;
            if (location.orePanPoint.Value != new Point((int)cursorTile.X, (int)cursorTile.Y)) return true;

            int previousIndex = player.CurrentToolIndex;
            var previousItem = player.Items[previousIndex];

            player.Items[previousIndex] = pan;
            player.lastClick = new Vector2((int)position.X, (int)position.Y);
            player.BeginUsingTool();

            void OnUpdateTicked(object s, StardewModdingAPI.Events.UpdateTickedEventArgs e)
            {
                if (player.UsingTool) return;
                player.Items[previousIndex] = previousItem;
                ModEntry.ModHelper.Events.GameLoop.UpdateTicked -= OnUpdateTicked;
            }

            ModEntry.ModHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

            return false;
        }
    }
}