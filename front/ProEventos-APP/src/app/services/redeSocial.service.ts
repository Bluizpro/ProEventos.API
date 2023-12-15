import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '@environments/environment';
import { RedesSocial } from '@pp/models/redes-social';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class RedeSocialService {
  baseURL = environment.apiURL + 'api/redesSociais';

  constructor(private http: HttpClient) {}

  /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrito em minúsculo.
   * @param id Precisa passar o PalestranteId ou o EventoId dependendo da sua Origem.
   * @returns Observable<RedeSocial[]>
   */
  public getRedesSociais(origem: string, id: number): Observable<RedesSocial[]> {
    let URL =
      id === 0
        ? `${this.baseURL}/${origem}`
        : `${this.baseURL}/${origem}/${id}`;

    return this.http.get<RedesSocial[]>(URL).pipe(take(1));
  }

  /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrito em minúsculo.
   * @param id Precisa passar o PalestranteId ou o EventoId dependendo da sua Origem.
   * @param redesSociais Precia adicionar Redes Sociais organizadas em RedeSocial[].
   * @returns Observable<RedeSocial[]>
   */
  public saveRedesSociais(
    origem: string,
    id: number,
    redesSociais: RedesSocial[]
  ): Observable<RedesSocial[]> {
    let URL =
      id === 0
        ? `${this.baseURL}/${origem}`
        : `${this.baseURL}/${origem}/${id}`;

    return this.http.put<RedesSocial[]>(URL, redesSociais).pipe(take(1));
  }

   /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrito em minúsculo.
   * @param id Precisa passar o PalestranteId ou o EventoId dependendo da sua Origem.
   * @param redeSocialId Precia usar o id da Rede Social
   * @returns Observable<any> - Pois é o retorno da Rota.
   */
    public deleteRedeSocial(
      origem: string,
      id: number,
      redeSocialId: number
    ): Observable<any> {
      let URL =
        id === 0
          ? `${this.baseURL}/${origem}/${redeSocialId}`
          : `${this.baseURL}/${origem}/${id}/${redeSocialId}`;

      return this.http.delete(URL).pipe(take(1));
    }
}
