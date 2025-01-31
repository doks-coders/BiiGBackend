using BiiGBackend.Models.ReqResponses;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.SharedModels;
using static BiiGBackend.ApplicationCore.Services.StaticItemsService;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
    public interface IStaticItemsService
    {
        Task<ResponseModal> CreateBrand(BrandRequest request);
        Task<ResponseModal> UpdateBrand(BrandRequest request);
        Task<ResponseModal> DeleteBrand(Guid Id);
        Task<ResponseModal> CreateCategory(CategoryRequest request);
        Task<ResponseModal> UpdateCategory(CategoryRequest request);
        Task<ResponseModal> DeleteCategory(Guid Id);

        Task<ResponseModal> GetStaticData();
        Task<StaticFields> GetStaticDataFromDatabase();
        Task<ResponseModal> SetStaticData(StaticFields fields);
        Task<ResponseModal> GetAllCategories();
        Task<ResponseModal> GetAllBrands();
        Task<ResponseModal> GetAllCollections();
        Task<ResponseModal> CreateCollection(CollectionReqResponse request);
        Task<ResponseModal> UpdateCollection(CollectionReqResponse request);
        Task<ResponseModal> DeleteCollection(Guid Id);
    }
}
