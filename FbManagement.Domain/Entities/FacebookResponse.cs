namespace FbManagement.Domain.Entities
{
    public class FacebookResponse<T>
    {
        public List<T> Data { get; set; }
        public PagingInfo Paging { get; set; }
    }
}
