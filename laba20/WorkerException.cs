using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba20
{
    public class WorkerException : Exception
    {
        public WorkerException(string message) : base(message) { }
    }
}
