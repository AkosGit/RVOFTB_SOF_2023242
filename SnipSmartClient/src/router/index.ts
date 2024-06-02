import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import { useClientStore } from '@/stores/clients'
import type { VueCookies } from 'vue-cookies'
import { inject } from 'vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    }
  ]
})

router.beforeEach(async (to, from) => {
  const clients = useClientStore()
  const $cookies = inject<VueCookies>('$cookies')
  const token = $cookies?.get('token')
  console.log(token)
  clients.updateJWT(token)

  if (token == null && to.name !== 'login') {
    return { name: 'login' }
  }
})

export default router
