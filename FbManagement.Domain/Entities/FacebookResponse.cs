namespace FbManagement.Domain.Entities
{
    public class FacebookResponse<T>
    {
        public List<T> data { get; set; }
        public PagingInfo paging { get; set; }
    }
}
