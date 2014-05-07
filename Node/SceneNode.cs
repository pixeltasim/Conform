using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;
using SFML.Time;
using Conform.Management;
using Conform.Input;
using Conform.State;
using Conform.Utility;

namespace Conform.Node
{
    enum Layer { Background, Air, LayerCount }; //copy in World.cs
    class SceneNode : Transformable, Drawable
    {
        List<SceneNode> mChildren;
        SceneNode mParent;

        private void updateChildren(Time dt, CommandQueue queue)
        {
            foreach (SceneNode child in mChildren) child.update(dt, queue);
        }

        public void onCommand(Command command, Time dt)
        {
            if (command.category == getCategory()) { command.actionfunc(this, dt); }
            foreach (SceneNode child in mChildren) { child.onCommand(command, dt); }
        }
        public void attachChild(SceneNode child) { child.mParent = this; mChildren.Add(child); }

        /// <summary>
        /// Removes a child node from parent node, returns child node, possible issues with how it is detached
        /// </summary>
        /// <param name="node">Node you wish to detach</param>
        /// <returns>Detached node</returns>
        public SceneNode detachChild(ref SceneNode node)
        {
            foreach (SceneNode nodel in mChildren)
            {
                if (nodel == node)
                {
                    nodel.mParent = null;
                    mChildren.Remove(nodel);
                    return nodel;

                }

            }
            return null;


        }

        public void update(Time dt, CommandQueue queue)
        {
            updateCurrent(dt, queue);
            updateChildren(dt, queue);
        }

        public Transform getWorldTranform()
        {
            Transform transform = Transform.Identity;
            for (SceneNode node = this; node != null; node = node.mParent)
            {
                transform = node.Transform * transform;
            }
            return transform;
        }

        public Vector2f getWorldPosition()
        {
            return getWorldTranform() * new Vector2f(0, 0); //originally in the book the method is return getWorldTranform() * Vector2f();

        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= this.Transform;
            DrawCurrent(target, states);

            foreach (SceneNode itr in mChildren)
            {
                itr.Draw(target, states);
            }

        }


        public virtual void DrawCurrent(RenderTarget target, RenderStates states) { }
        public virtual void updateCurrent(Time dt, CommandQueue queue) { }
        public virtual NodeCategory getCategory() { return NodeCategory.Scene; }

        //constructor
        public SceneNode() { mParent = null; mChildren = new List<SceneNode>(); this.Position = new Vector2f(0, 0); }

    }
}
