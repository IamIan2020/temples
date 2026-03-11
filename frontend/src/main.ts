import { createApp } from 'vue'
import { createPinia } from 'pinia'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import './assets/styles/public.css'
import zhTw from 'element-plus/es/locale/lang/zh-tw'
import App from './App.vue'
import router from './router'
import { useAuthStore } from './stores/auth'
import { useSettingsStore } from './stores/settings'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)
app.use(ElementPlus, { locale: zhTw })

// 初始化認證狀態
const authStore = useAuthStore()
authStore.init()

// 初始化系統設定（從快取載入，然後非同步更新）
const settingsStore = useSettingsStore()
settingsStore.init()
settingsStore.loadPublicSettings()

app.mount('#app')
