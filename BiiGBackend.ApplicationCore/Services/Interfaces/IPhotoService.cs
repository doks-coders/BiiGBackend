using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
	public interface IPhotoService
	{
		Task<DeletionResult> DeletePhotoAsync(string publicId);
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task DeleteProductImages(Guid productId);
	}
}
