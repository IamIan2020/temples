<template>
  <div style="display: flex; justify-content: center; align-items: center; min-height: 100vh">
    <el-card style="width: 420px">
      <template #header>
        <h2 style="margin: 0; text-align: center">重設密碼</h2>
      </template>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="0" @submit.prevent="handleSubmit">
        <el-form-item prop="newPassword">
          <el-input v-model="form.newPassword" type="password" placeholder="新密碼（至少 8 字元）" size="large" show-password />
        </el-form-item>
        <el-form-item prop="confirmPassword">
          <el-input v-model="confirmPassword" type="password" placeholder="確認新密碼" size="large" show-password />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" size="large" style="width: 100%" :loading="loading" native-type="submit">
            重設密碼
          </el-button>
        </el-form-item>
      </el-form>
      <div style="text-align: center">
        <router-link to="/login">返回登入</router-link>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { authApi } from '../api/auth'
import { ElMessage } from 'element-plus'
import type { FormInstance } from 'element-plus'

const route = useRoute()
const router = useRouter()
const formRef = ref<FormInstance>()
const loading = ref(false)
const confirmPassword = ref('')

const form = reactive({
  email: (route.query.email as string) || '',
  token: (route.query.token as string) || '',
  newPassword: '',
})

const rules = {
  newPassword: [
    { required: true, message: '新密碼為必填', trigger: 'blur' },
    { min: 8, message: '密碼至少需要 8 個字元', trigger: 'blur' },
  ],
  confirmPassword: [
    {
      validator: (_rule: any, _value: string, callback: (err?: Error) => void) => {
        if (confirmPassword.value !== form.newPassword) callback(new Error('確認密碼與密碼不一致'))
        else callback()
      },
      trigger: 'blur',
    },
  ],
}

const handleSubmit = async () => {
  if (confirmPassword.value !== form.newPassword) {
    ElMessage.error('確認密碼與密碼不一致')
    return
  }

  const valid = await formRef.value?.validate().catch(() => false)
  if (!valid) return

  loading.value = true
  try {
    await authApi.resetPassword(form)
    ElMessage.success('密碼重設成功，請重新登入')
    router.push('/login')
  } catch (err: any) {
    ElMessage.error(err.response?.data?.message || '密碼重設失敗')
  } finally {
    loading.value = false
  }
}
</script>
