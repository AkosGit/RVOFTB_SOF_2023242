<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import TheWelcome from '../components/TheWelcome.vue'
import type { VueCookies } from 'vue-cookies'
import { inject } from 'vue'
import { NButton } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()
const clients = useClientStore()
const $cookies = inject<VueCookies>('$cookies')
</script>

<template>
  <main>
    <p>Collections page</p>
  </main>
</template>
