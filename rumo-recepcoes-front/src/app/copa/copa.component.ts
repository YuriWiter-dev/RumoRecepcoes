import { Component } from '@angular/core';
import { NavbarComponent } from '../Navbar/navbar.component';
import { ToastrService } from 'ngx-toastr';
import { CopaService } from '../Shared/Services/Copa.service';
import { Copa } from '../Shared/Models/Copa';

@Component({
  selector: 'app-copa',
  templateUrl: './copa.component.html',
  styleUrl: './copa.component.scss'
})
export class CopaComponent {
  copas: Copa[] = [];
  constructor(private toastr: ToastrService, 
    private _copaService: CopaService) { }
    ngOnInit(): void {
      this.getCopa();
    }
    getCopa() {
    
      
      this._copaService.getCopa().subscribe(
        (data: Copa[]) => {
          this.copas = data;
        },  
        error => {
          this.openSnackBar('Erro ao carregar a cozinha', true);
        }
      );
    }

    openSnackBar(msg: string, eUmErro: boolean) {
      if (eUmErro) {
        this.toastr.error(msg);
      } else {
        this.toastr.success(msg);
      }
    }
}
