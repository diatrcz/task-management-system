import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { TasklistComponent } from './tasklist.component';
import { HeaderComponent } from '../../frame/header/header.component';
import {
  ApiService,
  TaskTypeDto,
  CreateTaskRequest,
} from '../../../services/api-service.service';
import { AuthService } from '../../../services/authentication/auth.service';

class MockApiService {
  task_GetAllTaskTypes = jasmine
    .createSpy('task_GetAllTaskTypes')
    .and.returnValue(
      of([
        {
          id: 'type-1',
          name: 'Purchase Request',
          description: 'Request for purchasing',
        },
        {
          id: 'type-2',
          name: 'Leave Request',
          description: 'Request for leave',
        },
        {
          id: 'type-3',
          name: 'Expense Report',
          description: 'Report expenses',
        },
      ])
    );

  task_CreateTask = jasmine
    .createSpy('task_CreateTask')
    .and.returnValue(of('new-task-id-123'));
}

class MockAuthService {
  private currentTeam = { id: 'team-1', name: 'Team Alpha' };

  getTeam = jasmine.createSpy('getTeam').and.callFake(() => this.currentTeam);

  setCurrentTeam(team: any) {
    this.currentTeam = team;
  }
}

class MockRouter {
  navigate = jasmine
    .createSpy('navigate')
    .and.returnValue(Promise.resolve(true));
}

describe('TasklistComponent', () => {
  let component: TasklistComponent;
  let fixture: ComponentFixture<TasklistComponent>;
  let mockApiService: MockApiService;
  let mockAuthService: MockAuthService;
  let mockRouter: MockRouter;

  beforeEach(async () => {
    mockApiService = new MockApiService();
    mockAuthService = new MockAuthService();
    mockRouter = new MockRouter();

    await TestBed.configureTestingModule({
      declarations: [TasklistComponent, HeaderComponent],
      imports: [HttpClientTestingModule],
      providers: [
        { provide: ApiService, useValue: mockApiService },
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TasklistComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should call loadTaskTypes on initialization', () => {
      spyOn(component, 'loadTaskTypes');

      component.ngOnInit();

      expect(component.loadTaskTypes).toHaveBeenCalled();
    });
  });

  describe('loadTaskTypes', () => {
    it('should load task types successfully', () => {
      component.loadTaskTypes();

      expect(mockApiService.task_GetAllTaskTypes).toHaveBeenCalled();
      expect(component.taskTypes.length).toBe(3);
    });

    it('should convert response data using TaskTypeDto.fromJS', () => {
      component.loadTaskTypes();

      expect(component.taskTypes.length).toBe(3);
      expect(component.taskTypes[0].id).toBe('type-1');
      expect(component.taskTypes[0].name).toBe('Purchase Request');
      expect(component.taskTypes[1].id).toBe('type-2');
      expect(component.taskTypes[1].name).toBe('Leave Request');
    });

    it('should handle error when loading task types fails', () => {
      const consoleSpy = spyOn(console, 'error');
      mockApiService.task_GetAllTaskTypes.and.returnValue(
        throwError(() => new Error('API Error'))
      );

      component.loadTaskTypes();

      expect(consoleSpy).toHaveBeenCalledWith(
        'Error loading task types',
        jasmine.any(Error)
      );
    });

    it('should handle empty task types response', () => {
      mockApiService.task_GetAllTaskTypes.and.returnValue(of([]));

      component.loadTaskTypes();

      expect(component.taskTypes.length).toBe(0);
    });
  });

  describe('startTask', () => {
    it('should create task and navigate to task details', () => {
      const taskTypeId = 'type-1';

      component.startTask(taskTypeId);

      expect(mockAuthService.getTeam).toHaveBeenCalled();
      expect(mockApiService.task_CreateTask).toHaveBeenCalledWith(
        jasmine.objectContaining({
          taskTypeId: 'type-1',
          teamId: 'team-1',
        })
      );
      expect(mockRouter.navigate).toHaveBeenCalledWith(
        ['/task-details', 'new-task-id-123'],
        { queryParams: { mode: 'edit' } }
      );
    });

    it('should use correct team ID from AuthService', () => {
      mockAuthService.setCurrentTeam({ id: 'team-999', name: 'Special Team' });

      component.startTask('type-2');

      const callArgs =
        mockApiService.task_CreateTask.calls.mostRecent().args[0];
      expect(callArgs.teamId).toBe('team-999');
      expect(callArgs.taskTypeId).toBe('type-2');
    });

    it('should handle error when task creation fails', () => {
      const consoleSpy = spyOn(console, 'error');
      mockApiService.task_CreateTask.and.returnValue(
        throwError(() => new Error('Creation failed'))
      );

      component.startTask('type-1');

      expect(consoleSpy).toHaveBeenCalledWith(
        'Error starting task',
        jasmine.any(Error)
      );
      expect(mockRouter.navigate).not.toHaveBeenCalled();
    });

    it('should log taskTypeId to console', () => {
      const consoleSpy = spyOn(console, 'log');

      component.startTask('type-1');

      expect(consoleSpy).toHaveBeenCalledWith('type-1');
    });

    it('should log success message after task creation', () => {
      const consoleSpy = spyOn(console, 'log');

      component.startTask('type-1');

      expect(consoleSpy).toHaveBeenCalledWith(
        'Task started successfully',
        'new-task-id-123'
      );
    });

    it('should create proper CreateTaskRequest instance', () => {
      component.startTask('type-3');

      expect(mockApiService.task_CreateTask).toHaveBeenCalled();
      const request = mockApiService.task_CreateTask.calls.mostRecent().args[0];
      expect(request).toBeInstanceOf(CreateTaskRequest);
      expect(request.taskTypeId).toBe('type-3');
      expect(request.teamId).toBe('team-1');
    });

    it('should handle multiple task creation requests', () => {
      component.startTask('type-1');
      component.startTask('type-2');
      component.startTask('type-3');

      expect(mockApiService.task_CreateTask).toHaveBeenCalledTimes(3);
      expect(mockRouter.navigate).toHaveBeenCalledTimes(3);
    });
  });

  describe('integration', () => {
    it('should load task types on component initialization', () => {
      fixture.detectChanges();

      expect(mockApiService.task_GetAllTaskTypes).toHaveBeenCalled();
      expect(component.taskTypes.length).toBe(3);
    });

    it('should handle full workflow: load types then start task', () => {
      component.loadTaskTypes();
      expect(component.taskTypes.length).toBe(3);

      const selectedTaskTypeId = component.taskTypes[0].id;
      component.startTask(selectedTaskTypeId);

      expect(mockApiService.task_CreateTask).toHaveBeenCalled();
      expect(mockRouter.navigate).toHaveBeenCalledWith(
        ['/task-details', 'new-task-id-123'],
        { queryParams: { mode: 'edit' } }
      );
    });
  });

  describe('edge cases', () => {
    it('should handle team with undefined id', () => {
      mockAuthService.getTeam.and.returnValue({ id: undefined, name: 'Test' });

      component.startTask('type-1');

      const request = mockApiService.task_CreateTask.calls.mostRecent().args[0];
      expect(request.teamId).toBeUndefined();
    });

    it('should handle empty taskTypeId', () => {
      component.startTask('');

      expect(mockApiService.task_CreateTask).toHaveBeenCalledWith(
        jasmine.objectContaining({
          taskTypeId: '',
          teamId: 'team-1',
        })
      );
    });
  });

  describe('task type data structure', () => {
    it('should handle task types without descriptions', () => {
      mockApiService.task_GetAllTaskTypes.and.returnValue(
        of([{ id: 'type-1', name: 'Simple Task' }])
      );

      component.loadTaskTypes();

      expect(component.taskTypes.length).toBe(1);
      expect(component.taskTypes[0].name).toBe('Simple Task');
    });
  });
});
