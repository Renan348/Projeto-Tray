using Eplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Eplace.Module
{
    public static class ProdutosEnd
    {
        public static void EndpointsProduto(this IEndpointRouteBuilder rotas)
        {
            RouteGroupBuilder rotaProduto = rotas.MapGroup("/Produtos");
            rotaProduto.MapGet("/", (eplaceDbContext dbContext) =>
            {
                IEnumerable<Produtos> produto = dbContext.Produtos;
                return produto;
            });

            rotaProduto.MapGet("/{Id}", (eplaceDbContext dbContext, int Id) => 
            {
                Produtos? produto = dbContext.Produtos.Find(Id);
                if (produto is null)
                {
                    return Results.NotFound();
                }

                return TypedResults.Ok<Produtos>(produto);
            }).Produces<Produtos>();

            rotaProduto.MapPost("/", (eplaceDbContext dbContext, Produtos produto) => 
            {
                dbContext.Produtos.Add(produto);
                dbContext.SaveChanges();

                return TypedResults.Created($"/Produtos/{produto.Id}", produto);
            });

            rotaProduto.MapPut("/{Id}", (eplaceDbContext dbContext, int Id, Produtos produto) => 
            {
                Produtos? ProdutoEncontrado = dbContext.Produtos.Find(Id);
                if (ProdutoEncontrado is null)
                {
                    return Results.NotFound();
                }

                produto.Id = Id;

                dbContext.Entry(ProdutoEncontrado).CurrentValues.SetValues(produto);
                dbContext.SaveChanges();

                return TypedResults.NoContent();
            });
            
            rotaProduto.MapDelete("/{Id}", (eplaceDbContext dbContext, int Id) => 
            {
                Produtos? ProdutoEncontrado = dbContext.Produtos.Find(Id);
                if (ProdutoEncontrado is null)
                {
                    return Results.NotFound();
                }

                dbContext.Produtos.Remove(ProdutoEncontrado);
                dbContext.SaveChanges();

                return TypedResults.NoContent();
            });
        }
    }
}
