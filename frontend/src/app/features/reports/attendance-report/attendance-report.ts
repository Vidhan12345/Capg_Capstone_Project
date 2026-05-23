import { Component, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe, NgClass, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ViewChild } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { AttendanceReportDto } from '../../../models/report';

@Component({
  selector: 'app-attendance-report',
  standalone: true,
  imports: [NgIf, NgFor, DecimalPipe, DatePipe, NgClass, FormsModule, MatCardModule, MatTableModule, MatPaginatorModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, MatSelectModule, MatButtonModule],
  templateUrl: './attendance-report.html',
  styleUrl: './report-common.css'
})
export class AttendanceReportComponent {
  private reportService = inject(ReportService);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  records: AttendanceReportDto[] = [];
  totalCount = 0;
  displayedColumns = ['employeeName', 'employeeCode', 'department', 'date', 'checkIn', 'checkOut', 'totalHours', 'status'];
  filters = { fromDate: '', toDate: '', department: '', status: '' };
  page = 1;
  pageSize = 10;

  loadReport(): void {
    this.reportService.getAttendance({ ...this.filters, page: this.page, pageSize: this.pageSize }).subscribe(res => {
      this.records = res.data.items;
      this.totalCount = res.data.totalCount;
    });
  }
}
