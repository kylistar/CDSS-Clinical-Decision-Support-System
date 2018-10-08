using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class Visit
    {
        private List<object> attributes;
        public List<object> Attributes { get => attributes; set => attributes = value; }

        public Visit()
        {
            attributes = new List<object>();
        }
    }
}
