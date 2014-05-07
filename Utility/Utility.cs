using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
namespace Conform.Utility
{
    static class Utility
    {

        public static void centerOrigin(this Text text)
        {
            FloatRect bounds = text.GetLocalBounds();
            text.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
        }
        public static void centerOrigin(this Sprite sprite)
        {
            FloatRect bounds = sprite.GetLocalBounds();
            sprite.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
        }
        public static void centerOrigin(this RectangleShape shape)
        {
            FloatRect bounds = shape.GetLocalBounds();
            shape.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
        }
        public static Vector2f getCenter(this Window window)
        {
            return new Vector2f(window.Size.X / 2, window.Size.Y / 2);
        }
        public static Vector2f getCenter(this View view)
        {
            return view.Center;
        }
        public static float length(Vector2f vector)
        {
	        return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2f unitVector(Vector2f vector)
        {
            if (vector.X != 0 && vector.Y != 0)
                return vector / length(vector);
            else return new Vector2f(0, 0);
        }
        public static IntRect getRect(this View view)
        {
            IntRect bounds = new IntRect();
            bounds.Left = (int)view.Center.X - (int)view.Size.X / 2;
            bounds.Top = (int)view.Center.Y - (int)view.Size.Y / 2;
            bounds.Height = (int)view.Size.Y;
            bounds.Width = (int)view.Size.X;
            return bounds;

        }
        public static float toRadian(float degree) { return 3.141592653589793238462643383f / 180 * degree; }
        public static float toDegree(float radian) { return 180 / 3.141592653589793238462643383f * radian; }

    }
}
