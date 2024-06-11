<script setup lang="ts">
import { inject, ref, watch, type Ref } from 'vue'
import { NSelect, NButton, NFlex } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'
import type { SelectMixedOption } from 'naive-ui/es/select/src/interface'
import { useSnippetStore } from '@/stores/snippets'

const router = useRouter()
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
watch(contentTypeValue, (contentType) => {
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

function search() {
  if (!contentTypeValue.value || !contentSubTypeValue.value) {
    alert('Both field have to be field!')
  } else {
    snippets.isSearchInProgress = true
    snippets.snippetSource = 'SEARCH'
    snippets.searchTargetType = String(contentTypeValue.value)
    snippets.searchTargetSubType = String(contentSubTypeValue.value)
    router.push({ name: 'home' })
    router.forward()
  }
}
</script>

<template>
  <n-flex :vertical="true" style="max-width: 30vw; min-width: 15vw">
    <h4>Content type:</h4>
    <n-select v-model:value="contentTypeValue" :options="contentTypeOptions" />
    <h4>Content subtype:</h4>
    <n-select v-model:value="contentSubTypeValue" :options="contentSubTypeOptions" />
    <n-button @click="search()">Search!</n-button>
  </n-flex>
</template>
