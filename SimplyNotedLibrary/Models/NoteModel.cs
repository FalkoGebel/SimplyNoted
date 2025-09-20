namespace SimplyNotedLibrary.Models
{
    public class NoteModel(int id)
    {
        public int Id { get; } = id;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}