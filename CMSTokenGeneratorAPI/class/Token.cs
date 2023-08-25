using System.Text;
using System.Security.Cryptography;
using DatabaseConnection;

namespace Token_Generator
{
    public class Token
    {
        public Token(string Value, DateTime Start, DateTime End){
            this.Value = Value;
            this.Start = Start;
            this.End = End;
        }

        public Token(DateTime Start, DateTime End){
            this.Start = Start;
            this.End = End;
            Value = Generate();
            HashedValueBytes = HashData(Value);
            HashedValue = ToStringValue(HashedValueBytes);
        }

        private readonly byte[]? HashedValueBytes;
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Value { get; set; }

        public string? HashedValue { get; set; }

        public bool IsEmpty() {
            return Value.Length == 0;
        }

        private static string Generate(){
            StringBuilder CurrentToken = new();
            Random random = new();

            char letter;

            int length = 16;
            
            for (int i = 0; i < length; i++)
            {
                // randomise if it a letter or a number
                bool isNumber = random.NextInt64() % 2 == 0;

                if (!isNumber){
                    double flt = random.NextDouble();
                    int shift = Convert.ToInt32(Math.Floor(25 * flt));
                    letter = Convert.ToChar(shift + 65);
                    CurrentToken.Append(letter);  
                } else
                {
                    long num = random.NextInt64(0, 9);
                    CurrentToken.Append(Convert.ToString(num));
                }
            }
            

            return CurrentToken.ToString();
        }

        public static byte[] HashData(string str){
            byte[] tmp = Encoding.UTF8.GetBytes(str);
            return SHA512.HashData(tmp);
        }

        public static string ToStringValue(byte[] byt){
            StringBuilder sb = new();

            if(byt.Length != 0){
                foreach (byte b in byt)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}