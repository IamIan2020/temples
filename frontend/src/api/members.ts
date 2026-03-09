import client from './client'
import type {
  ApiResponse,
  MemberProfileResponse,
  UpdateProfileRequest,
  ChangePasswordRequest,
  MemberListRequest,
  MemberListResponse,
  ChangeRoleRequest,
  ChangeStatusRequest,
} from '../types/api'

export const membersApi = {
  getMyProfile() {
    return client.get<ApiResponse<MemberProfileResponse>>('/members/me')
  },

  updateMyProfile(data: UpdateProfileRequest) {
    return client.put<ApiResponse<MemberProfileResponse>>('/members/me', data)
  },

  changeMyPassword(data: ChangePasswordRequest) {
    return client.put<ApiResponse>('/members/me/password', data)
  },

  getMemberList(params: MemberListRequest) {
    return client.get<ApiResponse<MemberListResponse>>('/members', { params })
  },

  getMemberById(id: string) {
    return client.get<ApiResponse<MemberProfileResponse>>(`/members/${id}`)
  },

  changeRole(id: string, data: ChangeRoleRequest) {
    return client.put<ApiResponse>(`/members/${id}/role`, data)
  },

  changeStatus(id: string, data: ChangeStatusRequest) {
    return client.put<ApiResponse>(`/members/${id}/status`, data)
  },

  softDelete(id: string) {
    return client.delete<ApiResponse>(`/members/${id}`)
  },
}
