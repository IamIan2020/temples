<template>
  <header class="public-header">
    <router-link to="/" class="public-header-brand">
      <img v-if="settingsStore.logoUrl" :src="settingsStore.logoUrl" alt="Logo" />
      <h1>{{ settingsStore.companyName }}</h1>
    </router-link>

    <nav class="public-header-nav">
      <router-link to="/">首頁</router-link>
      <el-dropdown v-if="serviceItems.length > 0" trigger="hover">
        <router-link to="/services" class="public-header-nav-link" @click.prevent>
          服務項目 <el-icon style="margin-left: 4px"><ArrowDown /></el-icon>
        </router-link>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item v-for="item in serviceItems" :key="item.id" @click="router.push(`/services/${item.id}`)">
              {{ item.title }}
            </el-dropdown-item>
            <el-dropdown-item divided @click="router.push('/services')">查看全部</el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
      <router-link v-else to="/services">服務項目</router-link>
    </nav>

    <div class="public-header-actions">
      <template v-if="authStore.isAuthenticated">
        <router-link to="/profile" style="color: #e8d5b7; text-decoration: none">
          {{ authStore.user?.displayName }}
        </router-link>
        <el-button size="small" @click="handleLogout">登出</el-button>
      </template>
      <template v-else>
        <router-link to="/login">
          <el-button size="small">登入</el-button>
        </router-link>
        <router-link to="/register">
          <el-button size="small" type="primary">註冊</el-button>
        </router-link>
      </template>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ArrowDown } from '@element-plus/icons-vue'
import { useAuthStore } from '../../stores/auth'
import { useSettingsStore } from '../../stores/settings'
import { serviceItemsApi } from '../../api/serviceItems'
import type { PublicServiceItemResponse } from '../../types/api'

const router = useRouter()
const authStore = useAuthStore()
const settingsStore = useSettingsStore()
const serviceItems = ref<PublicServiceItemResponse[]>([])

const emit = defineEmits<{ logout: [] }>()

const handleLogout = () => {
  authStore.logout()
  emit('logout')
}

onMounted(async () => {
  try {
    const res = await serviceItemsApi.getPublicList()
    serviceItems.value = res.data.data ?? []
  } catch {
    // 忽略
  }
})
</script>
