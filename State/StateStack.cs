using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conform.Management;
using Conform.Utility;
using Conform.Input;
using SFML.Time;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
namespace Conform.State
{
    class Context
    {
        public Context(RenderWindow window,ResourceHolder<Texture,TextureID> textures, ResourceHolder<Font,FontID> fonts, Player player) 
        {
            mWindow = window;
            mTextures = textures;
            mFonts = fonts;
            mPlayer = player;
        }
        public RenderWindow mWindow;
        public ResourceHolder<Texture, TextureID> mTextures;
        public ResourceHolder<Font, FontID> mFonts;
        public Player mPlayer;
    }
    public enum StateID
    {
        None,
        Menu,
        Game,
        Pause,
        Title
    }
    class StateStack
    {
        struct PendingChange
        {
            public Action action;
            public StateID stateID;
        }
        public enum Action { Push, Pop, Clear, };

        public Context mContext;
        private Stack<State> mStack;
        private List<PendingChange> mPendingList;

        public StateStack(Context context) 
        {
            mContext = context;
            mStack = new Stack<State>();
            mPendingList = new List<PendingChange>();

            TitleState state = new TitleState(this,mContext);
        }

        public void handleEvent(Input.Event even)
        {

            IEnumerator<State> itr = mStack.GetEnumerator();
            itr.MoveNext();


            for (int i = mStack.Count; i > 0; i--)
            {
                if (!itr.Current.handleEvent(even)) { applyPendingChanges(); return; }
                itr.MoveNext();
            }
            applyPendingChanges();
        }

        private void applyPendingChanges()
        {

            foreach (PendingChange change in mPendingList)
            {
                switch (change.action)
                {
                    case Action.Pop:
                        Console.WriteLine("Popping  State:" + mStack.Peek());
                        State state = mStack.Pop(); //-1 because the lowest number is actually zero so the count can be off
                        state = null;
                        break;
                    case Action.Push:
                        mStack.Push(createState(change.stateID));
                        Console.WriteLine("Starting State:" + change.stateID);
                        break;
                    case Action.Clear:
                        Console.WriteLine("Clearing States");
                        mStack.Clear();
                        break;
                }
            }
            mPendingList.Clear();
        }

        private State createState(StateID stateID)
        {
            State state = null;
            try
            {
                switch (stateID)
                {
                    case StateID.Menu:
                        state = new MenuState(this, mContext);
                        break;
                    case StateID.Game:
                        state = new GameState(this, mContext);
                        break;
                    case StateID.Pause:
                        state = new PauseState(this, mContext);
                        break;
                    case StateID.Title:
                        state = new TitleState(this, mContext);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " In state creation");
            }

            return state;
        }

        public void update(Time dt)
        {
            foreach (State state in mStack)
            {
                if (!state.update(dt)) { applyPendingChanges(); return; }
            }
            applyPendingChanges();
        }
        
        public void draw()
        {
            foreach (State state in mStack.Reverse())
            {
                if (!state.draw()) { return; }
            }
        }

        #region Utility Functions
        public bool isEmpty() 
        {
            if (mStack.Count == 0) 
            { 
                return true; 
            }
            else{return false;} 
        }
        public void pushState(StateID stateID)
        {
            PendingChange change;
            change.action = Action.Push;
            change.stateID = stateID;
            mPendingList.Add(change);
        }
        public void popState()
        {

            PendingChange change;
            change.action = Action.Pop;
            change.stateID = StateID.None;
            mPendingList.Add(change);
        }
        public void clearStates()
        {

            PendingChange change;
            change.action = Action.Clear;
            change.stateID = StateID.None;
            mPendingList.Add(change);
        }
        #endregion
    }
}
