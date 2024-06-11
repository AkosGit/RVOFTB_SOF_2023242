<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import TheWelcome from '../components/TheWelcome.vue'
import type { VueCookies } from 'vue-cookies'
import { inject, ref } from 'vue'
import {
  NCard,
  NIcon,
  NFlex,
  NInput,
  NPopconfirm,
  NButton,
  useMessage,
  NGrid,
  NGridItem,
  NDivider
} from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'
import type { CollectionModel } from '@/models/CollectionModel'
import { FolderOutline as FolderIcon, AddCircle as AddIcon } from '@vicons/ionicons5'
import { useSnippetStore } from '@/stores/snippets'
import type { SnippetModel } from '@/models/SnippetModel'

const router = useRouter()
const clients = useClientStore()
const collections = ref<Array<CollectionModel>>([])
const snippets = useSnippetStore()
const collectionName = ref('')
const message = useMessage()

function GetCollections() {
  clients.collection
    .GetCollections()
    .then((d: Array<CollectionModel>) => {
      console.log(d)
      collections.value = d
    })
    .catch((error: any) => {})
}

function GoToSnippets(collectionID: string, collectionName: string) {
  snippets.snippetSource = 'COLLECTION'
  snippets.CollectionName = collectionName
  snippets.collectionID = collectionID
  snippets.isSearchInProgress = true
  router.push({ name: 'home' })
  router.forward()
}
function OtherSnippets() {
  snippets.snippetSource = 'OTHER'
  snippets.isSearchInProgress = true
  router.push({ name: 'home' })
  router.forward()
}
function GetAllSnippets() {
  snippets.snippetSource = 'ALL'
  snippets.isSearchInProgress = true
  router.push({ name: 'home' })
  router.forward()
}

function trimName(maxchar: number, name: string) {
  if (name.length - 1 > maxchar) {
    return name.substring(0, maxchar) + '...'
  }
  return name
}
function AddNewCollection() {
  clients.collection
    .AddNewCollection(collectionName.value)
    .then(() => {
      GetCollections()
    })
    .catch((error: any) => {})
}
GetCollections()
</script>

<template>
  <div style="width: 100%; height: 100%">
    <n-flex>
      <n-popconfirm @positive-text="null" :negative-text="null" :show-icon="false">
        <template #trigger>
          <n-card class="card">
            <n-flex justify="center" align="center" class="card-flex">
              <n-icon class="card-icon">
                <AddIcon class="card-icon" />
              </n-icon>
              <p>new collection</p>
            </n-flex>
          </n-card>
        </template>
        <template #action>
          <n-input v-model:value="collectionName" type="text" placeholder="Name" />
          <n-button style="margin-left: 5px" v-on:click="AddNewCollection()">Add</n-button>
        </template>
      </n-popconfirm>

      <n-card class="card" v-on:click="OtherSnippets()">
        <n-flex justify="center" align="center" class="card-flex" :vertical="true">
          <n-icon class="card-icon">
            <FolderIcon class="card-icon" />
          </n-icon>
          <p>Other</p>
        </n-flex>
      </n-card>
      <n-card class="card" v-on:click="GetAllSnippets()">
        <n-flex justify="center" align="center" class="card-flex" :vertical="true">
          <n-icon class="card-icon">
            <FolderIcon class="card-icon" />
          </n-icon>
          <p>All snippets</p>
        </n-flex>
      </n-card>
    </n-flex>
    <n-divider></n-divider>
    <n-flex>
      <div v-for="collection in collections" :key="collection.collectionID">
        <n-card
          class="card"
          v-on:click="GoToSnippets(collection.collectionID, collection.collectionName)"
        >
          <n-flex justify="center" align="center" class="card-flex" :vertical="true">
            <n-icon class="card-icon">
              <FolderIcon class="card-icon" />
            </n-icon>
            <p>{{ trimName(16, collection.collectionName) }}</p>
          </n-flex>
        </n-card>
      </div>
    </n-flex>
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
.card-icon {
  width: 2.2vw;
  height: 2.2vw;
}
</style>
