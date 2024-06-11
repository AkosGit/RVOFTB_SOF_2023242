<script setup lang="ts">
import type { SnippetModel } from '@/models/SnippetModel'
import { useClientStore } from '@/stores/clients'
import { inject, ref, watch, type Ref } from 'vue'
import { NButton, NCard, NFlex, NInput, NCode, NMention, NDynamicTags, NSelect } from 'naive-ui'
import type { SelectMixedOption } from 'naive-ui/es/select/src/interface'
import CodeMirror from 'vue-codemirror6'
import { markdown as md } from '@codemirror/lang-markdown'
import { json } from '@codemirror/lang-json'
import { javascript, snippets } from '@codemirror/lang-javascript'
import { python } from '@codemirror/lang-python'
import type { CollectionModel } from '@/models/CollectionModel'
import type { TagModel } from '@/models/TagModel'
import { useRouter } from 'vue-router'

const props = defineProps<{
  oldSnippet?: SnippetModel
  title: string
  submitMessage: string
  onSubmitClicked: (snippet: SnippetModel, tags: Array<string>, collectionName?: string) => void
}>()

const router = useRouter()
const clients = useClientStore()
const link = ref('')
const description = ref('')
const code = ref('')
const tags = ref([])
const collectionsSelected = ref(undefined)
const collectionOptions: Ref<Array<SelectMixedOption>> = ref([])
//const dark: Ref<boolean> = ref(window.matchMedia('(prefers-color-scheme: dark)').matches)
const dark: Ref<boolean> = ref(false)

var lang = ref(md())

const contentSubTypeValue = ref('')
var contentSubTypeOptions: Ref<Array<SelectMixedOption>> = ref([])

const contentTypeValue = ref(null)

if (props.oldSnippet !== null && props.oldSnippet !== undefined) {
  //@ts-ignore
  contentSubTypeValue.value = props.oldSnippet.contentSubType
  SetLang(props.oldSnippet.contentSubType)
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
function DeleteSnippet() {
  clients.snippet
    .RemoveSnippet(String(props.oldSnippet?.snippetID))
    .then(() => {
      alert('Snippet deleted sucessfully')
      router.push({ name: 'home' })
      router.forward()
    })
    .catch(() => {
      alert('A network error has occured!')
    })
}
function SetLang(subContentType: string) {
  switch (subContentType) {
    case 'md': {
      //@ts-ignore
      lang.value = md()
      break
    }
    case 'json': {
      //@ts-ignore
      lang.value = json()
      break
    }
    case 'js': {
      //@ts-ignore
      lang.value = javascript()
      break
    }
    case 'python': {
      //@ts-ignore
      lang.value = python()
    }
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
  contentSubTypeValue.value = ''
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
  SetLang(String(newContentSubTypeValue))
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
  console.log('selected collection')
  console.log(collectionsSelected.value)
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
      tags.value,
      collectionsSelected.value
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
      tags.value,
      collectionsSelected.value
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
        <n-button
          v-if="props.oldSnippet !== null && props.oldSnippet !== undefined"
          @click="DeleteSnippet()"
          type="error"
          >Delete snippet</n-button
        >
        <n-button @click="submit()">{{ submitMessage }}</n-button>
      </n-flex>
    </n-card>
    <div style="height: 10vh">
      <h4 style="margin: 5px; margin-left: 0px">Snippet Content:</h4>
      <code-mirror
        :key="contentSubTypeValue"
        v-model="code"
        basic
        :dark="dark"
        :lang="lang"
        style="height: 100%"
      />
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
