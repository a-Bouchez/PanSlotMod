using StardewValley;
using StardewValley.Tools;

namespace PanSlotMod
{
    public static class PanSlotState
    {
        public static int PanSlotIndex => Game1.player.MaxItems;

        public static void EnsurePanSlotExists()
        {
            if (Game1.player.Items.Count <= PanSlotIndex)
                Game1.player.Items.Add(null);
        }

        public static Pan GetPan()
        {
            EnsurePanSlotExists();
            return Game1.player.Items[PanSlotIndex] as Pan;
        }

        public static void SetPan(Pan pan)
        {
            EnsurePanSlotExists();
            Game1.player.Items[PanSlotIndex] = pan;
        }

        public static void ClearPan()
        {
            EnsurePanSlotExists();
            Game1.player.Items[PanSlotIndex] = null;
        }
        public static void OnMaxItemsChanged(int oldMaxItems)
        {
            var pan = Game1.player.Items[oldMaxItems] as Pan;
            if (pan == null) return;

            Game1.player.Items[oldMaxItems] = null;
            EnsurePanSlotExists();
            Game1.player.Items[PanSlotIndex] = pan;
        }
    }
}