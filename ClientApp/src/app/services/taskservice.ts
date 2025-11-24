import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { User } from '../models/user';
import { Task } from '../models/todoitem';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TaskService {

  baseUrl = environment.apiUrl;

  constructor(private http:HttpClient) { }

  private GetHeaders(){
    const userString = localStorage.getItem('user');
    if(!userString){
      //userString = "";
      throw Error("invalid user token");
    }
    const user: User = JSON.parse(userString);
    return {
        'Authorization': 'Bearer '+ user.token
      }
  }

  GetTasks():Observable<Task[]>{

    
     return this.http.get(this.baseUrl+'task',{
        headers: this.GetHeaders()
    }).pipe(
      map( (response:any) => response.map((t:any) => {
        const task = new Task(
          t.id, 
          t.title, 
          t.description, 
          t.status || 'Pending',
          t.dueDate ? new Date(t.dueDate) : new Date(),
          t.priority || 'Medium'
        );
        return task;
      })
    ));
  }
  CreateNew(task:Task){
     return this.http.post(this.baseUrl+'task',task,{
        headers: this.GetHeaders()
    }).pipe(
      map( (t:any) => {
        const newTask = new Task(
          t.id, 
          t.title, 
          t.description, 
          t.status || 'Pending',
          t.dueDate ? new Date(t.dueDate) : new Date(),
          t.priority || 'Medium'
        );
        return newTask;
      })
    );
  }
  Edit(task:Task):Observable<any>{
     return this.http.put(this.baseUrl+'task/'+task.Id,task,{
        headers: this.GetHeaders()
    });
  }
  Remove(task: Task):Observable<any>{
     return this.http.delete(this.baseUrl+'task/'+task.Id,{
        headers: this.GetHeaders()
    });
  }
}
