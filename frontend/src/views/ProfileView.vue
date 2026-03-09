<template>
  <div style="max-width: 600px; margin: 0 auto">
    <h2>個人資料</h2>

    <el-card>
      <el-form ref="profileFormRef" :model="profileForm" :rules="profileRules" label-width="100px" @submit.prevent="handleUpdateProfile">
        <el-form-item label="Email">
          <el-input :model-value="profile?.email" disabled />
        </el-form-item>
        <el-form-item label="顯示名稱" prop="displayName">
          <el-input v-model="profileForm.displayName" />
        </el-form-item>
        <el-form-item label="中文姓名">
          <el-input v-model="profileForm.chineseName" />
        </el-form-item>
        <el-form-item label="生日">
          <el-date-picker v-model="profileForm.birthday" type="date" value-format="YYYY-MM-DD" style="width: 100%" />
        </el-form-item>
        <el-form-item label="性別">
          <el-select v-model="profileForm.gender" placeholder="請選擇" style="width: 100%">
            <el-option label="男" value="男" />
            <el-option label="女" value="女" />
            <el-option label="其他" value="其他" />
          </el-select>
        </el-form-item>
        <el-form-item label="地址">
          <el-input v-model="profileForm.address" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :loading="profileLoading" native-type="submit">儲存</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <h2 style="margin-top: 24px">變更密碼</h2>
    <el-card>
      <el-form ref="passwordFormRef" :model="passwordForm" :rules="passwordRules" label-width="100px" @submit.prevent="handleChangePassword">
        <el-form-item label="目前密碼" prop="currentPassword">
          <el-input v-model="passwordForm.currentPassword" type="password" show-password />
        </el-form-item>
        <el-form-item label="新密碼" prop="newPassword">
          <el-input v-model="passwordForm.newPassword" type="password" show-password />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :loading="passwordLoading" native-type="submit">變更密碼</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { membersApi } from '../api/members'
import { ElMessage } from 'element-plus'
import type { FormInstance } from 'element-plus'
import type { MemberProfileResponse } from '../types/api'

const profileFormRef = ref<FormInstance>()
const passwordFormRef = ref<FormInstance>()
const profileLoading = ref(false)
const passwordLoading = ref(false)
const profile = ref<MemberProfileResponse | null>(null)

const profileForm = reactive({
  displayName: '',
  chineseName: null as string | null,
  birthday: null as string | null,
  gender: null as string | null,
  address: null as string | null,
})

const profileRules = {
  displayName: [{ required: true, message: '顯示名稱為必填', trigger: 'blur' }],
}

const passwordForm = reactive({
  currentPassword: '',
  newPassword: '',
})

const passwordRules = {
  currentPassword: [{ required: true, message: '目前密碼為必填', trigger: 'blur' }],
  newPassword: [
    { required: true, message: '新密碼為必填', trigger: 'blur' },
    { min: 8, message: '密碼至少需要 8 個字元', trigger: 'blur' },
  ],
}

const loadProfile = async () => {
  try {
    const { data } = await membersApi.getMyProfile()
    profile.value = data.data
    if (data.data) {
      profileForm.displayName = data.data.displayName
      profileForm.chineseName = data.data.chineseName
      profileForm.birthday = data.data.birthday
      profileForm.gender = data.data.gender
      profileForm.address = data.data.address
    }
  } catch {
    ElMessage.error('載入個人資料失敗')
  }
}

const handleUpdateProfile = async () => {
  const valid = await profileFormRef.value?.validate().catch(() => false)
  if (!valid) return

  profileLoading.value = true
  try {
    await membersApi.updateMyProfile(profileForm)
    ElMessage.success('更新成功')
    await loadProfile()
  } catch (err: any) {
    ElMessage.error(err.response?.data?.message || '更新失敗')
  } finally {
    profileLoading.value = false
  }
}

const handleChangePassword = async () => {
  const valid = await passwordFormRef.value?.validate().catch(() => false)
  if (!valid) return

  passwordLoading.value = true
  try {
    await membersApi.changeMyPassword(passwordForm)
    ElMessage.success('密碼變更成功')
    passwordForm.currentPassword = ''
    passwordForm.newPassword = ''
  } catch (err: any) {
    ElMessage.error(err.response?.data?.message || '密碼變更失敗')
  } finally {
    passwordLoading.value = false
  }
}

onMounted(loadProfile)
</script>
