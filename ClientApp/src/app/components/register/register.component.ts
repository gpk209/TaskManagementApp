import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/accountService';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  errorMessage: string = '';

 constructor(private accountService: AccountService){

 }

  register(){
    this.errorMessage = ''; // Clear previous errors
    this.accountService.register(this.model).subscribe({
      next: () =>{
        this.cancel();
      },
      error: error => {
        console.log(error);
        if (error.status === 409) {
          // Extract error message from response
          this.errorMessage = typeof error.error === 'string' 
            ? error.error 
            : error.error?.message || 'Username already exists';
        } else {
          this.errorMessage = 'Registration failed. Please try again.';
        }
      }
    });
  }
  cancel(){
    this.cancelRegister.emit(false);    
  }
}
