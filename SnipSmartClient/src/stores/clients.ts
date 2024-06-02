import { defineStore } from 'pinia'
import { AuthClient } from '../clients/AuthClient'
import { CollectionClient } from '../clients/CollectionClient'
import { SnippetClient } from '../clients/SnippetClient'
import { TagClient } from '../clients/TagClient'
import { inject } from 'vue'
import type { VueCookies } from 'vue-cookies'

export const useClientStore = defineStore('clients', {
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
      this.tag.jwt = token
    }
  }
})
