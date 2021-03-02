using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace CW.Thiedze.Domain
{
    public class User: ITableEntity
    {
        public string Username {get; set;}
        public bool IsValid {get; set;}
        public string PartitionKey { get; set; } = "1";
        public string RowKey { get; set; } = "1";
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public string ETag { get ; set; } = "user";

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            throw new NotImplementedException();
        }
    }
}