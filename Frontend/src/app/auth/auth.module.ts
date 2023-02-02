import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';

@NgModule({
  declarations: [RegistrationComponent, LoginComponent, AuthComponent],
  imports: [
    CommonModule,
    FormsModule,
    AuthRoutingModule
  ],
  bootstrap:[AuthComponent]
})
export class AuthModule { }
