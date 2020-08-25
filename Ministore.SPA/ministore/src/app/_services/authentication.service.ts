import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { JwtHelper } from 'angular2-jwt';

import { RegisterModel } from "../_models/register-model";
import { LoginModel } from "../_models/login-model";
import { DropdownModel } from '../_models/dropdown-model';
import { Observable } from 'rxjs';
import { ResponseModel } from '../_models/response';
import { ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';

@Injectable()
export class AuthenticationService {
    decodedToken: any;
  constructor(private http: HttpClient, private jwt: JwtHelper, private activatedRoute: ActivatedRoute) {}

  baseUrl = environment.apiUrl + "auth/";
  stateUrl = environment.apiUrl + "state/";
  localGovernmentUrl = environment.apiUrl + "localgovernment/"

  
  setToken(user: any) {
    localStorage.setItem("user", user);
  }

  getToken() {
    return JSON.parse(localStorage.getItem("user"));
  }

  //register method
  register(model: RegisterModel) {
    return this.http.post(`${this.baseUrl}registeruser`, model);
  }

  getStates(): Observable<ResponseModel> {
    return this.http.get<ResponseModel>(`${this.stateUrl}getallstates`);
  }

  getLocalGovernments(id:string): Observable<ResponseModel> {
    return this.http.get<ResponseModel>(`${this.localGovernmentUrl}getalllocalgovernments?stateId=${id}`);
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
    const token = localStorage.getItem('user');
    if(token != null){
      return !this.jwt.isTokenExpired(token);
    }
    
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

  confirmAccount(token: string) {
    return this.http.get(`${this.baseUrl}confirmaccount?token=${token}`);
}

 resendConfirmationLink(model: any){
   return this.http.post(`${this.baseUrl}resendLink`, model);
 }

 sendPasswordResetLink(model:any){
   return this.http.post(`${this.baseUrl}sendpasswordresetlink`, model);
 }

 confirmResetPasswordCode(token: string){
   return this.http.get(`${this.baseUrl}confirmpasswordresetcode?token=${token}`);
 }

 resetPassword(token: string, newPassword: string){
  //i stopped here

  var data = {token, newPassword}
  return this.http.post(`${this.baseUrl}resetpassword`, data);
 }
}
