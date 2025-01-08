namespace FbManagement.Application.Dtos
{
    public class BatchCommentReplyRequest
    {
        public string PostId { get; set; }     // ID of the post containing the comment
        public string CommentId { get; set; } // ID of the comment to reply to
        public string Message { get; set; }   // The reply message
    }
}
