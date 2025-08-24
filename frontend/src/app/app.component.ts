import { Component } from '@angular/core';
import { NgFor } from '@angular/common';
import { MqttService } from './mqtt.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(public mqtt: MqttService) {}
}
