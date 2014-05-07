using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conform;
using Conform.Utility;
using Conform.Input;
using Conform.State;
using SFML.Time;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
namespace Conform.State
{
    class State
    {

        protected StateStack mStack;
        protected Context mContext;

        public State(StateStack stack, Context context)
        {
            mStack = stack;
            mContext = context;
        }

        #region Utility Functions
        public virtual bool draw() { return true; }
        public virtual bool update(Time dt) { return true; }
        public virtual bool handleEvent(Input.Event even) { return true; }


        protected void requestStackPush(StateID stateID) { mStack.pushState(stateID); }
        protected void requestStackPop() { mStack.popState(); }
        protected void requestStackClear() { mStack.clearStates(); }
        #endregion
    }
}
