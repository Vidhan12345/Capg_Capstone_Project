import { Component, OnInit, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ClientService } from '../../../services/client.service';
import { ClientDto } from '../../../models/client';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [NgIf, RouterLink, MatTableModule, MatButtonModule, MatIconModule, MatChipsModule, MatSnackBarModule],
  templateUrl: './client-list.html',
  styleUrl: './client-list.css'
})
export class ClientListComponent implements OnInit {
  private clientService = inject(ClientService);
  private snackBar = inject(MatSnackBar);

  clients: ClientDto[] = [];
  displayedColumns: string[] = ['clientName', 'clientLocation', 'clientPhoneNumber', 'status', 'actions'];

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.clientService.getAll().subscribe(res => this.clients = res.data);
  }

  onDelete(id: number, name: string): void {
    if (confirm(`Delete client "${name}"?`)) {
      this.clientService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Client deleted', 'Close', { duration: 3000 });
          this.loadClients();
        },
        error: (err) => this.snackBar.open(err.error?.message || 'Delete failed', 'Close', { duration: 5000 })
      });
    }
  }
}
