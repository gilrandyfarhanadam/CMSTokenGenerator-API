using Token_Generator;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace DatabaseConnection
{
    public class InsertToken : IInsert
    {
        public InsertToken(){

        }

        private readonly Database database = Database.GetInstance();

        public bool Single(Object obj) {
            Token token = (Token)obj;
            if (token != null) {
                string q = $"INSERT INTO Token_Admin (token, start, end) VALUES (@token, @start, @end)";

                MySqlCommand cmd = new(q, database.Connection);
                cmd.Parameters.AddWithValue("@token", token.HashedValue);
                cmd.Parameters.AddWithValue("@start", token.Start);
                cmd.Parameters.AddWithValue("@end",token.End);

                int RowsAffected = cmd.ExecuteNonQuery();

                return RowsAffected > 0;
            }
            return false;
        }

        public bool[] Many(Object[] obj){
            bool[] bools = new bool[obj.Length];
            for (int i = 0; i < obj.Length; i++)
            {
                bools[i] = Single(obj[i]);
            }

            return bools;
        }
    }

    public class CheckToken : ICheck
    {
        public CheckToken(){}

        private readonly Database database = Database.GetInstance();

        public Token? Tkn { get; private set; }

        public bool IsExist(string token)
        {
            if (token == null) {
                throw new ArgumentNullException("Token is null");
            }

            // hash the plain token
            byte[] bytes = Token.HashData(token);
            string TokenStr = Token.ToStringValue(bytes);

            // query string
            string q = $"SELECT * FROM Token_Admin WHERE token=@token";

            // execute
            MySqlCommand cmd = new(q, database.Connection);
            cmd.Parameters.AddWithValue("@token", TokenStr);

            MySqlDataReader reader = cmd.ExecuteReader();

            // save the state
            bool state = reader.Read();

            if (state) {
                Tkn = new(token, Convert.ToDateTime(reader.GetString("start")), reader.GetDateTime("end"))
                {
                    HashedValue = TokenStr
                };
            }

            // close reader
            reader.Close();

            return state;
        }

        public bool IsValid(string token) {
            if(!IsExist(token) || token == null) {
                return false;
            }

            // compare if it valid date
            if (Tkn != null) {
                return DateTime.Now >= Tkn.Start && DateTime.Now <= Tkn.End;
            }

            return false;
        }

        public bool IsMatch(string[] tokens) {
            return false;
        }
    }

    public class DeleteToken : IDelete
    {
        public DeleteToken(){}

        private readonly Database database = Database.GetInstance();

        public bool Single(string token) {
            CheckToken ct = new();
            bool IsThere = ct.IsExist(token);
            if(IsThere && ct.Tkn != null) {
                string q = "DELETE FROM Token_Admin WHERE token=@token";

                MySqlCommand cmd = new(q, database.Connection);
                cmd.Parameters.AddWithValue("@token", ct.Tkn.HashedValue);

                return cmd.ExecuteNonQuery() > 0;
            }
            return false;
        }

        public bool[] Bulk(string[] token) {
            bool [] bools = {false, false};
            return bools;
        }
    }
}