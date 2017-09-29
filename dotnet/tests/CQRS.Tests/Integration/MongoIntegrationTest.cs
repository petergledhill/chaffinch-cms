using System;
using System.Collections.Generic;
using System.Text;
using Mongo2Go;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.IO;
using System.Linq;

namespace Chaffinch.CQRS.Tests.Integration
{
    public class MongoIntegrationTest
    {
        internal static MongoDbRunner _runner;
        internal static IMongoDatabase _database;
        internal static string _databaseName = "IntegrationTest";

        internal static void CreateConnection()
        {
            _runner = MongoDbRunner.Start(); 

            MongoClient client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase(_databaseName);
        }

        public static IList<T> ReadBsonFile<T>(string fileName)
        {
            string[] content = File.ReadAllLines(fileName);
            return content.Select(s => BsonSerializer.Deserialize<T>(s)).ToList();
        }
    }
}
