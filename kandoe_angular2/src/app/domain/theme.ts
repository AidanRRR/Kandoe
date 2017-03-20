import {Card} from "./card";

export interface Theme {
  themeId: string;
  name: string;
  description: string;
  cards?: Card[];
  organizers: string[];
  updatedOn?: Date;
  createdOn?: Date;
  isEnabled?: boolean;
  isPublic: boolean;
  tags?: string[];
  username: string;
}
