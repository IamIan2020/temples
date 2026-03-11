<template>
  <div class="public-page">
    <h1 style="text-align: center; color: #1a1a2e">服務項目</h1>

    <div v-if="loading" style="text-align: center; padding: 40px">
      <el-icon class="is-loading" :size="32"><Loading /></el-icon>
    </div>

    <div v-else-if="items.length === 0" style="text-align: center; padding: 40px; color: #909399">
      目前尚無服務項目
    </div>

    <div v-else class="service-card-grid">
      <div
        v-for="item in items"
        :key="item.id"
        class="service-card"
        @click="router.push(`/services/${item.id}`)"
      >
        <img
          v-if="item.headerImage"
          :src="item.headerImage"
          :alt="item.title"
          class="service-card-image"
        />
        <div v-else class="service-card-image" style="display: flex; align-items: center; justify-content: center; color: #c0b090; font-size: 48px">
          &#9753;
        </div>
        <div class="service-card-body">
          <h3>{{ item.title }}</h3>
          <span class="more-link">了解更多 &rarr;</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Loading } from '@element-plus/icons-vue'
import { serviceItemsApi } from '../../api/serviceItems'
import type { PublicServiceItemResponse } from '../../types/api'

const router = useRouter()
const loading = ref(true)
const items = ref<PublicServiceItemResponse[]>([])

onMounted(async () => {
  try {
    const res = await serviceItemsApi.getPublicList()
    items.value = res.data.data ?? []
  } catch {
    // 忽略
  } finally {
    loading.value = false
  }
})
</script>
