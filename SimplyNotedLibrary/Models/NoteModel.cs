namespace SimplyNotedLibrary.Models
{
    public class NoteModel(int id)
    {
        public int Id { get; } = id;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}