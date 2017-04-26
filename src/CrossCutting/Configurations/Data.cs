namespace Template.CrossCutting.Configurations
{
    public class Data
    {
        public virtual DataSqlServer SqlServer { get; set; }

        public virtual DataMongoDb MongoDb { get; set; }
    }
}
