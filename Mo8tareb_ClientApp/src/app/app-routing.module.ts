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
import { IsInAdminRoleGuard } from './Guards/is-in-admin-role.guard';
import { RoomOwnerDashboardComponent } from './Components/room-owner-dashboard/room-owner-dashboard.component';
import { CheckingCredintialsComponent } from './Components/checking-credintials/checking-credintials.component';
import { RestPasswordComponent } from './Components/rest-password/rest-password.component';
import { IsInRoomOwnerRoleGuard } from './Guards/is-in-room-owner-role.guard';
import { AboutComponent } from './Components/about/about.component';
import { ContactComponent } from './Components/contact/contact.component';
import { RoomsComponent } from './Components/rooms/rooms.component';
import { RoomDetailsComponent } from './Components/room-details/room-details.component';
import { AdminReviewComponent } from './Components/admin-dashboard/admin-review/admin-review.component';
import { AdminServicesComponent } from './Components/admin-dashboard/admin-services/admin-services.component';
import { ReservationsComponent } from './Components/admin-dashboard/reservations/reservations.component';
import { AdminRoomsComponent } from './Components/admin-dashboard/admin-rooms/admin-rooms.component';
import { OwnerRoomsComponent } from './Components/room-owner-dashboard/owner-rooms/owner-rooms.component';
import { OwnerRerservationsComponent } from './Components/room-owner-dashboard/owner-rerservations/owner-rerservations.component';
import { OwnerCreateRoomComponent } from './Components/room-owner-dashboard/owner-create-room/owner-create-room.component';
import { AdminCreateServiceComponent } from './Components/admin-dashboard/admin-create-service/admin-create-service.component';
import { ReserveRoomComponent } from './Components/reserve-room/reserve-room.component';
import { UserIsLogedInGuard } from './Guards/user-is-loged-in.guard';
import { ReservationSuspendedComponent } from './Components/reservation-suspended/reservation-suspended.component';
import { ReservationApprovedComponent } from './Components/reservation-approved/reservation-approved.component';
import { ReservationRejectedComponent } from './Components/reservation-rejected/reservation-rejected.component';
import { PaymentComponent } from './Components/payment/payment.component';
import { ReservationGuardGuard } from './Guards/reservation-guard.guard';
import { CheckingReservationComponent } from './Components/checking-reservation/checking-reservation.component';
import { Mo8tarebGBTComponent } from './Components/mo8tareb-gbt/mo8tareb-gbt.component';
import { EditReservationComponent } from './Components/edit-reservation/edit-reservation.component';
import { MyRoomsComponent } from './Components/my-rooms/my-rooms.component';
import { JoinAsOwnerComponent } from './Components/join-as-owner/join-as-owner.component';
import { AIWelcomePageComponent } from './Components/aiwelcome-page/aiwelcome-page.component';
import { OwnerEditRoomsComponent } from './Components/room-owner-dashboard/owner-edit-rooms/owner-edit-rooms.component';
import { PaymentSuccessComponent } from './Components/payment-success/payment-success.component';
import { PaymentFailureComponent } from './Components/payment-failure/payment-failure.component';


const routes: Routes = [
  {path: "Admindashboard", component:AdminDashboardComponent,canActivate:[IsInAdminRoleGuard]},
  {path: "Admindashboard/reviews", component:AdminReviewComponent,canActivate:[IsInAdminRoleGuard]},
  {path: "Admindashboard/rooms", component:AdminRoomsComponent,canActivate:[IsInAdminRoleGuard]},
  {path: "Admindashboard/services", component:AdminServicesComponent,canActivate:[IsInAdminRoleGuard]},
  {path: "Admindashboard/reservations", component:ReservationsComponent,canActivate:[IsInAdminRoleGuard]},
  {path: "Admindashboard/AddService", component:AdminCreateServiceComponent,canActivate:[IsInAdminRoleGuard]},
  {path: "OwnerDashboard", component: RoomOwnerDashboardComponent, canActivate: [IsInRoomOwnerRoleGuard] },
  {path: "OwnerDashboard/reservations", component:OwnerRerservationsComponent,canActivate:[IsInRoomOwnerRoleGuard]},
  {path: "OwnerDashboard/rooms", component:OwnerRoomsComponent,canActivate:[IsInRoomOwnerRoleGuard]},
  {path: "OwnerDashboard/editroom/:id", component:OwnerEditRoomsComponent,canActivate:[IsInRoomOwnerRoleGuard]},
  {path: "OwnerDashboard/AddRoom", component:OwnerCreateRoomComponent,canActivate:[IsInRoomOwnerRoleGuard]},
  {path: "login", component:LoginComponent},
  {path:  "register", component: RegisterComponent },
  {path: "EmailConfirmation", component: EmailConfirmationComponent},
  {path: "forgetPassword",component: ForgetPasswordComponent},
  {path: "resetPassword", component: RestPasswordComponent},
  {path: "unAuthorized", component: UnAuthorizedErrorComponent},
  {path: "", component: HomeComponent},
  {path: "home", component: HomeComponent},
  {path: "about", component: AboutComponent},
  {path: "contact", component: ContactComponent},
  {path: "rooms", component: RoomsComponent},
  {path: "loading", component: CheckingCredintialsComponent},
  {path: "room/:id", component: RoomDetailsComponent },
  {path: "ReserveRoom/:id", component: ReserveRoomComponent,canActivate:[UserIsLogedInGuard] },
  {path: "ReservationSuspended/:id", component: ReservationSuspendedComponent,canActivate:[UserIsLogedInGuard] },
  {path: "ReservationApproved/:id", component: ReservationApprovedComponent,canActivate:[UserIsLogedInGuard] },
  {path: "ReservationRejected/:id", component: ReservationRejectedComponent,canActivate:[UserIsLogedInGuard] },
  {path: "CheckingReservation/:id", component: CheckingReservationComponent,canActivate:[UserIsLogedInGuard] },
  {path: "payment/:id", component: PaymentComponent,canActivate:[UserIsLogedInGuard] },
  {path: "Mo8tarebGBT", component : Mo8tarebGBTComponent,canActivate:[UserIsLogedInGuard]},
  {path: "EditReserveRoom/:id", component : EditReservationComponent,canActivate:[UserIsLogedInGuard]},
  {path: "MyRooms", component : MyRoomsComponent,canActivate:[UserIsLogedInGuard]},
  {path: "JoinUsAsOwner", component : JoinAsOwnerComponent,canActivate:[UserIsLogedInGuard]},
  {path: "payment-success", component : PaymentSuccessComponent,canActivate:[UserIsLogedInGuard]},
  {path: "payment-failure/:id", component : PaymentFailureComponent,canActivate:[UserIsLogedInGuard]},
  {path: "Mo8tarebGBTWelcomePage", component : AIWelcomePageComponent,canActivate:[UserIsLogedInGuard]},
  {path: "**", component:Error404Component}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
