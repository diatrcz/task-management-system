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
    return this.http.get<ChoiceSummary[]>(`/api/choices?${params}`).pipe(
      map((choices: ChoiceSummary[]) => {
        return choices.map(choice => ({
          id: choice.id,
          name: choice.name
        }));
      })
    );
  }
  
  getTaskSummaryById(taskId: string): Observable<TaskSummary> {
    const query = { params: { taskId } };
    return this.http.get<TaskSummary>('/api/task', query).pipe(
      map((task: TaskSummary) => {
        return {
          id: task.id,
          taskTypeId: task.taskTypeId,
          taskTypeName: task.taskTypeName,
          creatorId: task.creatorId,
          currentStateId: task.currentStateId,
          currentStateName: task.currentStateName || '',
          currentStateIsFinal: task.currentStateIsFinal,
          assigneeId: task.assigneeId,
          updatedAt: task.updatedAt,
          createdAt: task.createdAt
        };
      })
    );
  }

  getTaskFlowSummaryById(taskId: string): Observable<TaskFlowSummary> {
    const query = { params: { taskId } };
    return this.http.get<TaskFlowSummary>('api/taskflow', query).pipe(
      map((taskflow: TaskFlowSummary) => {
        return{
          id: taskflow.id,
          nextState: taskflow.nextState || [],
          editRoleId: taskflow.editRoleId || null,
          readOnlyRole: taskflow.readOnlyRole || []
        };
      })
    );
  }

  getStateNameById(stateId: string): Observable<string> {
    const query = { params: { stateId } };
    return this.http.get<{ stateName: string }>('api/state-name', query).pipe(
      map(response => response.stateName)
    );
}

  getClosedTasks(): Observable<TaskSummary[]> {
    return this.http.get<TaskSummary[]>(`/api/closed-tasks`).pipe(
      map((tasks: TaskSummary[]) => {
        return tasks.map(task => ({
          id: task.id,
          taskTypeId: task.taskTypeId,
          taskTypeName: task.taskTypeName,
          creatorId: task.creatorId,
          currentStateId: task.currentStateId,
          currentStateName: task.currentStateName || '',
          currentStateIsFinal: task.currentStateIsFinal,
          assigneeId: task.assigneeId,
          updatedAt: task.updatedAt,
          createdAt: task.createdAt
        }));
      })
    );
  }

  getOwnTasks(): Observable<TaskSummary[]> {
    return this.http.get<TaskSummary[]>('api/own-tasks').pipe(
      map((tasks: TaskSummary[]) => {
        return tasks.map(task => ({
          id: task.id,
          taskTypeId: task.taskTypeId,
          taskTypeName: task.taskTypeName,
          creatorId: task.creatorId,
          currentStateId: task.currentStateId,
          currentStateName: task.currentStateName || '',
          currentStateIsFinal: task.currentStateIsFinal,
          assigneeId: task.assigneeId,
          updatedAt: task.updatedAt,
          createdAt: task.createdAt
        }));
      })
    );
  }

   // -------------POST---------------

   startTask(taskTypeId: string): Observable<string> {
    const body = { taskTypeId };
    return this.http.post<{ taskId: string }>('/api/create-task', body).pipe(
      map(response => response.taskId)
    );
  }
  
  moveTask(request: MoveTaskRequest): Observable<string> {
    return this.http.post<{ taskId: string }>('/api/move-task', request).pipe(
      map(response => response.taskId)
    );
  } 
  
}
