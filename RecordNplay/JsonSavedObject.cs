using System.Collections.Generic;

namespace RecordNplay
{
    class JsonSavedObject
    {
        public List<MacroEvent> list { get; set; }
        public string loop { get; set; }
        public string waitTime { get; set; }
        public JsonSavedObject(List<MacroEvent> list, string loop, string waitTime)
        {
            this.list = list;
            this.loop = loop;
            this.waitTime = waitTime;
        }
    }
}
