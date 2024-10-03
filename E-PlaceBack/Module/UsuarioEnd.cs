using Eplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Eplace.Module
{
    public static class UsuarioEnd
    {
        public static void EndpointsUsuario(this IEndpointRouteBuilder rotas)
        {
            RouteGroupBuilder rotaUsuario = rotas.MapGroup("/Usuario");
            
            rotaUsuario.MapGet("/", async (eplaceDbContext dbContext) =>
            {
                return await dbContext.Usuarios.ToListAsync(); // Usando ToListAsync
            });

            rotaUsuario.MapGet("/{Id}", async (eplaceDbContext dbContext, int Id) =>
            {
                var usuario = await dbContext.Usuarios.FindAsync(Id);
                return usuario is null ? Results.NotFound() : TypedResults.Ok(usuario);
            }).Produces<Usuario>();

            rotaUsuario.MapPost("/", async (eplaceDbContext dbContext, Usuario usuario) => 
            {
                await dbContext.Usuarios.AddAsync(usuario);
                await dbContext.SaveChangesAsync();
                return TypedResults.Created($"/Usuario/{usuario.Id}", usuario);
            });

            rotaUsuario.MapPut("/{Id}", async (eplaceDbContext dbContext, int Id, Usuario usuario) => 
            {
                var usuarioEncontrado = await dbContext.Usuarios.FindAsync(Id);
                if (usuarioEncontrado is null) return Results.NotFound();

                usuario.Id = Id;
                dbContext.Entry(usuarioEncontrado).CurrentValues.SetValues(usuario);
                await dbContext.SaveChangesAsync();
                return TypedResults.NoContent();
            });

            rotaUsuario.MapDelete("/{Id}", async (eplaceDbContext dbContext, int Id) => 
            {
                var usuarioEncontrado = await dbContext.Usuarios.FindAsync(Id);
                if (usuarioEncontrado is null) return Results.NotFound();

                dbContext.Usuarios.Remove(usuarioEncontrado);
                await dbContext.SaveChangesAsync();
                return TypedResults.NoContent();
            });
        }
    }
}
