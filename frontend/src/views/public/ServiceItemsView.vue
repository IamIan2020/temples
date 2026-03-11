<template>
  <div class="public-page">
    <div v-if="loadingCategories" style="text-align: center; padding: 40px">
      <el-icon class="is-loading" :size="32"><Loading /></el-icon>
    </div>

    <div v-else-if="categories.length === 0" style="text-align: center; padding: 40px; color: #909399">
      目前尚無服務項目
    </div>

    <template v-else>
      <!-- 分類選擇 -->
      <div class="service-category-tabs">
        <button
          v-for="cat in categories"
          :key="cat.id"
          :class="['category-tab', { active: selectedId === cat.id }]"
          @click="selectCategory(cat.id)"
        >
          {{ cat.title }}
        </button>
      </div>

      <!-- 商品列表 -->
      <div v-if="loadingDetail" style="text-align: center; padding: 40px">
        <el-icon class="is-loading" :size="24"><Loading /></el-icon>
      </div>

      <div v-else-if="detail">
        <div v-if="detail.htmlContent" class="service-detail-content" v-html="detail.htmlContent" />

        <div v-if="detail.options.length > 0" class="service-option-list">
          <div v-for="opt in detail.options" :key="opt.id" class="service-option-card">
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

        <div v-else style="text-align: center; padding: 40px; color: #909399">
          此分類目前沒有商品
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Loading } from '@element-plus/icons-vue'
import { serviceItemsApi } from '../../api/serviceItems'
import type { PublicServiceItemResponse, PublicServiceItemDetailResponse } from '../../types/api'

const route = useRoute()
const loadingCategories = ref(true)
const loadingDetail = ref(false)
const categories = ref<PublicServiceItemResponse[]>([])
const selectedId = ref<number | null>(null)
const detail = ref<PublicServiceItemDetailResponse | null>(null)

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('zh-TW', { style: 'currency', currency: 'TWD', minimumFractionDigits: 0 }).format(price)
}

const selectCategory = async (id: number) => {
  selectedId.value = id
  loadingDetail.value = true
  try {
    const res = await serviceItemsApi.getPublicDetail(id)
    detail.value = res.data.data ?? null
  } catch {
    detail.value = null
  } finally {
    loadingDetail.value = false
  }
}

onMounted(async () => {
  try {
    const res = await serviceItemsApi.getPublicList()
    categories.value = res.data.data ?? []
    if (categories.value.length > 0) {
      const queryCat = Number(route.query.category)
      const initialId = queryCat && categories.value.some(c => c.id === queryCat) ? queryCat : categories.value[0]!.id
      await selectCategory(initialId)
    }
  } catch {
    // 忽略
  } finally {
    loadingCategories.value = false
  }
})
</script>

<style scoped>
.service-category-tabs {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
  justify-content: center;
  margin-bottom: 24px;
}

.category-tab {
  padding: 10px 24px;
  border: 2px solid #c0b090;
  border-radius: 8px;
  background: transparent;
  color: #1a1a2e;
  font-size: 15px;
  cursor: pointer;
  transition: all 0.2s;
}

.category-tab:hover {
  background: #f5f0e8;
}

.category-tab.active {
  background: #1a1a2e;
  color: #c9a84c;
  border-color: #1a1a2e;
}
</style>
