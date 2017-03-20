export interface User {
  userName: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  updatedOn?: string;
  createdOn?: string;
  isEnabled?: boolean;
  password: string;
  notifications: boolean;
}
