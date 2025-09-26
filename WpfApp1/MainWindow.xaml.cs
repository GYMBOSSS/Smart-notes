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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<Note> notes = new List<Note>();
        private List<NoteTag> tags = new List<NoteTag>();
        public MainWindow()
        {
            InitializeComponent();
            DB_API.LoadNotesFromDB_Async(this);
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
            tags = new List<NoteTag>();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            NoteCreateForm nf = new NoteCreateForm(this);
            nf.Show();
        }

        public void AddNoteToPanel(Note note)
        {
            Border mainBorder = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(5),
                Padding = new Thickness(8),
                CornerRadius = new CornerRadius(6),
                Background = new SolidColorBrush(Color.FromRgb(245, 245, 245))
            };
            TextBlock textBlock = new TextBlock
            {
                Text = note.main,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.Black,
                VerticalAlignment = VerticalAlignment.Center
            };
            mainBorder.MouseLeftButtonDown += (sender, e) =>
            {
                note.ToggleCompleeted();
                if (note.IsCompleeted)
                {
                    textBlock.TextDecorations = TextDecorations.Strikethrough;
                    textBlock.Foreground = Brushes.Gray;
                }
                else
                {
                    textBlock.TextDecorations = null;
                    textBlock.Foreground = Brushes.Black;
                }
                e.Handled = true;
            };
            

            ContextMenu menu = new ContextMenu();
            MenuItem DetailsItem = new MenuItem() { Header = "Посмотреть детали заметки" };
            
            DetailsItem.Click += (sender,e) =>
            {
                MessageBox.Show(note.details, note.main, MessageBoxButton.OK, MessageBoxImage.Information);
            };

            menu.Items.Add(DetailsItem);
            
            mainBorder.ContextMenu = menu;

            Grid contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });


            Border tagMarker = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Width = 20,
                Height = 40,
                Background = new SolidColorBrush(note.tag.tagColor),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 5, 0)
            };

            Grid.SetColumn(textBlock, 0);
            Grid.SetColumn(tagMarker, 1);

            contentGrid.Children.Add(textBlock);
            contentGrid.Children.Add(tagMarker);

            mainBorder.Child = contentGrid;

            FieldForNotes.Children.Add(mainBorder);
        }

        

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            TagCreateForm tagCreateForm = new TagCreateForm(this);
            tagCreateForm.Show();
        }

        public void AddTagToList(NoteTag tag)
        {
            tags.Add(tag);
        }
        public void AddNoteToList(Note note)
        {
            notes.Add(note);
        }


        public void RemoveTag(NoteTag tag) { }

        public List<NoteTag> GetTagList()
        {
            return tags;
        }
       
    }
}
