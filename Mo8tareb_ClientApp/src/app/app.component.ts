import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AccountApiService } from './Services/account-api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'clientApp';
  showNavbar:any;
  constructor(private ActivatedRoute:ActivatedRoute,private AuthenticationService: AccountApiService, private router:Router) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.showNavbar =! (event.url === '/login'
                            || event.url === '/register'
                            || event.url === '/EmailConfirmation'
                            || event.url === '/forgetPassword'
                            || event.url === '/resetPassword'
                            || event.url === '/unAuthorized'
                            || event.url === '/Admindashboard'
                            || event.url === '/Admindashboard/reviews'
                            || event.url === '/Admindashboard/services'
                            || event.url === '/Admindashboard/rooms'
                            || event.url === '/Admindashboard/reservations'
                            || event.url === '/OwnerDashboard'
                            || event.url === '/OwnerDashboard/rooms'
                            || event.url.indexOf('/OwnerDashboard/editroom') != -1
                            || event.url === '/OwnerDashboard/reservations'
                            || event.url === '/OwnerDashboard/AddRoom'
                            || event.url === '/Admindashboard/AddService'
                            || event.url === '/Mo8tarebGBT'
                            );
      }
    });
  }

}
