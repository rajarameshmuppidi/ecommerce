namespace EcommercePlatform.Services
{
    public interface ICartService
    {
        Task<bool> DeleteCartItemByIdAsync(Guid id);
    }
}
