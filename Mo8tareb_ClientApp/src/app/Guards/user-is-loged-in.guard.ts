import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AccountApiService } from '../Services/account-api.service';

@Injectable({
  providedIn: 'root'
})
export class UserIsLogedInGuard implements CanActivate {
  constructor(private authenticationService:AccountApiService){}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean | UrlTree> | boolean | UrlTree
  {
    if (this.authenticationService.IsLoggedIn())
      return true;
    else
      window.location.href = "/login/";
    return false;
  }
}
