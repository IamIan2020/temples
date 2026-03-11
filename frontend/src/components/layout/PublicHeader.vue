<template>
  <header class="public-header">
    <!-- 左側：Logo 圖片，點擊回首頁 -->
    <router-link to="/" class="public-header-brand">
      <img v-if="settingsStore.logoUrl" :src="settingsStore.logoUrl" alt="Logo" />
      <div v-else class="public-header-logo-placeholder">&#9753;</div>
    </router-link>

    <!-- 中間：導覽選單 -->
    <nav class="public-header-nav">
      <el-dropdown v-if="serviceItems.length > 0" trigger="hover" @command="handleCommand">
        <span class="public-header-nav-item">
          服務項目<span class="public-header-nav-sub">Services</span>
          <el-icon style="margin-left: 6px"><ArrowDown /></el-icon>
        </span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item v-for="item in serviceItems" :key="item.id" :command="`/services?category=${item.id}`">
              {{ item.title }}
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
      <router-link v-else to="/services" class="public-header-nav-item">
        服務項目<span class="public-header-nav-sub">Services</span>
      </router-link>
    </nav>

    <!-- 右側：登入/註冊 或 使用者資訊 -->
    <div class="public-header-actions">
      <template v-if="authStore.isAuthenticated">
        <router-link to="/profile" class="public-header-user">
          {{ authStore.user?.displayName }}
        </router-link>
        <button class="public-header-auth-btn" @click="handleLogout">登出</button>
      </template>
      <template v-else>
        <router-link to="/login" class="public-header-auth-btn">
          信眾登入<span class="public-header-auth-sep">|</span>註冊
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

const handleCommand = (path: string) => {
  router.push(path)
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
