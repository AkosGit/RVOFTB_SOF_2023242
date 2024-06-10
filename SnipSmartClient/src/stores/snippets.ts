import type { TagModel } from '@/models/TagModel'
import type { SnippetModel } from '../models/SnippetModel'
import { defineStore } from 'pinia'

export const useSnippetStore = defineStore('snippets', {
  state: () => ({
    snippets: new Array<SnippetModel>(),
    tags: new Array<TagModel>(),
    snippetSource: 'ALL',
    collectionID: '',
    isSearchInProgress: false,
    searchTargetType: '',
    searchTargetSubType: '',
    DistinctTagNames: []
  }),
  /*getters: {
        doubleCount: (state) => state.count * 2,
      },*/
  actions: {}
})
