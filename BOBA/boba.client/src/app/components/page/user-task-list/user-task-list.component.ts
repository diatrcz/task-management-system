import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskSummary } from '../../../models/TaskSummary';
import { ApiService } from '../../../services/api-service.service';

@Component({
    selector: 'app-user-task-list',
    templateUrl: './user-task-list.component.html',
    styleUrl: './user-task-list.component.css',
    standalone: false
})
export class UserTaskListComponent implements OnInit{
  listType!: string;
  tasks!: TaskSummary[];

  constructor(
      private router: Router,
      private route: ActivatedRoute,
      private apiService: ApiService
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
      if(this.listType == 'closed-tasks') {
        /*this.apiService.task_GetClosedTasksByTeamId().subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          }
        });*/
      }
      else if(this.listType == 'my-tasks') {
        /*this.apiService.getOwnTasks().subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          }
        });*/

      }
    });
  }

}
