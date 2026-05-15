using HarmonyLib;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Shops;
using StardewValley.Menus;
using StardewValley.Tools;

namespace PanSlotMod
{
    public static class ClintUpgradeSwap
    {
        private static int _swappedOutSlot = -1;

        public static void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.OldMenu is ShopMenu oldShop && oldShop.ShopId == "ClintUpgrade")
            {
                if (_swappedOutSlot == -1) return;
                if (Game1.player.Items[_swappedOutSlot] is Pan pan)
                {
                    PanSlotState.SetPan(pan);
                    Game1.player.Items[_swappedOutSlot] = null;
                }
                _swappedOutSlot = -1;
            }
        }

        [HarmonyPatch(typeof(ShopMenu), MethodType.Constructor, new[] {
            typeof(string),
            typeof(ShopData),
            typeof(ShopOwnerData),
            typeof(NPC),
            typeof(ShopMenu.OnPurchaseDelegate),
            typeof(Func<ISalable, bool>),
            typeof(bool)
        })]
        private class ShopMenuConstructorPatch
        {
            static void Prefix(string shopId)
            {
                if (shopId != "ClintUpgrade") return;
                Pan pan = PanSlotState.GetPan();
                if (pan == null) return;

                var player = Game1.player;
                for (int i = 0; i < player.Items.Count; i++)
                {
                    if (player.Items[i] == null)
                    {
                        _swappedOutSlot = i;
                        player.Items[i] = pan;
                        PanSlotState.ClearPan();
                        return;
                    }
                }
            }
        }
    }
}