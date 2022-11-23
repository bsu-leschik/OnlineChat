export class Constants {
  private constructor() {}

  public static readonly ServerUrl: string = 'https://localhost:7023';
  public static readonly ChathubUrl: string = Constants.ServerUrl + '/chat';
  public static readonly ChatroomsControllerUrl: string = Constants.ServerUrl + '/chatrooms';
  public static readonly CreateChatroomUrl = Constants.ChatroomsControllerUrl + '/create';
  public static readonly NicknameStorageField = 'nickname';
  public static readonly ChatIdStorageField = 'chatId';
}
