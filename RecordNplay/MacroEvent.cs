namespace RecordNplay
{
    public abstract class MacroEvent
    {
        public long startTime;

        public abstract void activate();

        public override abstract string ToString();

    }
}
