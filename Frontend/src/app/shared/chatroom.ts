export enum ChatType {
  Public = 0,
  Private
}

export interface ChatroomInfoBase {
  id: string;
  users: string[];
  type: ChatType;
  lastMessageDate: Date;
  unreadMessages: number;
}

export interface PrivateChatroomInfo extends ChatroomInfoBase {}

export interface PublicChatroomInfo extends ChatroomInfoBase {
  name: string;
  moderators: string[];
}
