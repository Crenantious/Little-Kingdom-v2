namespace LittleKingdom.Resources
{
    // TODO: JR - determine how this works. May need its own request record and to inherit from IHandleResources.
    public interface IProduceResources
    {
        public IPlayer Player { get; }
    }
}