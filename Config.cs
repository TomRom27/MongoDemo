
public static class Config
{

	public static DatabaseOptions Database { get; private set; }

	static Config()
	{
		Database = new()
		{
			Url = "mongodb://localhost:27017/trdemo?ssl=false&authSource=admin",
			LogQueries = true
		};
	}
}

public class DatabaseOptions
{
	public const string SectionName = "Database";
	public string Url { get; set; }
	public bool LogQueries { get; set; }
}