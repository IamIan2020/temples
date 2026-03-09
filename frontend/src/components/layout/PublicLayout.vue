<template>
  <el-container style="min-height: 100vh">
    <el-header style="display: flex; align-items: center; justify-content: space-between; border-bottom: 1px solid #dcdfe6; background: #fff">
      <router-link to="/" style="text-decoration: none; color: inherit">
        <h3 style="margin: 0">宮廟系統</h3>
      </router-link>
      <div style="display: flex; align-items: center; gap: 12px">
        <template v-if="authStore.isAuthenticated">
          <span>{{ authStore.user?.displayName }}</span>
          <el-button type="danger" size="small" @click="handleLogout">登出</el-button>
        </template>
        <template v-else>
          <router-link to="/login">
            <el-button size="small">登入</el-button>
          </router-link>
          <router-link to="/register">
            <el-button type="primary" size="small">註冊</el-button>
          </router-link>
        </template>
      </div>
    </el-header>
    <el-main>
      <router-view />
    </el-main>
  </el-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const handleLogout = () => {
  authStore.logout()
  router.push('/login')
}
</script>
