import { Evento } from "./evento";
import { RedesSocial } from "./redes-social";

export interface Palestrante {
    id: number;
    nome: string;
    minicurriculo: string;
    imagemURL: string;
    telefone: number;
    email: string;
    redesociais: RedesSocial[];
    palestranteseventos: Evento[];


}
