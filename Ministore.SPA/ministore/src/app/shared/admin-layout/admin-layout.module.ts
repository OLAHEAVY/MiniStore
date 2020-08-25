import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AdminLayoutComponent } from "./admin-layout.component";
import { CommonModule } from "@angular/common";
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { AdminHeaderComponent } from './admin-header/admin-header.component';
import { DashboardComponent } from 'src/app/dashboard/dashboard.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: "",
        component: AdminLayoutComponent,
        canActivate: [],
        children: [
          { path: 'dashboard', loadChildren: () => import('../../dashboard/dashboard.module').then(y => y.DashboardModule) },
        ]
      }
    ])
  ],
  declarations: [AdminLayoutComponent, AdminHeaderComponent],
  exports: [],
  providers: [AuthenticationService]
})
export class AdminLayoutModule {}
