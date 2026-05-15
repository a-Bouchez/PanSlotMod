namespace PanSlotMod.GMCM
{
    public enum SlotPosition
    {
        BelowBoots,
        RightOfHat,
        BelowTrashCan
    }

    public class ModConfig
    {
        public SlotPosition SlotPosition { get; set; } = SlotPosition.BelowBoots;
    }
}   