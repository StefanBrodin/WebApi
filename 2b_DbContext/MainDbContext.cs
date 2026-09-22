using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Configuration;
using DbModels;
using Microsoft.Extensions.Hosting.Internal;

namespace DbContext;

// DbContext namespace is a fundamental EFC layer of the database context and is
// used for all Database connection as well as for EFC CodeFirst migration and database updates 
public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    // Models of the database tables are defined here as DbSet<T> properties
    #region C# model of database tables
    public DbSet<QuoteDbM> Quotes { get; set; }
    #endregion

    // *Two* constructors are needed for the DbContext to work with EFC CodeFirst migration and database update commands
    #region constructors
    public MainDbContext() { }
    public MainDbContext(DbContextOptions options) : base(options)
    { }
    #endregion

    // Conventions that is common to all DbContexts can be defined here, for example, the default column type for string and decimal properties



    // Here we can affect the model building for all DbContexts, for example, we can define a default schema for all tables in the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region override modelbuilder
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }

    // Retrieve the connection string from appsettings.json for design time use, for example, for EFC CodeFirst migration and database update commands
    protected string GetConnectionString(string connectionStringName)
    {
        // Design time: manually create configuration to read appsettings.json
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var config = configBuilder.Build();
        var connectionString = config.GetConnectionString(connectionStringName);
        System.Console.WriteLine($"Design time Connection String from appsettings.json: {connectionString}");

        return connectionString;
    }

    // The various DbContext classes for different databases are defined here, for example, SqlServerDbContext, MySqlDbContext, PostgresDbContext. 
    // Each of these classes inherits from MainDbContext and can have their own specific configurations and conventions.
    #region DbContext for some popular databases
    public class SqlServerDbContext : MainDbContext
    {
        public SqlServerDbContext() { }
        public SqlServerDbContext(DbContextOptions options) 
            : base(options) { }


        // Used only for CodeFirst Database Migration and database update commands
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = GetConnectionString("SqlServerDocker");
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add your own modelling based on done migrations
            base.OnModelCreating(modelBuilder);
        }
    }

    public class MySqlDbContext : MainDbContext
    {
        public MySqlDbContext() { }
        public MySqlDbContext(DbContextOptions options) : base(options) { }


        // Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = GetConnectionString("MySqlDocker");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    b => b.SchemaBehavior(Microting.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);

        }
    }

    public class PostgresDbContext : MainDbContext
    {
        public PostgresDbContext() { }
        public PostgresDbContext(DbContextOptions options) : base(options){ }


        // Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = GetConnectionString("PostgreSqlDocker");
                System.Console.WriteLine($"Connection String: {connectionString}");
                
                optionsBuilder.UseNpgsql(connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");
            base.ConfigureConventions(configurationBuilder);
        }
    }
    #endregion
}
