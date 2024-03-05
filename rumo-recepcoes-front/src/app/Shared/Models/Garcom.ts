import { Prato, Bebida } from "./Cozinha";

export class Garcom{
    nomeSolicitante: string = '';
    mesa: string = '';
    bebida: bebida = new bebida;
    prato: Prato  = new Prato;
}

export class bebida {
    nome: string = '';
    quantidade: number = 0;
}

export class Cozinha {
    nomeSolicitante: string = '';
    mesa: string = '';
    prato: Prato = new Prato;
}