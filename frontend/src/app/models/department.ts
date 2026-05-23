export interface DepartmentDto {
  departmentId: number;
  name: string;
  description: string | null;
  employeeCount: number;
}

export interface CreateDepartmentDto {
  name: string;
  description?: string;
}

export interface UpdateDepartmentDto {
  name?: string;
  description?: string;
}
