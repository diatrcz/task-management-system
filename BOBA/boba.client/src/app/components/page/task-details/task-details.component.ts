import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../../services/task/task.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskSummary } from '../../../models/TaskSummary';
import { TaskFlowSummary } from '../../../models/TaskFlowSummary';
import { ChoiceSummary } from '../../../models/ChoiceSummary';
import { response } from 'express';
import { MoveTaskRequest } from '../../../models/MoveTaskRequest';

@Component({
  selector: 'app-task-details',
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.css'
})
export class TaskDetailsComponent implements OnInit{
  taskId!: string;
  task!: TaskSummary;
  taskflow!: TaskFlowSummary;
  choiceIds!: string[];
  choices!: ChoiceSummary[];
  selectedChoiceId: string | null = null;
  selectedChoice: ChoiceSummary | null = null;
  nextStateName: string | null = null;
  showConfirmDialog: boolean = false;
  

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private taskService: TaskService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    await this.loadRoute();
    await this.loadTask();
    await this.loadTaskFlow();
    await this.loadChoices();
  }

  loadRoute(): Promise<void> {
    return new Promise((resolve) => {
      this.route.paramMap.subscribe(params => {
        const id = params.get('id');
        if (id !== null) {
          this.taskId = id; 
        }
        resolve(); 
      });
    });
  }

  loadTask(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.taskService.getTaskSummaryById(this.taskId).subscribe({
        next: (task) => {
          this.task = task;
          console.log(this.task);
          resolve();
        },
        error: (err) => {
          console.error('Error loading task:', err);
          reject(err);
        }
      });
    });
  }

  loadTaskFlow(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.taskService.getTaskFlowSummaryById(this.taskId).subscribe({
        next: (taskflow) => {
          this.taskflow = taskflow;
          console.log(this.taskflow);
          resolve();
        },
        error: (err) => {
          console.error('Error loading taskflow:', err);
          reject(err);
        }
      });
    });
  }

  loadChoices(): Promise<void> {
    return new Promise((resolve, reject) => {
      try{
        this.choiceIds = [];
        if (this.taskflow?.nextState) {
          for (const item of this.taskflow.nextState) {
            if (item.choiceId) {
              this.choiceIds.push(item.choiceId);
            }
          }
        }
        this.taskService.getChoicesByIds(this.choiceIds).subscribe({
          next: (choices) => {
            this.choices = choices;
            console.log(this.choices);
            resolve();
          },
          error: (err) => {
            console.log('Error loading choices:', err)
            reject(err);
          }
        });
      } catch(error) {
        reject(error);
      }
    });
  }

  onChoiceSelected(choice: ChoiceSummary): void {
    this.selectedChoice = choice;
    if (this.taskflow?.nextState) {
      const nextState = this.taskflow.nextState.find(state => state.choiceId === choice.id);
      if (nextState && this.selectedChoiceId) {
        this.taskService.getStateNameById(this.selectedChoiceId).subscribe({
          next: (stateName) => {
            this.nextStateName = stateName;
            console.log('Next state: ' + stateName);
          },
          error: (err) => {
            console.log('Error loading statename:', err)
          }
        });
      }
    }
  }

  openConfirmationDialog(): void {
    this.showConfirmDialog = true;
  }

  cancelConfirmation(): void {
    this.showConfirmDialog = false;
  }

  submitTask(): void {
    console.log('Submitting task with choice:', this.selectedChoiceId);
    var request: MoveTaskRequest = {
      choiceId: this.selectedChoiceId!,
      taskId: this.taskId!
     }
    this.taskService.moveTask(request).subscribe(
      (response) => {
          console.log('Task moved to next state successfully', response);
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate([`/task-details/${this.taskId}`]);
          });
      },
      (error) => {
        console.error('Error moving task', error);
      }
    );
    this.showConfirmDialog = false;
  }
}
