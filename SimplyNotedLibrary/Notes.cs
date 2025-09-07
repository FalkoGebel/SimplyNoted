using SimplyNotedLibrary.Models;

namespace SimplyNotedLibrary
{
    public class Notes
    {
        public List<NoteModel> _notes = [];

        public List<NoteModel> CurrentNotes => _notes;

        public static Notes LoadFromFile(string filePath)
        {
            NotesModel notesModel = FileHelpers.ReadFromJsonFile<NotesModel>(filePath);

            return new Notes()
            {
                _notes = [.. notesModel.Notes]
            };
        }

        /// <summary>
        /// Add a new empty note to the collection and return its id.
        /// </summary>
        /// <returns>Id of the new note.</returns>
        public int AddNote()
        {
            var latestNote = _notes.OrderBy(n => n.Id).LastOrDefault();
            int id = latestNote == null ? 1 : latestNote.Id + 1;

            _notes.Add(new NoteModel(id));

            return id;
        }

        /// <summary>
        /// Delete the note with the given id from the collection.
        /// </summary>
        /// <param name="id">The id of the note to delete.</param>
        public void DeleteNote(int id)
        {
            var existingNote = GetNote(id);
            _notes.Remove(existingNote);
        }

        /// <summary>
        /// Get a note by its id.
        /// </summary>
        /// <param name="id">Id of the note to return.</param>
        /// <returns>Note for the given id.</returns>
        /// <exception cref="ArgumentException">Thrown, if the given id is not found in the collection.</exception>
        public NoteModel GetNote(int id)
            => _notes.Where(n => n.Id == id).FirstOrDefault()
                ?? throw new ArgumentException($"Note with id \"{id}\" does not exist.");

        public void SaveToFile(string filePath)
        {
            FileHelpers.WriteToJsonFile(filePath, new NotesModel() { Notes = [.. _notes] });
        }

        /// <summary>
        /// Update an existing note in the collection.
        /// </summary>
        /// <param name="note">The note to update in the collection.</param>
        public void UpdateNote(NoteModel note)
        {
            DeleteNote(note.Id);
            note.UpdatedAt = DateTime.Now;
            _notes.Add(note);
        }
    }
}