import './assets/main.css'

import { createApp } from 'vue'
import { createPinia, defineStore } from 'pinia'

import App from './App.vue'
import router from './router'
import { AuthClient } from './clients/AuthClient'
import { CollectionClient } from './clients/CollectionClient'
import { SnippetClient } from './clients/SnippetClient'
import { TagClient } from './clients/TagClient'

const app = createApp(App)

app.use(createPinia())
app.use(router)

export const useClientStore = defineStore('Clients', {
  state: () => ({
    auth: new AuthClient(),
    collection: new CollectionClient(),
    snippet: new SnippetClient(),
    tag: new TagClient()
  }),
  /*getters: {
      doubleCount: (state) => state.count * 2,
    },*/
  actions: {
    updateJWT(token: string) {
      this.auth.jwt = token
      this.collection.jwt = token
      this.snippet.jwt = token
      this.tag.jwt = token
    }
  }
})

app.mount('#app')
