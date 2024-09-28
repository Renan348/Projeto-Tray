using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eplace.Models
{
    public class Produtos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }

        private float _preco;
        public float Preço
        {
            get => _preco;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "O preço não pode ser negativo.");
                _preco = value;
            }
        }

        private int _quantidadeEmEstoque;
        public int QuantidadeEmEstoque
        {
            get => _quantidadeEmEstoque;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "A quantidade em estoque não pode ser negativa.");
                _quantidadeEmEstoque = value;
            }
        }

        public bool MercadoLivre { get; set; }
        public bool AliExpress { get; set; }
        public bool Shopee { get; set; }
        public Produtos(string modelo, string marca, float preco, int quantidadeEmEstoque, bool mercadoLivre, bool aliExpress, bool shopee)
        {
            this.Modelo = modelo;
            this.Marca = marca;
            this.Preço = preco;
            this.QuantidadeEmEstoque = quantidadeEmEstoque;
            this.MercadoLivre = mercadoLivre;
            this.AliExpress = aliExpress;
            this.Shopee = shopee;
        }
        private Produtos() { }
    }
}

