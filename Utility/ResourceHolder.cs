using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;

namespace Conform.Utility
{
    enum TextureID { Avenger, Raptor, Eagle, Desert, TitleScreen, ButtonNormal, ButtonSelected, ButtonPressed, Bullet , Missile, None }
    enum FontID { Main,Title }
    class ResourceHolder<Resource, Identifier>
    {



        Dictionary<Identifier, Resource> mResMap;

        public void remove(Identifier id)
        {
            mResMap.Remove(id);
        }

        public void load<Parameter>(Identifier id, String filename, ref Parameter param)
        {
            try
            {
                Resource resource;
                resource = (Resource)Activator.CreateInstance(typeof(Resource), new object[] { filename, param });
                mResMap.Add(id, resource);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured of type "
                + e.GetType()
                + " error: "
                + e.Message
                + ": This error occurred in the load function of a "
                + filename
                + " Holder");
            }
        }

        public void load(Identifier id, String filename)
        {
            try
            {
                Resource resource;
                resource = (Resource)Activator.CreateInstance(typeof(Resource), new object[] { filename });
                mResMap.Add(id, resource);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured of type "
                + e.GetType()
                + " error: "
                + e.Message
                + ": This error occurred in the load function of a "
                + filename
                + " Holder");
            }
        }

        public Resource get(Identifier id)
        {
            if (mResMap.ContainsKey(id))
            {
                Resource found = mResMap[id];
                return found;
            }
            else { return default(Resource); } //IT RETURNS FALSE IF FAILURE TO FIND
        }

        //Constructor
        public ResourceHolder() { mResMap = new Dictionary<Identifier, Resource>(); }


    }
}
