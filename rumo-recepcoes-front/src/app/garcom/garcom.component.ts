import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NavbarComponent } from '../Navbar/navbar.component';

@Component({
  selector: 'app-garcom',
  standalone: true,
  imports: [
    RouterModule,
    NavbarComponent
  ],
  templateUrl: './garcom.component.html',
  styleUrl: './garcom.component.scss'
})
export class GarcomComponent {

}
