using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.ReqResponses;
using BiiGBackend.Models.Requests;
using Microsoft.AspNetCore.Mvc;

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

		[HttpPost("create-brand")]
		public async Task<ActionResult> CreateBrand([FromBody] BrandRequest request)
		{
			return await _staticServices.CreateBrand(request);
		}
		[HttpPost("update-brand")]
		public async Task<ActionResult> UpdateBrand([FromBody] BrandRequest request)
		{
			return await _staticServices.UpdateBrand(request);
		}

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

		[HttpPost("create-category")]
		public async Task<ActionResult> CreateCategory([FromBody] CategoryRequest request)
		{
			return await _staticServices.CreateCategory(request);
		}
		[HttpPost("update-category")]
		public async Task<ActionResult> UpdateCategory([FromBody] CategoryRequest request)
		{
			return await _staticServices.UpdateCategory(request);
		}

		[HttpDelete("delete-category/{id}")]
		public async Task<ActionResult> DeleteCategory(Guid id)
		{
			return await _staticServices.DeleteCategory(id);
		}






		[HttpGet("get-collections")]
		public async Task<ActionResult> GetAllCollections()
		{
			return await _staticServices.GetAllCollections();
		}

		[HttpPost("create-collection")]
		public async Task<ActionResult> CreateCollection([FromBody] CollectionReqResponse request)
		{
			return await _staticServices.CreateCollection(request);
		}
		[HttpPost("update-collection")]
		public async Task<ActionResult> UpdateCollection([FromBody] CollectionReqResponse request)
		{
			return await _staticServices.UpdateCollection(request);
		}

		[HttpDelete("delete-collection/{id}")]
		public async Task<ActionResult> DeleteCollection(Guid id)
		{
			return await _staticServices.DeleteCollection(id);
		}
	}
}
