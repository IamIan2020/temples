import client from './client'
import type {
  ApiResponse,
  ServiceItemResponse,
  CreateServiceItemRequest,
  UpdateServiceItemRequest,
  UpdateSortOrderRequest,
  PublicServiceItemResponse,
  PublicServiceItemDetailResponse,
  ProductResponse,
  CreateProductRequest,
  UpdateProductRequest,
} from '../types/api'

// 分類 API
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

// 商品 API
export const productsApi = {
  getAll() {
    return client.get<ApiResponse<ProductResponse[]>>('/service-items/products')
  },

  getById(id: number) {
    return client.get<ApiResponse<ProductResponse>>(`/service-items/products/${id}`)
  },

  create(data: CreateProductRequest) {
    return client.post<ApiResponse<ProductResponse>>('/service-items/products', data)
  },

  update(id: number, data: UpdateProductRequest) {
    return client.put<ApiResponse<ProductResponse>>(`/service-items/products/${id}`, data)
  },

  delete(id: number) {
    return client.delete<ApiResponse>(`/service-items/products/${id}`)
  },
}
