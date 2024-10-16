
using MongoDB.Bson;

namespace MongoDemo;

public static class Samples
{
	public static async Task CreateProductAndGroups()
	{
		var sodaGroup = new ProductGroup()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Soda"
		};
		await Database.Collection<ProductGroup>().InsertOneAsync(sodaGroup);
		var juiceGroup = new ProductGroup()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Juice"
		};
		await Database.Collection<ProductGroup>().InsertOneAsync(juiceGroup);
		var waterGroup = new ProductGroup()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Water"
		};
		await Database.Collection<ProductGroup>().InsertOneAsync(waterGroup);

		var coke = new Product()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Coca_Cola",
			Group = sodaGroup.GetReference(),
			MeasureUnit = "liter"
		};
		await Database.Collection<Product>().InsertOneAsync(coke);
		var sprite = new Product()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Sprite",
			Group = sodaGroup.GetReference(),
			MeasureUnit = "liter",
			MinimalOrder = 5
		};
		await Database.Collection<Product>().InsertOneAsync(sprite);
		var fanta = new Product()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Fanta",
			Group = sodaGroup.GetReference(),
			MeasureUnit = "liter"
		};
		await Database.Collection<Product>().InsertOneAsync(fanta);


		var apple = new Product()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Apple Pure",
			Group = juiceGroup.GetReference(),
			MeasureUnit = "liter"
		};
		await Database.Collection<Product>().InsertOneAsync(apple);
	}

	public static async Task CreateCustomers()
	{
		var tesla = new Customer()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Tesla Corporation",
			Address = new Address() { City = "Austin", Street = "1 Tesla Road", ZipCode = "78725" }
		};
		await Database.Collection<Customer>().InsertOneAsync(tesla);

		var google = new Customer()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Alphabet Inc",
			Address = new Address() { City = "Mountain View", Street = "1600 Amphitheatre Parkway", ZipCode = "94043" }
		};
		await Database.Collection<Customer>().InsertOneAsync(google);
	}
}