using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CW.Thiedze.Domain
{
    public class User : ITableEntity
    {
        public string Username { get; set; }
        public bool IsValid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public string ETag { get; set; }

        public User(bool isValid)
        {
            IsValid = isValid;
        }

        public User()
        {
        }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            TableEntity.ReadUserObject(this, properties, operationContext);
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            return TableEntity.WriteUserObject(this, operationContext);
        }
    }
}