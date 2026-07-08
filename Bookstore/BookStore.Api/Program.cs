using BookStore.Api.Rules;
using BookStore.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Service
builder.Services.AddScoped<BookService>();

// Rules
builder.Services.AddScoped<IBookDeletionRule, PremiumBookDeletionRule>();
builder.Services.AddScoped<IBookDeletionRule, LoanedBookDeletionRule>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();