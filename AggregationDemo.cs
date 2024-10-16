
/*

var result = await usersCollection.Aggregate()
    .Group(u => u.Status, g => new { Status = g.Key, AvgAge = g.Average(u => u.Age) })
    .ToListAsync();

    Aggregation stages such as $match, $group, $project, and $sort can be combined to perform complex data manipulations.

Example Query in .NET: This full query example demonstrates a filter, projection, and sort together:

csharp

var filter = Builders<User>.Filter.Eq(u => u.Status, "Active");
var projection = Builders<User>.Projection.Include(u => u.Name).Exclude(u => u.Address);
var sort = Builders<User>.Sort.Ascending(u => u.Name);

var activeUsers = await usersCollection.Find(filter)
    .Project<User>(projection)
    .Sort(sort)
    .ToListAsync();

*/
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDemo;

public class AggregationDemo
{
	public AggregationDemo()
	{
		Database.Init();

	}

	[Fact]
	public async void Aggregate_Group()
	{
		var result = await Database.Collection<Product>().Aggregate()
			.Group(p => p.Group.Name, g => new { GroupName = g.Key, ProducsCount = g.Count() })
			.ToListAsync();

		/*
[
{
$group: {
  _id: "$group.name",
  aggregator: { $sum: 1 }
}
},
{
$project: {
  groupName: "$_id",
  producsCount: "$aggregator",
  _id: 0
}
}
]
		*/
	}

	[Fact]
	public async void Aggregate_Project()
	{
		var sweetFilter = Builders<Product>.Filter.In(p => p.Name, new string[] {
			"Coca Cola", "Apple Pure" });

		var sodaFilter = Builders<Product>.Filter.Eq(p => p.Group.Name, "Soda");

		var filter = Builders<Product>.Filter.And(sweetFilter, sodaFilter);

		var vehicles = await Database.Collection<Product>()
			.Aggregate()
			.Match(filter)
			.Project(
				Builders<Product>.Projection
					.Include(s => s.Id)
					.Include(s => s.Name)
			)
			.As<Product>()
			.ToListAsync();


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