using FbManagement.Application.Dtos;

namespace FbManagement.Application.Interfaces
{
    public interface IPostService
    {
        Task<string> CreatePostAsync(CreatePostRequest request);
    }

}
