# RabbitMQ Setup Guide

## Quick Start

1. **Start RabbitMQ with Docker Compose:**
   ```bash
   docker-compose up rabbitmq -d
   ```

2. **Access RabbitMQ Management UI:**
   - URL: http://localhost:15672
   - Username: `guest`
   - Password: `guest`

3. **Verify Setup:**
   - Check that `events` exchange exists
   - Verify queues: `ticketing.events`, `payments.events`, `notifications.events`
   - Confirm bindings are created

## Configuration

- **AMQP Port:** 5672
- **Management UI Port:** 15672
- **Connection String:** `amqp://guest:guest@localhost:5672/`

## Pre-configured Resources

### Exchange
- **Name:** `events`
- **Type:** `topic`
- **Durable:** `true`

### Queues
- `ticketing.events` - Handles reservation events
- `payments.events` - Handles payment events  
- `notifications.events` - Handles all events (catch-all)

### Routing Keys
- `ReservationCreated` → `ticketing.events`
- `ReservationConfirmed` → `ticketing.events`
- `ReservationCancelled` → `ticketing.events`
- `PaymentProcessed` → `payments.events`
- `PaymentFailed` → `payments.events`
- `#` (all) → `notifications.events`

## Testing

Test the outbox pattern:
```bash
# Start all services
docker-compose up -d

# Test outbox endpoint
curl -X POST http://localhost:5000/test/outbox

# Check RabbitMQ management UI for messages
```