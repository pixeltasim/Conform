using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conform.Utility
{
    static class UnitConverter
    {
        public static float toSimScale(float pixelscale) { return pixelscale / 64; }
        public static float toPixScale(float simscale) { return simscale * 64; }
    }
}
