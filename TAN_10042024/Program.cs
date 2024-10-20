using Microsoft.EntityFrameworkCore;
using TAN_10042024.Application.Abstractions;
using TAN_10042024.Application.Abstractions.Queries;
using TAN_10042024.Application.Abstractions.Repositories;
using TAN_10042024.Application.Services;
using TAN_10042024.Application.Utilities;
using TAN_10042024.Infrastructure.Data;
using TAN_10042024.Infrastructure.Data.Queries;
using TAN_10042024.Infrastructure.Data.Repositories;
using TAN_10042024.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>((options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CC")));

// Repositories
builder.Services.AddScoped<IApiSessionRepository, ApiSessionRepository>();
builder.Services.AddScoped<IAuthenticationSessionRepository, AuthenticationSessionRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

// Query Services
builder.Services.AddScoped<IApiSessionQueryService, ApiSessionQueryService>();
builder.Services.AddScoped<IAuthenticationSessionQueryService, AuthenticationSessionQueryService>();
builder.Services.AddScoped<IPersonQueryService, PersonQueryService>();
builder.Services.AddScoped<IFileQueryService, FileQueryService>();
builder.Services.AddScoped<IClientQueryService, ClientQueryService>();

// Application Services
builder.Services.AddScoped<IApiSession, ApiSessionService>();
builder.Services.AddScoped<IAuthentication, AuthenticationService>();
builder.Services.AddScoped<IFileUpload, FileUploadService>();
builder.Services.AddScoped<IReport, ReportService>();
builder.Services.AddScoped<IPerson, PersonService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ExecuteMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware(s)
app.UseMiddleware<RequestLoggerMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
