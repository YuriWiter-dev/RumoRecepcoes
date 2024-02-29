import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CozinhaComponent } from './cozinha/cozinha.component';
import { GarcomComponent } from './garcom/garcom.component';
import { NavbarComponent } from './Navbar/navbar.component';
import { CommonModule } from '@angular/common';
// Declare e exporte a variável routes
export const routes: Routes = [
  { path: '',  component: NavbarComponent }, 
  { path: 'cozinha', component: CozinhaComponent },
  { path: '/garcom', component: GarcomComponent },
];

@NgModule({
  imports: [ CommonModule,
    RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule  { }
