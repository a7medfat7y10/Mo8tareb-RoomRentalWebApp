import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { EmailConfirmationComponent } from './Components/email-confirmation/email-confirmation.component';
import { ForgetPasswordComponent } from './Components/forget-password/forget-password.component';
import { Error404Component } from './Components/error404/error404.component';
import { UnAuthorizedErrorComponent } from './Components/un-authorized-error/un-authorized-error.component';
import { HomeComponent } from './Components/home/home.component';
import { AdminDashboardComponent } from './Components/admin-dashboard/admin-dashboard.component';
import { IsInAdminRoleGuard } from './is-in-admin-role.guard';
import { RoomOwnerDashboardComponent } from './Components/room-owner-dashboard/room-owner-dashboard.component';
import { IsInRoomOwnerRoleGuard } from './is-in-room-owner-role.guard';
import { CheckingCredintialsComponent } from './Components/checking-credintials/checking-credintials.component';
import { RestPasswordComponent } from './Components/rest-password/rest-password.component';

const routes: Routes = [
  {path:"Admindashboard",component:AdminDashboardComponent,canActivate:[IsInAdminRoleGuard]},
  {path: "OwnerDashboard", component: RoomOwnerDashboardComponent, canActivate: [IsInRoomOwnerRoleGuard] },
  {path:"login",component:LoginComponent},
  { path: "register", component: RegisterComponent },
  {path:"EmailConfirmation",component:EmailConfirmationComponent},
  {path:"forgetPassword",component:ForgetPasswordComponent},
  {path:"resetPassword",component:RestPasswordComponent},
  {path:"unAuthorized",component:UnAuthorizedErrorComponent},
  // {path:"",component:HomeComponent},
  {path:"home",component:HomeComponent},
  {path:"loading",component:CheckingCredintialsComponent},
  {path:"**",component:Error404Component}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
