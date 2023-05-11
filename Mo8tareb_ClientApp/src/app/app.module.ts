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


//Translation
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AdminHeaderComponent } from './Components/admin-dashboard/admin-header/admin-header.component';
import { AdminReviewComponent } from './Components/admin-dashboard/admin-review/admin-review.component';
import { AdminServicesComponent } from './Components/admin-dashboard/admin-services/admin-services.component';
import { ReservationsComponent } from './Components/admin-dashboard/reservations/reservations.component';
import { AdminRoomsComponent } from './Components/admin-dashboard/admin-rooms/admin-rooms.component';
import { OwnerHeaderComponent } from './Components/room-owner-dashboard/owner-header/owner-header.component';
import { OwnerRoomsComponent } from './Components/room-owner-dashboard/owner-rooms/owner-rooms.component';
import { OwnerRerservationsComponent } from './Components/room-owner-dashboard/owner-rerservations/owner-rerservations.component';
import { OwnerCreateRoomComponent } from './Components/room-owner-dashboard/owner-create-room/owner-create-room.component';
import { AdminCreateServiceComponent } from './Components/admin-dashboard/admin-create-service/admin-create-service.component';

import { ReservationSuspendedComponent } from './Components/reservation-suspended/reservation-suspended.component';
import { ReservationApprovedComponent } from './Components/reservation-approved/reservation-approved.component';
import { ReservationRejectedComponent } from './Components/reservation-rejected/reservation-rejected.component';
import { PaymentComponent } from './Components/payment/payment.component';
import { CheckingReservationComponent } from './Components/checking-reservation/checking-reservation.component';
import { ReserveRoomComponent } from './Components/reserve-room/reserve-room.component';
import { MyRoomsComponent } from './Components/my-rooms/my-rooms.component';
import { JoinAsOwnerComponent } from './Components/join-as-owner/join-as-owner.component';
import { Mo8tarebGBTComponent } from './Components/mo8tareb-gbt/mo8tareb-gbt.component';
import { EditReservationComponent } from './Components/edit-reservation/edit-reservation.component';
import { UserControllerServiceService } from './Services/user-controller-service.service';
import { RoomServiceService } from './Services/room-service.service';
import { ReviewService } from './Services/review-service.service';
import { Mo8tarebGBTService } from './Services/mo8tareb-gbt.service';
import { ReservationServiceService } from './Services/reservation-service.service';
import { AIWelcomePageComponent } from './Components/aiwelcome-page/aiwelcome-page.component';
import { OwnerEditRoomsComponent } from './Components/room-owner-dashboard/owner-edit-rooms/owner-edit-rooms.component';
import { PaymentSuccessComponent } from './Components/payment-success/payment-success.component';
import { PaymentFailureComponent } from './Components/payment-failure/payment-failure.component';

//Translation
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

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
    RoomDetailsComponent,
    AdminHeaderComponent,
    AdminReviewComponent,
    AdminServicesComponent,
    ReservationsComponent,
    AdminRoomsComponent,
    OwnerHeaderComponent,
    OwnerRoomsComponent,
    OwnerRerservationsComponent,
    OwnerCreateRoomComponent,
    AdminCreateServiceComponent,
    ReserveRoomComponent,
    ReservationSuspendedComponent,
    ReservationApprovedComponent,
    ReservationRejectedComponent,
    PaymentComponent,
    CheckingReservationComponent,
    MyRoomsComponent,
    JoinAsOwnerComponent,
    Mo8tarebGBTComponent,
    EditReservationComponent,
    AIWelcomePageComponent,
    OwnerEditRoomsComponent,
    PaymentSuccessComponent,
    PaymentFailureComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,


    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })

  ],
  providers: [AccountApiService, UserControllerServiceService, Mo8tarebGBTService, ReservationServiceService,
    ReviewService,RoomServiceService,
      {
        provide: HTTP_INTERCEPTORS,
        useClass: TokenInterceptor,
        multi:true
      }],
  bootstrap: [AppComponent]
})
export class AppModule { }
