import { Component } from '@angular/core';
import { NgFor, AsyncPipe } from '@angular/common';
import { MqttService } from './mqtt.service';
import { Store } from '@ngrx/store';
import { selectMessages } from './notification.selectors';
import { ToastContainerComponent } from './toast.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgFor, AsyncPipe, ToastContainerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  messages$: Observable<string[]>;

  constructor(private store: Store, public mqtt: MqttService) {
    this.messages$ = this.store.select(selectMessages);
  }
}
