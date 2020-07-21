export interface ChangePasswordDTO {
  userName: string;
  currentPassword: string;
  newPassword: string;
  newPasswordConfirmation: string;
}

export interface User {
  userName: string;
  name: string;
  lastName: string;
  lastEditedReport: number | null;
  isAdmin: boolean;
}