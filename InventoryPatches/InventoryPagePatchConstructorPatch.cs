using HarmonyLib;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using PanSlotMod.GMCM;

namespace PanSlotMod.InventoryPatches
{
    [HarmonyPatch(typeof(InventoryPage), MethodType.Constructor,
        new[] { typeof(int), typeof(int), typeof(int), typeof(int) })]
    public class InventoryPageConstructorPatch
    {
        public static void Postfix(InventoryPage __instance)
        {
            var icons = __instance.equipmentIcons;
            var position = ModEntry.Config.SlotPosition;

            if (position == SlotPosition.BelowTrashCan)
            {
                icons.Add(new ClickableComponent(new Rectangle(0, 0, 64, 64), "PanSlot"));
                return;
            }

            string targetName = position switch
            {
                SlotPosition.BelowBoots => "Boots",
                SlotPosition.RightOfHat => "Hat",
                _ => throw new System.Exception("SlotPosition not recognized")
            };

            ClickableComponent reference = null;
            foreach (var c in icons)
                if (c.name == targetName) { reference = c; break; }

            if (reference == null) return;

            Rectangle bounds = position switch
            {
                SlotPosition.BelowBoots => new Rectangle(reference.bounds.X, reference.bounds.Y + 64, 64, 64),
                SlotPosition.RightOfHat => new Rectangle(reference.bounds.X + 64, reference.bounds.Y, 64, 64),
                _ => throw new System.ComponentModel.InvalidEnumArgumentException(nameof(position), (int)position, typeof(SlotPosition))
            };

            icons.Add(new ClickableComponent(bounds, "PanSlot"));
        }
    }
}