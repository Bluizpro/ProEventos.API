import { Evento } from "./evento";
import { UserUpdate } from "./identity/UserUpdate";
import { RedesSocial } from "./redes-social";

export interface Palestrante {
    id: number;    
    miniCurriculo: string;
   user: UserUpdate;
    redesociais: RedesSocial[];
    palestranteseventos: Evento[];

}
