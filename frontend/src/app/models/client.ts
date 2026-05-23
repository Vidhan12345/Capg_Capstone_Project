export interface ClientDto {
  clientId: number;
  clientName: string;
  clientAddress: string | null;
  clientPhoneNumber: string | null;
  clientLocation: string | null;
  status: boolean;
}

export interface CreateClientDto {
  clientName: string;
  clientAddress?: string;
  clientPhoneNumber?: string;
  clientLocation?: string;
  status?: boolean;
}

export interface UpdateClientDto {
  clientName?: string;
  clientAddress?: string;
  clientPhoneNumber?: string;
  clientLocation?: string;
  status?: boolean;
}
