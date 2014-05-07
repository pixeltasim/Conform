using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Conform;
using Conform.Utility;
using Conform.Input;
using Conform.State;
using SFML.Time;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using Box2CS;
namespace Conform.State
{
    class TitleState:State
    {
        Text titletext;
        RectangleShape square;
        Clock starttimer;

        World b2World;
        Body b2Body;

        public TitleState(StateStack stack, Context context)
            : base(stack, context)
        {
            starttimer = new Clock();
            starttimer.Restart();

            titletext = new Text();
            titletext.Scale = new Vector2f(2, 2);
            titletext.DisplayedString = "PIXELTASIM";
            titletext.Font = mContext.mFonts.get(FontID.Title);
            titletext.centerOrigin();
            titletext.Position = new Vector2f(mContext.mWindow.Size.X/2, mContext.mWindow.Size.Y / 8);
            titletext.Color = Color.Black;

            square = new RectangleShape(new Vector2f(UnitConverter.toPixScale(0.5f), UnitConverter.toPixScale(0.5f)));
            square.centerOrigin();
            square.FillColor = Color.Red;
            square.OutlineColor = Color.Black;
            square.OutlineThickness = 1;

            b2World = new World(new Vec2(0, 10), false);
            BodyDef b2Def = new BodyDef();
            b2Def.BodyType = BodyType.Dynamic;
            b2Def.Position = new Vec2(UnitConverter.toSimScale(mContext.mWindow.Size.X / 2), UnitConverter.toSimScale(mContext.mWindow.Size.Y / 8*3));
            b2Body = b2World.CreateBody(b2Def);

            PolygonShape dynamicbox = new PolygonShape();
            dynamicbox.SetAsBox(UnitConverter.toSimScale(32), UnitConverter.toSimScale(32));

            FixtureDef fixDef = new FixtureDef();
            fixDef.Shape = dynamicbox;
            fixDef.Density = 1;
            fixDef.Friction = 0.3f;
            b2Body.CreateFixture(fixDef);

            square.Position = new Vector2f(UnitConverter.toPixScale(b2Body.Position.X), UnitConverter.toPixScale(b2Body.Position.Y));
        }

        public override bool draw() 
        {
            //if (starttimer.ElapsedTime >= Time.FromMilliseconds(2000)) mContext.mWindow.Draw(square);
            mContext.mWindow.Draw(titletext);
            mContext.mWindow.Draw(square);
            return true; 
        }


        public override bool update(Time dt) 
        {
            //if (starttimer.ElapsedTime >= Time.FromMilliseconds(3000)) square.Position = new Vector2f(square.Position.X, square.Position.Y + (float)dt.Milliseconds / 2);   
            b2World.Step(dt.Milliseconds/1000f, 9, 2);
            square.Position = new Vector2f(UnitConverter.toPixScale(b2Body.Position.X), UnitConverter.toPixScale(b2Body.Position.Y));
            square.Rotation = b2Body.Angle;
            return true; 
            
        }
        public override bool handleEvent(Input.Event even) 
        { 
            return true; 
        }
        
    }
}
