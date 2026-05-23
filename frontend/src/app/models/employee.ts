export interface EmployeeDto {
  employeeId: number;
  employeeCode: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  dateOfBirth: string;
  dateOfJoining: string | null;
  gender: string | null;
  address: string | null;
  emergencyContact: string | null;
  profileImage: string | null;
  isActive: boolean;
  departmentName: string;
  roleName: string;
  departmentId: number;
  roleId: number;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  dateOfJoining?: string;
  gender?: string;
  address?: string;
  emergencyContact?: string;
  departmentId: number;
  roleId: number;
}

export interface UpdateEmployeeDto {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  dateOfJoining?: string;
  gender?: string;
  address?: string;
  emergencyContact?: string;
  departmentId?: number;
  roleId?: number;
  isActive?: boolean;
}
