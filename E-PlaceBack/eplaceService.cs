using Eplace;
using Eplace.Models;

public class EplaceService
{
    private eplaceDbContext dbContext = new eplaceDbContext();

    public void CriarUsuario(string email, string senha)
    {
        Usuario usuario = new Usuario(email, senha);

        dbContext.Usuarios.Add(usuario);

        dbContext.SaveChanges();
    }

    public void CriarProduto(string modelo, string marca, float preco, int quantidadeEmEstoque, bool mercadoLivre, bool aliExpress, bool shopee)
    {
        Produtos produto = new Produtos(modelo, marca, preco, quantidadeEmEstoque, mercadoLivre, aliExpress, shopee);

        dbContext.Produtos.Add(produto);

        dbContext.SaveChanges();
    }
}
