//using Services;
using Configuration;
using Configuration.Options;

var builder = WebApplication.CreateBuilder(args);


// NOTE: global cors policy needed for JS and React frontends
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

#region Initializing the standard sw stack
//using user secrets
//You dont need to use the assembly (reflection) to access user secrets at runtime
//But you need to use the assembly (reflection) to access user secrets at design time (efc migrations)
//In later branches this code is refactored into a configuration extension that is used both at design time and runtime
//to switch between user secrets (development) and azure key vault (production)
var currentDir = Directory.GetCurrentDirectory();
var assembly = System.Reflection.Assembly.Load("4_Configuration");
builder.Configuration.SetBasePath(Path.Combine(currentDir, "../0_AppWebApi"))
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddUserSecrets(assembly);

// adding options patterns to read appsettings and user secrets
builder.Services.Configure<AesEncryptionOptions>(
    options => builder.Configuration.GetSection(AesEncryptionOptions.Position).Bind(options));

// Registering encryption service
builder.Services.AddTransient<Encryptions>();

builder.Services.Configure<JwtOptions>(
    options => builder.Configuration.GetSection(JwtOptions.Position).Bind(options));

// adding options and service for multiple Database connections and their respective DbContexts
builder.Services.Configure<DbConnectionSetsOptions>(
    options => builder.Configuration.GetSection(DbConnectionSetsOptions.Position).Bind(options));

// Registering database connections service
builder.Services.AddSingleton<DatabaseConnections>();

// adding version info
builder.Services.Configure<VersionOptions>(options =>VersionOptions.ReadFromAssembly(options));

//Inject Custom logger, this will also register the InMemoryLoggerProvider logger
//hence, AddLogging should not be used here
builder.Services.AddSingleton<ILoggerProvider, InMemoryLoggerProvider>();

#endregion



// Add services to the container.



// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Attraction Vote API",
#if DEBUG
        Version = "v1.0 DEBUG",
#else
        Version = "v1.0",
#endif
        Description = "This is the Swagger/OpenAPI documentation for the \"Attraction Vote\" API by Stefan Brodin. It provides information about the available endpoints, request/response formats, and authentication requirements."
        + $"<br>DataSet: {builder.Configuration["DatabaseConnections:UseDataSetWithTag"]}"
        + $"<br>DefaultDataUser: {builder.Configuration["DatabaseConnections:DefaultDataUser"]}"
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
// for the purpose of this example, we will use Swagger also in production
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Attraction Vote API v1.0");
    });
}

app.UseHttpsRedirection();
app.UseCors(); 

app.UseAuthorization();

app.MapControllers();

app.Run();


