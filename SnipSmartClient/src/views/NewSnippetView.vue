<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import type { SnippetModel } from '@/models/SnippetModel'
import { useSnippetStore } from '@/stores/snippets'
import SnippetComponent from '@/components/SnippetComponent.vue'

const clients = useClientStore()
const snippetsStore = useSnippetStore()

function submit(snippet: SnippetModel, tags: Array<string>, collectionName?: string) {
  clients.snippet
    .AddNewSnippet(
      snippet.link,
      snippet.contentType,
      snippet.contentSubType,
      snippet.content,
      snippet.description
    )
    .then((snippetID: string) => {
      tags.map(async (tagName) => {
        await clients.tag.AddNewTag(tagName, snippetID).catch((error: any) => {
          console.error('Error:', error)
        })
        if (collectionName !== undefined && collectionName !== null) {
          await clients.collection
            .AddSnippetToCollection(collectionName, snippetID)
            .catch((error: any) => {
              console.error('Error:', error)
            })
        }
      })
      alert('Snippet created sucessfully!')
      snippetsStore.isSearchInProgress = true
    })
    .catch((error: any) => {
      console.error('Error:', error)
    })
}
</script>

<template>
  <SnippetComponent :on-submit-clicked="submit" title="Create new Snippet" submit-message="Add" />
</template>
