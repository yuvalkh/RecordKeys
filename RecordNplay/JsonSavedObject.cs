using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordNplay
{
    class JsonSavedObject
    {
        public List<PressedInput> list { get; set; }
        public string loop { get; set; }
        public string waitTime { get; set; }
        public JsonSavedObject(List<PressedInput> list, string loop, string waitTime)
        {
            this.list = list;
            this.loop = loop;
            this.waitTime = waitTime;
        }
    }
}
