<template>
  <div style="max-width: 600px; margin: 0 auto">
    <div style="display: flex; align-items: center; margin-bottom: 16px">
      <el-button @click="router.push('/admin/members')">← 返回列表</el-button>
      <h2 style="margin: 0 0 0 16px">會員詳情</h2>
    </div>

    <el-card v-if="member" v-loading="loading">
      <el-descriptions :column="1" border>
        <el-descriptions-item label="Email">{{ member.email }}</el-descriptions-item>
        <el-descriptions-item label="顯示名稱">{{ member.displayName }}</el-descriptions-item>
        <el-descriptions-item label="中文姓名">{{ member.chineseName || '-' }}</el-descriptions-item>
        <el-descriptions-item label="生日">{{ member.birthday || '-' }}</el-descriptions-item>
        <el-descriptions-item label="性別">{{ member.gender || '-' }}</el-descriptions-item>
        <el-descriptions-item label="地址">{{ member.address || '-' }}</el-descriptions-item>
        <el-descriptions-item label="會員編號">{{ member.memberNumber || '-' }}</el-descriptions-item>
        <el-descriptions-item label="建立時間">{{ new Date(member.createdAt).toLocaleString('zh-TW') }}</el-descriptions-item>
        <el-descriptions-item label="角色">
          <el-tag :type="getRoleTagType(member.roles[0] ?? '')">{{ member.roles[0] ?? '-' }}</el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="狀態">
          <el-tag :type="member.isActive ? 'success' : 'danger'">{{ member.isActive ? '啟用' : '停用' }}</el-tag>
        </el-descriptions-item>
      </el-descriptions>

      <div style="margin-top: 24px; display: flex; gap: 8px">
        <el-button
          v-if="member.isActive"
          type="warning"
          @click="handleChangeStatus(false)"
          :loading="statusLoading"
        >停用帳號</el-button>
        <el-button
          v-else
          type="success"
          @click="handleChangeStatus(true)"
          :loading="statusLoading"
        >啟用帳號</el-button>

        <el-select
          v-if="authStore.isSystemAdmin"
          v-model="selectedRole"
          placeholder="變更角色"
          style="width: 160px"
          @change="handleChangeRole"
        >
          <el-option label="SystemAdmin" value="SystemAdmin" />
          <el-option label="WebAdmin" value="WebAdmin" />
          <el-option label="Member" value="Member" />
        </el-select>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { membersApi } from '../../api/members'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { MemberProfileResponse } from '../../types/api'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()
const loading = ref(false)
const statusLoading = ref(false)
const member = ref<MemberProfileResponse | null>(null)
const selectedRole = ref('')

const memberId = route.params.id as string

const getRoleTagType = (role: string) => {
  switch (role) {
    case 'SystemAdmin': return 'danger'
    case 'WebAdmin': return 'warning'
    default: return 'info'
  }
}

const loadMember = async () => {
  loading.value = true
  try {
    const { data } = await membersApi.getMemberById(memberId)
    member.value = data.data
    selectedRole.value = data.data?.roles[0] || ''
  } catch {
    ElMessage.error('載入會員資料失敗')
  } finally {
    loading.value = false
  }
}

const handleChangeStatus = async (isActive: boolean) => {
  try {
    await ElMessageBox.confirm(
      `確定要${isActive ? '啟用' : '停用'}此會員？`,
      '確認',
      { type: 'warning' }
    )
  } catch { return }

  statusLoading.value = true
  try {
    await membersApi.changeStatus(memberId, { isActive })
    ElMessage.success(isActive ? '帳號已啟用' : '帳號已停用')
    await loadMember()
  } catch (err: any) {
    ElMessage.error(err.response?.data?.message || '操作失敗')
  } finally {
    statusLoading.value = false
  }
}

const handleChangeRole = async (role: string) => {
  try {
    await ElMessageBox.confirm(
      `確定要將角色變更為 ${role}？`,
      '確認',
      { type: 'warning' }
    )
  } catch {
    selectedRole.value = member.value?.roles[0] || ''
    return
  }

  try {
    await membersApi.changeRole(memberId, { role })
    ElMessage.success('角色變更成功')
    await loadMember()
  } catch (err: any) {
    ElMessage.error(err.response?.data?.message || '操作失敗')
    selectedRole.value = member.value?.roles[0] || ''
  }
}

onMounted(loadMember)
</script>
