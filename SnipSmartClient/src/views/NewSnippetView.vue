<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import TheWelcome from '../components/TheWelcome.vue'
import type { VueCookies } from 'vue-cookies'
import { inject, ref, watch, type Ref } from 'vue'
import { NButton, NCard, NFlex, NInput, NCode, NMention, NDynamicTags, NSelect } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'
import type { SelectMixedOption } from 'naive-ui/es/select/src/interface'
import CodeMirror from 'vue-codemirror6'
// CodeMirror extensions
import { markdown as md } from '@codemirror/lang-markdown'
import { json } from '@codemirror/lang-json'
import { javascript, snippets } from '@codemirror/lang-javascript'
import { python } from '@codemirror/lang-python'
import type { LanguageSupport } from '@codemirror/language'
import type { Extension } from '@codemirror/state'
import type { ViewUpdate } from '@codemirror/view'
import type { SnippetModel } from '@/models/SnippetModel'
import { useSnippetStore } from '@/stores/snippets'
import type { CollectionModel } from '@/models/CollectionModel'
import SnippetComponent from '@/components/SnippetComponent.vue'

const router = useRouter()
const route = useRoute()
const clients = useClientStore()
const snippetsStore = useSnippetStore()

function submit(snippet: SnippetModel, collectionName: string, tags: Array<string>) {
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
        if (collectionName != undefined) {
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
  <main>
    <SnippetComponent :on-submit-clicked="submit" title="Create new Snippet" submit-message="Add" />
  </main>
</template>
