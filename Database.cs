using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Linq;

namespace MongoDemo;

public class Database
{

	const string DatabaseName = "TRDemo";
	private static Dictionary<Type, string> collectionDefinitions;
	private static IMongoClient mongoClient;

	static Database()
	{
		collectionDefinitions = new();
	}
	public static void Init()
	{
		SetupConventionsAndMappings();
		InitClient();
		DefineCollections();
		EnsureCollections();
	}

	static void SetupConventionsAndMappings()
	{
		BsonSerializer.RegisterSerializer(new ObjectSerializer(t => true));

		// BsonSerializer.RegisterSerializer(
		// 	typeof(decimal),
		// 	new DecimalSerializer(BsonType.Decimal128,
		// 		new(
		// 			true, // allow overflow, return decimal.MinValue or decimal.MaxValue instead
		// 			true // allow truncation
		// 		))
		// );

		ConventionRegistry.Register(
			"IgnoreExtraElements",
			new ConventionPack { new IgnoreExtraElementsConvention(true) },
			_ => true
		);

		ConventionRegistry.Register(
			"EnumStringConvention",
			new ConventionPack { new EnumRepresentationConvention(BsonType.String) },
			_ => true
		);

		ConventionRegistry.Register(
			"camelcase",
			new ConventionPack { new CamelCaseElementNameConvention() },
			_ => true
		);
	}
	static void DefineCollections()
	{
		collectionDefinitions.Add(typeof(Product), "products");
		collectionDefinitions.Add(typeof(ProductGroup), "productGroups");
		collectionDefinitions.Add(typeof(Customer), "customers");
		collectionDefinitions.Add(typeof(Order), "orders");
	}

	static void EnsureCollections()
	{
		var existingCollections = mongoClient.GetDatabase(DatabaseName).ListCollectionNames().ToList();
		foreach (var name in collectionDefinitions.Values)
		{
			if (!existingCollections.Contains(name))
			{
				mongoClient.GetDatabase(DatabaseName).CreateCollectionAsync(name);
			}
		}
	}

	private static void InitClient()
	{
		var settings = MongoClientSettings.FromConnectionString(Config.Database.Url);
		settings.ServerSelectionTimeout = TimeSpan.FromSeconds(3);

		if (Config.Database.LogQueries)
		{
			settings.ClusterConfigurator = cb =>
			{
				cb.Subscribe<CommandStartedEvent>(e =>
				{
					System.Diagnostics.Trace.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
				});
			};
		}

		BsonSerializer.RegisterSerializer(
			typeof(decimal),
			new DecimalSerializer(BsonType.Decimal128,
				new(
					true, // allow overflow, return decimal.MinValue or decimal.MaxValue instead
					true // allow truncation
				))
		);

		mongoClient = new MongoClient(settings);
	}

	public static IMongoCollection<T> Collection<T>()
	{
		if (!collectionDefinitions.TryGetValue(typeof(T), out var name))
		{
			throw new Exception($"typeof(T) is not know collection");
		};
		// the below doesn't read from db (even though if uses client)
		return mongoClient.GetDatabase(DatabaseName).GetCollection<T>(name);
	}

	public static async Task Insert<T>(T o)
	{
		await Collection<T>().InsertOneAsync(o);
	}


	public static async Task DeleteWhereAsync<T>(T o, Expression<Func<T, bool>> where)
	{
		var result = await Collection<T>().DeleteOneAsync<T>(where);
	}

	public static async Task RunInTransactionAsync(Func<Task> callback)
	{
		using var session = mongoClient.StartSession();

		session.StartTransaction();
		await callback();
		await session.CommitTransactionAsync();
	}

	public static async Task DropCollections()
	{
		var existingCollections = mongoClient.GetDatabase(DatabaseName).ListCollectionNames().ToList();
		foreach (var name in collectionDefinitions.Values)
		{

			await mongoClient.GetDatabase(DatabaseName).DropCollectionAsync(name);
		}
	}
}