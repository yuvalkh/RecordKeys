namespace RecordNplay
{
    class LoopEvent : MacroEvent
    {
        public long numberOfLoops;
        public int numberOfEvents;
        public int startEventIndex;
        public long startEventTime;
        public int currentLoop;
        // int delayBetweenLoops; currently optional, might want to add delay time between loops
        public LoopEvent()
        {
            startTime = -1;
            numberOfEvents = -1;
            numberOfLoops = -1;
        }
        public LoopEvent(long startTime,long numberOfLoops, int numberOfEvents)
        {
            this.startTime = startTime;
            this.numberOfLoops = numberOfLoops;
            this.numberOfEvents = numberOfEvents;
        }
        public LoopEvent(LoopEvent loopEvent)
        {
            startTime = loopEvent.startTime;
            numberOfLoops = loopEvent.numberOfLoops;
            numberOfEvents = loopEvent.numberOfEvents;
            currentLoop = loopEvent.currentLoop;
        }
        public override void activate()
        {
            //new Task(() =>
            //{
                currentLoop = 1;
                startEventIndex = Form1.stepNumber+1;
                startEventTime = Form1.sw.ElapsedMilliseconds;
            //}).Start();
        }

        public override string ToString()
        {
            return "Loop " + numberOfLoops + " times the next " + numberOfEvents + " lines at " + startTime;
        }
    }
}
