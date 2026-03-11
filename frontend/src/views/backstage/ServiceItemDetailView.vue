<template>
  <div>
    <div style="display: flex; align-items: center; gap: 12px; margin-bottom: 16px">
      <el-button @click="router.push('/backstage/service-items')">返回列表</el-button>
      <h2 style="margin: 0">{{ isEdit ? '編輯服務項目' : '新增服務項目' }}</h2>
    </div>

    <el-card v-loading="loading" style="max-width: 900px">
      <el-form :model="form" label-width="120px">
        <el-form-item label="標題" required>
          <el-input v-model="form.title" placeholder="請輸入服務項目標題" />
        </el-form-item>

        <el-form-item label="Header 圖片">
          <div>
            <img v-if="form.headerImage" :src="form.headerImage" alt="" style="max-height: 120px; margin-bottom: 8px; display: block; border-radius: 4px" />
            <el-upload :show-file-list="false" :before-upload="handleImageUpload" accept=".jpg,.jpeg,.png,.gif,.webp">
              <el-button size="small" type="primary">{{ form.headerImage ? '更換圖片' : '上傳圖片' }}</el-button>
            </el-upload>
            <el-button v-if="form.headerImage" size="small" style="margin-top: 4px" @click="form.headerImage = null">移除圖片</el-button>
          </div>
        </el-form-item>

        <el-form-item label="排序">
          <el-input-number v-model="form.sortOrder" :min="0" />
        </el-form-item>

        <el-form-item label="啟用">
          <el-switch v-model="form.isActive" />
        </el-form-item>

        <el-form-item label="內容">
          <WangEditor v-model="form.htmlContent" style="width: 100%" />
        </el-form-item>

        <el-divider content-position="left">服務選項</el-divider>

        <div v-for="(opt, idx) in form.options" :key="idx" style="border: 1px solid #ebeef5; border-radius: 4px; padding: 16px; margin-bottom: 12px; position: relative">
          <el-button
            size="small"
            type="danger"
            circle
            style="position: absolute; top: 8px; right: 8px"
            @click="form.options.splice(idx, 1)"
          >
            <el-icon><Delete /></el-icon>
          </el-button>

          <el-row :gutter="12">
            <el-col :span="12">
              <el-form-item label="選項標題" label-width="100px">
                <el-input v-model="opt.title" placeholder="標題" />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item label="價格" label-width="60px">
                <el-input-number v-model="opt.price" :min="0" :precision="0" style="width: 100%" />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item label="單位" label-width="60px">
                <el-input v-model="opt.priceUnit" placeholder="如：盞、位" />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row :gutter="12">
            <el-col :span="12">
              <el-form-item label="副標題" label-width="100px">
                <el-input v-model="opt.subTitle" placeholder="副標題" />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item label="名額上限" label-width="100px">
                <el-input-number v-model="opt.maxDonorCount" :min="0" style="width: 100%" />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item label="金額上限" label-width="100px">
                <el-input-number v-model="opt.maxTotalAmount" :min="0" :precision="0" style="width: 100%" />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row :gutter="12">
            <el-col :span="18">
              <el-form-item label="說明" label-width="100px">
                <el-input v-model="opt.description" type="textarea" :rows="2" placeholder="選項說明" />
              </el-form-item>
            </el-col>
            <el-col :span="3">
              <el-form-item label="排序" label-width="60px">
                <el-input-number v-model="opt.sortOrder" :min="0" style="width: 100%" />
              </el-form-item>
            </el-col>
            <el-col :span="3">
              <el-form-item label="啟用" label-width="60px">
                <el-switch v-model="opt.isActive" />
              </el-form-item>
            </el-col>
          </el-row>
        </div>

        <el-button style="margin-bottom: 16px" @click="addOption">+ 新增選項</el-button>

        <el-form-item>
          <el-button type="primary" :loading="saving" @click="handleSave">儲存</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Delete } from '@element-plus/icons-vue'
import type { UploadRawFile } from 'element-plus'
import { serviceItemsApi } from '../../api/serviceItems'
import { uploadApi } from '../../api/upload'
import WangEditor from '../../components/WangEditor.vue'
import type { UpdateServiceItemOptionRequest } from '../../types/api'

const router = useRouter()
const route = useRoute()
const loading = ref(false)
const saving = ref(false)

const isEdit = computed(() => !!route.params.id)

interface FormOption extends UpdateServiceItemOptionRequest {}

const form = ref<{
  title: string
  headerImage: string | null
  sortOrder: number
  isActive: boolean
  htmlContent: string | null
  options: FormOption[]
}>({
  title: '',
  headerImage: null,
  sortOrder: 0,
  isActive: true,
  htmlContent: null,
  options: [],
})

const addOption = () => {
  form.value.options.push({
    id: null,
    title: '',
    price: 0,
    priceUnit: null,
    subTitle: null,
    description: null,
    maxDonorCount: null,
    maxTotalAmount: null,
    sortOrder: form.value.options.length,
    isActive: true,
  })
}

const handleImageUpload = async (file: UploadRawFile) => {
  try {
    const res = await uploadApi.uploadImage(file)
    form.value.headerImage = res.data.data!.url
    ElMessage.success('圖片上傳成功')
  } catch {
    ElMessage.error('圖片上傳失敗')
  }
  return false
}

const handleSave = async () => {
  if (!form.value.title) {
    ElMessage.warning('請填寫標題')
    return
  }
  saving.value = true
  try {
    if (isEdit.value) {
      await serviceItemsApi.update(Number(route.params.id), {
        ...form.value,
        options: form.value.options,
      })
      ElMessage.success('服務項目已更新')
    } else {
      await serviceItemsApi.create({
        ...form.value,
        options: form.value.options.map(({ id: _id, ...rest }) => rest),
      })
      ElMessage.success('服務項目已建立')
    }
    router.push('/backstage/service-items')
  } catch {
    ElMessage.error('儲存失敗')
  } finally {
    saving.value = false
  }
}

onMounted(async () => {
  if (!isEdit.value) return
  loading.value = true
  try {
    const res = await serviceItemsApi.getById(Number(route.params.id))
    const data = res.data.data!
    form.value = {
      title: data.title,
      headerImage: data.headerImage,
      sortOrder: data.sortOrder,
      isActive: data.isActive,
      htmlContent: data.htmlContent,
      options: data.options.map(o => ({
        id: o.id,
        title: o.title,
        price: o.price,
        priceUnit: o.priceUnit,
        subTitle: o.subTitle,
        description: o.description,
        maxDonorCount: o.maxDonorCount,
        maxTotalAmount: o.maxTotalAmount,
        sortOrder: o.sortOrder,
        isActive: o.isActive,
      })),
    }
  } catch {
    ElMessage.error('載入失敗')
  } finally {
    loading.value = false
  }
})
</script>
