import { Injectable } from "@angular/core";
import { JwtHelper } from 'angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { tap } from 'rxjs/operators';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'DoNotIntercept': 'true'
    })
}
@Injectable()

export class AuthenticationService{
    baseUrl = environment.apiUrl + 'auth/';
    constructor(private http: HttpClient, private jwt:JwtHelper){}

    setToken(user: any){
        localStorage.setItem('user', user);
    }

    getToken(){
        return JSON.parse(localStorage.getItem('user'))
    }

    getTokenData(){
        let data = JSON.parse(localStorage.getItem('user'))
        let obj = this.jwt.decodeToken(data.Token);
        return obj;
    }

    login(Username: string, Password: string) {
        let data = {
            UserName: Username,
            Password: Password
        }

        return this.http.post<any>(this.baseUrl + 'login', data, httpOptions).pipe(tap(response => {
            if (response) {
                if (response.Result) {

                    this.setToken(JSON.stringify(response.Result));
                }
                return response;
            }

        }))
    }
}