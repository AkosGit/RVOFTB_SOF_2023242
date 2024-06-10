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

function submit(snippet: SnippetModel, collectionName: string, tags: Array<string>) {
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
      clients.tag.RemoveTagsFromSnippet(snippet.snippetID).then(() => {
        tags.map(async (tagName) => {
          await clients.tag.AddNewTag(tagName, snippet.snippetID).catch((error: any) => {
            console.error('Error:', error)
          })
          if (collectionName != undefined) {
            if (snippetsStore.collectionID != '' || snippetsStore.collectionID != undefined) {
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
          }
        })
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
  <main>
    <SnippetComponent
      :on-submit-clicked="submit"
      title="Edit snippet"
      submit-message="Edit"
      :old-snippet="snippetsStore.CurrentSnippet"
    />
  </main>
</template>
