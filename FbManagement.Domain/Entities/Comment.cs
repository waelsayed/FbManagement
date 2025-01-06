namespace FbManagement.Domain.Entities
{
    public class Comment
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public User From { get; set; }
        public string CreatedTime { get; set; }
    }
}
