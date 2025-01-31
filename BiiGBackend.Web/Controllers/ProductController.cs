using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.Requests;
using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("initialise-product")]
        public async Task<ActionResult> IntialiseProduct()
        {
            return await _productService.IntialiseProduct();
        }


        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("set-main-image/{id}")]
        public async Task<ActionResult> SetMainImage(Guid id)
        {
            return await _productService.SetMainImage(id);
        }



        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("upload-image/{id}")]
        public async Task<ActionResult> UploadImage(IFormFile file, Guid id)
        {
            return await _productService.UploadImage(file, id);
        }



        [Authorize(Roles = RoleConstants.Admin)]
        [HttpDelete("delete-image/{id}")]
        public async Task<ActionResult> DeleteImage(Guid id)
        {
            return await _productService.DeleteImage(id);
        }



        [Authorize(Roles = RoleConstants.Admin)]
        [HttpDelete("delete-product/{productId}")]
        public async Task<ActionResult> DeleteProduct(Guid productId)
        {
            return await _productService.DeleteProduct(productId);
        }


        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("upsert-product/{id}")]
        public async Task<ActionResult> UpsertProduct([FromBody] ProductRequest request, Guid id)
        {
            return await _productService.UpdateProduct(request, id);
        }

        [HttpGet("filter-products")]
        public async Task<ActionResult> FilterProduct([FromQuery] FilterProductsPaginationRequest request)
        {

            return await _productService.FilterProducts(request);

        }


        [HttpGet("get-all-products")]
        public async Task<ActionResult> GetAllProduct()
        {
            return await _productService.GetAllProducts();
        }

        [HttpGet("get-product/{id}")]
        public async Task<ActionResult> GetProduct(Guid id)
        {
            return await _productService.GetProduct(id, User);
        }


        [HttpGet("get-recently-added")]
        public async Task<ActionResult> GetRecentlyAddedProducts(Guid id)
        {
            return await _productService.GetRecentlyAddedProducts();
        }
        [HttpGet("get-featured-added")]
        public async Task<ActionResult> GetFeaturedProducts(Guid id)
        {
            return await _productService.GetFeaturedProducts();
        }


        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("update-product-prop")]
        public async Task<ActionResult> UpdateProductProps([FromBody] UpdateProductPropRequest request)
        {
            return await _productService.UpdateProductProps(request);
        }


    }
}
