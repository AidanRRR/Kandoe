export interface User {
  userName: string;
  firstName?: string;
  lastName?: string;
  email: string;
  notifications: boolean;
  updatedOn?: string;
  createdOn?: string;
  isEnabled?: boolean;
}
