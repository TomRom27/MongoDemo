
using MongoDB.Bson;
using MongoDB.Driver;

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
			Name = "Coca Cola",
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

		var orange = new Product()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Name = "Orange",
			Group = juiceGroup.GetReference(),
			MeasureUnit = "liter"
		};
		await Database.Collection<Product>().InsertOneAsync(orange);
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

	public static async Task CreateAlphabetOrders()
	{
		var google = (await Database.Collection<Customer>()
			.FindAsync(Builders<Customer>.Filter.Eq(c => c.Name, "Alphabet Inc")))
			.FirstOrDefault();

		// #1
		var order = new Order()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Customer = google.GetReference(),
			DeliveryAddress = google.Address,
			CreatedAt = new DateTime(2024, 10, 20, 10, 20, 0)  // local
		};
		var sprite = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Sprite")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = sprite.GetReference(),
			Price = 2.34m,
			Quantity = 5
		});
		var fanta = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Fanta")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = fanta.GetReference(),
			Price = 1.99m,
			Quantity = 7
		});
		var coke = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Coca Cola")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = coke.GetReference(),
			Price = 2.49m,
			Quantity = 20
		});
		await Database.Collection<Order>().InsertOneAsync(order);
		// #2
		order = new Order()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Customer = google.GetReference(),
			DeliveryAddress = google.Address,
			CreatedAt = new DateTime(2024, 11, 20, 10, 20, 0)  // local
		};
		var juice = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Apple Pure")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = juice.GetReference(),
			Price = 0.99m,
			Quantity = 15
		});
		var orange = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Orange")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = orange.GetReference(),
			Price = 1.50m,
			Quantity = 10
		}); ;
		await Database.Collection<Order>().InsertOneAsync(order);
	}

	public static async Task CreateTeslaOrders()
	{
		var tesla = (await Database.Collection<Customer>()
			.FindAsync(Builders<Customer>.Filter.Eq(c => c.Name, "Tesla Corporation")))
			.FirstOrDefault();

		// #1
		var order = new Order()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Customer = tesla.GetReference(),
			DeliveryAddress = tesla.Address,
			CreatedAt = new DateTime(2024, 10, 10, 10, 20, 0)  // local
		};
		var orange = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Orange")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = orange.GetReference(),
			Price = 1.50m,
			Quantity = 80
		});
		var juice = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Apple Pure")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = juice.GetReference(),
			Price = 0.99m,
			Quantity = 40
		});

		await Database.Collection<Order>().InsertOneAsync(order);
		// #2
		order = new Order()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Customer = tesla.GetReference(),
			DeliveryAddress = tesla.Address,
			CreatedAt = new DateTime(2024, 11, 10, 10, 20, 0)  // local
		};
		var fanta = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Fanta")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = fanta.GetReference(),
			Price = 1.99m,
			Quantity = 20
		});
		var sprite = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Sprite")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = sprite.GetReference(),
			Price = 2.34m,
			Quantity = 15
		});

		var coke = (await Database.Collection<Product>()
			.FindAsync(Builders<Product>.Filter.Eq(p => p.Name, "Coca Cola")))
			.FirstOrDefault();
		order.AddLine(new OrderLine()
		{
			Product = coke.GetReference(),
			Price = 2.49m,
			Quantity = 15
		});
		await Database.Collection<Order>().InsertOneAsync(order);

	}

}