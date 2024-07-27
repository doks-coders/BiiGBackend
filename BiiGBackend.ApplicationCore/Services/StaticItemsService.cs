using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities.StaticItems;
using BiiGBackend.Models.Extensions;
using BiiGBackend.Models.ReqResponses;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.SharedModels;

namespace BiiGBackend.ApplicationCore.Services
{

	public class StaticItemsService : IStaticItemsService
	{
		private readonly IUnitOfWork _unitOfWork;

		public StaticItemsService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<ResponseModal> GetAllBrands()
		{
			var brands = await _unitOfWork.Brand.GetItems(u => u.Id != null);

			return ResponseModal.Send(brands);
		}

		public async Task<ResponseModal> CreateBrand(BrandRequest request)
		{
			var brand = new Brand()
			{
				Name = request.BrandName
			};
			await _unitOfWork.Brand.AddItem(brand);

			return ResponseModal.Send(brand);
		}

		public async Task<ResponseModal> UpdateBrand(BrandRequest request)
		{
			var brand = await _unitOfWork.Brand.GetItem(u => u.Id == request.Id);
			brand.Name = request.BrandName;
			await _unitOfWork.Save();
			return ResponseModal.Send(brand);
		}


		public async Task<ResponseModal> DeleteBrand(Guid Id)
		{
			var brand = await _unitOfWork.Brand.GetItem(u => u.Id == Id);
			await _unitOfWork.Brand.DeleteItem(brand);
			await _unitOfWork.Save();
			return ResponseModal.Send(brand);
		}


		public async Task<ResponseModal> GetAllCategories()
		{
			var categories = await _unitOfWork.Category.GetItems(u => u.Id != null);

			return ResponseModal.Send(categories);
		}


		public async Task<ResponseModal> CreateCategory(CategoryRequest request)
		{
			var category = new Category()
			{
				Name = request.CategoryName
			};
			await _unitOfWork.Category.AddItem(category);

			return ResponseModal.Send(category);
		}

		public async Task<ResponseModal> UpdateCategory(CategoryRequest request)
		{
			var category = await _unitOfWork.Category.GetItem(u => u.Id == request.Id);
			category.Name = request.CategoryName;
			await _unitOfWork.Save();
			return ResponseModal.Send(category);
		}


		public async Task<ResponseModal> DeleteCategory(Guid Id)
		{
			var category = await _unitOfWork.Category.GetItem(u => u.Id == Id);
			await _unitOfWork.Category.DeleteItem(category);
			return ResponseModal.Send(category);
		}


		public async Task<ResponseModal> GetAllCollections()
		{
			var categories = await _unitOfWork.Collections.GetItems(u => u.Id != null);

			return ResponseModal.Send(categories);
		}


		public async Task<ResponseModal> CreateCollection(CollectionReqResponse request)
		{

			var collection = request.ConvertFromRequest();
			await _unitOfWork.Collections.AddItem(collection);
			return ResponseModal.Send(collection);
		}

		public async Task<ResponseModal> UpdateCollection(CollectionReqResponse request)
		{
			var collection = await _unitOfWork.Collections.GetItem(u => u.Id == request.Id);
			collection.CollectionCaption = request.CollectionCaption;
			collection.CollectionName = request.CollectionName;
			collection.CollectionUrl = request.CollectionUrl;
			collection.CollectionImageUrl = request.CollectionImageUrl;
			await _unitOfWork.Save();
			return ResponseModal.Send(collection);
		}


		public async Task<ResponseModal> DeleteCollection(Guid Id)
		{
			var collection = await _unitOfWork.Collections.GetItem(u => u.Id == Id);
			await _unitOfWork.Collections.DeleteItem(collection);
			return ResponseModal.Send(collection);
		}
	}
}
