import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService, TeamSummaryDto } from '../../../services/api-service.service';
import { AuthService } from '../../../services/authentication/auth.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css',
    standalone: false
})
export class DashboardComponent implements OnInit {

    teams!: TeamSummaryDto[];

    constructor(
        private apiService: ApiService,
        private authService: AuthService
    ) {}

    ngOnInit(): void {
        this.loadData();
    }

    async loadData(): Promise<void> {
        await this.loadTeams();
    }

    loadTeams(): Promise<void> {
        return new Promise((resolve, reject) => {
            this.apiService.user_GetUserTeamsById().subscribe({
                next: (teams) => {
                    this.teams = teams;
                    console.log(this.teams);
                    resolve();
                },
                error: (err) => {
                    console.error('Error loading teams for user:', err);
                    reject(err);
                }
            });
        });
    }
}
