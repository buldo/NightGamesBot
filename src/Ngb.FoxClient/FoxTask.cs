using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngb.FoxClient
{
    public abstract class FoxTask
    {

        internal FoxTask()
        {
            
        }

        public EnterCodeResult CheckCode(FoxTask task, string code)
        {
            return new EnterCodeResult();
        }
    }
}
