using System;
namespace AntiTelemarketer.DatabaseHelper
{
    public interface ISQLite
    {
        SQLite.SQLiteConnection GetConnection();
    }
}
