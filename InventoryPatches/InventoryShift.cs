using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

namespace PanSlotMod
{
    [HarmonyPatch(typeof(Farmer), nameof(Farmer.shiftToolbar))]
    public class ShiftToolbarPatch
    {
        public static bool Prefix(Farmer __instance, bool right)
        {
            if (__instance.Items == null || __instance.Items.Count <= PanSlotState.PanSlotIndex)
                return true;

            var pan = __instance.Items[PanSlotState.PanSlotIndex];
            __instance.Items.RemoveAt(PanSlotState.PanSlotIndex);

            RunOriginalShift(__instance, right);

            __instance.Items.Add(pan);

            return false;
        }

        private static void RunOriginalShift(Farmer farmer, bool right)
        {
            if (farmer.Items == null || farmer.Items.Count < 12) return;
            if (farmer.UsingTool || Game1.dialogueUp || !farmer.CanMove ||
                !farmer.Items.HasAny() || Game1.eventUp || Game1.farmEvent != null) return;

            Game1.playSound("shwip", null);

            Item currentItem = farmer.CurrentItem;
            currentItem?.actionWhenStopBeingHeld(farmer);

            if (right)
            {
                IList<Item> toMove = farmer.Items.GetRange(0, 12);
                farmer.Items.RemoveRange(0, 12);
                farmer.Items.AddRange(toMove);
            }
            else
            {
                IList<Item> toMove2 = farmer.Items.GetRange(farmer.Items.Count - 12, 12);
                for (int i = 0; i < farmer.Items.Count - 12; i++)
                    toMove2.Add(farmer.Items[i]);
                farmer.Items.OverwriteWith(toMove2);
            }

            farmer.netItemStowed.Set(false);

            Item currentItem2 = farmer.CurrentItem;
            currentItem2?.actionWhenBeingHeld(farmer);

            for (int j = 0; j < Game1.onScreenMenus.Count; j++)
            {
                if (Game1.onScreenMenus[j] is Toolbar toolbar)
                {
                    toolbar.shifted(right);
                    return;
                }
            }
        }
    }
}