using ConnectionState = System.Data.ConnectionState;

namespace DiscordBotLogging
{
    public class Database
    {
        private bool Running { get; set; } = false;
        private SQLiteConnection Connection { get; set; }
        public void OpenConnection()
        {
            Running = true;
            if (Connection.State is not ConnectionState.Open)
                Connection.Open();
        }
        private void CloseConnection()
        {
            Running = false;
            if (Connection.State is not ConnectionState.Closed)
                Connection.Close();
        }
        public Database()
        {
            Connection = new("Data Source=DiscordBotLogging.db");
            if (!File.Exists("DiscordBotLogging.db"))
                SQLiteConnection.CreateFile("DiscordBotLogging.db");
            CreateTable("feedback", "userID INTEGER, userName TEXT, content TEXT, estimation INTEGER");
            CreateTable("channels", "userID INTEGER, userName TEXT, channelID INTEGER");
        }
        public DataTable? GetTable(string tableName)
        {
            SQLiteCommand cmd = new($"SELECT * FROM {tableName}");
            while (Running) { }
            OpenConnection();
            cmd.Prepare();
            try
            {
                DataTable events = new();
                new SQLiteDataAdapter(cmd).Fill(events);
                CloseConnection();
                return events;
            }
            catch
            {
                CloseConnection();
                return null;
            }
        }
        public void CreateTable(string Name, string Columns)
        {
            SQLiteCommand cmd = new($"CREATE TABLE IF NOT EXISTS {Name} ({Columns})", Connection);
            while (Running) { }
            OpenConnection();
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public void AddFeedBack(string UserName, ulong UserID, string Content, int Estimation)
        {
            SQLiteCommand cmd = new($"INSERT OR REPLACE INTO feedback (userID, userName, content, estimation) VALUES (@userName, @userID, @content, @estimation)", Connection);
            while (Running) { }
            OpenConnection();
            cmd.Parameters.AddWithValue("@userName", UserName);
            cmd.Parameters.AddWithValue("@userID", UserID);
            cmd.Parameters.AddWithValue("@content", Content);
            cmd.Parameters.AddWithValue("@estimation", Estimation);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
    }
}