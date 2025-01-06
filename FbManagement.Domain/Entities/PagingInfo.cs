namespace FbManagement.Domain.Entities
{
    public class PagingInfo
    {
        public Cursor Cursors { get; set; }
        public string Previous { get; set; } // Optional: For backward compatibility with traditional pagination.
        public string Next { get; set; }
    }
}
