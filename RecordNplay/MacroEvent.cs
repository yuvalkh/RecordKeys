using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordNplay
{
    public abstract class MacroEvent
    {
        public long startTime;

        public abstract void activate();

        public override abstract string ToString();

    }
}
