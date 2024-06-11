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
  <n-flex vertical justify="center" align="center" style="width: 100%; height: 100%" :size="[0, 0]">
    <n-card title="Register" style="max-width: 25vw">
      <n-flex vertical>
        <h4>Firstname:</h4>
        <n-input
          v-model:value="firstname"
          type="text"
          :status="statusInd"
          placeholder="Firstname"
        />
        <h4>Lastname:</h4>
        <n-input v-model:value="lastname" type="text" :status="statusInd" placeholder="Lastname" />
        <h4>Username:</h4>
        <n-input v-model:value="username" type="text" :status="statusInd" placeholder="Username" />
        <h4>Password:</h4>
        <n-input
          v-model:value="password"
          type="password"
          :status="statusInd"
          placeholder="Password"
        />
        <h4>Email</h4>
        <n-input v-model:value="email" type="text" :status="statusInd" placeholder="Email" />
        <n-button @click="submit">Register</n-button>
      </n-flex>
    </n-card>
  </n-flex>
</template>
