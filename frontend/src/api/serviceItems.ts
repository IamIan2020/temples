import client from './client'
import type {
  ApiResponse,
  ServiceItemResponse,
  CreateServiceItemRequest,
  UpdateServiceItemRequest,
  UpdateSortOrderRequest,
  PublicServiceItemResponse,
  PublicServiceItemDetailResponse,
} from '../types/api'

export const serviceItemsApi = {
  getAll() {
    return client.get<ApiResponse<ServiceItemResponse[]>>('/service-items')
  },

  getById(id: number) {
    return client.get<ApiResponse<ServiceItemResponse>>(`/service-items/${id}`)
  },

  create(data: CreateServiceItemRequest) {
    return client.post<ApiResponse<ServiceItemResponse>>('/service-items', data)
  },

  update(id: number, data: UpdateServiceItemRequest) {
    return client.put<ApiResponse<ServiceItemResponse>>(`/service-items/${id}`, data)
  },

  delete(id: number) {
    return client.delete<ApiResponse>(`/service-items/${id}`)
  },

  updateSortOrder(data: UpdateSortOrderRequest) {
    return client.put<ApiResponse>('/service-items/sort', data)
  },

  getPublicList() {
    return client.get<ApiResponse<PublicServiceItemResponse[]>>('/service-items/public')
  },

  getPublicDetail(id: number) {
    return client.get<ApiResponse<PublicServiceItemDetailResponse>>(`/service-items/public/${id}`)
  },
}
