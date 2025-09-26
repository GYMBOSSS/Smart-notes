using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;

namespace WpfApp1
{
    public class NoteTag
    {
        public int ID { get; set; }
        public int tagPriority { get; set; }
        public string tagName { get; set; }
        public Color tagColor { get; set; }
        public string tagColorHex { 
            get => tagColor.ToHex(); 
            set => tagColor = ColorConverter.FromHex(value);
        }
        
        //конструкторы
        public NoteTag(int priority, string name, Color color)
        {
            tagPriority = priority;
            tagName = name;
            tagColor = color;
        }
        public NoteTag() { }
    }
}
