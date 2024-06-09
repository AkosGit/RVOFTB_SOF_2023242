<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import TheWelcome from '../components/TheWelcome.vue'
import type { VueCookies } from 'vue-cookies'
import { inject, ref } from 'vue'
import { NCard, NIcon, NFlex, NInput, NPopconfirm, NButton, useMessage } from 'naive-ui'
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

function GoToSnippets(collectionID: string) {
  clients.collection
    .GetSnippetsFromCollection(collectionID)
    .then((s: Array<SnippetModel>) => {
      console.log(s)
      snippets.snippets = s
      router.push({ name: 'home' })
      router.forward()
    })
    .catch((error: any) => {})
}
function OtherSnippets() {
  clients.collection
    .GetSnippetsWithoutCollection()
    .then((s: Array<SnippetModel>) => {
      console.log(s)
      snippets.snippets = s
      router.push({ name: 'home' })
      router.forward()
    })
    .catch((error: any) => {})
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
  <main>
    <n-flex>
      <n-popconfirm @positive-text="null" :negative-text="null">
        <template #trigger>
          <n-card style="width: 20vw">
            <n-icon>
              <AddIcon />
            </n-icon>
            <p>new collection</p>
          </n-card>
        </template>
        <n-input v-model:value="collectionName" type="text" placeholder="Name" />
        <n-button v-on:click="AddNewCollection()">Add</n-button>
      </n-popconfirm>

      <n-card style="width: 15vw" v-on:click="OtherSnippets()">
        <n-icon>
          <FolderIcon />
        </n-icon>
        <p>Other</p>
      </n-card>

      <div v-for="collection in collections" :key="collection.collectionID">
        <n-card style="width: 15vw" v-on:click="GoToSnippets(collection.collectionID)">
          <n-icon>
            <FolderIcon />
          </n-icon>
          <p>{{ collection.collectionName }}</p>
        </n-card>
      </div>
    </n-flex>
  </main>
</template>
