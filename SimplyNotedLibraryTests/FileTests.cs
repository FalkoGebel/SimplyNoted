using FluentAssertions;
using SimplyNotedLibrary;
using SimplyNotedLibrary.Models;

namespace SimplyNotedLibraryTests
{
    [TestClass]
    public sealed class FileTests
    {

        [TestMethod]
        public void Save_notes_to_file_and_load_them_back_and_check_they_are_equal()
        {
            //Arrange
            Notes notes = new();
            int numberOfNotesToAdd = 1000;
            string filePath = Path.GetTempFileName();
            for (int i = 0; i < numberOfNotesToAdd; i++)
            {
                int id = notes.AddNote();
                NoteModel note = notes.GetNote(id);
                note.Title = $"Title {id}";
                note.Content = $"Content {id}";
                notes.UpdateNote(note);
            }

            // Act
            notes.SaveToFile(filePath);
            Notes loadedNotes = Notes.LoadFromFile(filePath);

            // Assert
            loadedNotes.Should().BeEquivalentTo(notes, options => options.AllowingInfiniteRecursion());

            // Cleanup
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}