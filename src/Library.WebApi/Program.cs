using Library.Application.Abstractions;
using Library.Application.UseCases;
using Library.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Library") ?? "Data Source=library.db";
DatabaseInitializer.Initialize(connectionString);

builder.Services.AddScoped<BorrowBookUseCase>();
builder.Services.AddScoped<ReturnBookUseCase>();
builder.Services.AddScoped<GetMemberLoansUseCase>();
builder.Services.AddScoped<IBookRepository>(_ => new DapperBookRepository(connectionString));
builder.Services.AddScoped<ILoanRepository>(_ => new DapperLoanRepository(connectionString));
builder.Services.AddScoped<IMemberRepository>(_ => new DapperMemberRepository(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
