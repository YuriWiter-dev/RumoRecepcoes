import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../Navbar/navbar.component';
@Component({
  selector: 'app-cozinha',
  standalone: true,
  imports: [RouterModule,NavbarComponent],
  templateUrl: './cozinha.component.html',
  styleUrl: './cozinha.component.scss'
})
export class CozinhaComponent {

}
