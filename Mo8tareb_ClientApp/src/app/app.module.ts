import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { AccountApiService } from './Services/account-api.service';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { EmailConfirmationComponent } from './Components/email-confirmation/email-confirmation.component';
import { ForgetPasswordComponent } from './Components/forget-password/forget-password.component';
import { Error404Component } from './Components/error404/error404.component';
import { UnAuthorizedErrorComponent } from './Components/un-authorized-error/un-authorized-error.component';
import { HomeComponent } from './Components/home/home.component';
import { AdminDashboardComponent } from './Components/admin-dashboard/admin-dashboard.component';
import { RoomOwnerDashboardComponent } from './Components/room-owner-dashboard/room-owner-dashboard.component';
import { CheckingCredintialsComponent } from './Components/checking-credintials/checking-credintials.component';
import { RestPasswordComponent } from './Components/rest-password/rest-password.component';
import { TokenInterceptor } from './Interceptors/token.interceptor';
import { HeaderComponent } from './Components/header/header.component';
import { FooterComponent } from './Components/footer/footer.component';
import { AboutComponent } from './Components/about/about.component';
import { ContactComponent } from './Components/contact/contact.component';
import { RoomsComponent } from './Components/rooms/rooms.component';
import { RoomDetailsComponent } from './Components/room-details/room-details.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    EmailConfirmationComponent,
    ForgetPasswordComponent,
    Error404Component,
    UnAuthorizedErrorComponent,
    HomeComponent,
    AdminDashboardComponent,
    RoomOwnerDashboardComponent,
    CheckingCredintialsComponent,
    RestPasswordComponent,
    HeaderComponent,
    FooterComponent,
    AboutComponent,
    ContactComponent,
    RoomsComponent,
    RoomDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [AccountApiService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi:true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
