using Eplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Eplace.Module
{
    public static class ProdutosEnd
    {
        public static void EndpointsProduto(this IEndpointRouteBuilder rotas)
        {
            RouteGroupBuilder rotaProduto = rotas.MapGroup("/Produtos");
            
            rotaProduto.MapGet("/", async (eplaceDbContext dbContext) =>
            {
                return await dbContext.Produtos.ToListAsync(); // Usando ToListAsync para operações assíncronas
            });

            rotaProduto.MapGet("/{Id}", async (eplaceDbContext dbContext, int Id) => 
            {
                var produto = await dbContext.Produtos.FindAsync(Id);
                return produto is null ? Results.NotFound() : TypedResults.Ok(produto);
            }).Produces<Produtos>();

            rotaProduto.MapPost("/", async (eplaceDbContext dbContext, Produtos produto) => 
            {
                await dbContext.Produtos.AddAsync(produto);
                await dbContext.SaveChangesAsync();
                return TypedResults.Created($"/Produtos/{produto.Id}", produto);
            });

            rotaProduto.MapPut("/{Id}", async (eplaceDbContext dbContext, int Id, Produtos produto) => 
            {
                var produtoEncontrado = await dbContext.Produtos.FindAsync(Id);
                if (produtoEncontrado is null) return Results.NotFound();

                produto.Id = Id;
                dbContext.Entry(produtoEncontrado).CurrentValues.SetValues(produto);
                await dbContext.SaveChangesAsync();
                return TypedResults.NoContent();
            });

            rotaProduto.MapDelete("/{Id}", async (eplaceDbContext dbContext, int Id) => 
            {
                var produtoEncontrado = await dbContext.Produtos.FindAsync(Id);
                if (produtoEncontrado is null) return Results.NotFound();

                dbContext.Produtos.Remove(produtoEncontrado);
                await dbContext.SaveChangesAsync();
                return TypedResults.NoContent();
            });
        }
    }
}
