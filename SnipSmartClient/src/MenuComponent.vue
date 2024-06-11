<script lang="ts">
import { RouterLink, RouterView, useRoute, useRouter } from 'vue-router'
import { defineComponent, h, inject, ref, watch, type Component } from 'vue'
import { NIcon, NLayoutSider, NLayout, NSpace, NMenu, NSwitch } from 'naive-ui'
import type { MenuOption } from 'naive-ui'
import {
  BookOutline as BookIcon,
  PersonOutline as PersonIcon,
  WineOutline as WineIcon,
  HomeOutline as HomeIcon,
  SearchOutline as SearchIcon,
  PersonCircleOutline as UserIcon,
  FolderOpenOutline as CollectionsIcon,
  AddCircleOutline as NewSnippetIcon
} from '@vicons/ionicons5'
import { useClientStore } from './stores/clients'
import type { VueCookies } from 'vue-cookies'

function renderIcon(icon: Component) {
  return () => h(NIcon, null, { default: () => h(icon) })
}

const menuOptions: MenuOption[] = [
  {
    label: () =>
      h(
        RouterLink,
        {
          to: {
            name: 'home',
            params: {
              lang: 'en-US'
            }
          }
        },
        { default: () => 'Going Home' }
      ),
    key: 'go-back-home',
    icon: renderIcon(HomeIcon)
  },
  {
    label: () =>
      h(
        RouterLink,
        {
          to: {
            name: 'newsnippet',
            params: {
              lang: 'en-US'
            }
          }
        },
        { default: () => 'New Snippet' }
      ),
    key: 'go-new-snippet',
    icon: renderIcon(NewSnippetIcon)
  },
  {
    label: () =>
      h(
        RouterLink,
        {
          to: {
            name: 'search',
            params: {
              lang: 'en-US'
            }
          }
        },
        { default: () => 'Search' }
      ),
    key: 'go-search',
    icon: renderIcon(SearchIcon)
  },
  {
    label: () =>
      h(
        RouterLink,
        {
          to: {
            name: 'collections',
            params: {
              lang: 'en-US'
            }
          }
        },
        { default: () => 'Collections' }
      ),
    key: 'go-collections',
    icon: renderIcon(CollectionsIcon)
  },
  {
    label: () =>
      h(
        RouterLink,
        {
          to: {
            name: 'register',
            params: {
              lang: 'en-US'
            }
          }
        },
        { default: () => 'Register new user' }
      ),
    key: 'go-register',
    icon: renderIcon(UserIcon)
  },
  {
    label: 'Logout',
    key: 'logout',
    icon: renderIcon(UserIcon)
  }
]

export default defineComponent({
  components: {
    NLayoutSider,
    NLayout,
    NSpace,
    NMenu,
    NSwitch
  },
  setup() {
    const collapsed = ref(true)
    const activeKey = ref<string | null>(null)

    const router = useRouter()
    const route = useRoute()
    const clients = useClientStore()
    const $cookies = inject<VueCookies>('$cookies')

    function Logout() {
      if ($cookies) {
        $cookies.remove('token')
        clients.updateJWT('')
        router.push({ name: 'login' })
        router.forward()
      }
    }

    watch(activeKey, async (newkey, oldkey) => {
      if (newkey == 'logout') {
        Logout()
      }
    })
    return {
      activeKey,
      collapsed,
      menuOptions
    }
  }
})
</script>

<style scoped>
.container {
  height: 99%;
  width: 99%;
  padding: 10px;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-left: 7px;
}
</style>

<template>
  <n-space vertical>
    <n-layout has-sider style="height: 100vh; width: 100vw">
      <n-layout-sider
        bordered
        collapse-mode="width"
        :collapsed-width="64"
        :width="240"
        :collapsed="collapsed"
        show-trigger
        @collapse="collapsed = true"
        @expand="collapsed = false"
      >
        <n-menu
          v-model:value="activeKey"
          :collapsed="collapsed"
          :collapsed-width="64"
          :collapsed-icon-size="22"
          :options="menuOptions"
        >
        </n-menu>
      </n-layout-sider>
      <n-layout>
        <div class="container">
          <RouterView />
        </div>
      </n-layout>
    </n-layout>
  </n-space>
</template>
