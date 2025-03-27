import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: HttpClient) {}

  getTaskTypes(): Observable<any[]> {
    return this.http.get<any[]>('/api/tasktypes');
  }

  startTask(taskTypeId: string): Observable<any> {
    console.log(taskTypeId);
    const body = { taskTypeId: taskTypeId };  // Ensure this matches your backend's expected format
  return this.http.post<any>('/api/create-task', body);
  }

  getChoicesByIds(choiceIds: string[]): Observable<any[]> {
    const params = choiceIds.map(id => `ids=${encodeURIComponent(id)}`).join('&');
    return this.http.get<any[]>(`/api/choices?${params}`);
  }
  
}
