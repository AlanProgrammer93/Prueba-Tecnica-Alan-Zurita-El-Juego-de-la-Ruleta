import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginRegisterView from '../views/LoginRegisterView.vue'
import { useAuthStore } from '@/stores/auth';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { requiresAuth: true }
    },
    {
      path: '/login',
      name: 'login',
      component: LoginRegisterView,
      meta: { public: true, onlyWhenLoggedOut: true }
    }
  ]
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const loggedIn = authStore.user

  if (to.matched.some(record => record.meta.requiresAuth) && !loggedIn) {
    next('/login');
  } else if (to.matched.some(record => record.meta.onlyWhenLoggedOut) && loggedIn) {
    next('/');
  } else {
    next();
  }
});

export default router
