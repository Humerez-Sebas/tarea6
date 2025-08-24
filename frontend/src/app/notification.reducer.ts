import { createReducer, on } from '@ngrx/store';
import { notificationReceived } from './notification.actions';

export interface NotificationState {
  messages: string[];
}

const initialState: NotificationState = {
  messages: []
};

export const notificationReducer = createReducer(
  initialState,
  on(notificationReceived, (state, { message }) => ({
    ...state,
    messages: [...state.messages, message]
  }))
);
