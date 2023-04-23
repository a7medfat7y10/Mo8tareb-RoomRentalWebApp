import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountApiService } from '../Services/account-api.service';

@Injectable({
  providedIn: 'root'
})
export class IsInAdminRoleGuard implements CanActivate {
  constructor(private AuthCrud:AccountApiService) {

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.AuthCrud.GetRole()?.includes("Admin"))
      return true;
    else
      window.location.href = "/unAuthorized/";
      return false;
  }
}
