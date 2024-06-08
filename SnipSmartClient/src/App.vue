<template>
  <n-loading-bar-provider>
    <n-message-provider>
      <n-notification-provider>
        <n-modal-provider>
          <n-dialog-provider>
            <MenuComponent />
          </n-dialog-provider>
        </n-modal-provider>
      </n-notification-provider>
    </n-message-provider>
  </n-loading-bar-provider>
</template>

<script>
import { defineComponent } from 'vue'
import MenuComponent from './MenuComponent.vue'
import {
  NLoadingBarProvider,
  NMessageProvider,
  NNotificationProvider,
  NModalProvider,
  NDialogProvider
} from 'naive-ui'
export default defineComponent({
  components: {
    MenuComponent,
    NLoadingBarProvider,
    NMessageProvider,
    NNotificationProvider,
    NModalProvider,
    NDialogProvider
  }
})
</script>
