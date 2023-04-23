import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountApiService } from '../Services/account-api.service';
// import { AccountApiService } from './Services/account-api.service';

@Injectable({
  providedIn: 'root'
})
export class IsInRoomOwnerRoleGuard implements CanActivate {

  constructor(private AuthCrud:AccountApiService){}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    if (this.AuthCrud.GetRole()?.includes("Owner")) {
      console.log(this.AuthCrud.GetRole());
        return true;
      }
      else
      window.location.href = "/unAuthorized";
      return false;
  }

}
