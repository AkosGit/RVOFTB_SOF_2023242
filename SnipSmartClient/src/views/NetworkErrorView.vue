<script setup lang="ts">
import { NResult } from 'naive-ui'
</script>

<template>
  <n-result
    status="418"
    title="418 I'm a Teapot"
    description="Because a server error has occured but error 500 is lame, check your network connection."
  >
    <template #footer> </template>
  </n-result>
</template>
