namespace LittleKingdom
{
    public class UpdatableInfo
    {
        public IUpdatable Updatable { get; }
        public float Delay { get; }
        public float Cooldown { get; set; }

        public UpdatableInfo(IUpdatable updatable, float delay = 0)
        {
            Updatable = updatable;
            Delay = delay;
        }
    }
}