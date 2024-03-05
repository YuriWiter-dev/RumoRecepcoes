export class Cozinha {
    nomeSolicitante: string = '';
    mesa: string = '';
    bebida: Bebida = new Bebida;
    prato: Prato = new Prato;
}

export class Prato {
    nome: string = '';
    quantidade: number = 0;
}

export class Bebida {
    nome: string = '';
    quantidade: number = 0;
}