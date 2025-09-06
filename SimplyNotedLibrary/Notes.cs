using SimplyNotedLibrary.Models;

namespace SimplyNotedLibrary
{
    public class Notes
    {
        public List<NoteModel> _notes = [];

        public int AddNote()
        {
            var latestNote = _notes.OrderBy(n => n.Id).LastOrDefault();
            int id = latestNote == null ? 1 : latestNote.Id + 1;

            _notes.Add(new NoteModel(id));

            return id;
        }

        public NoteModel GetNote(int id)
            => _notes.Where(n => n.Id == id).FirstOrDefault()
                ?? throw new ArgumentException($"Note with id \"{id}\" does not exist.");

        public void UpdateNote(NoteModel note)
        {
            var existingNote = GetNote(note.Id);

            _notes.Remove(existingNote);
            _notes.Add(note);
        }
    }
}