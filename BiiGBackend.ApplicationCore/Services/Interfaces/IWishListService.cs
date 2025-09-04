using BiiGBackend.Models.DTOs;
using BiiGBackend.Models.SharedModels;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
    public interface IWishListService
    {
        Task<ResponseModal> AddUserToWishlistAsync(AddUserToWishlistDto dto);
        Task<ResponseModal> CreateWishlistItemAsync(CreateWishlistItemDto dto);
        Task<ResponseModal> DeleteWishlistItemAsync(Guid id);
        Task<ResponseModal> GetAllAsync();
        Task<ResponseModal> GetByIdAsync(Guid id);
    }
}