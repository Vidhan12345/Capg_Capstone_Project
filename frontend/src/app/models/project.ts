export interface ProjectDto {
  projectId: number;
  name: string;
  description: string | null;
  startDate: string;
  endDate: string | null;
  status: string;
  clientName: string;
  clientId: number;
}

export interface CreateProjectDto {
  name: string;
  description?: string;
  startDate: string;
  endDate?: string;
  clientId: number;
  status?: string;
}

export interface UpdateProjectDto {
  name?: string;
  description?: string;
  startDate?: string;
  endDate?: string;
  clientId?: number;
  status?: string;
}

export interface AllocationDto {
  allocationId: number;
  employeeId: number;
  employeeName: string | null;
  projectId: number;
  projectName: string | null;
  roleOnProject: string | null;
  allocatedAt: string;
  releasedAt: string | null;
  isActive: boolean;
}

export interface CreateAllocationDto {
  employeeId: number;
  projectId: number;
  roleOnProject?: string;
}
