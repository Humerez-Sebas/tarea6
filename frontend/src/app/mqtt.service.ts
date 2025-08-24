import { Injectable } from '@angular/core';
import { Client, Message } from 'paho-mqtt';

@Injectable({ providedIn: 'root' })
export class MqttService {
  private client: Client;
  private messages: string[] = [];

  constructor() {
    const clientId = 'angular-' + Math.random().toString(16).substring(2, 8);
    this.client = new Client('localhost', 8083, '/mqtt', clientId);

    this.client.onConnectionLost = () => console.log('MQTT disconnected');
    this.client.onMessageArrived = (msg: Message) => {
      this.messages.push(msg.payloadString);
    };

    this.client.connect({
      onSuccess: () => {
        console.log('MQTT connected');
        this.client.subscribe('notifications');
      }
    });
  }

  get history(): string[] {
    return this.messages;
  }
}
