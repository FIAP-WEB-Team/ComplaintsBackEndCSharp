using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ComplaintsAPI.Services;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);
var connectionStringUsed=string.Empty;
var _connectionString =  string.Empty;
var _containerName = builder.Configuration.GetSection("AzureBlobStorage:ContainerName")?.Value ?? string.Empty;
// Add services to the container.
builder.Services.Configure<ComplaintsDataBaseSettings>(builder.Configuration.GetSection(nameof(ComplaintsDataBaseSettings)));
builder.Services.AddSingleton<IComplaintsDataBaseSettings>(sp => sp.GetRequiredService<IOptions<ComplaintsDataBaseSettings>>().Value);

if (builder.Environment.IsDevelopment()) {
    connectionStringUsed = builder.Configuration.GetValue<string>("ComplaintsDataBaseSettings:ConnectionMongoString");

    // Here we use the same connection used in ProductionEnviroment Because we don´t have money to create a new blobstorage
    _connectionString = builder.Configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
}
if (builder.Environment.IsProduction()) {  
    var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVault");
    var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
    var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
    var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");
    var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString());
    builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString(), new DefaultKeyVaultSecretManager());

    var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);
    connectionStringUsed =client.GetSecret("ConnectionMongoString").Value.Value.ToString();
   _connectionString = client.GetSecret("BlobStorageConnection").Value.Value.ToString();
}
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(connectionStringUsed));
builder.Services.AddSingleton<IPythonService, PythonService>();
builder.Services.AddScoped<IComplaintsService, ComplaintsService>();


builder.Services.AddScoped<IBlobService, BlobService>(s => new BlobService(_connectionString,_containerName));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
