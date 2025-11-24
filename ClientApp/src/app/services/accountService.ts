import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { User } from '../models/user';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http:HttpClient) { }

  login(model: any){
    return this.http.post<User>(this.baseUrl+'auth/login', model).pipe(
      map((response: User) =>{
        const user = response
        if(user){
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUserSource.next(user);
        }
      } )
    );
  }

 register(model: any){
  return this.http.post<User>(this.baseUrl+'auth/register', model).pipe(
    map(user => {
      if(user){
        localStorage.setItem('user', JSON.stringify(user))
        this.currentUserSource.next(user);
      }
    })
  )
 }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout(){
    console.log('AccountService.logout() called');
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    console.log('User set to null, currentUser$ should emit null');
  }
}
