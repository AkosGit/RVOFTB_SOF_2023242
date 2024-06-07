<script setup lang="ts">
import { useClientStore } from '@/stores/clients'
import { inject, ref, watch } from 'vue'
import type { VueCookies } from 'vue-cookies'
import { NFlex, NInput, NCard, NButton } from 'naive-ui'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()

const statusInd = ref<undefined | 'warning' | 'error'>(undefined)
const token = ref('')

const username = ref('')
const password = ref('')
const email = ref('')
const firstname = ref('')
const lastname = ref('')

const $cookies = inject<VueCookies>('$cookies')
watch(token, (newToken) => {
  if ($cookies) {
    $cookies.set('token', newToken)
  }
})

const clients = useClientStore()

function submit(event: Event) {
  clients.auth
    .Register(username.value, password.value, email.value, firstname.value, lastname.value)
    .then(() => {
      alert('Registration successful!')
      router.push({ name: 'login' })
      router.forward()
    })
    .catch((error: any) => {
      console.error('Error:', error)
      statusInd.value = 'error'
      alert('An error has occured:' + error + '!')
    })
}
</script>

<template>
  <n-card title="Register">
    <n-flex vertical>
      <h2>Firstname:</h2>
      <n-input v-model:value="firstname" type="text" :status="statusInd" placeholder="Firstname" />
      <h2>Lastname:</h2>
      <n-input v-model:value="lastname" type="text" :status="statusInd" placeholder="Lastname" />
      <h2>Username:</h2>
      <n-input v-model:value="username" type="text" :status="statusInd" placeholder="Username" />
      <h2>Password:</h2>
      <n-input
        v-model:value="password"
        type="password"
        :status="statusInd"
        placeholder="Password"
      />
      <h2>Email</h2>
      <n-input v-model:value="email" type="text" :status="statusInd" placeholder="Email" />
      <n-button @click="submit">Register</n-button>
    </n-flex>
  </n-card>
</template>
