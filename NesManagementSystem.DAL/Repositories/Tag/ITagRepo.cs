namespace NewsManagementSystem.DAL.Repositories.Tag;

public interface ITagRepo
{
    Task<List<BusinessObject.Entities.Tag>> GetAllTagsAsync();
    Task<List<BusinessObject.Entities.Tag>> GetTagsByIdsAsync(List<int> tagIds);
}