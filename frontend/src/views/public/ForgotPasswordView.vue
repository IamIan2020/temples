<template>
  <div style="display: flex; justify-content: center; align-items: center; min-height: 100vh">
    <el-card style="width: 420px">
      <template #header>
        <h2 style="margin: 0; text-align: center">忘記密碼</h2>
      </template>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="0" @submit.prevent="handleSubmit">
        <p style="color: #666; margin-bottom: 20px">請輸入您的 Email，我們將寄送密碼重設連結。</p>
        <el-form-item prop="email">
          <el-input v-model="form.email" placeholder="Email" size="large" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" size="large" style="width: 100%" :loading="loading" native-type="submit">
            送出
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
import { authApi } from '../../api/auth'
import { ElMessage } from 'element-plus'
import type { FormInstance } from 'element-plus'

const formRef = ref<FormInstance>()
const loading = ref(false)

const form = reactive({ email: '' })

const rules = {
  email: [
    { required: true, message: 'Email 為必填', trigger: 'blur' },
    { type: 'email' as const, message: 'Email 格式不正確', trigger: 'blur' },
  ],
}

const handleSubmit = async () => {
  const valid = await formRef.value?.validate().catch(() => false)
  if (!valid) return

  loading.value = true
  try {
    await authApi.forgotPassword(form)
    ElMessage.success('若該 Email 已註冊，將收到重設密碼信件')
  } catch {
    ElMessage.success('若該 Email 已註冊，將收到重設密碼信件')
  } finally {
    loading.value = false
  }
}
</script>
