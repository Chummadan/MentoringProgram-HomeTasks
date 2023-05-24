using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
	[Route("api/")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		DataSource dataSource;

		public CategoriesController(DataSource source)
			=> dataSource = source;

		[HttpGet("[controller]")]
		public ActionResult<List<Category>> Get()
		{
			return Ok(dataSource.Categories);
		}

		[HttpPost("Category")]
		public ActionResult Post([FromBody] string categoryName)
		{
			if (!dataSource.Categories.Any(c =>
				string.Equals(c.Name, categoryName, StringComparison.InvariantCultureIgnoreCase)))
			{
				var newCategory = new Category(categoryName);
				dataSource.Categories.Add(newCategory);
				return Ok(newCategory.Id);
			}
			return BadRequest($"Category {categoryName} already exist!");
		}

		[HttpPut("Category/{id}")]
		public ActionResult Put(Guid categoryId, [FromBody] string categoryName)
		{
			var category = dataSource.Categories.FirstOrDefault(c => c.Id == categoryId);
			if (category != null)
			{
				category.Name = categoryName;
				return Ok(category.Id);
			}
			return BadRequest($"Category with ID: {categoryId} doesn't exist!");
		}

		[HttpDelete("Category/{id}")]
		public ActionResult Delete(Guid categoryId)
		{
			var category = dataSource.Categories.FirstOrDefault(c => c.Id == categoryId);
			if (category != null)
			{
				dataSource.Categories.Remove(category);
				return Ok();
			}
			return BadRequest($"Category with ID: {categoryId} doesn't exist!");
		}
	}
}
