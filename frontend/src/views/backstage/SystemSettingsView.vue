<template>
  <div>
    <h2>系統設定</h2>
    <el-card v-loading="loading" style="max-width: 600px">
      <el-form :model="form" label-width="120px" @submit.prevent="handleSave">
        <el-form-item label="公司名稱" required>
          <el-input v-model="form.companyName" placeholder="請輸入公司名稱" />
        </el-form-item>
        <el-form-item label="網站名稱" required>
          <el-input v-model="form.websiteName" placeholder="請輸入網站名稱" />
        </el-form-item>
        <el-form-item label="電話">
          <el-input v-model="form.phone" placeholder="請輸入電話" />
        </el-form-item>
        <el-form-item label="統編">
          <el-input v-model="form.taxId" placeholder="請輸入統一編號" />
        </el-form-item>
        <el-form-item label="Copyright" required>
          <el-input v-model="form.copyright" placeholder="請輸入版權文字" />
        </el-form-item>
        <el-form-item label="閒置登出時間">
          <el-input-number v-model="form.sessionTimeoutMinutes" :min="1" :max="480" />
          <span style="margin-left: 8px; color: #909399">分鐘</span>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :loading="saving" @click="handleSave">儲存設定</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { settingsApi } from '../../api/settings'
import { useSettingsStore } from '../../stores/settings'
import type { UpdateSystemSettingRequest } from '../../types/api'

const settingsStore = useSettingsStore()
const loading = ref(false)
const saving = ref(false)

const form = ref<UpdateSystemSettingRequest>({
  companyName: '',
  websiteName: '',
  phone: null,
  taxId: null,
  copyright: '',
  sessionTimeoutMinutes: 30,
})

onMounted(async () => {
  loading.value = true
  try {
    const res = await settingsApi.getSettings()
    const data = res.data.data!
    form.value = {
      companyName: data.companyName,
      websiteName: data.websiteName,
      phone: data.phone,
      taxId: data.taxId,
      copyright: data.copyright,
      sessionTimeoutMinutes: data.sessionTimeoutMinutes,
    }
  } catch {
    ElMessage.error('載入設定失敗')
  } finally {
    loading.value = false
  }
})

const handleSave = async () => {
  if (!form.value.companyName || !form.value.websiteName || !form.value.copyright) {
    ElMessage.warning('請填寫必填欄位')
    return
  }
  saving.value = true
  try {
    const res = await settingsApi.updateSettings(form.value)
    const data = res.data.data!
    settingsStore.updateFromFullSettings(data.websiteName, data.copyright, data.sessionTimeoutMinutes)
    ElMessage.success('設定已儲存')
  } catch {
    ElMessage.error('儲存設定失敗')
  } finally {
    saving.value = false
  }
}
</script>
