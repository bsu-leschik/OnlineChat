import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import { StorageService } from '../shared/services/storage.service';
import { AuthGuard } from '../shared/guards/auth-guard.service';
import { AuthenticationService } from '../shared/services/authentication.service';

@NgModule({
  declarations: [RegistrationComponent, LoginComponent, AuthComponent],
  imports: [
    CommonModule,
    FormsModule,
    AuthRoutingModule
  ],
  providers: [StorageService, AuthGuard, AuthenticationService],
  bootstrap:[AuthComponent]
})
export class AuthModule { }
