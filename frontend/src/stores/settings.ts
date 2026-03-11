import { defineStore } from 'pinia'
import { ref } from 'vue'
import { settingsApi } from '../api/settings'

export const useSettingsStore = defineStore('settings', () => {
  const websiteName = ref('宮廟系統')
  const copyright = ref('© 2026 宮廟系統')
  const sessionTimeoutMinutes = ref(30)
  const loaded = ref(false)

  function init() {
    const cached = localStorage.getItem('publicSettings')
    if (cached) {
      try {
        const data = JSON.parse(cached)
        websiteName.value = data.websiteName
        copyright.value = data.copyright
        sessionTimeoutMinutes.value = data.sessionTimeoutMinutes
      } catch {
        // 忽略無效快取
      }
    }
  }

  async function loadPublicSettings() {
    try {
      const res = await settingsApi.getPublicSettings()
      const data = res.data.data!
      websiteName.value = data.websiteName
      copyright.value = data.copyright
      sessionTimeoutMinutes.value = data.sessionTimeoutMinutes
      localStorage.setItem('publicSettings', JSON.stringify(data))
      loaded.value = true
    } catch {
      // API 不可用時使用快取或預設值
      init()
      loaded.value = true
    }
  }

  function updateFromFullSettings(name: string, cr: string, timeout: number) {
    websiteName.value = name
    copyright.value = cr
    sessionTimeoutMinutes.value = timeout
    localStorage.setItem('publicSettings', JSON.stringify({
      websiteName: name,
      copyright: cr,
      sessionTimeoutMinutes: timeout,
    }))
  }

  return {
    websiteName,
    copyright,
    sessionTimeoutMinutes,
    loaded,
    init,
    loadPublicSettings,
    updateFromFullSettings,
  }
})
