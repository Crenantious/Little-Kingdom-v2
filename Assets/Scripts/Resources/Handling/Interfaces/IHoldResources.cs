namespace LittleKingdom.Resources
{
    // TODO: JR - make the Resources class auto handle capacity.
    public interface IHoldResources
    {
        public Resources Resources { get; }
        public Resources ResourcesCapacity { get; }
    }
}