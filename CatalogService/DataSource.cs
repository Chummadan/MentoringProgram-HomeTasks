using CatalogService.Models;

namespace CatalogService
{
	public class DataSource
	{
		public List<Category> Categories { get; set; }
		public List<Item> Items { get; set; }

		public DataSource() => GenerateInitialData();

		void GenerateInitialData()
		{
			var categoryId = Guid.NewGuid();

			Categories = new List<Category>
			{
				new Category("Fruits")
			};

			Items = new List<Item>
			{
				new Item("Apple", categoryId),
				new Item("Pineapple", categoryId)
			};
		}
	}
}
