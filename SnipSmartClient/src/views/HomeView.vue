<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import TheWelcome from '../components/TheWelcome.vue'
import type { VueCookies } from 'vue-cookies'
import { inject, ref, watch, type Ref } from 'vue'
import { NFlex, NCard, NSelect } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'
import { useSnippetStore } from '@/stores/snippets'
import type { SnippetModel } from '@/models/SnippetModel'
import type { TagModel } from '@/models/TagModel'
import type { SelectMixedOption } from 'naive-ui/es/select/src/interface'

const router = useRouter()
const route = useRoute()
const clients = useClientStore()
const $cookies = inject<VueCookies>('$cookies')
const snippets = useSnippetStore()
const tagsSelected = ref([])
const tagOptions: Ref<Array<SelectMixedOption>> = ref([])
const snippetsSelected: Ref<Array<SnippetModel>> = ref([])

function GetSnippetsInCollection(collectionID: string) {
  clients.collection
    .GetSnippetsFromCollection(collectionID)
    .then((s: Array<SnippetModel>) => {
      console.log(s)
      snippets.snippets = s
    })
    .catch((error: any) => {})
}
function OtherSnippets() {
  clients.collection
    .GetSnippetsWithoutCollection()
    .then((s: Array<SnippetModel>) => {
      console.log(s)
      snippets.snippets = s
    })
    .catch((error: any) => {})
}
function GetAllSnippets() {
  clients.snippet
    .GetAllSnippets()
    .then((s: Array<SnippetModel>) => {
      console.log(s)
      snippets.snippets = s
    })
    .catch((error: any) => {})
}
function GetSnippetsByContentType() {
  clients.snippet
    .GetSnippetsByContentType(snippets.searchTargetType, snippets.searchTargetSubType)
    .then((s: Array<SnippetModel>) => {
      console.log(s)
      snippets.snippets = s
    })
    .catch((error: any) => {})
}
function LoadTagOptions() {
  tagOptions.value = [{ label: 'ALL', value: 'ALL', disabled: false }]
  snippets.DistinctTagNames.forEach((tag) => {
    var obj = { label: tag, value: tag, disabled: false }
    //@ts-ignore
    tagOptions.value.push(obj)
  })
}
function GetAllTags() {
  clients.tag
    .GetAllTags()
    .then((s: Array<TagModel>) => {
      console.log(s)
      snippets.tags = s
    })
    .catch((error: any) => {})
  clients.tag
    .GetDistinctNames()
    .then((s: Array<String>) => {
      //@ts-ignore
      snippets.DistinctTagNames = s
      LoadTagOptions()
    })
    .catch((error: any) => {})
}
LoadTagOptions()
watch(tagsSelected, (newtagsSelected) => {
  //@ts-ignore
  var is_all = false
  tagsSelected.value.forEach((tag) => {
    //@ts-ignore
    if (tag == 'ALL') {
      is_all = true
    }
  })
  if (is_all) {
    snippetsSelected.value = snippets.snippets
  } else {
    var foundSnippets: Array<SnippetModel> = []
    snippets.snippets.forEach((snippet) => {
      snippets.tags.forEach((tag) => {
        //@ts-ignore
        if (tag.snippetID == snippet.snippetID && tagsSelected.value.includes(tag.tagName)) {
          foundSnippets.push(snippet)
        }
      })
    })
    snippetsSelected.value = Array.from(new Set(foundSnippets))
  }
})

//runtime
if (snippets.isSearchInProgress) {
  //@ts-ignore
  tagOptions.value = [{ label: 'ALL', value: 'ALL', disabled: false }]
  GetAllTags()

  if (snippets.snippetSource == 'ALL') {
    GetAllSnippets()
  } else {
    if (snippets.snippetSource == 'OTHER') {
      OtherSnippets()
    } else {
      if (snippets.snippetSource == 'SEARCH') {
        GetSnippetsByContentType()
      } else {
        GetSnippetsInCollection(snippets.collectionID)
      }
    }
  }
  snippets.isSearchInProgress = false
} else {
  LoadTagOptions()
}
</script>

<template>
  <main>
    <h3>Filter selected snippets by tags:</h3>
    <n-select
      v-model:value="tagsSelected"
      multiple
      :fallback-option="false"
      :options="tagOptions"
    />
    <div v-if="snippetsSelected.length != 0">
      <n-flex v-for="snippet in snippetsSelected" :key="snippet.snippetID">
        <n-card style="width: 15vw">
          <n-flex :vertical="true">
            <a v-bind:href="snippet.link">{{ snippet.link }}</a>
            <p>{{ snippet.description }}</p>
          </n-flex>
        </n-card>
      </n-flex>
    </div>
    <div v-else>
      <h2 style="padding: 3rem">
        Ooops nothing here, except for pixels. Search snippets or open a collection!
      </h2>
    </div>
  </main>
</template>
