<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import TheWelcome from '../components/TheWelcome.vue'
import type { VueCookies } from 'vue-cookies'
import { inject } from 'vue'
import { NFlex, NCard } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'
import { useSnippetStore } from '@/stores/snippets'

const router = useRouter()
const route = useRoute()
const clients = useClientStore()
const $cookies = inject<VueCookies>('$cookies')
const snippets = useSnippetStore()
</script>

<template>
  <main>
    <div v-if="snippets.snippets.length != 0">
      <n-flex v-for="snippet in snippets.snippets" :key="snippet.snippetID">
        <n-card style="width: 15vw">
          <n-flex :vertical="true">
            <a v-bind:href="snippet.link">{{ snippet.link }}</a>
            <p>{{ snippet.description }}</p>
          </n-flex>
        </n-card>
      </n-flex>
    </div>
    <div v-else>
      <h2 style="padding: 3rem">Ooops nothing here, except colors!</h2>
    </div>
  </main>
</template>
