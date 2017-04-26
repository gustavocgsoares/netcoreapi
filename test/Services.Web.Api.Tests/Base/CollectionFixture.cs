using System;
using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Template.Data.MongoDb.Helpers;
using Template.Domain.Entities.Corporate;
using Template.Domain.Enums.Corporate;

namespace Template.Services.Web.Api.Tests.Base
{
    public class CollectionFixture : IDisposable
    {
        #region Fields | Members
        private IConfigurationRoot configuration;

        private string databaseName;

        private IMongoDatabase database;

        private MongoClient client;
        #endregion

        #region Constructors | Destructors
        public CollectionFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.testing.json", optional: true);

            configuration = builder.Build();

            var connectionString = configuration.GetSection("Data:MongoDb:ConnectionString").Value;

            ClassMapHelper.RegisterConventionPacks();

            var mongoUrl = new MongoUrl(connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };

            client = new MongoClient(settings);
            databaseName = configuration.GetSection("Data:MongoDb:Database").Value;
            database = client.GetDatabase(databaseName);

            CreateCollections();
            InitializeDatabase();
        }
        #endregion

        #region IDisposable members
        public void Dispose()
        {
            DropCollections();
            client = null;
        }
        #endregion

        #region Private methods
        private void InitializeDatabase()
        {
            InitializeUsers();
        }

        private void CreateCollections()
        {
            database.CreateCollection("users");
        }

        private void DropCollections()
        {
            database.DropCollection("users");
        }

        private void InitializeUsers()
        {
            ClassMapHelper.SetupClassMap<User, Guid>();
            var collection = database.GetCollection<User>("users");

            collection.InsertOneAsync(new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Smith Doe", Email = "john@domain.com", Phone = "+5511123456789", Password = "Template@123", Gender = Gender.Male, BirthDate = DateTime.Now.Date.AddYears(-23).AddDays(-45), ProfileImage = "profile/image.png", AccessAttempts = 0, LastAcceptanceTermsDate = DateTime.Now.AddDays(-23), AddedDate = DateTime.Now.AddDays(-23), Blocked = false }, null);
            collection.InsertOneAsync(new User { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith Doe", Email = "jane@domain.com", Phone = "+5511123456789", Password = "Template@123", Gender = Gender.Female, BirthDate = DateTime.Now.Date.AddYears(-18).AddDays(-120), ProfileImage = "profile/image.png", AccessAttempts = 0, LastAcceptanceTermsDate = DateTime.Now.AddDays(-20), AddedDate = DateTime.Now.AddDays(-20), Blocked = false }, null);
            collection.InsertOneAsync(new User { Id = Guid.NewGuid(), FirstName = "Josh", LastName = "Smith Doe", Email = "conflict@domain.com", Phone = "+5511123456789", Password = "Template@123", Gender = Gender.Male, BirthDate = DateTime.Now.Date.AddYears(-22).AddDays(-90), ProfileImage = "profile/image.png", AccessAttempts = 0, LastAcceptanceTermsDate = DateTime.Now.AddDays(-40), AddedDate = DateTime.Now.AddDays(-40), Blocked = false }, null);
            collection.InsertOneAsync(new User { Id = Guid.NewGuid(), FirstName = "Alex", LastName = "Smith Doe", Email = "alex@domain.com", Phone = "+5511123456789", Password = "Template@123", Gender = Gender.Male, BirthDate = DateTime.Now.Date.AddYears(-45).AddDays(-10), ProfileImage = "profile/image.png", AccessAttempts = 0, LastAcceptanceTermsDate = DateTime.Now.AddDays(-33), AddedDate = DateTime.Now.AddDays(-33), Blocked = false }, null);
            collection.InsertOneAsync(new User { Id = Guid.NewGuid(), FirstName = "Johnny", LastName = "Smith Doe", Email = "johnny@domain.com", Phone = "+5511123456789", Password = "Template@123", Gender = Gender.Male, BirthDate = DateTime.Now.Date.AddYears(-20).AddDays(-60), ProfileImage = "profile/image.png", AccessAttempts = 0, LastAcceptanceTermsDate = DateTime.Now.AddDays(-67), AddedDate = DateTime.Now.AddDays(-67), Blocked = false }, null);
            collection.InsertOneAsync(new User { Id = Guid.NewGuid(), FirstName = "Jude", LastName = "Smith Doe", Email = "jude@domain.com", Phone = "+5511123456789", Password = "Template@123", Gender = Gender.Female, BirthDate = DateTime.Now.Date.AddYears(-32).AddDays(-75), ProfileImage = "profile/image.png", AccessAttempts = 4, LastAcceptanceTermsDate = DateTime.Now.AddDays(-12), AddedDate = DateTime.Now.AddDays(-12), Blocked = true }, null);
        }
        #endregion
    }
}
