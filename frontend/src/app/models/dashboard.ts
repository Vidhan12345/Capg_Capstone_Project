export interface DashboardStatsDto {
  totalEmployees: number;
  presentToday: number;
  absentToday: number;
  pendingLeaves: number;
  activeProjects: number;
}

export interface AttendanceTrendDto {
  date: string;
  present: number;
  absent: number;
}

export interface DepartmentDistributionDto {
  department: string;
  count: number;
}
