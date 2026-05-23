export interface AttendanceDto {
  attendanceId: number;
  employeeId: number;
  employeeName: string | null;
  date: string;
  checkIn: string;
  checkOut: string | null;
  totalHours: number | null;
  workMode: string;
  status: string;
}

export interface CheckInRequest {
  checkIn: string;
  workMode?: string;
}

export interface CheckOutRequest {
  checkOut: string;
}
