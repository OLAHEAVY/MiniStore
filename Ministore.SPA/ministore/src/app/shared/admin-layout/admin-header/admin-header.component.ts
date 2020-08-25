import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class AdminHeaderComponent implements OnInit {
  public sidebarMinimized = false;


  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  constructor() { }

  ngOnInit() {
  }

}
