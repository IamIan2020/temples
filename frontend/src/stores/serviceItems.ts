import { defineStore } from 'pinia'
import { ref } from 'vue'
import { serviceItemsApi } from '../api/serviceItems'
import type { PublicServiceItemResponse } from '../types/api'

export const useServiceItemsStore = defineStore('serviceItems', () => {
  const items = ref<PublicServiceItemResponse[]>([])
  const loaded = ref(false)

  async function loadItems() {
    if (loaded.value) return
    try {
      const res = await serviceItemsApi.getPublicList()
      items.value = res.data.data ?? []
      loaded.value = true
    } catch {
      // 忽略
    }
  }

  return { items, loaded, loadItems }
})
