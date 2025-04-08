import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { TaskSummary } from '../../models/TaskSummary';
import { TaskFlowSummary } from '../../models/TaskFlowSummary';
import { ChoiceSummary } from '../../models/ChoiceSummary';
import { MoveTaskRequest } from '../../models/MoveTaskRequest';
@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: HttpClient) {}

  // -------------GET---------------

  getTaskTypes(): Observable<any[]> {
    return this.http.get<any[]>('/api/tasktypes');
  }

  getChoicesByIds(choiceIds: string[]): Observable<ChoiceSummary[]> {
    const params = choiceIds.map(id => `ids=${encodeURIComponent(id)}`).join('&');
    return this.http.get<any[]>(`/api/choices?${params}`).pipe(
      map((choices: any[]) => {
        return choices.map(choice => ({
          id: choice.id,
          name: choice.name
        }));
      })
    );
  }
  
  getTaskSummaryById(taskId: string): Observable<TaskSummary> {
    const query = { params: { taskId } };
    return this.http.get<any>('/api/task', query).pipe(
      map((task: any) => {
        return {
          id: task.id,
          taskTypeId: task.taskTypeId,
          taskTypeName: task.taskType?.name,
          creatorId: task.creatorId,
          currentStateId: task.currentStateId,
          currentStateName: task.currentState?.name || '',
          currentStateIsFinal: task.currentState?.isFinal,
          assigneeId: task.assigneeId,
          updatedAt: task.updatedAt,
          createdAt: task.createdAt
        };
      })
    );
  }

  getTaskFlowSummaryById(taskId: string): Observable<TaskFlowSummary> {
    const query = { params: { taskId } };
    return this.http.get<any>('api/taskflow', query).pipe(
      map((taskflow: any) => {
        return{
          id: taskflow.id,
          nextState: taskflow.nextState || [],
          editRoleId: taskflow.editRoleId || null,
          readOnlyRole: taskflow.readOnlyRole || []
        };
      })
    );
  }

  getStateNameById(stateId: string): Observable<any> {
    const query = { params: {stateId} };
    return this.http.get<any>('api/state-name', query).pipe(
      map((state: any) => { return state.stateName; })
    );
  }

   // -------------POST---------------

  startTask(taskTypeId: string): Observable<any> {
    console.log(taskTypeId);
    const body = { taskTypeId: taskTypeId }; 
    return this.http.post<any>('/api/create-task', body);
  }

  moveTask(request: MoveTaskRequest): Observable<any> {
    return this.http.post<any>('/api/move-task', request);
  }
  
}
