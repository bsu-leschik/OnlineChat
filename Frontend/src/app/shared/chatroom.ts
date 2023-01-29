export enum ChatType {
  Public = 0,
  Private
}

export interface MinimumChatroomInfo{
  id: string
  users: string[];
  chatType: ChatType.Private
  lastMessageDate: Date;
}

export interface StandardChatroomInfo{
  id: string
  users: string[];
  owner: string;
  moderators: string;
  chatType: ChatType.Public
  lastMessageDate: Date;
}
