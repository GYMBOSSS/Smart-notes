using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;
    
namespace WpfApp1
{
    public class Note
    {
        public int ID { get; set; }
        public string main { get; set; }
        public string details { get; set; }
        public DateTime createDateTime { get; set; }
        public DateTime? makeUpToDateTime { get; set; }
        public bool IsCompleeted { get; set; }

        public NoteTag tag { get; set; }

        //конструкторы
        public Note(string main, string details, DateTime makeUpToDate)
        {
            this.main = main;
            if (details != null)
            {
                this.details = details;
            }
            else
            {
                this.details = "Подробного описания нет";
            }
            this.createDateTime = DateTime.Now;
            this.makeUpToDateTime = makeUpToDate;
            IsCompleeted = false;
        }
        public Note(string main, string details)
        {
            this.main = main;
            this.details = details;
            createDateTime = DateTime.Now;
        }
        public Note(string main)
        {
            this.main = main;
            createDateTime = DateTime.Now;
        }
        public Note() { }

        public void ToggleCompleeted()
        {
            this.IsCompleeted = !this.IsCompleeted;
        }
    }
}
