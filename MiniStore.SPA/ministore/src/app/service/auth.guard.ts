import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { JwtHelper } from 'angular-jwt'
import { ToastrService } from 'ngx-toastr';
import { Title } from '@angular/platform-browser';
import { AuthenticationService } from './authentication.service';


@Injectable()

export class AuthGuard implements CanActivate {

    constructor(private authenticationService: AuthenticationService,
        private router: Router,
        private title: Title,
        private toastr: ToastrService,
        private jwtHelper: JwtHelper) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        let token = this.authenticationService.getToken();

        this.title.setTitle(route.data['title']);
        if (token) {
            if (!this.jwtHelper.isTokenExpired(token.Token)) {
                return true;
            }
            else {
                this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } })
                return false;
            }
        }
        this.router.navigate(['/login'])
        return false;
    }
}