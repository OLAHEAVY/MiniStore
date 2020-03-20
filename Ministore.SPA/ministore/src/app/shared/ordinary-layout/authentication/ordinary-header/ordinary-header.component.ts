import { Component, OnInit } from "@angular/core";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { ContactModalComponent } from "../contact-modal/contact-modal.component";
import { AuthenticationService } from "src/app/_services/authentication.service";
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: "ordinary-header",
  templateUrl: "./ordinary-header.component.html",
  styleUrls: ["./ordinary-header.component.css"]
})
export class OrdinaryHeaderComponent implements OnInit {
  bsModalRef: BsModalRef;

  constructor(
    private modalService: BsModalService,
    private authService: AuthenticationService,
    private router: Router,
    private toastr: ToastrService,
  ) {}

  ngOnInit() {}

  //modal on the page
  contactModal() {
    this.bsModalRef = this.modalService.show(ContactModalComponent);
  }

  loggedIn() {
    return this.authService.loggedin();
  }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    this.authService.decodedToken = null;
    this.toastr.warning("Logged Out", "Warning");
    this.router.navigate(["/home"]);
  }
}
