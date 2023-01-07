import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {ChatComponent} from "./chat/chat.component";
import {ChatSelectorComponent} from "./chat-selector/chat-selector.component";
import {RegistrationComponent} from "./registration/registration.component";

const routes: Routes = [
    {path: 'login', component: LoginComponent},
    {path: 'chat', component: ChatComponent},
    {path: 'select-chat', component: ChatSelectorComponent},
    {path: 'registration', component: RegistrationComponent},
    {path: '**', redirectTo: 'login'}
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
