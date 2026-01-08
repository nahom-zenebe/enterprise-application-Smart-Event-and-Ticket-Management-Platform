#!/bin/bash

# Wait for RabbitMQ to be ready
sleep 30

# Create exchange
rabbitmqadmin declare exchange name=events type=topic durable=true

# Create queues
rabbitmqadmin declare queue name=ticketing.events durable=true
rabbitmqadmin declare queue name=payments.events durable=true
rabbitmqadmin declare queue name=notifications.events durable=true

# Create bindings
rabbitmqadmin declare binding source=events destination=ticketing.events routing_key="ReservationCreated"
rabbitmqadmin declare binding source=events destination=ticketing.events routing_key="ReservationConfirmed"
rabbitmqadmin declare binding source=events destination=ticketing.events routing_key="ReservationCancelled"

rabbitmqadmin declare binding source=events destination=payments.events routing_key="PaymentProcessed"
rabbitmqadmin declare binding source=events destination=payments.events routing_key="PaymentFailed"

rabbitmqadmin declare binding source=events destination=notifications.events routing_key="*"

echo "RabbitMQ setup completed!"