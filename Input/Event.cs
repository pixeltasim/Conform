using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
namespace Conform.Input
{
    /*
     * Just want to be clear the whole purpose of this class is so the regular event loop can be used and passed down into Statestack and junk
    */
    class Event
    {
        public uint Type;
        public EventArgs args;
        public Event(EventArgs args, EventType type) 
        {
            this.Type = (uint)type;
            if (args is KeyEventArgs) { this.args = (KeyEventArgs)args; return; }
            this.args = args;
        }
    }
}
