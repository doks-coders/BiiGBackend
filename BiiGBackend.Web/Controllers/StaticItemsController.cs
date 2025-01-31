using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.ReqResponses;
using BiiGBackend.Models.Requests;
using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BiiGBackend.ApplicationCore.Services.StaticItemsService;

namespace BiiGBackend.Web.Controllers
{
    public class StaticItemsController : BaseController
    {
        private readonly IStaticItemsService _staticServices;

        public StaticItemsController(IStaticItemsService staticServices)
        {
            _staticServices = staticServices;
        }


        [HttpGet("get-brands")]
        public async Task<ActionResult> GetBrands()
        {
            return await _staticServices.GetAllBrands();
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("create-brand")]
        public async Task<ActionResult> CreateBrand([FromBody] BrandRequest request)
        {
            return await _staticServices.CreateBrand(request);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("update-brand")]
        public async Task<ActionResult> UpdateBrand([FromBody] BrandRequest request)
        {
            return await _staticServices.UpdateBrand(request);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpDelete("delete-brand/{id}")]
        public async Task<ActionResult> DeleteBrand(Guid id)
        {
            return await _staticServices.DeleteBrand(id);
        }

        [HttpGet("get-categories")]
        public async Task<ActionResult> GetCategories()
        {
            return await _staticServices.GetAllCategories();
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("create-category")]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryRequest request)
        {
            return await _staticServices.CreateCategory(request);
        }
        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("update-category")]
        public async Task<ActionResult> UpdateCategory([FromBody] CategoryRequest request)
        {
            return await _staticServices.UpdateCategory(request);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpDelete("delete-category/{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            return await _staticServices.DeleteCategory(id);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("set-static-field")]
        public async Task<ActionResult> SetStaticData([FromBody] StaticFields request)
        {
            return await _staticServices.SetStaticData(request);
        }


        [HttpGet("get-static-field")]
        public async Task<ActionResult> GetStaticData()
        {
            return await _staticServices.GetStaticData();
        }


        [HttpGet("get-collections")]
        public async Task<ActionResult> GetAllCollections()
        {
            return await _staticServices.GetAllCollections();
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("create-collection")]
        public async Task<ActionResult> CreateCollection([FromBody] CollectionReqResponse request)
        {
            return await _staticServices.CreateCollection(request);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("update-collection")]
        public async Task<ActionResult> UpdateCollection([FromBody] CollectionReqResponse request)
        {
            return await _staticServices.UpdateCollection(request);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpDelete("delete-collection/{id}")]
        public async Task<ActionResult> DeleteCollection(Guid id)
        {
            return await _staticServices.DeleteCollection(id);
        }
    }
}
