import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  AbstractControl,
  FormBuilder,
  Validators
} from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { HttpErrorResponse } from "@angular/common/http";

import { AuthenticationService } from "src/app/_services/authentication.service";
import { LoginModel } from "src/app/_models/login-model";
import { ResponseModel } from "src/app/_models/response";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent implements OnInit {
  submitted: boolean = false;
  loading: boolean = false;
  loginForm: FormGroup;
  returnUrl: string;
  user: LoginModel;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.loginForm = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required]]
    }
    );
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"];
  }

  login() {
    this.submitted = true;
    if (this.loginForm.valid) {
      
      this.loading = true;
      this.user = Object.assign({}, this.loginForm.value);

      this.authService
        .login(this.user)
        // .subscribe(this.handleLoginSuccess, this.handleLoginError);
        .subscribe((x:ResponseModel)=>{
          if (!x.HasError) {
            this.toastr.success(x.Message, "Success");
            this.router.navigate([this.returnUrl || '/shophomepage'])
          } else {
            this.toastr.error(x.Message, "Error");
            console.log(x.Message)
          }
          this.loading = false;
        },
        (err: HttpErrorResponse) =>{
          if (
            err.status === 401 ||
            err.status === 403 ||
            err.status === 500 ||
            err.status === 400
          ) {
            this.toastr.error(err.error || "An error has occured.", "Status");
            this.loading = false;
          }
        }
        )
    }
  }

 
}
