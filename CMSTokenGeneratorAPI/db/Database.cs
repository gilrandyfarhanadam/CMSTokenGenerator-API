using System.Text.Json;
using MySql.Data.MySqlClient;

namespace DatabaseConnection
{
    public class Database
    {
        // Constructor
        private Database(){
            ReadJSONFIle();
            Connection = new(ConnectionString.ToString());
        }

        // attributes
        private static Database? _instance;
        private MySqlConnectionStringBuilder ConnectionString = new();
        public MySqlConnection Connection { get; private set; }

        // Operations

        // Singleton : Getting instance
        public static Database GetInstance(){
            _instance ??= new Database();
            return _instance;
        }

        public void ReadJSONFIle(){
            string text = File.ReadAllText("../../connection.json");

            using JsonDocument document = JsonDocument.Parse(text);
                JsonElement root = document.RootElement;

                string? Server = root.GetProperty("Server").GetString();
                uint Port = root.GetProperty("Port").GetUInt16();
                string? Password = root.GetProperty("Password").GetString();
                string? DB = root.GetProperty("Database").GetString();

                ConnectionString = new(){
                    Server = Server,
                    Port = Port,
                    UserID = "root",
                    Password = Password,
                    Database = DB
                };
        }

        public bool Connect(){
            try
            {
                Connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}