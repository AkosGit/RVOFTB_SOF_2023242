<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import { inject, ref, watch } from 'vue'
import type { VueCookies } from 'vue-cookies'
import { NFlex, NInput, NCard, NButton } from 'naive-ui'
import { useRouter } from 'vue-router'

const router = useRouter()

const statusInd = ref<undefined | 'warning' | 'error'>(undefined)
const token = ref('')
const username = ref('')
const password = ref('')

const $cookies = inject<VueCookies>('$cookies')
watch(token, (newToken) => {
  if ($cookies) {
    $cookies.set('token', newToken)
  }
})

const clients = useClientStore()
function submit(event: Event) {
  clients.auth
    .Login(username.value, password.value)
    .then((t: string) => {
      clients.updateJWT(t)
      token.value = t
      router.push({ name: 'home' })
      router.forward()
    })
    .catch((error: any) => {
      console.error('Error:', error)
      statusInd.value = 'error'
    })
}
</script>

<template>
  <n-card title="Login">
    <n-flex vertical>
      <h2>Username:</h2>
      <n-input v-model:value="username" type="text" :status="statusInd" placeholder="Username" />
      <h2>Password</h2>
      <n-input v-model:value="password" type="text" :status="statusInd" placeholder="Password" />
      <n-button @click="submit">Login</n-button>
    </n-flex>
  </n-card>
</template>
