// 統一回應格式
export interface ApiResponse<T = unknown> {
  success: boolean
  data: T | null
  message: string | null
  errors: string[]
}

// Auth DTOs
export interface RegisterRequest {
  email: string
  password: string
  confirmPassword: string
  displayName: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface LoginResponse {
  accessToken: string
  refreshToken: string
  expiresAt: string
  user: UserInfo
}

export interface UserInfo {
  id: string
  email: string
  displayName: string
  roles: string[]
  permissions: string[]
}

export interface ForgotPasswordRequest {
  email: string
}

export interface ResetPasswordRequest {
  email: string
  token: string
  newPassword: string
}

export interface RefreshTokenRequest {
  refreshToken: string
}

// Member DTOs
export interface MemberProfileResponse {
  id: string
  email: string
  displayName: string
  chineseName: string | null
  birthday: string | null
  gender: string | null
  address: string | null
  memberNumber: string | null
  joinDate: string | null
  createdAt: string
  isActive: boolean
  roles: string[]
}

export interface UpdateProfileRequest {
  displayName: string
  chineseName: string | null
  birthday: string | null
  gender: string | null
  address: string | null
}

export interface ChangePasswordRequest {
  currentPassword: string
  newPassword: string
}

export interface MemberListRequest {
  search?: string
  page?: number
  pageSize?: number
}

export interface MemberListResponse {
  items: MemberProfileResponse[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export interface ChangeRoleRequest {
  role: string
}

export interface ChangeStatusRequest {
  isActive: boolean
}

// Settings DTOs
export interface PublicSettingResponse {
  companyName: string
  websiteName: string
  phone: string | null
  copyright: string
  sessionTimeoutMinutes: number
  address: string | null
  fax: string | null
  lineUrl: string | null
  facebookUrl: string | null
  googleMapUrl: string | null
  logoUrl: string | null
}

export interface SystemSettingResponse {
  id: number
  companyName: string
  websiteName: string
  phone: string | null
  taxId: string | null
  copyright: string
  sessionTimeoutMinutes: number
  address: string | null
  fax: string | null
  lineUrl: string | null
  facebookUrl: string | null
  googleMapUrl: string | null
  logoUrl: string | null
  updatedAt: string
}

export interface UpdateSystemSettingRequest {
  companyName: string
  websiteName: string
  phone: string | null
  taxId: string | null
  copyright: string
  sessionTimeoutMinutes: number
  address: string | null
  fax: string | null
  lineUrl: string | null
  facebookUrl: string | null
  googleMapUrl: string | null
  logoUrl: string | null
}

// Role DTOs
export interface RoleResponse {
  id: string
  name: string
  description: string | null
  isBuiltIn: boolean
  permissions: string[]
}

export interface CreateRoleRequest {
  name: string
  description: string | null
  permissions: string[]
}

export interface UpdateRoleRequest {
  name: string
  description: string | null
  permissions: string[]
}

export interface PermissionItem {
  code: string
  displayName: string
}

export interface PermissionGroupResponse {
  groupName: string
  permissions: PermissionItem[]
}
