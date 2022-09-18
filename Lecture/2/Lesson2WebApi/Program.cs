using System.Collections.Concurrent;using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    }));


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5176, o => o.Protocols = HttpProtocols.Http2);
    options.ListenLocalhost(5175, o => o.Protocols = HttpProtocols.Http1);
});

var app = builder.Build();
// Run Map Use
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

// расказать что это
//app.UseResponseCompression();

app.MapControllers();

app.Run();
