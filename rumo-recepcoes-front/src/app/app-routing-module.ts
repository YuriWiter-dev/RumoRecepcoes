import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CozinhaComponent } from './cozinha/cozinha.component';
import { GarcomComponent } from './garcom/garcom.component';
import { NavbarComponent } from './Navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { CopaComponent } from './copa/copa.component';
// Declare e exporte a variável routes
const routes: Routes= [
  { path: 'garcom',  component: NavbarComponent }, 
  { path: 'cozinha', component: CozinhaComponent },
  { path: 'garcom', component: GarcomComponent },
  { path: 'copa', component: CopaComponent },
];

@NgModule({
  imports: [ CommonModule,
    RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule  { }
