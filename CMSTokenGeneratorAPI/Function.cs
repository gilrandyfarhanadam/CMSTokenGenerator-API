using Token_Generator;
using DatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;
using System.Text.Json;

namespace Function
{
    public sealed class TokenCallback{

        public TokenCallback(){
            
        }

        public async Task GenerateToken(HttpContext context){
            // Read request body as JSON
            StreamReader reader = new(context.Request.Body);
            string body = await reader.ReadToEndAsync();

            // convert to json element
            JsonDocument document = JsonDocument.Parse(body);
            JsonElement root = document.RootElement;
            context.Response.ContentType = "application/json";

            try
            {
                // Get the value of start date and end date
                DateTime start = root.GetProperty("start").GetDateTime();
                DateTime end = root.GetProperty("end").GetDateTime();

                // generate the token
                Token tkn = new(DateTime.Parse(start.ToString()), DateTime.Parse(end.ToString()));

                // insert token to db
                InsertToken ins = new();
                bool succ = ins.Single(tkn);
                
                string resp = JsonSerializer.Serialize(new {
                    success = true,
                    Token = tkn.Value,
                    ValidFrom = tkn.Start.ToString("dd-MM-yyyy"),
                    ValidUntil = tkn.End.ToString("dd-MM-yyyy")
                });

                if (succ){
                    await context.Response.WriteAsync(resp);
                } else {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                        success = false
                    }));
                }
            }
            catch (Exception ex)
            {
                string resp = JsonSerializer.Serialize(new {
                    success = false,
                    message = ex.Message
                });

                await context.Response.WriteAsync(resp);
            }
        }

        public async Task CheckToken(HttpContext context){
            // Read request body as JSON
            StreamReader reader = new(context.Request.Body);
            string body = await reader.ReadToEndAsync();

            // Get the value of start date and end date
            JsonDocument document = JsonDocument.Parse(body);
            JsonElement root = document.RootElement;
            context.Response.ContentType = "application/json";

            // Get the value of token
            string token = root.GetProperty("token").ToString();

            CheckToken ct = new();

            if (token is not null && ct.IsValid(token)){
                DeleteToken dt = new();

                if (dt.Single(token))
                {
                    string resp = JsonSerializer.Serialize(new {
                        valid = true,
                        message = ""
                    });
                    await context.Response.WriteAsync(resp);
                } else {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new{
                        valid = false,
                        message = "Something went wrong!"
                    }));
                }

            } else {
                await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                    valid = false,
                    message = "Invalid token"
                }));
            }
        }
    }
}