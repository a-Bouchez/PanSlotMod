using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PanSlotMod.GMCM;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace PanSlotMod.InventoryPatches
{
    [HarmonyPatch(typeof(InventoryPage), nameof(InventoryPage.draw))]
    public class InventoryPageDrawPatch
    {
        public static void Postfix(InventoryPage __instance, SpriteBatch b)
        {
            var position = ModEntry.Config.SlotPosition;

            ClickableComponent panSlot = null;
            foreach (var c in __instance.equipmentIcons)
            {
                if (c.name == "PanSlot") { panSlot = c; break; }
            }

            if (panSlot == null) return;
            
            switch (position)
            {
                case SlotPosition.BelowTrashCan:
                    var trash = __instance.trashCan;

                    int slotX = trash.bounds.X + (trash.bounds.Width - 64) / 2;
                    int slotY = trash.bounds.Y + trash.bounds.Height + 24;

                    int maxY = __instance.yPositionOnScreen + __instance.height - 64;
                    if (slotY > maxY) slotY = maxY;

                    panSlot.bounds = new Rectangle(slotX, slotY, 64, 64);

                    int padding = 13;

                    IClickableMenu.drawTextureBox(
                        b,
                        Game1.mouseCursors,
                        new Rectangle(384, 373, 18, 18),
                        panSlot.bounds.X - padding,
                        panSlot.bounds.Y - padding,
                        64 + padding * 2,
                        64 + padding * 2,
                        Color.White,
                        4f
                    );

                    break;

                case SlotPosition.BelowBoots:
                case SlotPosition.RightOfHat:
                    b.Draw(
                        Game1.menuTexture,
                        panSlot.bounds,
                        Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1),
                        Color.White
                    );
                    break;
            }

            Pan pan = PanSlotState.GetPan();

            if (pan != null)
            {
                pan.drawInMenu(
                    b,
                    new Vector2(panSlot.bounds.X, panSlot.bounds.Y),
                    panSlot.scale
                );
            }
            else
            {
                b.Draw(
                    ModEntry.PanSlotTexture,
                    panSlot.bounds,
                    Color.White
                );
            }
        }
    }
}