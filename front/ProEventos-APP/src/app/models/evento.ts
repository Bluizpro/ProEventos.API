import { DateTimeFormatPipe } from "@pp/helpers/date-time-format.pipe";
import { Lote } from "./lote";
import { Palestrante } from "./palestrante";
import { RedesSocial } from "./redes-social";

export interface Evento {
    id: number;
    local: string;
    dataEvento?: Date;
    tema: string
    qtdPessoas: number;
    imagemURL: string;
    telefone: string;
    email: string;
    lotes: Lote[];
    redesociais: RedesSocial[];
    palestranteseventos: Palestrante[];
}
