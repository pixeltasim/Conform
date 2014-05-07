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
namespace Conform.Management
{
    class GameApplication
    {
        public GameApplication()
        {
            mWindow = new RenderWindow(new VideoMode(640, 480), "SFML App", Styles.Close);
            mWindow.SetKeyRepeatEnabled(false);

            mPlayer = new Player();

            mTextures = new ResourceHolder<Texture, TextureID>();


            mFonts = new ResourceHolder<Font, FontID>();
            mFonts.load(FontID.Title, "Assets/KarmaFuture.ttf");

            mStateStack = new StateStack(new Context(mWindow, mTextures, mFonts, mPlayer));
            eventqueue = new Queue<Input.Event>();

            registerStates();
            mStateStack.pushState(StateID.Title);

            mWindow.Closed += onClosed;
            mWindow.GainedFocus += gainedFocus;
            mWindow.LostFocus += lostFocus;
            mWindow.KeyPressed += keyPressed;


        }
        public void run()
        {
            Clock clock = new Clock();
            Time timeSinceLastUpdate = Time.Zero;

            while (mWindow.IsOpen())
            {

                Time elapsedTime = clock.Restart();
                timeSinceLastUpdate += elapsedTime;
                while (timeSinceLastUpdate > TimePerFrame)
                {
                    timeSinceLastUpdate -= TimePerFrame;
                    processInput();
                    update(TimePerFrame);
                    if (mStateStack.isEmpty())
                        mWindow.Close();
                }

                render();

            }
        }

        private void processInput()
        {

            mWindow.DispatchEvents();
            while (eventqueue.Count != 0)
            {

                mStateStack.handleEvent(eventqueue.First());
                eventqueue.Dequeue();
            }

        }

        #region event methods

        private void onClosed(object sender, EventArgs arg)
        {
            mWindow.Close();
        }
        private void gainedFocus(object sender, EventArgs arg)
        {

        }
        private void lostFocus(object sender, EventArgs arg)
        {
            
        }
        private void keyPressed(object sender, KeyEventArgs arg)
        {
            eventqueue.Enqueue(new Input.Event(arg, EventType.KeyPressed));
        }

        #endregion

        private void update(Time dt) { mStateStack.update(dt); }
        private void render()
        {
            mWindow.Clear(Color.White);

            mStateStack.draw();

            mWindow.SetView(mWindow.DefaultView);

            mWindow.Display();
        }

        private void registerStates()
        {

            
        }

        private static Time TimePerFrame = Time.FromMilliseconds(17);

        private RenderWindow mWindow;
        private ResourceHolder<Texture, TextureID> mTextures;
        private ResourceHolder<Font, FontID> mFonts;
        Player mPlayer;

        private Queue<Input.Event> eventqueue;
        
        StateStack mStateStack;



    }
}
