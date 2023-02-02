import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthComponent } from "./auth.component";
import { LoginComponent } from "./login/login.component";
import { RegistrationComponent } from "./registration/registration.component";

const authRoutes : Routes = [
    {path: 'auth', component: AuthComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegistrationComponent},
  ]

@NgModule({
    imports:[RouterModule.forChild(authRoutes)],
    exports:[RouterModule]
})

export class AuthRoutingModule{ }