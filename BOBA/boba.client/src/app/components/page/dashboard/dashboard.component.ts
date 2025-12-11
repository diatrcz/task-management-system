import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService, TeamSummaryDto } from '../../../services/api-service.service';
import { AuthService } from '../../../services/authentication/auth.service';
import { Team } from '../../../models/Team';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css',
    standalone: false
})
export class DashboardComponent implements OnInit {

    teams!: TeamSummaryDto[];
    selectedTeamId!: string;
    taskCounts: { [key: string]: number } = {};

    constructor(
        private apiService: ApiService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.loadData();
    }

    async loadData(): Promise<void> {
        await this.loadTeams();
        const currentTeam = this.authService.getTeam();
        if (currentTeam) {
            this.selectedTeamId = currentTeam.id;
            this.loadTaskCounts(this.selectedTeamId);
        }
    }

    loadTeams(): Promise<void> {
        return new Promise((resolve, reject) => {
            this.apiService.user_GetUserTeamsById().subscribe({
                next: (teams) => {
                    this.teams = teams;
                    console.log(this.teams);
                    if (this.teams.length > 0 && !this.authService.getTeam()) {
                        this.authService.setTeam({
                            id: this.teams[0].id!,
                            name: this.teams[0].name!
                        });
                    }
                    this.selectedTeamId = this.teams[0].id!;
                    resolve();
                },
                error: (err) => {
                    console.error('Error loading teams for user:', err);
                    reject(err);
                }
            });
        });
    }

    loadTaskCounts(teamId: string): void {
        this.apiService.task_GetTasksCount(teamId).subscribe({
            next: (counts) => {
                this.taskCounts = counts;
                console.log('Task counts:', this.taskCounts);
            },
            error: (err) => {
                console.error('Error loading task counts:', err);
            }
        });
    }


    onTeamSelected(event: Event): void {
        const selectedId = (event.target as HTMLSelectElement).value;

        const selectedTeam = this.teams.find(t => t.id === selectedId);
        if (selectedTeam) {
            this.authService.setTeam({
                id: selectedTeam.id!,
                name: selectedTeam.name!
            });;

            this.selectedTeamId = selectedTeam.id!;
            this.loadTaskCounts(this.selectedTeamId);
        }
    }
}
