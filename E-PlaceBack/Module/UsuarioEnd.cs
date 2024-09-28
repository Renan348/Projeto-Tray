using Eplace;
using Eplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Eplace.Module
{
    public static class UsuarioEnd
    {
        public static void EndpointsUsuario(this IEndpointRouteBuilder rotas)
        {
            RouteGroupBuilder rotaUsuario = rotas.MapGroup("/Usuario");
            rotaUsuario.MapGet("/", (eplaceDbContext dbContext) =>
            {
                IEnumerable<Usuario> usuario = dbContext.Usuarios;
                return usuario;
            });

            rotaUsuario.MapGet("/{Id}", (eplaceDbContext dbContext, int Id) =>
            {
                Usuario? usuario = dbContext.Usuarios.Find(Id);
                if (usuario is null)
                {
                    return Results.NotFound();
                }

                return TypedResults.Ok<Usuario>(usuario);
            }).Produces<Usuario>();

            rotaUsuario.MapPost("/", (eplaceDbContext dbContext, Usuario usuario) => 
            {
                dbContext.Usuarios.Add(usuario);
                dbContext.SaveChanges();

                return TypedResults.Created($"/Usuario/{usuario.Id}", usuario);
            });

            rotaUsuario.MapPut("/{Id}", (eplaceDbContext dbContext, int Id, Usuario usuario) => 
            {
                Usuario? UsuarioEncontrado = dbContext.Usuarios.Find(Id);
                if (UsuarioEncontrado is null)
                {
                    return Results.NotFound();
                }

                usuario.Id = Id;

                dbContext.Entry(UsuarioEncontrado).CurrentValues.SetValues(usuario);
                dbContext.SaveChanges();

                return TypedResults.NoContent();
            });

            rotaUsuario.MapDelete("/{Id}", (eplaceDbContext dbContext, int Id) => 
            {
                Usuario? UsuarioEncontrado = dbContext.Usuarios.Find(Id);
                if (UsuarioEncontrado is null)
                {
                    return Results.NotFound();
                }

                dbContext.Usuarios.Remove(UsuarioEncontrado);
                dbContext.SaveChanges();

                return TypedResults.NoContent();
            });
        }
    }
}