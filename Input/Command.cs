using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conform.Management;
using Conform.Node;
using SFML.Time;
namespace Conform.Input
{
    [Flags]
    enum NodeCategory
    {
        None = 0x0,
        Scene = 0x1
    }
    class Command // is class because value-types causes issues
    {
        public delegate void commandfunc(SceneNode node, Time time);
        public commandfunc actionfunc;
        public NodeCategory category;

        public commandfunc derivedAction(commandfunc func)
        {
            return (node, time) =>
            {
                func(node, time);
            };
        }

    }
    /// <summary>
    /// It's just a queue wrapper for commands, whoop de doo
    /// </summary>
    class CommandQueue
    {
        //public methods
        public void push(Command command) { mQueue.Enqueue(command); }
        public Command pop() { return mQueue.Dequeue(); }
        public bool isEmpty() { if (mQueue.Count == 0) return true; else return false; }
        //private vars
        Queue<Command> mQueue;

        public CommandQueue()
        {
            mQueue = new Queue<Command>();
        }
    }
}
