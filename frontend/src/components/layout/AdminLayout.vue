<template>
  <el-container style="min-height: 100vh">
    <el-aside width="220px" style="background: #304156">
      <div style="padding: 20px; text-align: center; color: #fff; font-size: 18px; font-weight: bold">
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
  </el-container>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { UserFilled } from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const handleLogout = () => {
  authStore.logout()
  router.push('/backstage/login')
}
</script>
