
using MongoDB.Driver;

namespace MongoDemo;

public class AggregationDemoNet
{
	public AggregationDemoNet()
	{
		Database.Init();

	}

	[Fact]
	public async void Aggregate_Group()
	{
		await Samples.CreateProductAndGroups();

		var result = await Database.Collection<Product>().Aggregate()
			.Group(p => p.Group.Name, g => new { GroupName = g.Key, ProducsCount = g.Count() })
			.ToListAsync();

		Assert.Equal(2, result.Count);
		/*
[
{
$group: {
  _id: "$group.name",
  total: { $sum: 1 }
}
},
{
$project: {
  groupName: "$_id",
  producsCount: "$total",
  _id: 0
}
}
]
		*/
	}

	[Fact]
	public async void Aggregate_Project()
	{
		await Samples.CreateProductAndGroups();

		var sweetFilter = Builders<Product>.Filter.In(p => p.Name, new string[] {
			"Coca Cola", "Apple Pure" });

		var sodaFilter = Builders<Product>.Filter.Eq(p => p.Group.Name, "Soda");

		var filter = Builders<Product>.Filter.And(sweetFilter, sodaFilter);

		var products = await Database.Collection<Product>()
			.Aggregate()
			.Match(filter)
			.Project(
				Builders<Product>.Projection
					.Include(s => s.Id)
					.Include(s => s.Name)
			)
			.As<Product>()
			.ToListAsync();

		Assert.Equal(4, products.Count);
		/*

[
{
$match: {
  $and: [
	{
	  "name": {
		$in: ["Coca Cola", "Apple Pure"]
	  }
	},
	{ "group.name": "Soda" }
  ]
}
},
{
$project: {
  _id: 1,
  name: 1
}
}
]

		*/
	}
}