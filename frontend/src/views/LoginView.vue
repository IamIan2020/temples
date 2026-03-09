<template>
  <div style="display: flex; justify-content: center; align-items: center; min-height: 100vh">
    <el-card style="width: 420px">
      <template #header>
        <h2 style="margin: 0; text-align: center">宮廟系統 - 登入</h2>
      </template>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="0" @submit.prevent="handleLogin">
        <el-form-item prop="email">
          <el-input v-model="form.email" placeholder="Email" prefix-icon="Message" size="large" />
        </el-form-item>
        <el-form-item prop="password">
          <el-input v-model="form.password" type="password" placeholder="密碼" prefix-icon="Lock" size="large" show-password />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" size="large" style="width: 100%" :loading="loading" native-type="submit">
            登入
          </el-button>
        </el-form-item>
      </el-form>
      <div style="text-align: center">
        <router-link to="/forgot-password">忘記密碼？</router-link>
        <span style="margin: 0 8px">|</span>
        <router-link to="/register">註冊帳號</router-link>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { ElMessage } from 'element-plus'
import type { FormInstance } from 'element-plus'

const router = useRouter()
const authStore = useAuthStore()
const formRef = ref<FormInstance>()
const loading = ref(false)

const form = reactive({
  email: '',
  password: '',
})

const rules = {
  email: [
    { required: true, message: 'Email 為必填', trigger: 'blur' },
    { type: 'email' as const, message: 'Email 格式不正確', trigger: 'blur' },
  ],
  password: [{ required: true, message: '密碼為必填', trigger: 'blur' }],
}

const handleLogin = async () => {
  const valid = await formRef.value?.validate().catch(() => false)
  if (!valid) return

  loading.value = true
  try {
    await authStore.login(form)
    ElMessage.success('登入成功')
    router.push('/profile')
  } catch (err: any) {
    ElMessage.error(err.response?.data?.message || '登入失敗')
  } finally {
    loading.value = false
  }
}
</script>
