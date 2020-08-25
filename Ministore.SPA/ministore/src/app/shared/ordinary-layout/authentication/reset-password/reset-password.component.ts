import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthenticationService } from "src/app/_services/authentication.service";
import { ActivatedRoute, Router } from "@angular/router";
import { ResponseModel } from 'src/app/_models/response';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: "app-reset-password",
  templateUrl: "./reset-password.component.html",
  styleUrls: ["./reset-password.component.css"]
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  submitted: boolean = false;
  loading: boolean = false;
  decodedToken: any;
  token: any;
  userEmail: string;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit() {
   this.createResetPasswordForm();
   this.getEmail();
  }

  //create resetPassword form

  createResetPasswordForm(){
    this.resetPasswordForm = this.fb.group(
      {
        newPassword: [
          "",
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(11)
          ]
        ],
        confirmPassword: ["", Validators.required]
      },
      { validator: this.passwordMatchValidator }
    );
  }

  // custom validator for the password fields
  passwordMatchValidator(g: FormGroup) {
    return g.get("newPassword").value === g.get("confirmPassword").value
      ? null
      : { mismatch: true };
  }

  getEmail(){
    var snapshot = this.activatedRoute.snapshot;
    this.token = snapshot.queryParamMap.get("token");
    this.decodedToken = encodeURIComponent(this.token);
    this.authService.confirmResetPasswordCode(this.decodedToken).subscribe((x : ResponseModel) => {
        if(!x.HasError){
          this.userEmail = x.Result;
        }
    });

  }

  resetPassword() {
    this.submitted = true;
    if (this.resetPasswordForm.valid) {
      this.loading = true;
      var snapshot = this.activatedRoute.snapshot;
      this.token = snapshot.queryParamMap.get("token");
      this.decodedToken = encodeURIComponent(this.token);

      this.authService.resetPassword(
        this.decodedToken,
        this.resetPasswordForm.get("newPassword").value
      ).subscribe((x : ResponseModel) => {
        if(!x.HasError){
          this.toastr.success(x.Message, "Success");
          this.router.navigate(['/login'])
        }else{
          this.toastr.error(x.Message, "Error");
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
      });
    }
  }
}
