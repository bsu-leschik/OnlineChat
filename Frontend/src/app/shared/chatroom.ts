export enum ChatType {
  Public = 0,
  Private
}

export interface ChatroomInfo {
  id: string;
  usersCount: number;
  users: string[];
  chatType: ChatType;
}
