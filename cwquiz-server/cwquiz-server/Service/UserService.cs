using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CW.Thiedze.Domain;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CW.Thiedze.Service
{
    public class UserService : IUserService
    {
        private string JwtKey { get; } = "";
        private CloudTable UserTable { get; }

        public UserService()
        {
            var connectionString = Environment.GetEnvironmentVariable("STORAGE_CONNCTION_STRING", EnvironmentVariableTarget.Process);
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            UserTable = tableClient.GetTableReference("user");
        }

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
            User user = GetUser(username, passwordSha256);

            return user;
        }

        private User GetUser(string username, string passwordSha256)
        {
            var partionKeyCondition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "user");
            var usernameCondition = TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username);
            var passwordCondition = TableQuery.GenerateFilterCondition("PasswordSha256", QueryComparisons.Equal, passwordSha256);
            var combinedConditions = TableQuery.CombineFilters(TableQuery.CombineFilters(usernameCondition, TableOperators.And, passwordCondition), TableOperators.And, partionKeyCondition);
            var query = new TableQuery<User>().Where(combinedConditions);

            TableQuerySegment<User> querySegment = null;
            var users = new List<User>();
            do
            {
                querySegment = UserTable.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken).Result;
                users.AddRange(querySegment.Results);

            } while (querySegment.ContinuationToken != null);

            return users.Count > 0 ? users[0] : new User(false);
        }


    }
}