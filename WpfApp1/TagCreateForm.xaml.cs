using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для TagCreateForm.xaml
    /// </summary>
    public partial class TagCreateForm : Window
    {
        private MainWindow _mainWindow;
        public TagCreateForm(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private void CreateTagButton_Click(object sender, RoutedEventArgs e)
        {
            NoteTag newTag = new NoteTag(int.Parse(TagPriority.Text), TagName.Text, SetColor.SelectedColor.Value);
            _mainWindow.AddTagToList(newTag);
            DB_API.LoadTagToDB(newTag);

            this.Close();
        }
    }
}
