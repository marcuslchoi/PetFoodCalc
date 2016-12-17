using SQLite;

namespace locky2
{
	public interface ISQLiteDb
	{
		SQLiteAsyncConnection GetConnection();
	}
}
