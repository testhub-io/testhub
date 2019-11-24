import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { retry, catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { pluck, share, shareReplay, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private apiURL = environment.apiURL;

  constructor(private http: HttpClient) { }

  public getData(uri): Observable<any>{
    return this.http.get<any>(this.apiURL + uri)
    .pipe(
      retry(1),
      tap(_ => console.log('executed')),
      pluck('url'),
      catchError(this.errorHandl),
      shareReplay(1),
    );
  }

  // Error handling
  errorHandl(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
