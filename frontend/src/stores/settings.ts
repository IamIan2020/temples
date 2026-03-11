import { defineStore } from 'pinia'
import { ref } from 'vue'
import { settingsApi } from '../api/settings'

export const useSettingsStore = defineStore('settings', () => {
  const companyName = ref('宮廟系統')
  const websiteName = ref('宮廟系統')
  const phone = ref<string | null>(null)
  const copyright = ref('© 2026 宮廟系統')
  const sessionTimeoutMinutes = ref(30)
  const address = ref<string | null>(null)
  const fax = ref<string | null>(null)
  const lineUrl = ref<string | null>(null)
  const facebookUrl = ref<string | null>(null)
  const googleMapUrl = ref<string | null>(null)
  const logoUrl = ref<string | null>(null)
  const loaded = ref(false)

  function init() {
    const cached = localStorage.getItem('publicSettings')
    if (cached) {
      try {
        const data = JSON.parse(cached)
        applyData(data)
      } catch {
        // 忽略無效快取
      }
    }
  }

  function applyData(data: Record<string, unknown>) {
    companyName.value = (data.companyName as string) ?? '宮廟系統'
    websiteName.value = (data.websiteName as string) ?? '宮廟系統'
    phone.value = (data.phone as string | null) ?? null
    copyright.value = (data.copyright as string) ?? '© 2026 宮廟系統'
    sessionTimeoutMinutes.value = (data.sessionTimeoutMinutes as number) ?? 30
    address.value = (data.address as string | null) ?? null
    fax.value = (data.fax as string | null) ?? null
    lineUrl.value = (data.lineUrl as string | null) ?? null
    facebookUrl.value = (data.facebookUrl as string | null) ?? null
    googleMapUrl.value = (data.googleMapUrl as string | null) ?? null
    logoUrl.value = (data.logoUrl as string | null) ?? null
  }

  async function loadPublicSettings() {
    try {
      const res = await settingsApi.getPublicSettings()
      const data = res.data.data!
      applyData(data as unknown as Record<string, unknown>)
      localStorage.setItem('publicSettings', JSON.stringify(data))
      loaded.value = true
    } catch {
      init()
      loaded.value = true
    }
  }

  function updateFromFullSettings(data: Record<string, unknown>) {
    applyData(data)
    localStorage.setItem('publicSettings', JSON.stringify({
      companyName: companyName.value,
      websiteName: websiteName.value,
      phone: phone.value,
      copyright: copyright.value,
      sessionTimeoutMinutes: sessionTimeoutMinutes.value,
      address: address.value,
      fax: fax.value,
      lineUrl: lineUrl.value,
      facebookUrl: facebookUrl.value,
      googleMapUrl: googleMapUrl.value,
      logoUrl: logoUrl.value,
    }))
  }

  return {
    companyName,
    websiteName,
    phone,
    copyright,
    sessionTimeoutMinutes,
    address,
    fax,
    lineUrl,
    facebookUrl,
    googleMapUrl,
    logoUrl,
    loaded,
    init,
    loadPublicSettings,
    updateFromFullSettings,
  }
})
