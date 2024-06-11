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
import Edit from '@/views/NewSnippetView.vue'
import EditSnippetView from '@/views/EditSnippetView.vue'
import NetworkErrorView from '@/views/NetworkErrorView.vue'

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
    },
    {
      path: '/editsnippet',
      name: 'editsnippet',
      component: EditSnippetView
    },
    {
      path: '/networkerror',
      name: 'networkerror',
      component: NetworkErrorView
    }
  ]
})

router.beforeEach(async (to, from, next) => {
  const clients = useClientStore()
  const $cookies = inject<VueCookies>('$cookies')
  const token = $cookies?.get('token')
  clients.updateJWT(token)

  if (from.name !== 'networkerror') {
    try {
      await clients.auth.Health()
    } catch (e) {
      console.log('network error', e)
      next({ name: 'networkerror' })
    }
  }

  if (token == null && to.name !== 'login' && to.name !== 'register') {
    next({ name: 'login' })
  }

  if (token != null && to.name === 'login') {
    next({ name: 'collections' })
  }

  next()
})

export default router
