namespace CatalogService.Models
{
	public class Category
	{
		public Guid Id { get; private set; }
		public string Name { get; set; }

		public Category(string name)
		{
			Id = Guid.NewGuid();
			Name = name;
		}
	}
}
