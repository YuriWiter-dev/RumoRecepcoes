import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../Navbar/navbar.component';
import { ToastrService } from 'ngx-toastr';
import { Cozinha } from '../Shared/Models/Cozinha';
import { CozinhaService } from '../Shared/Services/Cozinha.service';
@Component({
  selector: 'app-cozinha',
  templateUrl: './cozinha.component.html',
  styleUrl: './cozinha.component.scss'
})
export class CozinhaComponent implements OnInit {
  cozinhas: Cozinha[] = [];
  constructor(private toastr: ToastrService, 
  private _cozinhaService: CozinhaService) { }
  ngOnInit(): void {
    this.getCozinha();

    
  }
  nomeSolicitante: string = '';
  mesa: string = '';
  bebidaSelecionada: number = 0;
  quantidadeBebida: number = 0;
  pratoSelecionado: number = 0;
  quantidadePrato: number = 0;
  bebidasDisponiveis: any[] = [];
  pratosDisponiveis: any[] = [];

  getCozinha() {
    
    debugger;
    
    this._cozinhaService.getCozinha().subscribe(
      (data: Cozinha[]) => {
        this.cozinhas = data;
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
