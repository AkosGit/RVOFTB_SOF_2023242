<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import type { VueCookies } from 'vue-cookies'
import { inject, ref, watch, type Ref } from 'vue'
import { NFlex, NCard, NSelect, NDivider, NButton } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'
import { useSnippetStore } from '@/stores/snippets'
import type { SnippetModel } from '@/models/SnippetModel'
import type { TagModel } from '@/models/TagModel'
import type { SelectMixedOption } from 'naive-ui/es/select/src/interface'
import { TrashBinOutline as DeleteIcon } from '@vicons/ionicons5'

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
function EditSnippet(snippet: SnippetModel) {
  console.log(snippet)
  snippets.CurrentSnippet = snippet
  router.push({ name: 'editsnippet' })
  router.forward()
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
function trimName(maxchar: number, name: string) {
  if (name.length - 1 > maxchar) {
    return name.substring(0, maxchar) + '...'
  }
  return name
}
function DeleteCollection(CollectionID: string) {
  clients.collection
    .RemoveCollection(CollectionID)
    .then(() => {
      alert('Collection removed succesfully!')
      snippets.snippetSource = 'NONE'
      snippets.isSearchInProgress = false
      snippets.snippets = []
      snippets.tags = []
      snippets.DistinctTagNames = []
    })
    .catch((error: any) => {
      alert('Collection removal failed!')
    })
}

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
  <div style="width: 100%; height: 100%">
    <n-flex>
      <div>
        <h3 v-if="snippets.snippetSource !== 'NONE'">Current results for:</h3>
        <h4 class="search-result" v-if="snippets.snippetSource === 'COLLECTION'">
          '{{ snippets.CollectionName }}' collection
        </h4>
        <h4 class="search-result" v-if="snippets.snippetSource === 'OTHER'">
          Snippets that are not in a collection
        </h4>
        <h4 class="search-result" v-if="snippets.snippetSource === 'ALL'">
          Snippets from all collections
        </h4>
        <h4 class="search-result" v-if="snippets.snippetSource === 'SEARCH'">
          Search for '{{ snippets.searchTargetType }}' content type and '{{
            snippets.searchTargetSubType
          }}' content subtype
        </h4>
      </div>
      <div
        v-if="snippets.snippetSource === 'COLLECTION'"
        style="height: 100%; display: flex; justify-items: center; align-items: center"
      >
        <n-button @click="DeleteCollection(snippets.collectionID)" type="error" class="Delete"
          >Delete collection</n-button
        >
      </div>
    </n-flex>
    <n-divider></n-divider>
    <h3 style="padding-top: 0.5rem">Filter selected snippets by tags:</h3>
    <n-select
      style="padding-top: 0.5rem; max-width: 20vw"
      v-model:value="tagsSelected"
      multiple
      :fallback-option="false"
      :options="tagOptions"
    />
    <n-card style="margin-top: 1.5rem; height: 100%">
      <div v-if="snippetsSelected.length != 0">
        <n-flex>
          <div v-for="snippet in snippetsSelected" :key="snippet.snippetID">
            <n-card class="card" v-on:click="EditSnippet(snippet)">
              <n-flex justify="center" align="center" class="card-flex" :vertical="true">
                <a v-bind:href="snippet.link">{{
                  trimName(15, snippet.link.replace('http://', '').replace('https://', ''))
                }}</a>
                <p>{{ trimName(15, snippet.description) }}</p>
              </n-flex>
            </n-card>
          </div>
        </n-flex>
      </div>
      <div v-else>
        <h3>
          Ooops nothing here, except for pixels. Search snippets or open a collection than filter
          for a tag!
        </h3>
      </div>
    </n-card>
  </div>
</template>
<style scoped>
.card:hover {
  cursor: pointer;
}
.card {
  width: 10.5vw;
  height: 8.5vw;
}
.card-flex {
  width: 100%;
  height: 100%;
}
.search-result {
  font-weight: bold;
}
.delete {
  width: 30px;
  height: 10px;
}
.delete:hover {
  cursor: pointer;
}
</style>
