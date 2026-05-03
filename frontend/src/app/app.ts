import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [CommonModule],
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('AI Flix');
}
