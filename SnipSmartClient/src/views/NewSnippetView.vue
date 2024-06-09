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
import { javascript } from '@codemirror/lang-javascript'
import { python } from '@codemirror/lang-python'
import type { LanguageSupport } from '@codemirror/language'
import type { Extension } from '@codemirror/state'
import type { ViewUpdate } from '@codemirror/view'
import type { SnippetModel } from '@/models/SnippetModel'

const router = useRouter()
const route = useRoute()
const clients = useClientStore()
const $cookies = inject<VueCookies>('$cookies')
const link = ref('')
const description = ref('')
const code = ref('')
const tags = ref([])
//const dark: Ref<boolean> = ref(window.matchMedia('(prefers-color-scheme: dark)').matches)
const dark: Ref<boolean> = ref(false)

var lang: LanguageSupport = md()

const contentSubTypeValue = ref(undefined)
var contentSubTypeOptions: Ref<Array<SelectMixedOption>> = ref([])

const contentTypeValue = ref(null)
watch(contentTypeValue, async (contentType) => {
  if (contentType == 'code') {
    contentSubTypeOptions.value = [
      {
        label: 'Javascript',
        value: 'js',
        disabled: false
      },
      {
        label: 'Python',
        value: 'python',
        disabled: false
      }
    ]
  }
  if (contentType == 'text') {
    contentSubTypeOptions.value = [
      {
        label: 'plain text',
        value: 'plain',
        disabled: false
      },
      {
        label: 'Markdown',
        value: 'md',
        disabled: false
      },
      {
        label: 'JSON',
        value: 'json',
        disabled: false
      }
    ]
  }
})

const contentTypeOptions = [
  {
    label: 'code',
    value: 'code',
    disabled: false
  },
  {
    label: 'text',
    value: 'text',
    disabled: false
  }
]

watch(contentSubTypeValue, async (newContentSubTypeValue) => {
  switch (String(newContentSubTypeValue)) {
    case 'md': {
      lang = md()
      break
    }
    case 'json': {
      lang = json()
      break
    }
    case 'js': {
      lang = javascript()
      break
    }
    case 'python': {
      lang = python()
    }
  }
})
function submit() {
  clients.snippet
    .AddNewSnippet(
      link.value,
      String(contentTypeValue.value),
      String(contentSubTypeValue.value),
      code.value,
      description.value
    )
    .then((snippetID: string) => {
      tags.value.map(async (tagName) => {
        await clients.tag.AddNewTag(tagName, snippetID).catch((error: any) => {
          console.error('Error:', error)
        })
      })
      alert('Snippet created sucessfully!')
    })
    .catch((error: any) => {
      console.error('Error:', error)
    })
}
</script>

<template>
  <main>
    <n-card title="Create new snippet">
      <n-flex vertical>
        <h4>Content source link:</h4>
        <n-input v-model:value="link" type="text" placeholder="link" />
        <h4>Description:</h4>
        <n-mention type="textarea" v-model:value="description" placeholder="desciption" />
        <h4>Content type:</h4>
        <n-select v-model:value="contentTypeValue" :options="contentTypeOptions" />
        <h4>Content subtype:</h4>
        <n-select v-model:value="contentSubTypeValue" :options="contentSubTypeOptions" />
        <h4>Content:</h4>
        <code-mirror v-model="code" basic :dark="dark" :lang="lang" />
        <h4>Tags:</h4>
        <n-dynamic-tags v-model:value="tags" />
        <n-button @click="submit()">Create snippet</n-button>
      </n-flex>
    </n-card>
  </main>
</template>
