﻿// <copyright file="MongoConnection.cs" company="Mercedes-Benz Grand Prix Limited">
// Copyright (c) Mercedes-Benz Grand Prix Limited. All rights reserved.
// </copyright>

namespace GuildTracker.Common.Database
{
    using GuildTracker.Common.Models;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;

    public class MongoConnection : IDatabaseConnection
    {
        private const string dbName = "guild-tracker";
        private readonly IMongoCollection<GuildRecord> collection;

        public MongoConnection(IConfiguration configuration)
        {
            var dbConfig = configuration.GetSection("Database");

            var client = new MongoClient($"mongodb+srv://{dbConfig["User"]}:{dbConfig["Password"]}@guild-tracker-0.1bmbc.mongodb.net/{dbName}?retryWrites=true&w=majority");
            var database = client.GetDatabase("test");
            this.collection = database.GetCollection<GuildRecord>("guild-records");
        }

        public void StoreGuild(GuildRecord guild)
        {
            this.collection.InsertOne(guild);
        }
    }
}