namespace NewsManagementSystem.BLL.Services.Tag;

public interface ITagService
{
    Task<List<DAL.Entities.Tag>> GetAllTagsAsync();
    Task<List<DAL.Entities.Tag>> GetTagsByIdsAsync(List<int> tagIds);
}