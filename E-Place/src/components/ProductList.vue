<template>
  <div>
    <div class="product-list">
      <h2>Histórico de Produtos</h2>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nome</th>
            <th>Marca</th>
            <th>Preço</th>
            <th>Ecommerce</th>
            <th>Qtde. Estoque</th>
            <th>Data de Cadastro</th>
            <th>Ações</th>
            <tr>
              <button class="btn btn-primary" @click="abrirModalCadastro">Cadastrar Produtos</button>
            </tr>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(product, index) in products" :key="product.id">
            <td>{{ product.id }}</td>
            <td>{{ product.modelo }}</td>
            <td>{{ product.marca }}</td>
            <td v-if="product.preço">R$ {{ product.preço | currency }}</td>
            <td v-else></td>
            <td>{{ formatarEcommerce(product) }}</td>
            <td>{{ product.quantidadeEmEstoque }}</td>
            <td>{{ formatarData(product.dataCadastro) }}</td>
            <td>
              <i @click="selecionarProdutoParaExcluir(index)" class="bi bi-trash"></i>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal Cadastro -->
    <div id="escolhaEcommerceModal" class="modal fade" tabindex="-1" aria-labelledby="escolhaEcommerceModalLabel" aria-hidden="true" v-if="showModal">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <form @submit.prevent="cadastrarProduto">
            <div class="modal-header">
              <h4 class="modal-title" id="escolhaEcommerceModalLabel">Cadastre seu produto</h4>
              <button type="button" class="btn-close" @click="fecharModalCadastro" aria-label="Close"></button>
            </div>
            <div class="modal-body">
              <div class="form-group">
                <input v-model="produto.modelo" placeholder="Modelo" required class="form-control">
              </div>
              <div class="form-group">
                <input v-model="produto.marca" placeholder="Marca" required class="form-control">
              </div>
              <div class="form-group">
                <input v-model.number="produto.preço" placeholder="Preço" required class="form-control">
              </div>
              <div class="form-group">
                <input v-model.number="produto.quantidadeEmEstoque" placeholder="Qtde Estoque" required class="form-control">
              </div>
              <div class="form-group">
                <label>
                  <input type="checkbox" v-model="produto.mercadoLivre"> Mercado Livre
                </label>
                <label>
                  <input type="checkbox" v-model="produto.aliExpress"> AliExpress
                </label>
                <label>
                  <input type="checkbox" v-model="produto.shopee"> Shopee
                </label>
              </div>
              <p v-if="error" class="error">{{ error }}</p>
            </div>
            <div class="modal-footer">
              <button type="submit" class="btn btn-primary">Cadastrar Produto</button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <!-- Modal Exclusão -->
    <div id="deleteCursoModal" class="modal fade" tabindex="-1" aria-labelledby="deleteCursoModalLabel" aria-hidden="true" v-if="showDeleteModal">
      <div class="modal-dialog">
        <div class="modal-content">
          <form @submit.prevent="excluirProduto">
            <div class="modal-header">
              <h4 class="modal-title" id="deleteCursoModalLabel">Delete o Produto</h4>
              <button type="button" class="btn-close" @click="fecharModalExcluir" aria-label="Close"></button>
            </div>
            <div class="modal-body">
              <p>Tem certeza que deseja excluir este produto?</p>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" @click="fecharModalExcluir">Fechar</button>
              <button type="submit" class="btn btn-danger">Excluir</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import api from "../router/api";

export default {
  name: 'ProductList',
  data() {
    return {
      products: [],
      produto: {
        modelo: '',
        marca: '',
        preço: 0,
        quantidadeEmEstoque: 0,
        mercadoLivre: false,
        aliExpress: false,
        shopee: false,
        dataCadastro: ''
      },
      produtoParaExcluir: null,
      error: '',
      showModal: false,        // Para controlar o modal de cadastro
      showDeleteModal: false,  // Para controlar o modal de exclusão
    };
  },
  created() {
    this.getProducts();
  },
  methods: {
    abrirModalCadastro() {
      this.produto = { modelo: '', marca: '', preço: 0, quantidadeEmEstoque: 0, mercadoLivre: false, aliExpress: false, shopee: false };
      this.showModal = true; // Abre o modal de cadastro
      this.$nextTick(() => {
        const modalElement = new bootstrap.Modal(document.getElementById('escolhaEcommerceModal'));
        modalElement.show();
      });
    },
    async getProducts() {
      try {
        const response = await api.get("/Produtos");
        this.products = response.data;
      } catch (error) {
        console.error("Erro ao obter produtos:", error);
      }
    },
    async cadastrarProduto() {
      this.produto.dataCadastro = new Date().toISOString(); // Adiciona a data atual
      try {
        const response = await api.post("/Produtos", this.produto);
        console.log("Produto cadastrado com sucesso:", response.data);
        
        // Adiciona o produto à lista de produtos diretamente
        this.products.push(response.data); // Use o dado retornado do servidor

        // Limpa o objeto produto para o próximo cadastro
        this.produto = { modelo: '', marca: '', preço: 0, quantidadeEmEstoque: 0, mercadoLivre: false, aliExpress: false, shopee: false };

        this.showModal = false; // Fecha o modal de cadastro
      } catch (error) {
        console.error("Erro ao cadastrar produto:", error);
        
        // Adicionando mais detalhes ao log
        if (error.response) {
          console.error("Dados de resposta do servidor:", error.response.data);
          console.error("Código de status:", error.response.status);
          this.error = `Erro ao cadastrar produto: ${error.response.data.message || 'Erro desconhecido'}`;
        } else if (error.request) {
          console.error("Nenhuma resposta recebida:", error.request);
          this.error = "Erro de comunicação com o servidor. Tente novamente.";
        } else {
          console.error("Erro ao configurar a requisição:", error.message);
          this.error = "Erro ao cadastrar produto. Verifique sua conexão.";
        }
      }
    },
    async excluirProduto() {
      if (this.produtoParaExcluir !== null) {
        try {
          await api.delete(`/Produtos/${this.products[this.produtoParaExcluir].id}`);
          this.products.splice(this.produtoParaExcluir, 1);
          this.produtoParaExcluir = null;
          this.showDeleteModal = false; // Fecha o modal de exclusão
        } catch (error) {
          console.error("Erro ao excluir produto:", error);
        }
      }
    },
    formatarData(data) {
      const date = new Date(data);
      return date.toLocaleDateString('pt-BR');
    },
    formatarEcommerce(product) {
      const ecommerces = [];
      if (product.mercadoLivre) ecommerces.push('Mercado Livre');
      if (product.aliExpress) ecommerces.push('AliExpress');
      if (product.shopee) ecommerces.push('Shopee');
      return ecommerces.join(', ');
    },
    selecionarProdutoParaExcluir(index) {
      this.produtoParaExcluir = index;
      this.showDeleteModal = true;  // Abre o modal de exclusão
    },
    fecharModalCadastro() {
      this.showModal = false;
      // Certifique-se de que o modal está oculto completamente
      const modalElement = bootstrap.Modal.getInstance(document.getElementById('escolhaEcommerceModal'));
      if (modalElement) modalElement.hide();
    },
    fecharModalExcluir() {
      this.showDeleteModal = false;
      // Certifique-se de que o modal está oculto completamente
      const modalElement = bootstrap.Modal.getInstance(document.getElementById('deleteCursoModal'));
      if (modalElement) modalElement.hide();
    }
  }
};
</script>

<style>
.product-list {
  background-color: #FFBD59;
  padding: 20px;
  border-radius: 25px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  border: 1px solid #dddddd;
  font-weight: bold;
  padding: 8px;
  text-align: center;
  height: 30px; 
}

th {
  background-color: white;
}

.modal-title {
  margin-left: 100px;
}

.form-group {
  margin: 10px 0;
}

.error {
  color: red;
  font-weight: bold;
}
</style>
