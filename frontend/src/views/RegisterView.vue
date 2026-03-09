<template>
  <div style="display: flex; justify-content: center; align-items: center; min-height: 100vh">
    <el-card style="width: 420px">
      <template #header>
        <h2 style="margin: 0; text-align: center">宮廟系統 - 註冊</h2>
      </template>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="0" @submit.prevent="handleRegister">
        <el-form-item prop="displayName">
          <el-input v-model="form.displayName" placeholder="顯示名稱" size="large" />
        </el-form-item>
        <el-form-item prop="email">
          <el-input v-model="form.email" placeholder="Email" size="large" />
        </el-form-item>
        <el-form-item prop="password">
          <el-input v-model="form.password" type="password" placeholder="密碼（至少 8 字元）" size="large" show-password />
        </el-form-item>
        <el-form-item prop="confirmPassword">
          <el-input v-model="form.confirmPassword" type="password" placeholder="確認密碼" size="large" show-password />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" size="large" style="width: 100%" :loading="loading" native-type="submit">
            註冊
          </el-button>
        </el-form-item>
      </el-form>
      <div style="text-align: center">
        已有帳號？<router-link to="/login">前往登入</router-link>
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
  displayName: '',
  email: '',
  password: '',
  confirmPassword: '',
})

const rules = {
  displayName: [{ required: true, message: '顯示名稱為必填', trigger: 'blur' }],
  email: [
    { required: true, message: 'Email 為必填', trigger: 'blur' },
    { type: 'email' as const, message: 'Email 格式不正確', trigger: 'blur' },
  ],
  password: [
    { required: true, message: '密碼為必填', trigger: 'blur' },
    { min: 8, message: '密碼至少需要 8 個字元', trigger: 'blur' },
  ],
  confirmPassword: [
    { required: true, message: '確認密碼為必填', trigger: 'blur' },
    {
      validator: (_rule: any, value: string, callback: (err?: Error) => void) => {
        if (value !== form.password) callback(new Error('確認密碼與密碼不一致'))
        else callback()
      },
      trigger: 'blur',
    },
  ],
}

const handleRegister = async () => {
  const valid = await formRef.value?.validate().catch(() => false)
  if (!valid) return

  loading.value = true
  try {
    await authStore.register(form)
    ElMessage.success('註冊成功')
    router.push('/profile')
  } catch (err: any) {
    ElMessage.error(err.response?.data?.message || '註冊失敗')
  } finally {
    loading.value = false
  }
}
</script>
