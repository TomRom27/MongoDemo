using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDemo;

public class BasicDemo
{
	public BasicDemo()
	{
		Database.Init();

	}

	[Fact]
	public async void RecreatedDefaults()
	{
		await Database.DropCollections();
		await Samples.CreateProductAndGroups();
	}

	[Fact]
	public async void OnDefaults_AddProductsForExistsCheck()
	{
		// first run Recreate then this test
		//
		var group = Database.Collection<ProductGroup>().AsQueryable()
			.Where(g => g.Name == "Water")
			.SingleOrDefault();

		var product = new Product()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Water Base",
			Group = group.GetReference(),
			MeasureUnit = "liter"
		};
		await Database.Collection<Product>().InsertOneAsync(product);
		var product2 = new Product()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Water Standard",
			Group = group.GetReference(),
			MeasureUnit = "liter",
			// IsCertified = true
		};
		await Database.Collection<Product>().InsertOneAsync(product2);
	}
}