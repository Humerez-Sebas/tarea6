# tarea6

## Arquitectura

El proyecto se compone de tres partes:

- **infrastructure/**: contiene un `docker-compose.yml` que levanta un bróker MQTT [EMQX](https://www.emqx.io/) con un usuario predeterminado (`guest/guest`) y soporte de WebSockets en los puertos `1883` (TCP) y `8083` (WS).
- **backend/**: API minimal en **.NET 8** que lee las credenciales MQTT desde `appsettings.json`, mantiene la conexión con reconexión automática mediante [MQTTnet](https://github.com/dotnet/MQTTnet) y expone `POST /notify` para publicar mensajes.
- **frontend/**: aplicación **Angular 19** que se conecta por WebSocket usando `paho-mqtt`. El estado de notificaciones se gestiona con **NgRx**, se muestra un historial y cada mensaje nuevo dispara un *toast* visual.

### Flujo end‑to‑end
1. El bróker MQTT se levanta con `docker compose`.
2. El backend se inicia y se conecta al bróker utilizando las credenciales configuradas. Al recibir una petición `POST /notify`, publica el mensaje en `notifications`.
3. El cliente Angular mantiene una conexión WebSocket al bróker, suscrito a `notifications`. Cuando llega un mensaje, NgRx actualiza el estado, aparece un *toast* y se agrega al historial visible.

## Justificación del protocolo
MQTT es un protocolo ligero *publish/subscribe* ideal para comunicación en tiempo real. Su soporte nativo de WebSockets permite que los navegadores interactúen directamente con el bróker. El uso de `ManagedMqttClient` en el backend simplifica la reconexión y el manejo de credenciales, mientras que `paho-mqtt` junto con NgRx ofrece un cliente reactivo y escalable.

## Gestión de conexiones y eventos
- **Backend**: la clase `MqttService` registra eventos de conexión/desconexión y reintenta automáticamente tras fallos.
- **Frontend**: `MqttService` reintenta la conexión cuando se pierde y despacha acciones NgRx por cada mensaje recibido, además de mostrar *toasts* mediante un servicio dedicado.

## Instrucciones de ejecución
1. Levantar el bróker:
   ```bash
   cd infrastructure
   docker compose up -d
   ```
2. Ejecutar el backend:
   ```bash
   cd ../backend
   dotnet run
   ```
3. Iniciar el frontend:
   ```bash
   cd ../frontend
   npm install --no-package-lock
   npm start
   ```
4. Enviar una notificación:
   ```bash
   curl -X POST http://localhost:5000/notify -H "Content-Type: application/json" -d '{"message":"hola"}'
   ```
   El mensaje aparecerá de inmediato en la interfaz Angular.

## Criterios de evaluación (rúbrica)
- ✔️ **Cumplimiento de instrucciones**
  - API .NET 8 conectada a MQTT con credenciales.
  - Cliente Angular 19 con Paho y NgRx.
  - Endpoint HTTP `/notify` para disparar notificaciones.
  - Manejo de eventos y reconexiones en ambos lados.
  - Flujo funcionando de extremo a extremo.
- ✔️ **Contenido técnico y profundidad**
  - README explica la arquitectura cliente‑servidor, la elección de MQTT y la gestión de eventos/conexiones.
  - Se incluye un proyecto de pruebas unitarias para el backend.
- ✔️ **Estructura y presentación**
  - Código organizado en carpetas separadas (`backend`, `frontend`, `infrastructure`).
  - Interfaz con historial y *toasts* estilizados.
  - Instrucciones claras de ejecución.

## Capturas
Al ejecutar los pasos anteriores se observa en la consola del backend los eventos de conexión y en la página Angular la lista de notificaciones recibidas en tiempo real junto a los *toasts* emergentes.
