using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tt
{
    struct KeyMap
    {
        public Keys Key { get; private set; }

        public string Modified { get; private set; }

        public string Original { get; private set; }

        public KeyMap (Keys key, string original, string modified = null) : this()
        {
            Key = key;
            Original = original;
            Modified = modified;
        }
    }
}
