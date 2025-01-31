using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.SharedModels;
using System.Data.Entity;

namespace BiiGBackend.ApplicationCore.Services
{
    public class BackUpService
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApplicationDbContext _dbContext;

        public BackUpService(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<ResponseModal> GetBrandData()
        {
            return ResponseModal.Send(await _unitOfWork.Brand.GetItems(u => u.Id != null));
        }
        public async Task<ResponseModal> GetCategoryData()
        {
            return ResponseModal.Send(await _unitOfWork.Category.GetItems(u => u.Id != null));
        }

        public async Task<ResponseModal> GetCollectionData()
        {
            return ResponseModal.Send(await _unitOfWork.Collections.GetItems(u => u.Id != null));
        }
        public async Task<ResponseModal> GetProductData()
        {
            return ResponseModal.Send(await _unitOfWork.Product.GetItems(u => u.Id != null));
        }

        public async Task<ResponseModal> GetPhotoData()
        {
            return ResponseModal.Send(await _dbContext.Photos.ToListAsync());
        }
    }
}
