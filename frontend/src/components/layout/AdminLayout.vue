<template>
  <el-container style="min-height: 100vh">
    <el-aside width="220px" style="background: #304156">
      <div style="padding: 20px; text-align: center; color: #fff; font-size: 18px; font-weight: bold">
        {{ settingsStore.websiteName }}
      </div>
      <div style="padding: 0 20px 10px; text-align: center; color: #bfcbd9; font-size: 12px">
        管理後台
      </div>
      <el-menu
        :default-active="route.path"
        background-color="#304156"
        text-color="#bfcbd9"
        active-text-color="#409eff"
        router
      >
        <el-menu-item index="/backstage/members">
          <el-icon><UserFilled /></el-icon>
          <span>會員管理</span>
        </el-menu-item>
        <el-sub-menu v-if="authStore.isSystemAdmin" index="system-management">
          <template #title>
            <el-icon><Setting /></el-icon>
            <span>系統管理</span>
          </template>
          <el-menu-item index="/backstage/settings">系統設定</el-menu-item>
          <el-menu-item index="/backstage/roles">權限群組</el-menu-item>
        </el-sub-menu>
      </el-menu>
    </el-aside>

    <el-container>
      <el-header style="display: flex; align-items: center; justify-content: flex-end; border-bottom: 1px solid #dcdfe6; background: #fff">
        <span style="margin-right: 16px">{{ authStore.user?.displayName }}</span>
        <el-tag size="small" style="margin-right: 16px">{{ authStore.userRoles[0] }}</el-tag>
        <el-button type="danger" size="small" @click="handleLogout">登出</el-button>
      </el-header>

      <el-main>
        <router-view />
      </el-main>
    </el-container>
    <IdleWarningDialog
      :visible="idleTimeout.showWarning.value"
      :remaining-seconds="idleTimeout.remainingSeconds.value"
      @extend="idleTimeout.extend"
      @logout="handleIdleLogout"
    />
  </el-container>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { useSettingsStore } from '../../stores/settings'
import { useIdleTimeout } from '../../composables/useIdleTimeout'
import IdleWarningDialog from '../IdleWarningDialog.vue'
import { UserFilled, Setting } from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()
const settingsStore = useSettingsStore()
const idleTimeout = useIdleTimeout()

const handleLogout = () => {
  authStore.logout()
  router.push('/backstage/login')
}

const handleIdleLogout = () => {
  idleTimeout.doLogout()
  router.push('/backstage/login')
}
</script>
