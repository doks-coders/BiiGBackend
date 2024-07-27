using BiiGBackend.Models.Requests;
using BiiGBackend.Models.SharedModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
	public interface IProductService
	{
		Task<ResponseModal> GetAllProducts();
		Task<ResponseModal> UploadImage(IFormFile file, Guid Id);
		Task<ResponseModal> IntialiseProduct();
		Task<ResponseModal> DeleteProduct(Guid productId);
		Task<ResponseModal> UpdateProduct(ProductRequest request, Guid productId);
		Task<ResponseModal> GetProduct(Guid id, ClaimsPrincipal user);
		Task<ResponseModal> FilterProducts(FilterProductsPaginationRequest request);
		Task<ResponseModal> DeleteImage(Guid Id);
		Task<ResponseModal> SetMainImage(Guid Id);
		Task<ResponseModal> GetRecentlyAddedProducts();
		Task<ResponseModal> GetFeaturedProducts();
		Task<ResponseModal> UpdateProductProps(UpdateProductPropRequest request);
	}
}
