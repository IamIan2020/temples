<template>
  <div class="public-page">
    <div v-if="loading" style="text-align: center; padding: 40px">
      <el-icon class="is-loading" :size="32"><Loading /></el-icon>
    </div>

    <div v-else-if="!item" style="text-align: center; padding: 40px; color: #909399">
      找不到此服務項目
    </div>

    <template v-else>
      <el-button style="margin-bottom: 16px" @click="router.push('/services')">返回服務項目</el-button>

      <img
        v-if="item.headerImage"
        :src="item.headerImage"
        :alt="item.title"
        class="service-detail-header"
      />

      <h1 style="color: #1a1a2e">{{ item.title }}</h1>

      <div v-if="item.htmlContent" class="service-detail-content" v-html="item.htmlContent" />

      <div v-if="item.options.length > 0" class="service-option-list">
        <h2 style="color: #1a1a2e; margin-bottom: 16px">服務方案</h2>
        <div v-for="opt in item.options" :key="opt.id" class="service-option-card">
          <div class="service-option-info">
            <h4>{{ opt.title }}</h4>
            <div v-if="opt.subTitle" class="subtitle">{{ opt.subTitle }}</div>
            <div v-if="opt.description" class="description">{{ opt.description }}</div>
            <div v-if="opt.maxDonorCount" style="margin-top: 4px; font-size: 13px; color: #909399">
              名額：{{ opt.currentDonorCount }} / {{ opt.maxDonorCount }}
            </div>
          </div>
          <div class="service-option-price">
            <span class="price">{{ formatPrice(opt.price) }}</span>
            <span v-if="opt.priceUnit" class="unit"> / {{ opt.priceUnit }}</span>
            <div style="margin-top: 8px">
              <el-button type="primary" size="small" disabled>線上登記</el-button>
            </div>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Loading } from '@element-plus/icons-vue'
import { serviceItemsApi } from '../../api/serviceItems'
import type { PublicServiceItemDetailResponse } from '../../types/api'

const route = useRoute()
const router = useRouter()
const loading = ref(true)
const item = ref<PublicServiceItemDetailResponse | null>(null)

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('zh-TW', { style: 'currency', currency: 'TWD', minimumFractionDigits: 0 }).format(price)
}

onMounted(async () => {
  try {
    const res = await serviceItemsApi.getPublicDetail(Number(route.params.id))
    item.value = res.data.data ?? null
  } catch {
    // 忽略
  } finally {
    loading.value = false
  }
})
</script>
