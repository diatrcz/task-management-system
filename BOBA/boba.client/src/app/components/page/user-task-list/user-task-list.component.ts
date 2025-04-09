import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../../services/task/task.service';
import { TaskSummary } from '../../../models/TaskSummary';

@Component({
  selector: 'app-user-task-list',
  templateUrl: './user-task-list.component.html',
  styleUrl: './user-task-list.component.css'
})
export class UserTaskListComponent implements OnInit{
  listType!: string;
  tasks!: TaskSummary[];

  constructor(
      private router: Router,
      private route: ActivatedRoute,
      private taskService: TaskService
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
        this.taskService.getClosedTasks().subscribe({
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
      else if(this.listType == 'my-tasks') {

      }
    });
  }

}
