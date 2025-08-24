import { Injectable } from '@angular/core';
import { Client, Message } from 'paho-mqtt';
import { Store } from '@ngrx/store';
import { notificationReceived } from './notification.actions';
import { ToastService } from './toast.service';

@Injectable({ providedIn: 'root' })
export class MqttService {
  private client: Client;

  constructor(private store: Store, private toast: ToastService) {
    const clientId = 'angular-' + Math.random().toString(16).substring(2, 8);
    this.client = new Client('localhost', 8083, '/mqtt', clientId);

    this.client.onConnectionLost = () => {
      console.log('MQTT disconnected');
      setTimeout(() => this.connect(), 5000);
    };
    this.client.onMessageArrived = (msg: Message) => {
      const payload = msg.payloadString;
      this.store.dispatch(notificationReceived({ message: payload }));
      this.toast.show(payload);
    };

    this.connect();
  }

  private connect() {
    this.client.connect({
      onSuccess: () => {
        console.log('MQTT connected');
        this.client.subscribe('notifications');
      },
      onFailure: () => {
        console.error('MQTT connection failed, retrying...');
        setTimeout(() => this.connect(), 5000);
      }
    });
  }
}
