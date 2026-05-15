using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace PanSlotMod.InventoryPatches
{
    [HarmonyPatch(typeof(InventoryPage), nameof(InventoryPage.receiveLeftClick))]
    public class InventoryPageClickPatch
    {
        public static bool Prefix(InventoryPage __instance, int x, int y)
        {
            foreach (var c in __instance.equipmentIcons)
            {
                if (c.name == "PanSlot" && c.containsPoint(x, y))
                {
                    HandlePanSlotClick();
                    return false;
                }
            }

            return true;
        }

        private static void HandlePanSlotClick()
        {
            Item heldItem = Game1.player.CursorSlotItem;

            Pan currentPan = PanSlotState.GetPan();

            if (heldItem is Pan heldPan)
            {
                PanSlotState.SetPan(heldPan);

                Game1.player.CursorSlotItem = currentPan;

                Game1.playSound("dwop");

                return;
            }

            if (heldItem == null && currentPan != null)
            {
                Game1.player.CursorSlotItem = currentPan;

                PanSlotState.ClearPan();

                Game1.playSound("dwop");
            }
        }
    }
}