export interface AttendanceReportDto {
  attendanceId: number;
  employeeId: number;
  employeeName: string;
  employeeCode: string;
  department: string;
  date: string;
  checkIn: string;
  checkOut: string | null;
  totalHours: number | null;
  workMode: string;
  status: string;
}

export interface LeaveReportDto {
  leaveId: number;
  employeeId: number;
  employeeName: string;
  employeeCode: string;
  department: string;
  fromDate: string;
  toDate: string;
  daysRequested: number;
  leaveType: string;
  reason: string;
  status: string;
  approvedByName: string | null;
  appliedAt: string;
  reviewedAt: string | null;
}

export interface EmployeeReportDto {
  employeeId: number;
  employeeCode: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string | null;
  departmentName: string;
  roleName: string;
  isActive: boolean;
  dateOfBirth: string;
  totalProjects: number;
  totalLeaves: number;
  totalAttendance: number;
}
