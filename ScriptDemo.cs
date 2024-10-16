using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDemo;

public class ScriptDemo
{
	public ScriptDemo()
	{
		Database.Init();

	}

	[Fact]
	public async void PrepareCustomers()
	{
		await Database.DropCollections();
		await Samples.CreateProductAndGroups();
		await Samples.CreateCustomers();
	}

	[Fact]
	public async void OnDefaults_UpdateGoogleViaScript()
	{
		// first run Recreate then this test
		//

	}
}