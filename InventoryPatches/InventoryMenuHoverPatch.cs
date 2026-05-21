using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace PanSlotMod.InventoryPatches
{
    [HarmonyPatch(typeof(InventoryMenu), nameof(InventoryMenu.hover))]
    public class InventoryMenuHoverPatch
    {
        private static Pan _savedPan;

        public static void Prefix(InventoryMenu __instance)
        {
            if (__instance.actualInventory != Game1.player.Items) return;

            int panIndex = PanSlotState.PanSlotIndex;
            if (__instance.actualInventory.Count > panIndex)
            {
                _savedPan = PanSlotState.GetPan();
                __instance.actualInventory[panIndex] = null;
            }
        }

        public static void Postfix(InventoryMenu __instance)
        {
            if (__instance.actualInventory != Game1.player.Items)
            {
                _savedPan = null;
                return;
            }

            int panIndex = PanSlotState.PanSlotIndex;
            if (__instance.actualInventory.Count > panIndex)
                __instance.actualInventory[panIndex] = _savedPan;

            _savedPan = null;
        }
    }
}