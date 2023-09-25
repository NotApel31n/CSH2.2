using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ejednevnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Timetable Timetable;
        public static DateTime SelectedDate = DateTime.Today;

        public MainWindow()
        {
            InitializeComponent();
            Timetable = new Timetable(SelectedDate);
            UpdateTaskList();
            dateContainer.SelectedDate = SelectedDate;
        }

        public void UpdateTaskList()
        {
            SelectedDate = Timetable.SelectedDate;
            TaskContainer.Items.Clear();
            Timetable.RefreshNotes();
            foreach (Note note in Timetable.TodayNotes)
            {
                TaskContainer.Items.Add(note.Title);
            }
            titleBox.Text = "";
            descBox.Text = "";
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            string title = titleBox.Text;
            string desc = descBox.Text;
            Timetable.NewNote(title, desc, SelectedDate);
            UpdateTaskList();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            string title = titleBox.Text;
            string desc = descBox.Text;
            Timetable.EditNote(title, desc, SelectedDate);
            UpdateTaskList();
        }

        private void ChangeSelect(object sender, SelectionChangedEventArgs e)
        {
            if (TaskContainer.SelectedIndex != -1)
            {
                Timetable.SelectedTaskId = TaskContainer.SelectedIndex;
                if (Timetable.SelectedTaskId < Timetable.TodayNotes.Count)
                {
                    Note selectedNote = Timetable.TodayNotes[Timetable.SelectedTaskId];
                    titleBox.Text = selectedNote.Title;
                    descBox.Text = selectedNote.Description;
                }
            }
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dateContainer.SelectedDate != null)
                {
                    SelectedDate = dateContainer.SelectedDate.Value;
                    UpdateTaskList();
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки
                Console.WriteLine("Ошибка при изменении даты: " + ex.Message);
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Timetable.DeleteNote(todayId: Timetable.SelectedTaskId);
            UpdateTaskList();
        }
    }

}
