<script setup lang="ts">
import type { SnippetModel } from '@/models/SnippetModel'
import { useClientStore } from '@/stores/clients'
import { inject, ref, watch, type Ref } from 'vue'
import { NButton, NCard, NFlex, NInput, NCode, NMention, NDynamicTags, NSelect } from 'naive-ui'
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
import { useSnippetStore } from '@/stores/snippets'
import type { CollectionModel } from '@/models/CollectionModel'
import type { TagModel } from '@/models/TagModel'

const props = defineProps<{
  oldSnippet?: SnippetModel
  title: string
  submitMessage: string
  onSubmitClicked: (snippet: SnippetModel, collectionName: string, tags: Array<string>) => void
}>()

const clients = useClientStore()
const link = ref('')
const description = ref('')
const code = ref('')
const tags = ref([])
const collectionsSelected = ref(undefined)
const collectionOptions: Ref<Array<SelectMixedOption>> = ref([])
//const dark: Ref<boolean> = ref(window.matchMedia('(prefers-color-scheme: dark)').matches)
const dark: Ref<boolean> = ref(false)

var lang: LanguageSupport = md()

const contentSubTypeValue = ref(undefined)
var contentSubTypeOptions: Ref<Array<SelectMixedOption>> = ref([])

const contentTypeValue = ref(null)

if (props.oldSnippet != null) {
  //@ts-ignore
  contentSubTypeValue.value = props.oldSnippet.contentSubType
  //@ts-ignore
  contentTypeValue.value = props.oldSnippet.contentType
  SetContentType(props.oldSnippet.contentType)
  code.value = props.oldSnippet.content
  description.value = props.oldSnippet.description
  link.value = props.oldSnippet.link
  clients.tag.GetTagsFromSnippet(props.oldSnippet.snippetID).then((oldtags: Array<TagModel>) => {
    oldtags.forEach((tag) => {
      //@ts-ignore
      tags.value.push(tag.tagName)
    })
  })
  if (props.oldSnippet.collectionID !== '' || props.oldSnippet.collectionID !== undefined) {
    clients.collection
      .GetCollectionById(props.oldSnippet.collectionID)
      .then((collection: CollectionModel) => {
        //@ts-ignore
        collectionsSelected.value = collection.collectionName
      })
  }
}
function SetContentType(contentType: string) {
  if (contentType == 'code') {
    contentSubTypeOptions.value = [
      {
        label: 'js',
        value: 'js',
        disabled: false
      },
      {
        label: 'python',
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
        label: 'md',
        value: 'md',
        disabled: false
      },
      {
        label: 'json',
        value: 'json',
        disabled: false
      }
    ]
  }
}
watch(contentTypeValue, async (contentType) => {
  contentSubTypeValue.value = undefined
  SetContentType(String(contentType))
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
function GetCollections() {
  clients.collection
    .GetCollections()
    .then((d: Array<CollectionModel>) => {
      console.log(d)
      d.forEach((collection) => {
        collectionOptions.value.push({
          label: collection.collectionName,
          value: collection.collectionName,
          disabled: false
        })
      })
    })
    .catch((error: any) => {})
}
function submit() {
  if (props.oldSnippet != undefined) {
    var collectionID = ''
    if (props.oldSnippet.collectionID != null || props.oldSnippet.collectionID != undefined) {
      collectionID = props.oldSnippet.collectionID
    }
    props.onSubmitClicked(
      {
        snippetID: props.oldSnippet.snippetID,
        collectionID: collectionID,
        link: link.value,
        contentType: String(contentTypeValue.value),
        contentSubType: String(contentSubTypeValue.value),
        content: code.value,
        description: description.value
      } as SnippetModel,
      String(collectionsSelected.value),
      tags.value
    )
  } else {
    props.onSubmitClicked(
      {
        snippetID: '',
        collectionID: '',
        link: link.value,
        contentType: String(contentTypeValue.value),
        contentSubType: String(contentSubTypeValue.value),
        content: code.value,
        description: description.value
      } as SnippetModel,
      String(collectionsSelected.value),
      tags.value
    )
  }
}
GetCollections()
</script>

<template>
  <div class="editor">
    <n-card :title="title" style="width: 100%; height: 100%">
      <n-flex vertical style="width: 100%; height: 100%">
        <h4>Content source link:</h4>
        <n-input v-model:value="link" type="text" placeholder="link" />
        <h4>Description:</h4>
        <n-mention type="textarea" v-model:value="description" placeholder="desciption" />
        <h4>Content type:</h4>
        <n-select v-model:value="contentTypeValue" :options="contentTypeOptions" />
        <h4>Content subtype:</h4>
        <n-select v-model:value="contentSubTypeValue" :options="contentSubTypeOptions" />
        <h4>Collection:</h4>
        <n-select
          v-model:value="collectionsSelected"
          :fallback-option="false"
          :options="collectionOptions"
        />
        <h4>Tags:</h4>
        <n-dynamic-tags v-model:value="tags" />
        <n-button @click="submit()">{{ submitMessage }}</n-button>
      </n-flex>
    </n-card>
    <div style="height: 10vh">
      <code-mirror v-model="code" basic :dark="dark" :lang="lang" style="height: 100%" />
    </div>
  </div>
</template>

<style scoped>
.editor {
  display: grid;
  grid-template-columns: 1fr;
  gap: 10px;
  width: 100%;
  height: 100%;
}

/* Desktop view */
@media (min-width: 768px) {
  .editor {
    grid-template-columns: 1fr 1fr;
  }
}
</style>
