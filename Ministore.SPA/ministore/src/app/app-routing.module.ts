import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  { path: '', redirectTo: 'register', pathMatch: 'full' },
  { path: '', loadChildren: () => import('./shared/ordinary-layout/ordinary-layout.module').then(x => x.OrdinaryLayoutModel) },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
