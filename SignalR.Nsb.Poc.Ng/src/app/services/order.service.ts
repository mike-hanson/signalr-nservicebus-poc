import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class OrderService {
  public readonly url: string = '/api/order';

  constructor(private httpClient: HttpClient) { }

  public create(): void {
     this.httpClient.post(this.url, null).subscribe(() => {}, err => console.log(err)); 
  }
}
