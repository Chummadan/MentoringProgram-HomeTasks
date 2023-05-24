using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
	[Route("api/")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		const int DefaultPageSize = 5;
		DataSource dataSource;

		public ItemsController(DataSource source)
			=> dataSource = source;

		[HttpGet("[controller]")]
		public ActionResult<List<Item>> Get(Guid? categoryId,
			int pageIndex = 1,
			int pageSize = DefaultPageSize)
		{
			var skipCount = (pageIndex - 1) * pageSize;
			var itemsToTake = dataSource.Items;

			if (categoryId != null && dataSource.Items.Any(i => i.CategoryId == categoryId))
				itemsToTake = itemsToTake.Where(i => i.CategoryId == categoryId).ToList();

			return Ok(itemsToTake.Skip(skipCount).Take(pageSize));
		}

		[HttpPost("Item")]
		public ActionResult Post([FromBody] string itemName, Guid categoryId)
		{
			if (dataSource.Items.Any(i =>
				string.Equals(i.Name, itemName, StringComparison.InvariantCultureIgnoreCase)))
				return BadRequest($"Item {itemName} already exist!");

			if (!dataSource.Categories.Any(c => c.Id == categoryId))
				return BadRequest($"Category with ID: {categoryId} doesn't exist!");

			var newItem = new Item(itemName, categoryId);
			dataSource.Items.Add(newItem);
			return Ok(newItem.Id);
		}

		[HttpPut("Item/{id}")]
		public ActionResult Put(Guid itemId, [FromBody] string itemName, Guid categoryId)
		{
			if (!dataSource.Categories.Any(c => c.Id == categoryId))
				return BadRequest($"Category with ID: {categoryId} doesn't exist!");

			var item = dataSource.Items.FirstOrDefault(i => i.Id == itemId);
			if (item != null)
			{
				item.Name = itemName;
				item.CategoryId = categoryId;
				return Ok(item.Id);
			}
			return BadRequest($"Item with ID: {itemId} doesn't exist!");
		}

		[HttpDelete("Item/{id}")]
		public ActionResult Delete(Guid itemId)
		{
			var item = dataSource.Items.FirstOrDefault(i => i.Id == itemId);
			if (item != null)
			{
				dataSource.Items.Remove(item);
				return Ok();
			}
			return BadRequest($"Item with ID: {itemId} doesn't exist!");
		}
	}
}
