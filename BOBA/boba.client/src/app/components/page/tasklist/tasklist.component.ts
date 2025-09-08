import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../../services/task/task.service';
import { Router } from '@angular/router';
import { ApiService, TaskTypeDto } from '../../../services/api-service.service';

@Component({
    selector: 'app-tasklist',
    templateUrl: './tasklist.component.html',
    styleUrl: './tasklist.component.css',
    standalone: false
})
export class TasklistComponent implements OnInit {
  taskTypes: any[] = [];

  constructor(private taskService: TaskService, private apiService: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.loadTaskTypes();
  }

  loadTaskTypes(): void {
  this.apiService.task_GetAllTaskTypes().subscribe(
    (data) => {
      this.taskTypes = data.map(item => TaskTypeDto.fromJS(item));
    },
    (error) => {
      console.error('Error loading task types', error);
    }
  );
}


  startTask(taskTypeId: string): void {
    console.log(taskTypeId);
    this.taskService.startTask(taskTypeId).subscribe(
      (response) => {
        console.log('Task started successfully', response);
        this.router.navigate(['/task-details', response]);
      },
      (error) => {
        console.error('Error starting task', error);
      }
    );
  }
}