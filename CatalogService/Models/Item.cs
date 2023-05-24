namespace CatalogService.Models
{
	public class Item
	{
		public Guid Id { get; private set; }
		public string Name { get; set; }
		public Guid CategoryId { get; set; }

		public Item(string name, Guid categoryId)
		{
			Id = Guid.NewGuid();
			Name = name;
			CategoryId = categoryId;
		}
	}
}
