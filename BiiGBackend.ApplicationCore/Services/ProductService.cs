using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities.Product;
using BiiGBackend.Models.Extensions;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.Responses;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.SharedModels.Enums;
using BiiGBackend.StaticDefinitions.Constants;
using CloudinaryDotNet.Actions;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BiiGBackend.ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _db;

        public ProductService(IUnitOfWork unitOfWork, IPhotoService photoService, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _db = db;
        }

        public async Task<ResponseModal> FilterProducts(FilterProductsPaginationRequest request)
        {
            var predicate = PredicateBuilder.New<ProductItem>(true);

            predicate = predicate.And(u => u.ProductDetailsUploaded == true && u.isDeleted == false);
            if (!string.IsNullOrEmpty(request.Brands))
            {
                predicate = predicate.And(u => request.Brands.Contains(u.ProductBrand.Name));
            }

            if (!string.IsNullOrEmpty(request.Size))
            {
                var sizes = request.Size.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var size in sizes)
                {
                    string temp = size; // Prevent closure issue
                    predicate = predicate.And(u => u.ProductSizes.Contains(temp));
                }
            }

            if (!string.IsNullOrEmpty(request.Category) && request.Category != "All")
            {
                predicate = predicate.And(u => u.ProductCategory.Name == request.Category);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                predicate = predicate.And(u => u.ProductName.ToLower().Contains(request.Name.ToLower()));
            }

            if (request.Category == "All")
            {
                predicate = predicate.And(u => u.Id != null);
            }




            var paginationResponse = await _unitOfWork.Product.GetPaginationItems(request, predicate, includeProperties: "ProductPhotos,ProductBrand,ProductCategory");

            var productItem = paginationResponse.Items as List<ProductItem>;
            var response = productItem.GetProductResponse();
            paginationResponse.Items = response;

            return ResponseModal.Send(paginationResponse);
        }

        public async Task<ResponseModal> GetAllProducts()
        {
            var products = await _unitOfWork.Product.GetItems(u => u.ProductDetailsUploaded && u.isDeleted == false, includeProperties: "ProductPhotos,ProductBrand,ProductCategory");

            var response = products.GetProductResponse();

            return ResponseModal.Send(response);
        }


        public async Task<ResponseModal> GetProduct(Guid id, ClaimsPrincipal user)
        {
            var product = await _unitOfWork.Product.GetItems(u => u.Id == id, includeProperties: "ProductPhotos,ProductBrand,ProductCategory");

            var response = product.GetExtendedProductResponse();

            return ResponseModal.Send(response.FirstOrDefault());
        }




        public async Task<ResponseModal> GetRecentlyAddedProducts()
        {
            var product = await _unitOfWork.Product.GetItems(u => u.isRecentlyAdded == true && u.isDeleted == false, includeProperties: "ProductPhotos,ProductBrand,ProductCategory");
            var response = product.GetProductResponse();
            return ResponseModal.Send(response);
        }



        public async Task<ResponseModal> GetFeaturedProducts()
        {
            var product = await _unitOfWork.Product.GetItems(u => u.isFeatured == true && u.isDeleted == false, includeProperties: "ProductPhotos,ProductBrand,ProductCategory");
            var response = product.GetProductResponse();
            return ResponseModal.Send(response);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> UpdateProductProps(UpdateProductPropRequest request)
        {
            var product = await _unitOfWork.Product.GetItem(u => u.Id == request.ProductId);

            if (request.Field == "isFeatured")
            {
                product.isFeatured = request.Value;
            }
            if (request.Field == "isRecentlyAdded")
            {
                product.isRecentlyAdded = request.Value;
            }

            await _unitOfWork.Save();
            return ResponseModal.Send(product);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> IntialiseProduct()
        {
            var product = new ProductItem();
            await _unitOfWork.Product.AddItem(product);
            return ResponseModal.Send(product.Id);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> UpdateProduct(ProductRequest request, Guid productId)
        {
            var product = await _unitOfWork.Product.GetItem(u => u.Id == productId && u.isDeleted == false);

            if (product == null)
            {
                throw new CustomException(ErrorCodes.ProductDeleted);
            }

            product.IsDiscounted = request.IsDiscounted;
            product.ProductBrandId = request.ProductBrandId;
            product.ProductCategoryId = request.ProductCategoryId;
            product.ProductRealPrice = request.ProductRealPrice;
            product.ProductDiscountPercent = request.ProductDiscountPercent;
            product.ProductDescription = request.ProductDescription;
            product.ProductSizes = request.ProductSizes;
            product.IsDiscounted = request.IsDiscounted;
            product.ProductName = request.ProductName;
            product.ProductStockAmount = request.ProductStockAmount;
            product.ProductDetailsUploaded = true;
            await _unitOfWork.Save();
            return ResponseModal.Send("Updated Successfully");
        }


        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> DeleteProduct(Guid productId)
        {
            await _photoService.DeleteProductImages(productId);
            if (await _unitOfWork.Product.DeleteOne(productId))
            {
                return ResponseModal.Send("Deleted Successfully");
            }
            return ResponseModal.Send("Did not Delete Successfully");

        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> UploadImage(IFormFile file, Guid Id)
        {
            ImageUploadResult res = await _photoService.AddPhotoAsync(file);

            if (res.Error != null) throw new CustomException(res.Error.Message);

            Photo photo = new Photo()
            {
                Url = res.SecureUrl.AbsoluteUri,
                PublicId = res.PublicId,
            };

            var product = await _unitOfWork.Product.GetItem(u => u.Id == Id, includeProperties: "ProductPhotos");
            if (product.ProductPhotos.Count() == 0)
            {
                photo.IsMain = true;
            }
            product.ProductPhotos.Add(photo);
            await _unitOfWork.Save();

            var response = new PhotoResponse()
            {
                Id = photo.Id,
                IsMain = photo.IsMain,
                ProductId = photo.ProductId,
                Url = photo.Url
            };

            return ResponseModal.Send(response);

        }


        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> DeleteImage(Guid Id)
        {
            var photo = await _db.Photos.Where(u => u.Id == Id).FirstOrDefaultAsync();

            DeletionResult res = await _photoService.DeletePhotoAsync(photo.PublicId);

            if (res.Error != null) throw new CustomException(res.Error.Message);

            _db.Photos.Remove(photo);

            await _unitOfWork.Save();

            return ResponseModal.Send(photo.Id);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> SetMainImage(Guid Id)
        {
            var photo = await _db.Photos.Where(u => u.Id == Id).FirstOrDefaultAsync();
            var productPhotos = await _db.Photos.Where(u => u.ProductId == photo.ProductId).ToListAsync();
            if (productPhotos.Count() > 0)
            {
                productPhotos.ForEach(u =>
                {
                    if (u.Id == Id)
                    {
                        u.IsMain = true;
                    }
                    else
                    {
                        u.IsMain = false;
                    }
                });
            }
            await _unitOfWork.Save();

            return ResponseModal.Send(photo.Id);

        }


    }
}
