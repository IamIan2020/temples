<template>
  <div class="public-page">
    <section style="text-align: center; padding: 40px 0 20px">
      <img v-if="settingsStore.logoUrl" :src="settingsStore.logoUrl" alt="Logo" style="height: 80px; margin-bottom: 16px" />
      <h1 style="color: #1a1a2e; margin: 0 0 12px">{{ settingsStore.companyName }}</h1>
      <p style="color: #606266; font-size: 16px">歡迎蒞臨本宮廟網站</p>
    </section>

    <section v-if="serviceItemsStore.items.length > 0">
      <h2 style="text-align: center; color: #1a1a2e; margin-bottom: 8px">服務項目</h2>
      <div class="service-card-grid">
        <div
          v-for="item in serviceItemsStore.items"
          :key="item.id"
          class="service-card"
          @click="router.push(`/services?category=${item.id}`)"
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
    </section>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useSettingsStore } from '../../stores/settings'
import { useServiceItemsStore } from '../../stores/serviceItems'

const router = useRouter()
const settingsStore = useSettingsStore()
const serviceItemsStore = useServiceItemsStore()

onMounted(() => {
  serviceItemsStore.loadItems()
})
</script>
