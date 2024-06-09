import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginRegisterView from '../views/LoginRegisterView.vue'
import { useAuthStore } from '@/stores/auth';
import clientAxios from '@/utils/axiosConfig';

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

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore();
  const token = localStorage.getItem('token');
  if(token) {
    const res = await getUser(token);
    if (res.status == 200) {
      authStore.login(res.data)
      localStorage.setItem("token", res.data.token)
    } else {
      localStorage.removeItem("token")
    }
  }
  
  const loggedIn = authStore.user

  if (to.matched.some(record => record.meta.requiresAuth) && !loggedIn) {
    next('/login');
  } else if (to.matched.some(record => record.meta.onlyWhenLoggedOut) && loggedIn) {
    next('/');
  } else {
    next();
  }
});

const getUser = async (token) => {
  return await clientAxios.get('/users/getUser', {
      headers: {
          'Authorization': `Bearer ${token}`
      }
  })
}

export default router
