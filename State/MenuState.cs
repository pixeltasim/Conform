using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conform.State
{
    class MenuState : State
    {
        public MenuState(StateStack stack, Context context)
            : base(stack, context)
        {
        }
    }
}
