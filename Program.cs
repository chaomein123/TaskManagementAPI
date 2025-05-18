using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

var builder = WebApplication.CreateBuilder(args);


// Configure Entity Framework Core with an in-memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TaskDB"));

var jwtSecretKey = builder.Configuration["Jwt:Key"] ?? "superSecretKey@345"; // Use config or fallback
// Add authentication using JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add controllers
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Tasks.AddRange(
        new TaskItem 
        { 
            Id = 1, 
            Title = "Fix Bug", 
            Description = "Fix login issue", 
            AssignedToUserId = "user1", 
            Status = "Pending" 
        },
        new TaskItem 
        { 
            Id = 2, 
            Title = "Add Feature", 
            Description = "Add search functionality", 
            AssignedToUserId = "user2", 
            Status = "In Progress" 
        },
        new TaskItem 
        { 
            Id = 3, 
            Title = "Write Documentation", 
            Description = "Document API endpoints", 
            AssignedToUserId = "user1", 
            Status = "Completed" 
        },
        new TaskItem 
        { 
            Id = 4, 
            Title = "Code Review", 
            Description = "Review team pull requests", 
            AssignedToUserId = "user3", 
            Status = "Pending" 
        },
        new TaskItem 
        { 
            Id = 5, 
            Title = "Deploy to Production", 
            Description = "Deploy the latest release", 
            AssignedToUserId = "user2", 
            Status = "Pending" 
        }
    );

    dbContext.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();