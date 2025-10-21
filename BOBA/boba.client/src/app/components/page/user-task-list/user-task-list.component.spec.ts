import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { UserTaskListComponent } from './user-task-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HeaderComponent } from '../../frame/header/header.component';
import { ApiService } from '../../../services/api-service.service';
import { AuthService } from '../../../services/authentication/auth.service';

class MockApiService {
  task_GetClosedTasksByTeamId = jasmine.createSpy('task_GetClosedTasksByTeamId').and.returnValue(
    of([
      { id: 'task-1', name: 'Closed Task 1', status: 'closed' },
      { id: 'task-2', name: 'Closed Task 2', status: 'closed' }
    ])
  );

  task_GetAssignedTasksForUserByTeamId = jasmine.createSpy('task_GetAssignedTasksForUserByTeamId').and.returnValue(
    of([
      { id: 'task-3', name: 'My Task 1', assignedTo: 'user-1' },
      { id: 'task-4', name: 'My Task 2', assignedTo: 'user-1' }
    ])
  );

  task_GetUnassignedTasksByTeamId = jasmine.createSpy('task_GetUnassignedTasksByTeamId').and.returnValue(
    of([
      { id: 'task-5', name: 'Unassigned Task 1', assignedTo: null },
      { id: 'task-6', name: 'Unassigned Task 2', assignedTo: null }
    ])
  );

  task_GetExternalTasksByTeamId = jasmine.createSpy('task_GetExternalTasksByTeamId').and.returnValue(
    of([
      { id: 'task-7', name: 'External Task 1', isExternal: true },
      { id: 'task-8', name: 'External Task 2', isExternal: true }
    ])
  );
}

class MockAuthService {
  private currentTeam = { id: 'team-1', name: 'Team Alpha' };

  getTeam = jasmine.createSpy('getTeam').and.callFake(() => this.currentTeam);

  setCurrentTeam(team: any) {
    this.currentTeam = team;
  }
}

class MockRouter {
  navigate = jasmine.createSpy('navigate').and.returnValue(Promise.resolve(true));
}

class MockActivatedRoute {
  private currentParamValue: string | null = 'my-tasks';
  
  paramMap = of({
    get: (key: string) => key === 'type' ? this.currentParamValue : null
  });

  setParamValue(value: string | null) {
    this.currentParamValue = value;
    this.paramMap = of({
      get: (key: string) => key === 'type' ? this.currentParamValue : null
    });
  }
}

describe('UserTaskListComponent', () => {
  let component: UserTaskListComponent;
  let fixture: ComponentFixture<UserTaskListComponent>;
  let mockApiService: MockApiService;
  let mockAuthService: MockAuthService;
  let mockRouter: MockRouter;
  let mockActivatedRoute: MockActivatedRoute;

  beforeEach(async () => {
    mockApiService = new MockApiService();
    mockAuthService = new MockAuthService();
    mockRouter = new MockRouter();
    mockActivatedRoute = new MockActivatedRoute();

    await TestBed.configureTestingModule({
      declarations: [UserTaskListComponent, HeaderComponent],
      imports: [HttpClientTestingModule],
      providers: [
        { provide: ApiService, useValue: mockApiService },
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UserTaskListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should call loadData on initialization', () => {
      spyOn(component, 'loadData').and.returnValue(Promise.resolve());
      
      component.ngOnInit();
      
      expect(component.loadData).toHaveBeenCalled();
    });
  });

  describe('loadRoute', () => {
    it('should extract listType from route params', async () => {
      mockActivatedRoute.setParamValue('closed-tasks');
      
      await component.loadRoute();
      
      expect(component.listType).toBe('closed-tasks');
    });

    it('should handle different list types', async () => {
      const listTypes = ['my-tasks', 'closed-tasks', 'unassigned-tasks', 'external-tasks'];
      
      for (const type of listTypes) {
        mockActivatedRoute.setParamValue(type);
        await component.loadRoute();
        expect(component.listType).toBe(type);
      }
    });

    it('should not set listType when route param is null', async () => {
      mockActivatedRoute.setParamValue(null as any);
      component.listType = 'previous-value';
      
      await component.loadRoute();
      
      expect(component.listType).toBe('previous-value');
    });
  });

  describe('loadTasks - closed-tasks', () => {
    beforeEach(() => {
      component.listType = 'closed-tasks';
    });

    it('should load closed tasks successfully', async () => {
      await component.loadTasks();
      
      expect(mockAuthService.getTeam).toHaveBeenCalled();
      expect(mockApiService.task_GetClosedTasksByTeamId).toHaveBeenCalledWith('team-1');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].id).toBe('task-1');
    });

    it('should handle error when loading closed tasks fails', async () => {
      mockApiService.task_GetClosedTasksByTeamId.and.returnValue(
        throwError(() => new Error('API Error'))
      );
      const consoleSpy = spyOn(console, 'error');

      try {
        await component.loadTasks();
        fail('Expected promise to reject');
      } catch (err) {
        expect(consoleSpy).toHaveBeenCalledWith('Error loading tasks:', jasmine.any(Error));
        expect(err).toBeTruthy();
      }
    });
  });

  describe('loadTasks - my-tasks', () => {
    beforeEach(() => {
      component.listType = 'my-tasks';
    });

    it('should load assigned tasks successfully', async () => {
      await component.loadTasks();
      
      expect(mockAuthService.getTeam).toHaveBeenCalled();
      expect(mockApiService.task_GetAssignedTasksForUserByTeamId).toHaveBeenCalledWith('team-1');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].name).toBe('My Task 1');
    });

    it('should handle error when loading assigned tasks fails', async () => {
      mockApiService.task_GetAssignedTasksForUserByTeamId.and.returnValue(
        throwError(() => new Error('API Error'))
      );
      const consoleSpy = spyOn(console, 'error');

      try {
        await component.loadTasks();
        fail('Expected promise to reject');
      } catch (err) {
        expect(consoleSpy).toHaveBeenCalledWith('Error loading tasks:', jasmine.any(Error));
        expect(err).toBeTruthy();
      }
    });
  });

  describe('loadTasks - unassigned-tasks', () => {
    beforeEach(() => {
      component.listType = 'unassigned-tasks';
    });

    it('should load unassigned tasks successfully', async () => {
      await component.loadTasks();
      
      expect(mockAuthService.getTeam).toHaveBeenCalled();
      expect(mockApiService.task_GetUnassignedTasksByTeamId).toHaveBeenCalledWith('team-1');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].name).toBe('Unassigned Task 1');
    });

    it('should handle error when loading unassigned tasks fails', async () => {
      mockApiService.task_GetUnassignedTasksByTeamId.and.returnValue(
        throwError(() => new Error('API Error'))
      );
      const consoleSpy = spyOn(console, 'error');

      try {
        await component.loadTasks();
        fail('Expected promise to reject');
      } catch (err) {
        expect(consoleSpy).toHaveBeenCalledWith('Error loading tasks:', jasmine.any(Error));
        expect(err).toBeTruthy();
      }
    });
  });

  describe('loadTasks - external-tasks', () => {
    beforeEach(() => {
      component.listType = 'external-tasks';
    });

    it('should load external tasks successfully', async () => {
      await component.loadTasks();
      
      expect(mockAuthService.getTeam).toHaveBeenCalled();
      expect(mockApiService.task_GetExternalTasksByTeamId).toHaveBeenCalledWith('team-1');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].name).toBe('External Task 1');
    });

    it('should handle error when loading external tasks fails', async () => {
      mockApiService.task_GetExternalTasksByTeamId.and.returnValue(
        throwError(() => new Error('API Error'))
      );
      const consoleSpy = spyOn(console, 'error');

      try {
        await component.loadTasks();
        fail('Expected promise to reject');
      } catch (err) {
        expect(consoleSpy).toHaveBeenCalledWith('Error loading tasks:', jasmine.any(Error));
        expect(err).toBeTruthy();
      }
    });
  });

  describe('loadTasks - edge cases', () => {
    it('should handle empty task list', async () => {
      mockApiService.task_GetAssignedTasksForUserByTeamId.and.returnValue(of([]));
      component.listType = 'my-tasks';

      await component.loadTasks();

      expect(component.tasks).toEqual([]);
    });
  });

  describe('loadData', () => {
    it('should call loadRoute and loadTasks in sequence', async () => {
      spyOn(component, 'loadRoute').and.returnValue(Promise.resolve());
      spyOn(component, 'loadTasks').and.returnValue(Promise.resolve());

      await component.loadData();

      expect(component.loadRoute).toHaveBeenCalled();
      expect(component.loadTasks).toHaveBeenCalled();
    });

    it('should complete full data loading workflow', async () => {
      mockActivatedRoute.setParamValue('my-tasks');

      await component.loadData();

      expect(component.listType).toBe('my-tasks');
      expect(component.tasks.length).toBe(2);
      expect(mockApiService.task_GetAssignedTasksForUserByTeamId).toHaveBeenCalledWith('team-1');
    });
  });

  describe('integration tests', () => {
    it('should load closed tasks for closed-tasks list type', async () => {
      mockActivatedRoute.setParamValue('closed-tasks');

      await component.loadData();

      expect(component.listType).toBe('closed-tasks');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].status).toBe('closed');
    });

    it('should load my tasks for my-tasks list type', async () => {
      mockActivatedRoute.setParamValue('my-tasks');

      await component.loadData();

      expect(component.listType).toBe('my-tasks');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].assignedTo).toBe('user-1');
    });

    it('should load unassigned tasks for unassigned-tasks list type', async () => {
      mockActivatedRoute.setParamValue('unassigned-tasks');

      await component.loadData();

      expect(component.listType).toBe('unassigned-tasks');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].assignedTo).toBeNull();
    });

    it('should load external tasks for external-tasks list type', async () => {
      mockActivatedRoute.setParamValue('external-tasks');

      await component.loadData();

      expect(component.listType).toBe('external-tasks');
      expect(component.tasks.length).toBe(2);
      expect(component.tasks[0].isExternal).toBe(true);
    });

    it('should use correct team id from AuthService', async () => {
      mockAuthService.setCurrentTeam({ id: 'team-999', name: 'Special Team' });
      mockActivatedRoute.setParamValue('my-tasks');

      await component.loadData();

      expect(mockApiService.task_GetAssignedTasksForUserByTeamId).toHaveBeenCalledWith('team-999');
    });

    it('should switch between different task lists', async () => {
      mockActivatedRoute.setParamValue('my-tasks');
      await component.loadData();
      expect(component.listType).toBe('my-tasks');
      expect(component.tasks.length).toBe(2);

      mockActivatedRoute.setParamValue('closed-tasks');
      await component.loadData();
      expect(component.listType).toBe('closed-tasks');
      expect(component.tasks.length).toBe(2);
    });
  });

  describe('console logging', () => {
    it('should log listType when loading route', async () => {
      const consoleSpy = spyOn(console, 'log');
      mockActivatedRoute.setParamValue('my-tasks');

      await component.loadRoute();

      expect(consoleSpy).toHaveBeenCalledWith('my-tasks');
    });

    it('should log tasks when loading tasks successfully', async () => {
      const consoleSpy = spyOn(console, 'log');
      component.listType = 'my-tasks';

      await component.loadTasks();

      expect(consoleSpy).toHaveBeenCalledWith(component.tasks);
    });
  });
});