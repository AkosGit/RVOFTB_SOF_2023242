<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import { useRouter, useRoute } from 'vue-router'
import type { SnippetModel } from '@/models/SnippetModel'
import { useSnippetStore } from '@/stores/snippets'
import SnippetComponent from '@/components/SnippetComponent.vue'

const router = useRouter()
const route = useRoute()
const clients = useClientStore()
const snippetsStore = useSnippetStore()

function submit(snippet: SnippetModel, tags: Array<string>, collectionName?: string) {
  clients.snippet
    .EditSnippet(
      snippet.link,
      snippet.contentType,
      snippet.contentSubType,
      snippet.content,
      snippet.description,
      snippet.snippetID,
      snippet.collectionID
    )
    .then(() => {
      clients.tag.RemoveTagsFromSnippet(snippet.snippetID).then(async () => {
        if (tags !== undefined && tags !== null) {
          tags.map(async (tagName) => {
            await clients.tag.AddNewTag(tagName, snippet.snippetID).catch((error: any) => {
              console.error('Error:', error)
            })
          })
        }

        console.log('collectionName')
        console.log(collectionName)
        if (collectionName !== undefined && collectionName !== null && collectionName !== '') {
          console.log(snippetsStore.CurrentSnippet.collectionID)
          if (
            snippetsStore.CurrentSnippet.collectionID !== '' &&
            snippetsStore.CurrentSnippet.collectionID !== undefined &&
            snippetsStore.CurrentSnippet.collectionID !== null
          ) {
            console.log('removing old collection')
            await clients.collection.RemoveSnippetFromCollectionById(
              snippetsStore.CurrentSnippet.collectionID,
              snippet.snippetID
            )
          }
          await clients.collection
            .AddSnippetToCollection(collectionName, snippet.snippetID)
            .catch((error: any) => {
              console.error('Error:', error)
            })

          alert('Snippet modified sucessfully!')
          snippetsStore.isSearchInProgress = true
        } else {
          alert('Snippet modified sucessfully!')
          snippetsStore.isSearchInProgress = true
        }
      })
    })
    .catch((error: any) => {
      console.error('Error:', error)
    })
}
</script>

<template>
  <SnippetComponent
    :on-submit-clicked="submit"
    title="Edit snippet"
    submit-message="Edit"
    :old-snippet="snippetsStore.CurrentSnippet"
  />
</template>
