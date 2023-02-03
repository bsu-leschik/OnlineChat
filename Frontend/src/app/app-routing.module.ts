import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { AppComponent } from './app.component';
import { AuthComponent } from './auth/auth.component';
import { LoginComponent } from './auth/login/login.component';
import { RegistrationComponent } from './auth/registration/registration.component';
import { ChatSelectorComponent } from './chats/chat-selector/chat-selector.component';
import { ChatComponent } from './chats/chat/chat.component';
import { CreateChatComponent } from './chats/create-chat/create-chat.component';
import { AuthGuard } from './shared/Guards/auth-guard.service';
import { ChatsGuard } from './shared/Guards/chats-guard.service';

const routes: Routes = [
  {path: "", component: AppComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
