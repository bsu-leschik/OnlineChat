import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { AppComponent } from './app.component';
import { AuthGuard } from './shared/guards/auth-guard.service';
import { ChatsGuard } from './shared/guards/chats-guard.service';

const routes: Routes = [
  {path: '', component: AppComponent},
  {path: 'auth', canActivate: [AuthGuard], loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)},
  {path: 'chats', canActivate: [ChatsGuard], loadChildren: () => import('./chats/chats.module').then(m => m.ChatsModule)}

];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation: 'reload'})],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
