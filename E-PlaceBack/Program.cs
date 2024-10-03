using Eplace;
using Eplace.Models;
using Microsoft.EntityFrameworkCore;
using Eplace.Module;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar todos os serviços antes de construir o app
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        // Adicionar DbContext e outros serviços
        builder.Services.AddDbContext<eplaceDbContext>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Agora, cria o app
        var app = builder.Build();

        // Configuração do middleware (após a construção do app)
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Habilitar CORS
        app.UseCors("AllowAll");

        // Mapear rotas
        app.MapGet("/", () => "O Retorno!");

        // Rotas personalizadas
        app.EndpointsUsuario();
        app.EndpointsProduto();

        // Executar a aplicação
        app.Run();
    }
}
