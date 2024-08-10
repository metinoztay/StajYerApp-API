using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using StajYerApp_API.Models;
using StajYerApp_API.Services;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", builder =>
	{
		builder.AllowAnyHeader()
		.AllowAnyMethod()
		.SetIsOriginAllowed((host) => true)
		.AllowCredentials(); 
	});
});

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddDbContext<Db6761Context>(options =>
	options.UseSqlServer("Server=db6761.public.databaseasp.net; Database=db6761; User Id=db6761; Password=Nz3_9#aF@B5y; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;Connection Timeout=30"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "StajYerApp API", Version = "v1" });

	// XML yorumlarýný ekle
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "StajYerApp API V1");
});
//}

app.UseHttpsRedirection();


var rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");
var userProfilePhotosPath = Path.Combine(rootFolderPath, "UserProfilePhotos");
var companyLogosPath = Path.Combine(rootFolderPath, "CompanyLogos");

if (!Directory.Exists(userProfilePhotosPath))
{
    Directory.CreateDirectory(userProfilePhotosPath);
}

if (!Directory.Exists(companyLogosPath))
{
    Directory.CreateDirectory(companyLogosPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos", "UserProfilePhotos")),
    RequestPath = "/Photos/UserProfilePhotos"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos", "CompanyLogos")),
    RequestPath = "/Photos/CompanyLogos"
});



app.UseAuthorization();

app.MapControllers();

app.Run();
