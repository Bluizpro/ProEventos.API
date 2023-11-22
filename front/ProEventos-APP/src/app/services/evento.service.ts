import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Evento } from '../models/evento';
import { ReturnStatement } from '@angular/compiler';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})

export class EventoService {
  [key: string]: any
 baseURL = environment.apiURL + 'api/eventos';
 tokenHeader = new HttpHeaders({'Authorization': 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0IiwidW5pcXVlX25hbWUiOiJjYXJsb3MiLCJuYmYiOjE3MDA2MjAzOTcsImV4cCI6MTcwMDc5MzE5NywiaWF0IjoxNzAwNjIwMzk3fQ.Qxh4YROs6bMDm63sCQPJw1vQa8m6sinSoqkBlJT2JktD7o1SuRpAg9MuWFt4t_-ESUObJqoANkjHDOspgw7DgA'});
  constructor( private http: HttpClient) { }
 

  public getEventos(): Observable<Evento[]>{
    return this.http.get<Evento[]>(this.baseURL, {headers: this.tokenHeader}).pipe(take(1))
  }
  public getEventosByTema(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.baseURL}/${tema}/tema`).pipe(take(1))
  }
  public getEventoById( id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.baseURL}/${id}`).pipe(take(1))
  }
  public post(evento: Evento): Observable<Evento>{
   return this.http.post<Evento>(this.baseURL, evento).pipe(take(1))
  }

  public put(evento: Evento): Observable<Evento>{
return this.http.put<Evento>( `${this.baseURL}/${evento.id}`, evento).pipe(take(1))
  }
public deleteEvento(id: number): Observable<any>{
  return this.http.delete(`${this.baseURL}/${id}`).pipe(take(1))
}
postUpload(eventoId: number, file: File): Observable<Evento>{
  const fileToUpload = file[0] as File;
  const formData = new FormData();
  formData.append('file', fileToUpload);
  return this.http
  .post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData)
  .pipe(take(1))
}


}
