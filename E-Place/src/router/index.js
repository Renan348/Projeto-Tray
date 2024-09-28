import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import PerfilUser from '../views/PerfilUser.vue';
import { isAuthenticated } from './auth'; // Importa a função de verificação de autenticação

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/perfil',
      name: 'perfilUser',
      component: PerfilUser,
      meta: { requiresAuth: true } // Adiciona meta para identificar rotas que requerem autenticação
    },
  ]
});

// Adiciona uma guarda global para verificar autenticação
router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!isAuthenticated()) {
      next('/'); // Redireciona para a página inicial se não estiver autenticado
    } else {
      next(); // Prossegue para a rota protegida
    }
  } else {
    next(); // Prossegue para a rota normalmente
  }
});

export default router;