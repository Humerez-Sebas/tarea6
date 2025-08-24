# tarea6

## Arquitectura

El proyecto se compone de tres partes:

- **infrastructure/**: contiene un `docker-compose.yml` que levanta un bróker MQTT [EMQX](https://www.emqx.io/) con soporte de WebSockets en los puertos `1883` (TCP) y `8083` (WS).
- **backend/**: API minimal en **.NET 8** que mantiene una conexión al bróker usando [MQTTnet](https://github.com/dotnet/MQTTnet). Expone un endpoint HTTP `POST /notify` para publicar notificaciones en el tópico `notifications`.
- **frontend/**: aplicación **Angular 19** que se conecta al bróker mediante WebSocket usando la librería `paho-mqtt`. Escucha el tópico `notifications` y muestra un historial de mensajes en la interfaz.

### Flujo end‑to‑end
1. El bróker MQTT se levanta con `docker compose`.
2. El backend se inicia y se conecta al bróker. Cuando recibe una petición `POST /notify`, publica el mensaje en `notifications`.
3. El cliente Angular mantiene una conexión WebSocket al bróker, suscrito a `notifications`. Cuando el backend publica un mensaje, el cliente lo recibe en tiempo real y lo agrega a la lista visible.

## Justificación del protocolo
MQTT es un protocolo ligero basado en `publish/subscribe` que reduce el acoplamiento entre productores y consumidores. Su soporte nativo para WebSockets permite que los navegadores participen directamente en la comunicación sin necesidad de un servidor intermedio. El uso de `ManagedMqttClient` en el backend maneja automáticamente reconexiones, mientras que Paho en el frontend expone eventos para detectar pérdidas de conexión.

## Gestión de conexiones y eventos
- **Backend**: `ManagedMqttClient` registra eventos de conexión y desconexión, escribiendo en consola cuando cambia el estado.
- **Frontend**: el cliente Paho maneja `onConnectionLost` y `onMessageArrived`. Los mensajes recibidos se almacenan en un arreglo local que sirve como historial durante la sesión.

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
   npm start
   ```
4. Enviar una notificación:
   ```bash
   curl -X POST http://localhost:5000/notify -H "Content-Type: application/json" -d '{"message":"hola"}'
   ```
   El mensaje aparecerá de inmediato en la interfaz Angular.

## Criterios de evaluación
- ✔️ **Cumplimiento de instrucciones**
  - API .NET 8 conectada a MQTT.
  - Cliente Angular 19 con Paho/WebSocket.
  - Endpoint HTTP `/notify` para disparar notificaciones.
  - Flujo funcionando de extremo a extremo.
- ✔️ **Contenido técnico**
  - README describe la arquitectura cliente‑servidor, la elección de MQTT y el manejo de eventos/conexiones.
- ✔️ **Estructura y presentación**
  - Código organizado en carpetas separadas (`backend`, `frontend`, `infrastructure`).
  - Instrucciones claras de ejecución.

## Capturas
Al ejecutar los pasos anteriores se observa en la consola del backend los eventos de conexión y en la página Angular la lista de notificaciones recibidas en tiempo real.
