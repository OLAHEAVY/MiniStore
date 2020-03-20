import { Observable } from 'rxjs/Rx';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import {AuthenticationService} from '../_services/authentication.service';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Injectable()
export class ConfirmAccountResolver implements Resolve<any>{
    constructor(private authService: AuthenticationService, private toastr: ToastrService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        let token = route.queryParamMap.get('token');
        let encodedToken = encodeURIComponent(token);
        return this.authService.confirmAccount(encodedToken).catch(error => {
            console.log(error);

            this.toastr.error("Problem confirming your email");
            this.router.navigate(['/login']);

            return Observable.of(error)
        })
    }
}
