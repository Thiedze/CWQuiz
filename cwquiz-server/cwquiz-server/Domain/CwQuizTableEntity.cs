using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace CW.Thiedze.Domain
{
    public abstract class CwQuizTableEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public string ETag { get; set; }

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
