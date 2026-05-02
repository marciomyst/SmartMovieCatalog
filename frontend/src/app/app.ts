import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [CommonModule],
  styleUrl: './app.css'
})
export class App implements OnInit {
  public forecasts: WeatherForecast[] = [];

  constructor(private readonly http: HttpClient) {}

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe({
      next: (result) => {
        this.forecasts = result;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  protected readonly title = signal('Smartmoviecatalog.Angular');
}
