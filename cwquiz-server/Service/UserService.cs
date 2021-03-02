using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CW.Thiedze.Domain;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.IdentityModel.Tokens;

namespace CW.Thiedze.Service
{
    public class UserService
    {
        private string JwtKey { get; } = "";
        public string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(user.Username, user.Username, claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User Login(string username, string passwordSha256)
        {
            User user = new User();

            var connectionString = "";
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("user");

            List<User> users = GetEntitiesFromTable<User>(table);

            return user;
        }

        private static List<T> GetEntitiesFromTable<T>(CloudTable table) where T : ITableEntity, new()
        {
            TableQuerySegment<T> querySegment = null;
            var entities = new List<T>();
            var query = new TableQuery<T>();

            do
            {
                querySegment = table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken).Result;
                entities.AddRange(querySegment.Results);
            } while (querySegment.ContinuationToken != null);

            return entities;
        }


    }
}