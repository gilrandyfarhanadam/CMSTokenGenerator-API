using Token_Generator;
using DatabaseConnection;

namespace Function
{
    public sealed class TokenCallback{

        public TokenCallback(){
            
        }

        public async Task GenerateToken(HttpContext context){
            // Read a form data
            IFormCollection form = await context.Request.ReadFormAsync();

            // Get the value of start date and end date
            var start = form["start"];
            var end = form["end"];

            // generate the token
            Token token = new(DateTime.Parse(start.ToString()), DateTime.Parse(end.ToString()));

            // insert token to db
            InsertToken ins = new();
            bool succ = ins.Single(token);

            // Respond string
            string resp = "Failed to generate";

            if (succ){
                resp = $"Token {token.Value} will be valid from {token.Start} until {token.End}";
            }

            await context.Response.WriteAsync(resp);
        }

        public async Task CheckToken(HttpContext context){
            // Read a form data
            IFormCollection form = await context.Request.ReadFormAsync();

            // Get the value of token
            var token = Convert.ToString(form["token"]);

            CheckToken ct = new();

            if (token is not null && ct.IsValid(token)){
                DeleteToken dt = new();

                if (dt.Single(token))
                {
                    await context.Response.WriteAsync("Valid");
                } else {
                    await context.Response.WriteAsync("Something went wrong!");
                }

            } else {
                await context.Response.WriteAsync("Invalid");
            }
        }
    }
}