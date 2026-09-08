using BlueprintApi;
using Microsoft.EntityFrameworkCore;
// using BlueprintTracker.Models; // Uncomment or adjust based on your namespace

var builder = WebApplication.CreateBuilder(args);

// 1. Inject the In-Memory Database into the DI Container
builder.Services.AddDbContext<BlueprintDbContext>(options =>
    options.UseInMemoryDatabase("BlueprintDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. Seed Initial Data safely using a Service Scope
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlueprintDbContext>();
    db.Database.EnsureCreated(); // Ensures the in-memory DB is ready

    if (!db.StudyModules.Any()) // Only seed if empty
    {
        db.StudyModules.AddRange(
            new StudyModule { Title = "Master C# 13 Features", IsCompleted = false },
            new StudyModule { Title = "Understand Minimal APIs", IsCompleted = true }
        );
        db.SaveChanges();
    }
}

// 3. Define Minimal API Endpoints
var modulesApi = app.MapGroup("/api/modules");

// GET: Retrieve all modules
modulesApi.MapGet("/", async (BlueprintDbContext db) =>
{
    return await db.StudyModules.ToListAsync();
})
.WithName("GetAllModules")
.WithOpenApi();

// POST: Create a new module
modulesApi.MapPost("/", async (StudyModule module, BlueprintDbContext db) =>
{
    db.StudyModules.Add(module);
    await db.SaveChangesAsync();

    // Returns a 201 Created response with the location header
    return Results.Created($"/api/modules/{module.Id}", module);
})
.WithName("CreateModule")
.WithOpenApi();

app.Run();