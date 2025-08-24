import { createAction, props } from '@ngrx/store';

export const notificationReceived = createAction(
  '[MQTT] Notification Received',
  props<{ message: string }>()
);
