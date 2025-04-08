import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../../services/task/task.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tasklist',
  templateUrl: './tasklist.component.html',
  styleUrl: './tasklist.component.css'
})
export class TasklistComponent implements OnInit {
  taskTypes: any[] = [];

  constructor(private taskService: TaskService, private router: Router) {}

  ngOnInit(): void {
    this.loadTaskTypes();
  }

  loadTaskTypes(): void {
    this.taskService.getTaskTypes().subscribe(
      (data) => {
        this.taskTypes = data;
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
        this.router.navigate(['/task-details', response.id]); // Navigate to the task details page
      },
      (error) => {
        console.error('Error starting task', error);
      }
    );
  }
}