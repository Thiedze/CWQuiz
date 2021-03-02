using CW.Thiedze.Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace CW.Thiedze.Service
{
    public class UserService : IUserService
    {
        private CloudTable UserTable { get; }

        public UserService()
        {
            var connectionString = Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING", EnvironmentVariableTarget.Process);
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            UserTable = tableClient.GetTableReference("user");
        }

        public User Login(string username, string password)
        {
            User user = GetUser(username, AuthorizationService.ComputeSha256Hash(password));
            user.Token = AuthorizationService.GenerateSecurityToken(user);
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

            if (users.Count == 1)
            {
                users[0].IsValid = true;
                return users[0];
            }
            else
            {
                return new User(false);
            }
        }

    }
}