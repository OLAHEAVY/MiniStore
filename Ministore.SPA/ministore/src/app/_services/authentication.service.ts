import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { JwtHelper } from 'angular2-jwt';

import { RegisterModel } from "../_models/register-model";
import { LoginModel } from "../_models/login-model";

@Injectable()
export class AuthenticationService {
    decodedToken: any;
  constructor(private http: HttpClient, private jwt: JwtHelper) {}

  baseUrl = environment.apiUrl + "auth/";

  
  setToken(user: any) {
    localStorage.setItem("user", user);
  }

  getToken() {
    return JSON.parse(localStorage.getItem("user"));
  }

  //register method
  register(model: RegisterModel) {
    return this.http.post(`${this.baseUrl}register`, model);
  }

  //login method
  login(model: LoginModel) {

    return this.http.post<any>(`${this.baseUrl}login`, model).pipe(
      map(this.handleResponse.bind(this))
    );
  }
  handleResponse(response: any){
    if (response) {
      if (response.Result) {
        this.decodedToken = this.jwt.decodeToken(response.Result.Token);
        this.setToken(JSON.stringify(response.Result.Token));
      }
      return response;
    }
  };
  loggedin() {
    const token = localStorage.getItem('token');
    return !this.jwt.isTokenExpired(token);
  }

  roleMatch(allowedRoles): boolean {
    let isMatch = false;
    const userRoles = this.decodedToken.role as Array<string>;
    allowedRoles.forEach(element => {
      if (userRoles.includes(element)) {
        isMatch = true;
        return;
      }
    });
    return isMatch;
  }
}
