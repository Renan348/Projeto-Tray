<template>
  <div>
    <h2 style="color: white; margin-left: 220px;">Histórico de Produtos</h2>
    <div class="product-list">
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
          </tr>
        </thead>
        <tbody>
          <tr v-for="(product, index) in products" :key="product.id" style="background-color: aliceblue;">
            <td>{{ product.id }}</td>
            <td>{{ product.nome }}</td>
            <td>{{ product.marca }}</td>
            <td v-if="product.preco">R$ {{ product.preco | currency }}</td>
            <td v-else></td>
            <td>{{ product.ecommerce.join(', ') }}</td>
            <td>{{ product.estoque }}</td>
            <td>{{ formatarData(product.dataCadastro) }}</td>
            <td><i class="bi bi-trash3" @click="selecionarProdutoParaExcluir(index)" data-bs-toggle="modal" data-bs-target="#deleteCursoModal"></i></td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal de cadastro de produtos -->
    <div id="escolhaEcommerceModal" class="modal fade">
      <div class="modal-dialog">
        <div class="modal-content">
          <form @submit.prevent="cadastrarProduto">
            <div class="modal-header">
              <h4 class="modal-title" style="color: #FFBD59; font-weight: bold; font-size: 30px;">Cadastre seu produto</h4>
            </div>
            <div class="modal-body">
              <div class="form-group">
                <label for="nomeModal">Modelo:</label>
                <input type="text" class="form-control" id="nomeModal" v-model="produto.nome" placeholder="Digite o modelo do produto">
              </div>
              <div class="form-group">
                <label for="marcaModal">Marca:</label>
                <input type="text" class="form-control" id="marcaModal" v-model="produto.marca" placeholder="Digite a marca do produto">
              </div>
              <div class="form-group">
                <label for="precoModal">Preço:</label>
                <input type="text" class="form-control" id="precoModal" v-model="produto.preco" @input="formatarPreco" placeholder="Digite o preço do produto">
              </div>
              <div class="form-group">
                <label for="estoqueModal">Quantidade em Estoque:</label>
                <input type="number" class="form-control" id="estoqueModal" v-model.number="produto.estoque" placeholder="Digite a quantidade em estoque">
              </div>
              <div class="form-group">
                <label>Ecommerce:</label><br>
                <div class="form-check form-check-inline">
                  <input class="form-check-input" type="checkbox" id="aliexpress" value="AliExpress" v-model="produto.ecommerce">
                  <label class="form-check-label" for="aliexpress">AliExpress</label>
                </div>
                <div class="form-check form-check-inline">
                  <input class="form-check-input" type="checkbox" id="mercadolivre" value="Mercado Livre" v-model="produto.ecommerce">
                  <label class="form-check-label" for="mercadolivre">Mercado Livre</label>
                </div>
                <div class="form-check form-check-inline">
                  <input class="form-check-input" type="checkbox" id="shopee" value="Shopee" v-model="produto.ecommerce">
                  <label class="form-check-label" for="shopee">Shopee</label>
                </div>
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Fechar</button>
              <button type="submit" class="btn" style="background-color: #FFBD59; color: white;">Cadastrar Produto</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal de exclusão -->
  <div id="deleteCursoModal" class="modal fade">
    <div class="modal-dialog">
      <div class="modal-content">
        <form @submit.prevent="excluirProduto">
          <div class="modal-header">
            <h4 class="modal-title">Delete o Produto</h4>
          </div>
          <div class="modal-body">
            <p>Tem certeza que deseja excluir este produto?</p>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Fechar</button>
            <button type="submit" class="btn btn-danger" data-bs-dismiss="modal">Excluir</button>
          </div>
        </form>
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
      products: [],  // Lista de produtos
      produto: {     // Produto atual para cadastro
        nome: '',
        marca: '',
        preco: '',
        estoque: '',
        ecommerce: [],
        dataCadastro: ''
      },
      produtoParaExcluir: null // Produto selecionado para exclusão
    };
  },
  created() {
    this.getProducts(); // Carrega os produtos ao criar o componente
  },
  methods: {
    formatarPreco(event) {
      let value = event.target.value;
      value = value.replace(/[^\d,]/g, '');
      let parts = value.split(',');
      if (parts.length > 2) {
        parts = [parts[0], parts.slice(1).join('')];
      }
      value = parts.join(',');
      event.target.value = value;
      this.produto.preco = value;
    },
    getProducts() {
      api
        .get("/Produtos")
        .then((response) => {
          this.products = response.data; // Atualiza a lista de produtos
        })
        .catch((error) => {
          console.error("Erro ao obter produtos:", error);
        });
    },
    cadastrarProduto() {
      if (!this.produto.nome || !this.produto.marca || !this.produto.preco || !this.produto.estoque || this.produto.ecommerce.length === 0) {
        alert('Por favor, preencha todos os campos obrigatórios.');
        return;
      }

      api
        .post("/Produtos", this.produto)
        .then(() => {
          this.getProducts(); // Atualiza a lista após o cadastro
          $('#escolhaEcommerceModal').modal('hide');
        })
        .catch((error) => {
          console.error("Erro ao cadastrar produto:", error);
        });
    },
    updateProduct(id) {
      const produtoAtualizado = {
        ...this.produto, // Aqui você poderia ajustar para os dados de atualização específicos
        id
      };

      api
        .put(`/Produtos/${id}`, produtoAtualizado)
        .then(() => {
          this.getProducts(); // Atualiza a lista após a atualização
        })
        .catch((error) => {
          console.error("Erro ao atualizar produto:", error);
        });
    },
    formatarData(data) {
      const date = new Date(data);
      return date.toLocaleDateString('pt-BR');
    },
    selecionarProdutoParaExcluir(index) {
      this.produtoParaExcluir = index;
    },
    excluirProduto() {
      if (this.produtoParaExcluir !== null) {
        this.products.splice(this.produtoParaExcluir, 1);
        this.produtoParaExcluir = null;
      }
    }
  },
  filters: {
    currency(value) {
      if (!value || value === '0,00') return '';
      return new Intl.NumberFormat('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(value);
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
  margin-bottom: 15px;
}

.btn {
  margin-left: 10px;
}

.btn:hover {
  transform: scale(1.1);
  transition: 0.5s;
}
</style>
