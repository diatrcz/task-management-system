import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { DashboardComponent } from './dashboard.component';
import { HeaderComponent } from '../../frame/header/header.component';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../../services/api-service.service';
import { AuthService } from '../../../services/authentication/auth.service';
import { TeamSummaryDto } from '../../../services/api-service.service';
import { of, throwError } from 'rxjs';

class MockApiService {
  user_GetUserTeamsById = jasmine.createSpy().and.returnValue(
    of([
      { id: '1', name: 'Team A' },
      { id: '2', name: 'Team B' }
    ])
  );

  task_GetTasksCount = jasmine.createSpy().and.returnValue(
    of({ open: 3, closed: 7 })
  );
}
class MockAuthService {
  private currentTeam: any = null;

  getTeam = jasmine.createSpy().and.callFake(() => this.currentTeam);
  setTeam = jasmine.createSpy().and.callFake((team: any) => (this.currentTeam = team));
}

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let mockApiService: MockApiService;
  let mockAuthService: MockAuthService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DashboardComponent, HeaderComponent],
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule],
      providers: [
        { provide: ApiService, useClass: MockApiService },
        { provide: AuthService, useClass: MockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    mockApiService = TestBed.inject(ApiService) as any;
    mockAuthService = TestBed.inject(AuthService) as any;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load teams and set selectedTeamId', async () => {
    await component.loadTeams();
    expect(component.teams.length).toBe(2);
    expect(component.selectedTeamId).toBe('1');
    expect(mockAuthService.setTeam).toHaveBeenCalledWith({ id: '1', name: 'Team A' });
  });

  it('should call loadTaskCounts when team exists', async () => {
    const spyLoadTeams = spyOn(component, 'loadTeams').and.callThrough();
    const spyLoadTaskCounts = spyOn(component, 'loadTaskCounts').and.callThrough();

    mockAuthService.getTeam.and.returnValue({ id: '2', name: 'Team B' });

    await component.loadData();

    expect(spyLoadTeams).toHaveBeenCalled();
    expect(spyLoadTaskCounts).toHaveBeenCalledWith('2');
  });

  it('should load task counts and update taskCounts property', () => {
    component.loadTaskCounts('1');
    expect(component.taskCounts).toEqual({ open: 3, closed: 7 });
  });

  it('should handle error in loadTeams gracefully', async () => {
    mockApiService.user_GetUserTeamsById.and.returnValue(
      throwError(() => new Error('API Error'))
    );

    try {
      await component.loadTeams();
      fail('Expected promise to reject');
    } catch (err) {
      expect(err).toBeTruthy();
      expect(mockApiService.user_GetUserTeamsById).toHaveBeenCalled();
    }
  });

  it('should update selected team and call loadTaskCounts on team selection', () => {
    const team1 = new TeamSummaryDto();
    team1.id = '1';
    team1.name = 'Team A';

    const team2 = new TeamSummaryDto();
    team2.id = '2';
    team2.name = 'Team B';

    component.teams = [team1, team2];
    const spyLoad = spyOn(component, 'loadTaskCounts');

    const mockEvent = { target: { value: '2' } } as any as Event;
    component.onTeamSelected(mockEvent);

    expect(mockAuthService.setTeam).toHaveBeenCalledWith(
        jasmine.objectContaining({ id: '2', name: 'Team B' })
    );
    expect(component.selectedTeamId).toBe('2');
    expect(spyLoad).toHaveBeenCalledWith('2');
  });
});
