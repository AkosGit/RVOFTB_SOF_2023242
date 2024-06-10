import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import { useClientStore } from '@/stores/clients'
import type { VueCookies } from 'vue-cookies'
import { inject } from 'vue'
import RegisterView from '@/views/RegisterView.vue'
import CollectionsView from '@/views/CollectionsView.vue'
import SearchView from '@/views/SearchView.vue'
import NewSnippetView from '@/views/NewSnippetView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView
    },
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/collections',
      name: 'collections',
      component: CollectionsView
    },
    {
      path: '/search',
      name: 'search',
      component: SearchView
    },
    {
      path: '/newsnippet',
      name: 'newsnippet',
      component: NewSnippetView
    }
  ]
})

router.beforeEach(async (to, from) => {
  const clients = useClientStore()
  const $cookies = inject<VueCookies>('$cookies')
  const token = $cookies?.get('token')
  clients.updateJWT(token)

  //if no token go to login
  if (token == null && to.name !== 'login' && to.name !== 'register') {
    return { name: 'login' }
  }

  //if login is navigated to when logged in nav to home
  if (token != null && to.name == 'login') {
    return { name: 'home' }
  }
})

export default router
