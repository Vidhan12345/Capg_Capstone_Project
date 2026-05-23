import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { AuditLogService } from '../../../services/audit-log.service';
import { AuditLogDto } from '../../../models/audit-log';

@Component({
  selector: 'app-audit-log-list',
  standalone: true,
  imports: [NgIf, NgFor, NgClass, DatePipe, FormsModule, MatCardModule, MatTableModule, MatPaginatorModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule],
  templateUrl: './audit-log-list.html',
  styleUrl: './audit-log-list.css'
})
export class AuditLogListComponent implements OnInit {
  private auditLogService = inject(AuditLogService);
  records: AuditLogDto[] = [];
  totalCount = 0;
  displayedColumns = ['auditId', 'entityName', 'recordId', 'action', 'oldValues', 'newValues', 'createdBy', 'createdOn'];
  filters = { entityName: '', action: '', createdBy: '', fromDate: '', toDate: '' };
  page = 1;
  pageSize = 10;
  actions = ['Insert', 'Update', 'Delete'];

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.auditLogService.getAll({ ...this.filters, page: this.page, pageSize: this.pageSize }).subscribe(res => {
      this.records = res.data.items;
      this.totalCount = res.data.totalCount;
    });
  }
}
