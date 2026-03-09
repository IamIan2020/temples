import client from './client'
import type {
  ApiResponse,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest,
} from '../types/api'

export const authApi = {
  register(data: RegisterRequest) {
    return client.post<ApiResponse<LoginResponse>>('/auth/register', data)
  },

  login(data: LoginRequest) {
    return client.post<ApiResponse<LoginResponse>>('/auth/login', data)
  },

  refreshToken(refreshToken: string) {
    return client.post<ApiResponse<LoginResponse>>('/auth/refresh-token', { refreshToken })
  },

  forgotPassword(data: ForgotPasswordRequest) {
    return client.post<ApiResponse>('/auth/forgot-password', data)
  },

  resetPassword(data: ResetPasswordRequest) {
    return client.post<ApiResponse>('/auth/reset-password', data)
  },
}
