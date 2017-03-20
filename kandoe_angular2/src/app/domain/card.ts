export interface Card {
  cardId: string;
  imageUrl: string;
  text: string;
  themeId: string;
  reactions?: CardReaction[];
  updatedOn?: string;
  createdOn?: string;
  isEnabled?: boolean;
  // Enkel client-side
  selected?: boolean;
}

export interface CardReaction{
  cardId: string;
  username: string;
  message: string;
  time?: string;
}
