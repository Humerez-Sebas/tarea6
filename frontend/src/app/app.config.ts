import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideStore } from '@ngrx/store';
import { notificationReducer } from './notification.reducer';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideStore({ notifications: notificationReducer })
  ]
};
