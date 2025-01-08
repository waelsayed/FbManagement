namespace FbManagement.Domain.Entities
{
    public class Comment
    {
        public string id { get; set; }
        public string message { get; set; }
        public from from { get; set; } // user
        public string created_time { get; set; }
    }
}
