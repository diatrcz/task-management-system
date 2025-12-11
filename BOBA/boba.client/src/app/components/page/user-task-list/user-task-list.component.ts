import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../services/api-service.service';
import { AuthService } from '../../../services/authentication/auth.service';

type SortColumn =
  | 'taskTypeName'
  | 'id'
  | 'currentStateName'
  | 'createdAt'
  | 'creator';
type SortDirection = 'asc' | 'desc' | null;

@Component({
  selector: 'app-user-task-list',
  templateUrl: './user-task-list.component.html',
  styleUrl: './user-task-list.component.css',
  standalone: false,
})
export class UserTaskListComponent implements OnInit {
  listType!: string;
  tasks!: any[];
  mode: string = 'view';
  isAssigning: boolean = false;

  sortColumn: SortColumn | null = null;
  sortDirection: SortDirection = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private apiService: ApiService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    await this.loadRoute();
    await this.loadTasks();
  }

  loadRoute(): Promise<void> {
    return new Promise((resolve) => {
      this.route.paramMap.subscribe((params) => {
        const type = params.get('type');
        if (type !== null) {
          this.listType = type;
        }
        console.log(this.listType);
        resolve();
      });
    });
  }

  loadTasks(): Promise<void> {
    return new Promise((resolve, reject) => {
      const team_id = this.authService.getTeam()?.id;

      if (this.listType == 'closed-tasks' && team_id) {
        this.mode = 'view';
        this.apiService.task_GetClosedTasksByTeamId(team_id).subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          },
        });
      } else if (this.listType == 'my-tasks' && team_id) {
        this.mode = 'edit';
        this.apiService
          .task_GetAssignedTasksForUserByTeamId(team_id)
          .subscribe({
            next: (tasks) => {
              this.tasks = tasks;
              console.log(this.tasks);
              resolve();
            },
            error: (err) => {
              console.error('Error loading tasks:', err);
              reject(err);
            },
          });
      } else if (this.listType == 'unassigned-tasks' && team_id) {
        this.mode = 'edit';
        this.apiService.task_GetUnassignedTasksByTeamId(team_id).subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          },
        });
      } else if (this.listType == 'external-tasks' && team_id) {
        this.mode = 'view';
        this.apiService.task_GetExternalTasksByTeamId(team_id).subscribe({
          next: (tasks) => {
            this.tasks = tasks;
            console.log(this.tasks);
            resolve();
          },
          error: (err) => {
            console.error('Error loading tasks:', err);
            reject(err);
          },
        });
      }
    });
  }

  onTaskClick(event: Event, task: any): void {
    if (this.isAssigning) {
      return;
    }

    if (this.mode === 'edit' && !task.assigneeName) {
      this.isAssigning = true;

      this.apiService.task_AssignTask(task.id).subscribe({
        next: (taskId) => {
          console.log('Task assigned successfully:', taskId);
          this.isAssigning = false;

          this.router.navigate(['/task-details', task.id], {
            queryParams: { mode: this.mode },
          });
        },
        error: (err) => {
          console.error('Error assigning task:', err);
          this.isAssigning = false;
          alert('Failed to assign task. Please try again.');
        },
      });
    } else {
      this.router.navigate(['/task-details', task.id], {
        queryParams: { mode: this.mode },
      });
    }
  }

  sortBy(column: SortColumn): void {
    if (this.sortColumn === column) {
      if (this.sortDirection === 'asc') {
        this.sortDirection = 'desc';
      } else if (this.sortDirection === 'desc') {
        this.sortDirection = null;
        this.sortColumn = null;
      } else {
        this.sortDirection = 'asc';
      }
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    if (this.sortDirection) {
      this.applySorting();
    } else {
      this.loadTasks();
    }
  }

  private applySorting(): void {
    if (!this.sortColumn || !this.sortDirection) return;

    this.tasks.sort((a, b) => {
      let valueA: any;
      let valueB: any;

      switch (this.sortColumn) {
        case 'taskTypeName':
          valueA = a.taskTypeName?.toLowerCase() || '';
          valueB = b.taskTypeName?.toLowerCase() || '';
          break;
        case 'id':
          valueA = a.id?.toLowerCase() || '';
          valueB = b.id?.toLowerCase() || '';
          break;
        case 'currentStateName':
          valueA = a.currentStateName?.toLowerCase() || '';
          valueB = b.currentStateName?.toLowerCase() || '';
          break;
        case 'createdAt':
          valueA = new Date(a.createdAt).getTime();
          valueB = new Date(b.createdAt).getTime();
          break;
        case 'creator':
          valueA = a.assigneeName?.toLowerCase() || '';
          valueB = b.assigneeName?.toLowerCase() || '';
          break;
        default:
          return 0;
      }

      if (valueA < valueB) {
        return this.sortDirection === 'asc' ? -1 : 1;
      }
      if (valueA > valueB) {
        return this.sortDirection === 'asc' ? 1 : -1;
      }
      return 0;
    });
  }

  getSortIcon(column: SortColumn): string {
    if (this.sortColumn !== column) {
      return 'sort-default';
    }
    return this.sortDirection === 'asc' ? 'sort-asc' : 'sort-desc';
  }
}
