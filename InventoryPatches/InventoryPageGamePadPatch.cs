using HarmonyLib;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace PanSlotMod
{
    [HarmonyPatch(typeof(InventoryPage), nameof(InventoryPage.receiveGamePadButton))]
    public class InventoryPageGamePadPatch
    {
        public static bool Prefix(Buttons button, InventoryPage __instance)
        {
            if (button != Buttons.Back)
                return true;

            var organizeButton = AccessTools.Field(typeof(InventoryPage), "organizeButton").GetValue(__instance);
            if (organizeButton == null)
                return true;

            var items = Game1.player.Items;
            if (items == null || items.Count <= PanSlotState.PanSlotIndex)
                return true;

            var pan = items[PanSlotState.PanSlotIndex];
            items.RemoveAt(PanSlotState.PanSlotIndex);

            ItemGrabMenu.organizeItemsInList(items);

            items.Insert(PanSlotState.PanSlotIndex, pan);
            Game1.playSound("Ship", null);

            return false;
        }
    }
}