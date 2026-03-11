<template>
  <footer class="public-footer">
    <div class="public-footer-content">
      <div class="public-footer-info">
        <h3>{{ settingsStore.companyName }}</h3>
        <p v-if="settingsStore.address">地址：{{ settingsStore.address }}</p>
        <p v-if="settingsStore.phone">電話：{{ settingsStore.phone }}</p>
        <p v-if="settingsStore.fax">傳真：{{ settingsStore.fax }}</p>
        <div class="public-footer-social">
          <a v-if="settingsStore.facebookUrl" :href="settingsStore.facebookUrl" target="_blank" rel="noopener" title="Facebook">FB</a>
          <a v-if="settingsStore.lineUrl" :href="settingsStore.lineUrl" target="_blank" rel="noopener" title="LINE">LINE</a>
          <a v-if="settingsStore.googleMapUrl" :href="settingsStore.googleMapUrl" target="_blank" rel="noopener" title="Google Map">MAP</a>
        </div>
      </div>
    </div>
    <div class="public-footer-copyright">
      {{ settingsStore.copyright }}
    </div>
  </footer>
</template>

<script setup lang="ts">
import { useSettingsStore } from '../../stores/settings'

const settingsStore = useSettingsStore()
</script>
