using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace WpfApp1
{
    internal static class ColorConverter
    {
        public static string ToHex(this Color color){
            return $"#{color.R:X2}{color.G:X}{color.B:X2}";
        }
        public static Color FromHex(string hex) {
            hex = hex.Replace("#", "");
            byte R = Convert.ToByte(hex.Substring(0,2), 16);
            byte G = Convert.ToByte(hex.Substring(2, 2), 16);
            byte B = Convert.ToByte(hex.Substring(4, 2), 16);
        
            Color color = Color.FromRgb(R, G, B);
            return color;
        }
    }
}
