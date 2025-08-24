import { createFeatureSelector, createSelector } from '@ngrx/store';
import { NotificationState } from './notification.reducer';

export const selectNotificationState = createFeatureSelector<NotificationState>('notifications');

export const selectMessages = createSelector(
  selectNotificationState,
  state => state.messages
);
