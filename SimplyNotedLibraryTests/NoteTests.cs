using FluentAssertions;
using SimplyNotedLibrary;
using SimplyNotedLibrary.Models;

namespace SimplyNotedLibraryTests
{
    [TestClass]
    public sealed class NoteTests
    {
        [TestMethod]
        public void Create_new_note_the_first_time_and_return_1_as_id()
        {
            //Arrange
            Notes Notes = new();

            // Act
            int id = Notes.AddNote();

            // Assert
            id.Should().Be(1);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(53)]
        [DataRow(99)]
        public void Create_new_note_for_given_times_and_return_correct_id_and_find_correct_number_of_notes(int notesToAdd)
        {
            //Arrange
            Notes Notes = new();
            int lastId = 0;

            // Act
            for (int i = 0; i < notesToAdd; i++)
                lastId = Notes.AddNote();

            // Assert
            lastId.Should().Be(notesToAdd);
            Notes.CurrentNotes.Count.Should().Be(notesToAdd);
        }

        [TestMethod]
        public void Try_to_get_note_with_invalid_id_and_get_exception()
        {
            //Arrange
            Notes Notes = new();

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                NoteModel note = Notes.GetNote(1);
            });
        }

        [TestMethod]
        public void Create_new_note_and_get_it_and_correct_fields()
        {
            //Arrange
            Notes Notes = new();
            int id = Notes.AddNote();

            // Act
            NoteModel note = Notes.GetNote(id);

            // Assert
            note.Id.Should().Be(id);
            note.Title.Should().Be(string.Empty);
            note.Content.Should().Be(string.Empty);
            note.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(30));
            note.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(30));
        }

        [TestMethod]
        public void Try_to_update_not_existing_note_and_get_exception()
        {
            //Arrange
            Notes Notes = new();
            NoteModel noteToUpdate = new(1);

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                Notes.UpdateNote(noteToUpdate);
            });
        }

        [TestMethod]
        public void Update_note_and_get_it_and_correct_fields()
        {
            //Arrange
            Notes Notes = new();
            int id = Notes.AddNote();
            NoteModel note = Notes.GetNote(id);
            string title = "New Title";
            string content = "New Content";
            Thread.Sleep(2); // Ensure UpdatedAt is different from CreatedAt

            // Act
            note.Title = title;
            note.Content = content;
            Notes.UpdateNote(note);
            note = Notes.GetNote(note.Id);

            // Assert
            note.Id.Should().Be(id);
            note.Title.Should().Be(title);
            note.Content.Should().Be(content);
            note.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(30));

            int millisecondsDifference = (int)(note.UpdatedAt - note.CreatedAt).TotalMilliseconds;
            millisecondsDifference.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void Try_to_delete_not_existing_note_and_get_exception()
        {
            //Arrange
            Notes Notes = new();

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                Notes.DeleteNote(1);
            });
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(53, 22)]
        [DataRow(99, 45)]
        public void Delete_existing_notes_and_find_correct_number_of_notes(int numberOfNotesToAdd, int numberOfNotesToDelete)
        {
            //Arrange
            Notes Notes = new();

            // Act
            for (int i = 0; i < numberOfNotesToAdd; i++)
                _ = Notes.AddNote();

            for (int i = 0; i < numberOfNotesToDelete; i++)
                Notes.DeleteNote(Notes.CurrentNotes.First().Id);

            // Assert
            Notes.CurrentNotes.Count.Should().Be(numberOfNotesToAdd - numberOfNotesToDelete);
        }
    }
}