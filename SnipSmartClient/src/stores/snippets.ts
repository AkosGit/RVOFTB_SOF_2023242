import type { SnippetModel } from '../models/SnippetModel'
import { defineStore } from 'pinia'

export const useSnippetStore = defineStore('snippets', {
  state: () => ({
    snippets: new Array<SnippetModel>()
  }),
  /*getters: {
        doubleCount: (state) => state.count * 2,
      },*/
  actions: {}
})
