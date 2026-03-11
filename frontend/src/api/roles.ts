import client from './client'
import type {
  ApiResponse,
  RoleResponse,
  CreateRoleRequest,
  UpdateRoleRequest,
  PermissionGroupResponse,
} from '../types/api'

export const rolesApi = {
  getAllRoles() {
    return client.get<ApiResponse<RoleResponse[]>>('/roles')
  },

  getRoleById(id: string) {
    return client.get<ApiResponse<RoleResponse>>(`/roles/${id}`)
  },

  getAllPermissions() {
    return client.get<ApiResponse<PermissionGroupResponse[]>>('/roles/permissions')
  },

  createRole(data: CreateRoleRequest) {
    return client.post<ApiResponse<RoleResponse>>('/roles', data)
  },

  updateRole(id: string, data: UpdateRoleRequest) {
    return client.put<ApiResponse<RoleResponse>>(`/roles/${id}`, data)
  },

  deleteRole(id: string) {
    return client.delete<ApiResponse<void>>(`/roles/${id}`)
  },
}
