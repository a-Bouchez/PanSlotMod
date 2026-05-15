using HarmonyLib;
using StardewValley.Menus;
using StardewValley.Tools;

namespace PanSlotMod.InventoryPatches
{
    [HarmonyPatch(typeof(InventoryPage), nameof(InventoryPage.performHoverAction))]
    public class InventoryPageHoverPatch
    {
        public static void Postfix(InventoryPage __instance, int x, int y)
        {
            foreach (var c in __instance.equipmentIcons)
            {
                if (c.name == "PanSlot" && c.containsPoint(x, y))
                {
                    Pan pan = PanSlotState.GetPan();

                    if (pan != null)
                    {
                        __instance.hoveredItem = pan;
                        __instance.hoverText = pan.getDescription();
                        __instance.hoverTitle = pan.DisplayName;
                    }
                }
            }
        }
    }
}