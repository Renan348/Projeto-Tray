<template>
  <div id="app">
    <Navbar />
    <div class="container">
      <ProductList />
    </div>
  </div>
</template>

<script>
import ProductList from '../components/ProductList.vue';

export default {
  name: 'PerfilUser',
  components: {
    ProductList
  }
}
</script>

<style>
.container {
  margin: 20px;
}

#app {
  margin: 100px 45px;
}
</style>
