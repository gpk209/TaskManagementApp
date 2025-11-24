import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../services/accountService';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, FormsModule, NgbDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent implements OnInit {
  model: any = {}
  errorMessage: string = '';

  ngOnInit(): void { }

  constructor(public accoutnService: AccountService) { }

  login() {
    this.errorMessage = ''; // Clear previous errors
    this.accoutnService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        // Clear form on successful login
        this.model = {};
      },
      error: error => {
        console.log(error);
        if (error.status === 401) {
          // Extract error message from response
          this.errorMessage = typeof error.error === 'string' 
            ? error.error 
            : error.error?.message || 'Invalid username or password';
        } else {
          this.errorMessage = 'Login failed. Please try again.';
        }
      }
    });
  }
  logout() {
    this.accoutnService.logout();
  }

}
