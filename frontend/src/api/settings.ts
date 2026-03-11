import client from './client'
import type {
  ApiResponse,
  PublicSettingResponse,
  SystemSettingResponse,
  UpdateSystemSettingRequest,
} from '../types/api'

export const settingsApi = {
  getPublicSettings() {
    return client.get<ApiResponse<PublicSettingResponse>>('/system-settings/public')
  },

  getSettings() {
    return client.get<ApiResponse<SystemSettingResponse>>('/system-settings')
  },

  updateSettings(data: UpdateSystemSettingRequest) {
    return client.put<ApiResponse<SystemSettingResponse>>('/system-settings', data)
  },
}
