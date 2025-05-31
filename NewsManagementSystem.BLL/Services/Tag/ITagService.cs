namespace NewsManagementSystem.BLL.Services.Tag;

public interface ITagService
{
    Task<List<BusinessObject.Entities.Tag>> GetAllTagsAsync();
    Task<List<BusinessObject.Entities.Tag>> GetTagsByIdsAsync(List<int> tagIds);
}