namespace NewsManagementSystem.DAL.Repositories.Tag;
using NewsManagementSystem.DAL.Entities;

public interface ITagRepo
{
    Task<List<Tag>> GetAllTagsAsync();
    Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds);
}