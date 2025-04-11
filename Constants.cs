namespace Stargazer
{
    public static class Constants
    {
        public const int PageSize = 4;

        public const double JupiterMass = 1.898e27; // kg
        public const double JupiterRadius = 69911; // km

        public const float GasGiantThreshold = 2000;

        public const string ApiUrl = "https://api.api-ninjas.com/v1/";
        public const string ApiKey = "oLT3O/t5LKiJh5H7ooMDJA==pGJEWqiH2HqTyUJ0";

        public const string DatabaseFilename = "Celestial.db3";

        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
