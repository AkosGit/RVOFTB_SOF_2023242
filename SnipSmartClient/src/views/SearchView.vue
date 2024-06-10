<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import TheWelcome from '../components/TheWelcome.vue'
import type { VueCookies } from 'vue-cookies'
import { inject, ref, watch, type Ref } from 'vue'
import { NSelect, NButton, NFlex } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'
import type { SelectMixedOption } from 'naive-ui/es/select/src/interface'
import { useSnippetStore } from '@/stores/snippets'

const router = useRouter()
const clients = useClientStore()
const snippets = useSnippetStore()

const contentSubTypeValue = ref(undefined)
var contentSubTypeOptions: Ref<Array<SelectMixedOption>> = ref([])

const contentTypeValue = ref(null)
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
function search(Event: Event) {
  console.log('Seeearch')
  snippets.isSearchInProgress = true
  snippets.snippetSource = 'SEARCH'
  snippets.searchTargetType = String(contentTypeValue.value)
  snippets.searchTargetSubType = String(contentSubTypeValue.value)
  router.push({ name: 'home' })
  router.forward()
}
</script>

<template>
  <main>
    <n-flex :vertical="true">
      <h4>Content type:</h4>
      <n-select v-model:value="contentTypeValue" :options="contentTypeOptions" />
      <h4>Content subtype:</h4>
      <n-select v-model:value="contentSubTypeValue" :options="contentSubTypeOptions" />
      <n-button @onclick="search">Search!</n-button>
    </n-flex>
  </main>
</template>
