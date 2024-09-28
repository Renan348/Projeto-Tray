using Eplace;
using Eplace.Models;
using Microsoft.EntityFrameworkCore;
using Eplace.Module;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<eplaceDbContext>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapGet("/", () => "O Retorno!");
        
        app.EndpointsUsuario();

        app.EndpointsProduto();

        app.Run();
    }
}
