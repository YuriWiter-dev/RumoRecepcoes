import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './Navbar/navbar.component';
import { AppRoutingModule } from './app.routes';
import { GarcomComponent } from "./garcom/garcom.component";
import { CozinhaComponent } from './cozinha/cozinha.component';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Rumo Recepções';
}
