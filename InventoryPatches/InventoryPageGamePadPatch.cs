using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

namespace PanSlotMod
{
    [HarmonyPatch(typeof(ItemGrabMenu), nameof(ItemGrabMenu.organizeItemsInList))]
    public class OrganizeItemsGlobalPatch
    {
        public static void Prefix(IList<Item> items, out Item __state)
        {
            __state = null;

            if (items != null && object.ReferenceEquals(items, Game1.player.Items) && items.Count > PanSlotState.PanSlotIndex)
            {
                __state = items[PanSlotState.PanSlotIndex];
                items.RemoveAt(PanSlotState.PanSlotIndex);
            }
        }

        public static void Postfix(IList<Item> items, Item __state)
        {
            if (__state != null)
            {
                items.Insert(PanSlotState.PanSlotIndex, __state);
            }
        }
    }
}