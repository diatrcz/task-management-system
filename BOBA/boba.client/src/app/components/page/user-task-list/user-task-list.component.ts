import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../services/api-service.service';
import { AuthService } from '../../../services/authentication/auth.service';

@Component({
    selector: 'app-user-task-list',
    templateUrl: './user-task-list.component.html',
    styleUrl: './user-task-list.component.css',
    standalone: false
})
export class UserTaskListComponent implements OnInit{
  listType!: string;
  tasks!: any[];

  constructor(
      private router: Router,
      private route: ActivatedRoute,
      private apiService: ApiService,
      private authService: AuthService
    ) {}

  ngOnInit(): void {
    this.loadData()
  }

  async loadData(): Promise<void> {
    await this.loadRoute();
    await this.loadTasks();
  }

  loadRoute(): Promise<void> {
    return new Promise((resolve) => {
      this.route.paramMap.subscribe(params => {
        const type = params.get('type');
        if (type !== null) {
          this.listType = type;
        }
        console.log(this.listType);
        resolve();
      });
    });
  }

  loadTasks():Promise<void> {
    return new Promise((resolve, reject) => {
      const team_id = this.authService.getTeam()?.id;

      if(this.listType == 'closed-tasks' && team_id) {
        this.apiService.task_GetClosedTasksByTeamId(team_id).subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          }
        });
      }

      else if(this.listType == 'my-tasks' && team_id) {
        this.apiService.task_GetAssignedTasksForUserByTeamId(team_id).subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          }
        });
      }

      else if(this.listType == 'unassigned-tasks' && team_id) {
        this.apiService.task_GetUnassignedTasksByTeamId(team_id).subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          }
        });
      }

      else if(this.listType == 'external-tasks' && team_id) {
        this.apiService.task_GetExternalTasksByTeamId(team_id).subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          }
        });
      }
    });
  }

}
