using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json;
namespace Ejednevnik
{
    public class Note
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Note()
        {
        }
    }

    public class Timetable
    {
        public List<Note> TodayNotes { get; set; }
        public DateTime SelectedDate { get; set; }
        public int SelectedTaskId { get; set; } = -1;
        public List<Note> AllNotes { get; set; }

        public Timetable(DateTime date)
        {
            TodayNotes = Data.LoadNotes(date);
            AllNotes = Data.LoadNotes(default);
            SelectedDate = date;
        }

        public void RefreshNotes()
        {
            TodayNotes = Data.LoadNotes(SelectedDate);
        }

        public void UpdateNotes()
        {
            Data.SaveNotes(AllNotes);
        }

        public void NewNote(string title, string desc, DateTime date)
        {
            Note note = new Note { Id = AllNotes.Count, Title = title, Description = desc, Date = date };
            AllNotes.Add(note);
            Data.SaveNotes(AllNotes);
            RefreshNotes();
        }

        public void EditNote(string title, string desc, DateTime date)
        {
            if (SelectedTaskId != -1)
            {
                Note note = new Note { Id = TodayNotes[SelectedTaskId].Id, Title = title, Description = desc, Date = date };
                DeleteNote(TodayNotes[SelectedTaskId].Id);
                AllNotes.Add(note);
                UpdateNotes();
                RefreshNotes();
                SelectedTaskId = -1;
            }
        }

        public void DeleteNote(int id = -1, int todayId = -1)
        {
            if (todayId != -1)
                id = TodayNotes[todayId].Id;

            List<Note> newNotes = new List<Note>();
            foreach (Note note in AllNotes)
            {
                if (note.Id != id)
                {
                    newNotes.Add(note);
                }
            }

            AllNotes = newNotes;
            RefreshNotes();
            UpdateNotes();
        }
    }

    public class Data
    {
        public static void SaveNotes(List<Note> notes)
        {
            try
            {
                File.WriteAllText("notes.json", JsonConvert.SerializeObject(notes));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении заметок: " + ex.Message);
            }
        }

        public static List<Note> LoadNotes(DateTime date = default)
        {
            List<Note> notes = new List<Note>();
            try
            {
                List<Note> rawNotes = JsonConvert.DeserializeObject<List<Note>>(File.ReadAllText("notes.json"));
                foreach (Note note in rawNotes)
                {
                    if (note.Date == date || date == default)
                        notes.Add(note);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке заметок: " + ex.Message);
                notes = new List<Note>();
                File.WriteAllText("notes.json", JsonConvert.SerializeObject(notes));
            }
            return notes;
        }
    }
}
