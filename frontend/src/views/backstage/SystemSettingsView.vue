<template>
  <div>
    <h2>系統設定</h2>
    <el-card v-loading="loading" style="max-width: 700px">
      <el-form :model="form" label-width="140px" @submit.prevent="handleSave">
        <el-divider content-position="left">基本資訊</el-divider>
        <el-form-item label="公司名稱" required>
          <el-input v-model="form.companyName" placeholder="請輸入公司名稱" />
        </el-form-item>
        <el-form-item label="網站名稱" required>
          <el-input v-model="form.websiteName" placeholder="請輸入網站名稱" />
        </el-form-item>
        <el-form-item label="電話">
          <el-input v-model="form.phone" placeholder="請輸入電話" />
        </el-form-item>
        <el-form-item label="傳真">
          <el-input v-model="form.fax" placeholder="請輸入傳真號碼" />
        </el-form-item>
        <el-form-item label="地址">
          <el-input v-model="form.address" placeholder="請輸入地址" />
        </el-form-item>
        <el-form-item label="統編">
          <el-input v-model="form.taxId" placeholder="請輸入統一編號" />
        </el-form-item>
        <el-form-item label="Copyright" required>
          <el-input v-model="form.copyright" placeholder="請輸入版權文字" />
        </el-form-item>

        <el-divider content-position="left">社群連結</el-divider>
        <el-form-item label="LINE URL">
          <el-input v-model="form.lineUrl" placeholder="https://line.me/..." />
        </el-form-item>
        <el-form-item label="Facebook URL">
          <el-input v-model="form.facebookUrl" placeholder="https://facebook.com/..." />
        </el-form-item>
        <el-form-item label="Google Map URL">
          <el-input v-model="form.googleMapUrl" placeholder="https://maps.google.com/..." />
        </el-form-item>

        <el-divider content-position="left">Logo 設定</el-divider>
        <el-form-item label="Logo">
          <div>
            <img v-if="form.logoUrl" :src="form.logoUrl" alt="Logo" style="max-height: 80px; margin-bottom: 8px; display: block" />
            <el-upload
              :show-file-list="false"
              :before-upload="handleLogoUpload"
              accept=".jpg,.jpeg,.png,.gif,.webp"
            >
              <el-button size="small" type="primary">{{ form.logoUrl ? '更換 Logo' : '上傳 Logo' }}</el-button>
            </el-upload>
          </div>
        </el-form-item>

        <el-divider content-position="left">系統設定</el-divider>
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
import type { UploadRawFile } from 'element-plus'
import { settingsApi } from '../../api/settings'
import { uploadApi } from '../../api/upload'
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
  address: null,
  fax: null,
  lineUrl: null,
  facebookUrl: null,
  googleMapUrl: null,
  logoUrl: null,
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
      address: data.address,
      fax: data.fax,
      lineUrl: data.lineUrl,
      facebookUrl: data.facebookUrl,
      googleMapUrl: data.googleMapUrl,
      logoUrl: data.logoUrl,
    }
  } catch {
    ElMessage.error('載入設定失敗')
  } finally {
    loading.value = false
  }
})

const handleLogoUpload = async (file: UploadRawFile) => {
  try {
    const res = await uploadApi.uploadImage(file)
    form.value.logoUrl = res.data.data!.url
    ElMessage.success('Logo 上傳成功')
  } catch {
    ElMessage.error('Logo 上傳失敗')
  }
  return false
}

const handleSave = async () => {
  if (!form.value.companyName || !form.value.websiteName || !form.value.copyright) {
    ElMessage.warning('請填寫必填欄位')
    return
  }
  saving.value = true
  try {
    const res = await settingsApi.updateSettings(form.value)
    const data = res.data.data!
    settingsStore.updateFromFullSettings(data as unknown as Record<string, unknown>)
    ElMessage.success('設定已儲存')
  } catch {
    ElMessage.error('儲存設定失敗')
  } finally {
    saving.value = false
  }
}
</script>
