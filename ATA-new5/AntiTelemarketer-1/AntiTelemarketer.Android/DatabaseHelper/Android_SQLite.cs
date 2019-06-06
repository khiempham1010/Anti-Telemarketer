using AntiTelemarketer.DatabaseHelper;
using AntiTelemarketer.Droid.DatabaseHelper;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(Android_SQLite))]
namespace AntiTelemarketer.Droid.DatabaseHelper
{

    public class Android_SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "PRIMAS_AntiTelemarketer.sqlite";
            var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var path = System.IO.Path.Combine(dbPath, dbName);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}
