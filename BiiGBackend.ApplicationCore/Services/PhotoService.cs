using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.SharedModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BiiGBackend.ApplicationCore.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly Cloudinary cloudinary;
		public PhotoService(IOptions<CloudinarySettings> config)
		{
			Account acc = new Account()
			{
				Cloud = config.Value.CloudName,
				ApiKey = config.Value.ApiKey,
				ApiSecret = config.Value.ApiSecret,
			};
			cloudinary = new Cloudinary(acc);
		}
		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
		{
			var uploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				//create file stream
				using (var stream = file.OpenReadStream()) //we use "using" so it can be disposed earlier
				{
					ImageUploadParams uploadParams = new ImageUploadParams()
					{
						File = new FileDescription(file.Name, stream),
						//Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
						Folder = "biig-backend"
					};

					uploadResult = await cloudinary.UploadAsync(uploadParams);
				};
			}
			return uploadResult;

		}

		public async Task<DeletionResult> DeletePhotoAsync(string publicId)
		{
			DeletionResult deletionResult = new DeletionResult();
			DeletionParams deleteParams = new DeletionParams(publicId);

			deletionResult = await cloudinary.DestroyAsync(deleteParams);
			return deletionResult;

		}
	}
}
