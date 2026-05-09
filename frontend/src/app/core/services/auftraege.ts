import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuftraegeService {
  private apiUrl = 'http://localhost:5000/api/auftraege';

  constructor(private http: HttpClient) {}

  // CRUD Methoden
  getAuftraege(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getAuftrag(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  createAuftrag(auftrag: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, auftrag);
  }

  updateAuftrag(id: number, auftrag: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, auftrag);
  }

  deleteAuftrag(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
