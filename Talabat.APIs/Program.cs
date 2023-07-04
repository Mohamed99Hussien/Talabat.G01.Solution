using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Excensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middleware;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IRepositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers(); // add allServices Api
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppIdentityDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
{
    var connection = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));

    return ConnectionMultiplexer.Connect(connection);
});

builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors(options => {

    options.AddPolicy("CorsPolicy", options => {

        options.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"); 
       //AllowAnyOrigin();
    
    });
});
//builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles)); Old Way
builder.Services.AddAutoMapper(typeof(MappingProfiles));
var app = builder.Build();
    
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var context = services.GetRequiredService<StoreContext>();
    context.Database.Migrate(); // Update-Database
    await StoreContextSeed.SeedAsync(context,loggerFactory);

    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityContext.Database.MigrateAsync();

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await AppIdentityDbContextSeed.SeedUserAsync(userManager); 

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExcepationMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");  // cheek policy
app.UseAuthentication();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
