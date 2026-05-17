using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace PanSlotMod.InventoryPatches
{
    [HarmonyPatch(typeof(InventoryPage), nameof(InventoryPage.receiveLeftClick))]
    public class InventoryPageClickPatch
    {
        private static Pan _savedPan;

        public static bool Prefix(InventoryPage __instance, int x, int y)
        {
            if (__instance.organizeButton != null && __instance.organizeButton.containsPoint(x, y))
            {
                _savedPan = PanSlotState.GetPan();
                Game1.player.Items[PanSlotState.PanSlotIndex] = null;
                return true;
            }

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

        public static void Postfix(InventoryPage __instance, int x, int y)
        {
            if (__instance.organizeButton != null && __instance.organizeButton.containsPoint(x, y))
            {
                if (Game1.player.Items.Count <= PanSlotState.PanSlotIndex)
                    Game1.player.Items.Add(_savedPan);
                else
                    Game1.player.Items[PanSlotState.PanSlotIndex] = _savedPan;
                _savedPan = null;
            }
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