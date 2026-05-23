export interface AnnouncementDto {
  announcementId: number;
  title: string;
  content: string;
  postedByName: string | null;
  postedAt: string;
  expiresAt: string | null;
  isActive: boolean;
}

export interface CreateAnnouncementDto {
  title: string;
  content: string;
  expiresAt?: string;
}

export interface UpdateAnnouncementDto {
  title?: string;
  content?: string;
  expiresAt?: string;
  isActive?: boolean;
}
