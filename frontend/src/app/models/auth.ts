export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
  role: string;
  employeeName: string;
  employeeId: number;
}

export interface LoggedInUser {
  employeeId: number;
  username: string;
  email: string;
  role: string;
  employeeCode: string;
  employeeName: string;
  token: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}
