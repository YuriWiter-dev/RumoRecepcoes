import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { GarcomService } from '../Shared/Services/Garcom.service';
import { Garcom } from '../Shared/Models/Garcom';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { PageParams } from '../Shared/Models/page-params';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-garcom',
  templateUrl: './garcom.component.html',
  styleUrls: ['./garcom.component.scss']
})
export class GarcomComponent implements OnInit {
  columnsToDisplay: string[] = ['posicao', 'nomeSolicitante', 'mesa', 'bebidas', 'pratos'];
  pedidos: Garcom[] = [];
  dataSource = new MatTableDataSource<any>();
  expandedElement: Garcom | null;

  pageValue: number = 0;
  sizeValue: number = 10;
  totalElements: number = 10;
  pageSizeOptions = [5, 10, 20];
  pageParams: PageParams = {
    page: this.pageValue,
    linesPerPage: this.sizeValue,
    orderBy: 'posicao',
    direction: 'ASC',
  };

  nomeSolicitante: string = '';
  mesa: string = '';
  bebidaSelecionada: number = 0;
  quantidadeBebida: number = 0;
  pratoSelecionado: number = 0;
  quantidadePrato: number = 0;
  bebidasDisponiveis: any[] = [];
  pratosDisponiveis: any[] = [];


  convertNameLabel(nomeColuna: string) {
    switch (nomeColuna) {
      case 'nomeSolicitante':
        return 'Nome do Solicitante';
      case 'mesa':
        return 'mesa';
      case 'bebidas':
        return 'nome';
      case 'pratos':
        return 'nome';
      default:
        return '';
    }
  }

  @ViewChild('pedidoForm') pedidoForm!: NgForm;
  constructor(
    private toastr: ToastrService,
    private _garcomService: GarcomService
  ) {
    this.expandedElement = null;

    this.bebidasDisponiveis = [
      { id: 1, nome: 'Suco' },
      { id: 2, nome: 'Água' },
      { id: 3, nome: 'Refrigerante' }
    ];

    this.pratosDisponiveis = [
      { id: 1, nome: 'Feijoada' },
      { id: 2, nome: 'Macarrão' },
      { id: 3, nome: 'Tropeiro' }
    ];



  }

  ngOnInit(): void {
    this.getPedidos();


  }

  getPedidos() {
    this._garcomService.getPedidos().subscribe(
      (data: Garcom[]) => {
        this.pedidos = data;
      },
      error => {
        this.openSnackBar('Erro ao carregar pedidos', true);
      }
    );
  }

  submitForm(form: NgForm): void {

    if (form.valid) {


      const pedido = {
        nomeSolicitante: this.nomeSolicitante,
        mesa: this.mesa,
        bebida: { nome: form.value.bebidaSelecionada, quantidade: form.value.quantidadeBebida },
        prato: { nome: form.value.pratoSelecionado, quantidade: form.value.quantidadePrato }
      };




      this._garcomService.salvarPedido(pedido)
        .subscribe(
          response => {
            this.openSnackBar('Pedido efetuado com sucesso.',true);
            window.location.reload();
            this.getPedidos;
          },
          error => {
            this.openSnackBar('Erro ao efetuar o pedido.',true);
            // Trate o erro, exiba uma mensagem de erro, etc.
          }
        );
    }
  }
  openSnackBar(msg: string, eUmErro: boolean) {
    if (eUmErro) {
      this.toastr.error(msg);
    } else {
      this.toastr.success(msg);
    }
  }
}
