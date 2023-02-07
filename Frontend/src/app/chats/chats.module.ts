import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatsRoutingModule } from './chats-routing.module';
import { ChatComponent } from './chat/chat.component';
import { ChatSelectorComponent } from './chat-selector/chat-selector.component';
import { CreateChatComponent } from './create-chat/create-chat.component';
import { ChatroomService } from './shared/services/chatroom.service';
import { UsersCommunicatorService } from './shared/services/users-communicator.service';
import { HubService } from './shared/services/hub.service';



@NgModule({
  declarations: [ChatComponent, ChatSelectorComponent, CreateChatComponent],
  imports: [
    CommonModule,
    FormsModule,
    ChatsRoutingModule
  ],
  providers: [ChatroomService, UsersCommunicatorService, HubService],
  bootstrap: [ChatSelectorComponent]
})
export class ChatsModule { }
