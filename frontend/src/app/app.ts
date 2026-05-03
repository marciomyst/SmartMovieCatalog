import { Component } from '@angular/core';
import { LoginPage } from './auth/login-page/login-page';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [LoginPage],
  styleUrl: './app.css'
})
export class App {
}
