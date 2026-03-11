<template>
  <div v-loading="loading">
    <div style="display: flex; align-items: center; gap: 12px; margin-bottom: 16px">
      <el-button @click="router.push('/backstage/service-items')">返回列表</el-button>
      <h2 style="margin: 0">{{ form.title || '編輯分類' }}</h2>
    </div>

    <el-card style="max-width: 600px">
      <el-form :model="form" label-width="100px">
        <el-form-item label="分類名稱" required>
          <el-input v-model="form.title" placeholder="分類名稱" />
        </el-form-item>
        <el-form-item label="啟用">
          <el-switch v-model="form.isActive" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :loading="saving" @click="handleSave">儲存</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ElMessage } from 'element-plus'
import { serviceItemsApi } from '../../api/serviceItems'
import type { ServiceItemResponse } from '../../types/api'

const router = useRouter()
const route = useRoute()
const loading = ref(false)
const saving = ref(false)
const originalData = ref<ServiceItemResponse | null>(null)

const form = ref({
  title: '',
  isActive: true,
})

const handleSave = async () => {
  if (!form.value.title.trim()) {
    ElMessage.warning('請填寫分類名稱')
    return
  }
  saving.value = true
  try {
    await serviceItemsApi.update(Number(route.params.id), {
      title: form.value.title,
      headerImage: originalData.value?.headerImage ?? null,
      sortOrder: originalData.value?.sortOrder ?? 0,
      isActive: form.value.isActive,
      htmlContent: originalData.value?.htmlContent ?? null,
      options: [],
    })
    ElMessage.success('分類已儲存')
  } catch {
    ElMessage.error('儲存失敗')
  } finally {
    saving.value = false
  }
}

onMounted(async () => {
  loading.value = true
  try {
    const res = await serviceItemsApi.getById(Number(route.params.id))
    originalData.value = res.data.data!
    form.value = {
      title: originalData.value.title,
      isActive: originalData.value.isActive,
    }
  } catch {
    ElMessage.error('載入失敗')
  } finally {
    loading.value = false
  }
})
</script>
