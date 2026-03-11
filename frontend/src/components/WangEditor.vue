<template>
  <div style="border: 1px solid #ccc; line-height: normal">
    <Toolbar
      style="border-bottom: 1px solid #ccc"
      :editor="editorRef"
      :defaultConfig="toolbarConfig"
      mode="default"
    />
    <Editor
      style="height: 400px; overflow-y: auto"
      v-model="valueHtml"
      :defaultConfig="editorConfig"
      mode="default"
      @onCreated="handleCreated"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, shallowRef, watch, onBeforeUnmount } from 'vue'
import { Editor, Toolbar } from '@wangeditor/editor-for-vue'
import type { IDomEditor, IEditorConfig, IToolbarConfig } from '@wangeditor/editor'
import '@wangeditor/editor/dist/css/style.css'

const props = defineProps<{
  modelValue: string | null
}>()

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const editorRef = shallowRef<IDomEditor>()
const valueHtml = ref(props.modelValue ?? '')

watch(() => props.modelValue, (val) => {
  if (val !== valueHtml.value) {
    valueHtml.value = val ?? ''
  }
})

watch(valueHtml, (val) => {
  emit('update:modelValue', val)
})

const toolbarConfig: Partial<IToolbarConfig> = {}

const editorConfig: Partial<IEditorConfig> = {
  placeholder: '請輸入內容...',
  MENU_CONF: {
    uploadImage: {
      async customUpload(file: File, insertFn: (url: string, alt: string, href: string) => void) {
        const formData = new FormData()
        formData.append('file', file)
        try {
          const token = localStorage.getItem('accessToken')
          const res = await fetch('/api/upload/image', {
            method: 'POST',
            headers: token ? { Authorization: `Bearer ${token}` } : {},
            body: formData,
          })
          const data = await res.json()
          if (data.success && data.data?.url) {
            insertFn(data.data.url, file.name, data.data.url)
          } else {
            alert(data.message || '圖片上傳失敗')
          }
        } catch {
          alert('圖片上傳失敗，請稍後再試')
        }
      },
    },
  },
}

const handleCreated = (editor: IDomEditor) => {
  editorRef.value = editor
}

onBeforeUnmount(() => {
  const editor = editorRef.value
  if (editor) editor.destroy()
})
</script>
