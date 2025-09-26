using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для NoteCreateForm.xaml
    /// </summary>
    public partial class NoteCreateForm : Window
    {
        private MainWindow mainWindow;
        private List<NoteTag> tags;
        public NoteCreateForm(MainWindow _mainWindow)
        {
            this.mainWindow = _mainWindow;
            InitializeComponent();
            this.tags = _mainWindow.GetTagList();

            foreach (NoteTag tag in tags)
            {
                TagSelector.Items.Add(tag.tagName);
            }
        }

        public void SendNoteButton_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note(Main.Text, Details.Text, DataPick.DisplayDate);
            NoteTag newTag = new NoteTag();
            foreach (NoteTag tag in tags)
            {
                if (tag.tagName == TagSelector.SelectedItem.ToString())
                {
                    newTag = tag;
                    break;
                }
            }
            newNote.tag = newTag;
            mainWindow.AddNoteToPanel(newNote);
            mainWindow.AddNoteToList(newNote);
            DB_API.LoadNoteToDB(newNote);
            this.Close();
        }
    }
}
