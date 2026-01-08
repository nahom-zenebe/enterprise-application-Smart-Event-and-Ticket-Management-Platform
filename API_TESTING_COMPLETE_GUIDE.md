# Smart Event Platform - API Testing Guide

## Base URL
```
http://localhost:5160
```

## Authentication
Most endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

---

# BOUNDED CONTEXTS

## 1. EVENT PLANNING CONTEXT
*Responsibility: Create events, sessions, schedules, assign venues and performers, define seating categories, manage capacity rules*

### 1.1 Events Management

#### Create Event
```http
POST /events
```
**Request Body:**
```json
{
  "name": "Summer Music Festival",
  "description": "A fantastic outdoor music festival featuring top artists",
  "startDateUtc": "2024-07-15T18:00:00Z",
  "endDateUtc": "2024-07-15T23:00:00Z",
  "category": "Music",
  "venue": "Central Park Amphitheater"
}
```

#### Get All Events (Event Listings)
```http
GET /events
```
**Query Parameters:**
- `dateUtc`: Filter by date
- `category`: Filter by category
- `venue`: Filter by venue

#### Get Event by ID
```http
GET /events/{eventId}
```

#### Update Event
```http
PUT /events/{eventId}
```

#### Delete Event
```http
DELETE /events/{eventId}
```

### 1.2 Venues Management (Seat Allocation)

#### Get All Venues
```http
GET /api/venues
```
**Response:**
```json
[
  {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Central Park Amphitheater",
    "location": "New York, NY",
    "capacity": 5000,
    "seatingLayoutId": "456e7890-e89b-12d3-a456-426614174000",
    "contactInfo": "contact@venue.com"
  }
]
```

#### Create Venue (Define Seating Categories)
```http
POST /api/venues
```
**Request Body:**
```json
{
  "name": "Madison Square Garden",
  "location": "New York, NY",
  "capacity": 20000,
  "seatingLayoutId": "456e7890-e89b-12d3-a456-426614174000",
  "contactInfo": "info@msg.com"
}
```

#### Update Venue
```http
PUT /api/venues/{venueId}
```

### 1.3 Performers Management

#### Get All Performers
```http
GET /api/performers
```

#### Create Performer
```http
POST /api/performers
```
**Request Body:**
```json
{
  "name": "John Doe Band",
  "bio": "Award-winning rock band from California",
  "imageUrl": "https://example.com/performer-image.jpg",
  "performanceType": "Music",
  "socialLinks": {
    "instagram": "@johndoeband",
    "twitter": "@johndoeband"
  }
}
```

#### Get Performer by ID
```http
GET /api/performers/{performerId}
```

#### Update Performer
```http
PUT /api/performers/{performerId}
```

#### Delete Performer
```http
DELETE /api/performers/{performerId}
```

### 1.4 Sessions Management (Event Scheduling)

#### Create Session for Event
```http
POST /api/events/{eventId}/sessions
```
**Request Body:**
```json
{
  "name": "Opening Act",
  "startTime": "2024-07-15T18:00:00Z",
  "endTime": "2024-07-15T19:30:00Z",
  "performerIds": [
    "123e4567-e89b-12d3-a456-426614174000"
  ],
  "capacity": 1000,
  "seatingMapId": "789e0123-e89b-12d3-a456-426614174000"
}
```

#### Get Sessions for Event
```http
GET /api/events/{eventId}/sessions
```

---

## 2. TICKETING CONTEXT
*Responsibility: Ticket creation, pricing, discounts, reservations, QR/Barcode generation, ticket validation and check-in logic*

### 2.1 Reservations Management (Add-to-cart, Reservation Flow)

#### Create Reservation (Real-time Seat Locking)
```http
POST /reservations
```
**Request Body:**
```json
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "eventId": "456e7890-e89b-12d3-a456-426614174000",
  "expiresAt": "2024-07-15T17:00:00Z"
}
```

#### Get All Reservations
```http
GET /reservations
```

#### Get Reservation by ID
```http
GET /reservations/{reservationId}
```

#### Get Reservations by User
```http
GET /reservations/user/{userId}
```

#### Get Reservations by Event
```http
GET /reservations/event/{eventId}
```

#### Confirm Reservation
```http
POST /reservations/{reservationId}/confirm
```

#### Cancel Reservation
```http
POST /reservations/{reservationId}/cancel
```

#### Add Ticket to Reservation (Multi-tier Pricing)
```http
POST /reservations/{reservationId}/tickets
```
**Request Body:**
```json
{
  "type": 1,
  "price": 99.99,
  "discountCode": "SUMMER20",
  "reservationID": "123e4567-e89b-12d3-a456-426614174000"
}
```

#### Process Payment for Reservation (Payment Flow)
```http
POST /reservations/{reservationId}/payment
```
**Request Body:**
```json
{
  "paymentId": "789e0123-e89b-12d3-a456-426614174000"
}
```

#### Get Reservation Total (Dynamic Pricing)
```http
GET /reservations/{reservationId}/total
```

### 2.2 Tickets Management (Ticket Creation & Pricing)

#### Create Ticket
```http
POST /tickets
```
**Request Body:**
```json
{
  "type": 1,
  "price": 149.99,
  "discountCode": "VIP10",
  "reservationID": "123e4567-e89b-12d3-a456-426614174000"
}
```

#### Get All Tickets
```http
GET /tickets
```

#### Get Ticket by ID
```http
GET /tickets/{ticketId}
```

#### Update Ticket
```http
PUT /tickets/{ticketId}
```

#### Confirm Ticket
```http
POST /tickets/{ticketId}/confirm
```

#### Cancel Ticket
```http
POST /tickets/{ticketId}/cancel
```

#### Check-in Ticket (Fast Check-in & Scanning)
```http
POST /tickets/{ticketId}/checkin
```

#### Get Ticket QR Code (QR Code Generation)
```http
GET /tickets/{ticketId}/qrcode
```
**Response:**
```json
{
  "qrCode": "base64-encoded-qr-code-image"
}
```

#### Validate Ticket (Gate Access Validation)
```http
POST /tickets/validate
```
**Request Body (By Ticket ID):**
```json
{
  "ticketId": "123e4567-e89b-12d3-a456-426614174000"
}
```
**Request Body (By QR Code):**
```json
{
  "qrCode": "scanned-qr-code-data"
}
```

---

## 3. CUSTOMER EXPERIENCE & ACCESS CONTROL CONTEXT
*Responsibility: Gate access validation, event recommendations, like/dislike and save events to favorites*

### 3.1 Event Interactions (Customer Experience & Engagement)

**Note:** All interaction endpoints require authentication.

#### Like Event (Personalized Recommendations)
```http
POST /api/events/{eventId}/like
```
**Headers:** `Authorization: Bearer <token>`
**Response:**
```json
{
  "liked": true,
  "message": "Event liked",
  "interaction": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "eventId": "789e0123-e89b-12d3-a456-426614174000",
    "interactionType": 1,
    "timestamp": "2024-01-08T12:00:00Z"
  },
  "likes": 15,
  "dislikes": 2
}
```

#### Dislike Event
```http
POST /api/events/{eventId}/dislike
```
**Headers:** `Authorization: Bearer <token>`

#### Save Event to Favorites
```http
POST /api/events/{eventId}/save
```
**Headers:** `Authorization: Bearer <token>`

#### Remove Event from Favorites
```http
DELETE /api/events/{eventId}/save
```
**Headers:** `Authorization: Bearer <token>`

#### Get User Favorites
```http
GET /api/users/{userId}/favorites
```
**Headers:** `Authorization: Bearer <token>`
**Response:**
```json
{
  "userId": "456e7890-e89b-12d3-a456-426614174000",
  "savedEvents": [
    {
      "interactionId": "123e4567-e89b-12d3-a456-426614174000",
      "eventId": "789e0123-e89b-12d3-a456-426614174000",
      "eventTitle": "Summer Music Festival",
      "eventDate": "2024-07-15T18:00:00Z",
      "interactionTime": "2024-01-08T12:00:00Z"
    }
  ],
  "totalSaved": 1
}
```

#### Get Event Stats (Engagement Analytics)
```http
GET /api/events/{eventId}/interactions/stats
```
**Response:**
```json
{
  "eventId": "789e0123-e89b-12d3-a456-426614174000",
  "likes": 25,
  "dislikes": 3,
  "saves": 12,
  "attended": 8,
  "userInteraction": 1
}
```

#### Get User Interactions
```http
GET /api/users/{userId}/interactions?type=1
```
**Headers:** `Authorization: Bearer <token>`
**Query Parameters:**
- `type`: Filter by interaction type (1=Like, 2=Dislike, 3=Saved, 4=Attended)

#### Get Event Interactions
```http
GET /api/events/{eventId}/interactions?type=1
```

---

# SUPPORTING SUBDOMAINS

## 4. PAYMENTS & BILLING
*Payment gateway integration, card processing, receipts*

#### Create Payment
```http
POST /api/payments
```
**Request Body:**
```json
{
  "ticketId": 123,
  "userId": 456,
  "amount": 99.99,
  "method": 1
}
```

**Payment Methods:**
- `1`: CreditCard
- `2`: PayPal
- `3`: BankTransfer
- `4`: Cash

## 5. AUTHENTICATION & USER ACCOUNTS
*Login/Signup, role-based access, password reset*

#### Public Endpoint (No Auth Required)
```http
GET /api/public
```

#### Test Authentication
```http
GET /api/secure
```
**Headers:** `Authorization: Bearer <token>`

#### Admin Only
```http
GET /api/admin
```
**Headers:** `Authorization: Bearer <admin-token>`

#### Buyer Only
```http
GET /api/buyer
```
**Headers:** `Authorization: Bearer <buyer-token>`

#### Seller Only
```http
GET /api/seller
```
**Headers:** `Authorization: Bearer <seller-token>`

#### Debug Token Claims
```http
GET /api/debug/token
```
**Headers:** `Authorization: Bearer <token>`

---

# TRANSACTIONAL OUTBOX PATTERN TESTING

*Testing the implementation for eventual transition to microservices with guaranteed message delivery and eventual consistency*

## Overview
The Transactional Outbox Pattern ensures reliable event publishing by:
1. **The Mechanism**: Saving Domain Events to an Outbox table in the same database transaction as Aggregate changes
2. **The Publisher**: Background worker (Quartz.NET) polling the Outbox table and publishing to RabbitMQ
3. **The Subscriber**: Other modules subscribing to events for Eventual Consistency

---

## 6. OUTBOX PATTERN TESTING

### 6.1 Test Event Creation with Outbox

#### Create Test Outbox Event
```http
POST /test/outbox
```
**Response:**
```json
"Test event added to outbox"
```
**What Happens:**
- Creates a test event in the OutboxEvents table
- Event will be picked up by background publisher job
- Demonstrates the mechanism working

### 6.2 Business Operations that Trigger Outbox Events

#### Create Reservation (Triggers ReservationCreated Event)
```http
POST /reservations
```
**Request Body:**
```json
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "eventId": "456e7890-e89b-12d3-a456-426614174000",
  "expiresAt": "2024-07-15T17:00:00Z"
}
```
**Outbox Event Generated:**
- EventType: `"ReservationCreated"`
- EventData: `{"ReservationId": "...", "UserId": "...", "EventId": "..."}`
- Stored in same transaction as reservation

#### Confirm Reservation (Triggers ReservationConfirmed Event)
```http
POST /reservations/{reservationId}/confirm
```
**Outbox Event Generated:**
- EventType: `"ReservationConfirmed"`
- EventData: `{"ReservationId": "...", "UserId": "..."}`

#### Cancel Reservation (Triggers ReservationCancelled Event)
```http
POST /reservations/{reservationId}/cancel
```
**Outbox Event Generated:**
- EventType: `"ReservationCancelled"`
- EventData: `{"ReservationId": "...", "UserId": "..."}`

#### Process Payment (Triggers PaymentProcessed Event)
```http
POST /reservations/{reservationId}/payment
```
**Request Body:**
```json
{
  "paymentId": "789e0123-e89b-12d3-a456-426614174000"
}
```
**Outbox Event Generated:**
- EventType: `"PaymentProcessed"`
- EventData: `{"ReservationId": "...", "PaymentId": "..."}`

### 6.3 Verify Outbox Table Contents

#### Check Database Outbox Events
```sql
-- Connect to PostgreSQL database
SELECT 
    "Id",
    "EventType",
    "EventData",
    "CreatedAt",
    "IsProcessed",
    "RetryCount",
    "NextRetryAt",
    "LastError"
FROM "OutboxEvents" 
ORDER BY "CreatedAt" DESC;
```

**Expected Results:**
```
Id                                   | EventType           | IsProcessed | RetryCount
-------------------------------------|--------------------|-----------|-----------
123e4567-e89b-12d3-a456-426614174000| ReservationCreated | false     | 0
456e7890-e89b-12d3-a456-426614174001| TestEvent          | true      | 0
789e0123-e89b-12d3-a456-426614174002| PaymentProcessed   | false     | 1
```

### 6.4 Monitor Background Publisher Job

#### Check Application Logs
```bash
# Monitor logs for Quartz.NET job execution
docker-compose logs -f backend-api | grep -i "outbox"

# Expected log entries:
# Starting outbox publisher job
# Successfully published event 123e4567-e89b-12d3-a456-426614174000 of type ReservationCreated
# Failed to publish event 456e7890-e89b-12d3-a456-426614174001 (attempt 1)
# Outbox publisher job completed. Processed: 2, Failed: 1
```

#### Job Execution Frequency
- **Interval**: Every 30 seconds
- **Concurrency**: DisallowConcurrentExecution prevents overlapping
- **Retry Logic**: Exponential backoff (2^retryCount minutes)

### 6.5 Test Retry Mechanism

#### Verify Retry Behavior
1. **Create multiple test events**:
   ```bash
   for i in {1..10}; do
     curl -X POST http://localhost:5160/test/outbox
   done
   ```

2. **Monitor retry attempts**:
   ```sql
   SELECT 
       "EventType",
       "RetryCount",
       "NextRetryAt",
       "LastError"
   FROM "OutboxEvents" 
   WHERE "IsProcessed" = false
   ORDER BY "RetryCount" DESC;
   ```

3. **Expected retry schedule**:
   - Attempt 1: Immediate
   - Attempt 2: After 2 minutes
   - Attempt 3: After 4 minutes  
   - Attempt 4: After 8 minutes
   - Max retries reached: Dead letter

### 6.6 Test Transactional Guarantees

#### Verify ACID Compliance
```http
# This should create both reservation and outbox event atomically
POST /reservations
```
**Verification:**
```sql
-- Both should exist or both should be missing
SELECT COUNT(*) FROM "Reservations" WHERE "Id" = 'reservation-id';
SELECT COUNT(*) FROM "OutboxEvents" WHERE "EventType" = 'ReservationCreated';
```

### 6.7 Performance Testing

#### Load Test Outbox Pattern
```bash
# Generate high volume of events
for i in {1..100}; do
  curl -X POST http://localhost:5160/reservations \
    -H "Content-Type: application/json" \
    -d '{
      "userId": "123e4567-e89b-12d3-a456-426614174000",
      "eventId": "456e7890-e89b-12d3-a456-426614174000",
      "expiresAt": "2024-07-15T17:00:00Z"
    }' &
done
wait
```

#### Monitor Performance Metrics
```sql
-- Check outbox table size and processing status
SELECT 
    COUNT(*) as total_events,
    COUNT(CASE WHEN "IsProcessed" = true THEN 1 END) as processed,
    COUNT(CASE WHEN "IsProcessed" = false THEN 1 END) as pending,
    AVG("RetryCount") as avg_retries
FROM "OutboxEvents";
```

---

## OUTBOX PATTERN TESTING CHECKLIST

### ✅ **Core Pattern Testing**
- [ ] Events stored in same transaction as business data
- [ ] Background job polls outbox table every 30 seconds
- [ ] Events published to message broker (RabbitMQ simulation)
- [ ] Successful events marked as processed
- [ ] Failed events retry with exponential backoff
- [ ] Dead letter handling after max retries

### ✅ **Business Event Testing**
- [ ] ReservationCreated event on reservation creation
- [ ] ReservationConfirmed event on confirmation
- [ ] ReservationCancelled event on cancellation
- [ ] PaymentProcessed event on payment
- [ ] All events contain proper event data

### ✅ **Reliability Testing**
- [ ] ACID transaction guarantees
- [ ] No lost events during failures
- [ ] No duplicate event processing
- [ ] Proper error handling and logging
- [ ] Retry mechanism with backoffnt publishing by:
1. **The Mechanism**: Saving Domain Events to an Outbox table in the same database transaction as Aggregate changes
2. **The Publisher**: Background worker (Quartz.NET) polling the Outbox table and publishing to RabbitMQ
3. **The Subscriber**: Other modules subscribing to events for Eventual Consistency

---

## 6. OUTBOX PATTERN TESTING

### 6.1 Test Event Creation with Outbox

#### Create Test Outbox Event
```http
POST /test/outbox
```
**Response:**
```json
"Test event added to outbox"
```
**What Happens:**
- Creates a test event in the OutboxEvents table
- Event will be picked up by background publisher job
- Demonstrates the mechanism working

### 6.2 Business Operations that Trigger Outbox Events

#### Create Reservation (Triggers ReservationCreated Event)
```http
POST /reservations
```
**Request Body:**
```json
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "eventId": "456e7890-e89b-12d3-a456-426614174000",
  "expiresAt": "2024-07-15T17:00:00Z"
}
```
**Outbox Event Generated:**
- EventType: `"ReservationCreated"`
- EventData: `{"ReservationId": "...", "UserId": "...", "EventId": "..."}`
- Stored in same transaction as reservation

#### Confirm Reservation (Triggers ReservationConfirmed Event)
```http
POST /reservations/{reservationId}/confirm
```
**Outbox Event Generated:**
- EventType: `"ReservationConfirmed"`
- EventData: `{"ReservationId": "...", "UserId": "..."}`

#### Cancel Reservation (Triggers ReservationCancelled Event)
```http
POST /reservations/{reservationId}/cancel
```
**Outbox Event Generated:**
- EventType: `"ReservationCancelled"`
- EventData: `{"ReservationId": "...", "UserId": "..."}`

#### Process Payment (Triggers PaymentProcessed Event)
```http
POST /reservations/{reservationId}/payment
```
**Request Body:**
```json
{
  "paymentId": "789e0123-e89b-12d3-a456-426614174000"
}
```
**Outbox Event Generated:**
- EventType: `"PaymentProcessed"`
- EventData: `{"ReservationId": "...", "PaymentId": "..."}`

### 6.3 Verify Outbox Table Contents

#### Check Database Outbox Events
```sql
-- Connect to PostgreSQL database
SELECT 
    "Id",
    "EventType",
    "EventData",
    "CreatedAt",
    "IsProcessed",
    "RetryCount",
    "NextRetryAt",
    "LastError"
FROM "OutboxEvents" 
ORDER BY "CreatedAt" DESC;
```

**Expected Results:**
```
Id                                   | EventType           | IsProcessed | RetryCount
-------------------------------------|--------------------|-----------|-----------
123e4567-e89b-12d3-a456-426614174000| ReservationCreated | false     | 0
456e7890-e89b-12d3-a456-426614174001| TestEvent          | true      | 0
789e0123-e89b-12d3-a456-426614174002| PaymentProcessed   | false     | 1
```

### 6.4 Monitor Background Publisher Job

#### Check Application Logs
```bash
# Monitor logs for Quartz.NET job execution
docker-compose logs -f backend-api | grep -i "outbox"

# Expected log entries:
# Starting outbox publisher job
# Successfully published event 123e4567-e89b-12d3-a456-426614174000 of type ReservationCreated
# Failed to publish event 456e7890-e89b-12d3-a456-426614174001 (attempt 1)
# Outbox publisher job completed. Processed: 2, Failed: 1
```

#### Job Execution Frequency
- **Interval**: Every 30 seconds
- **Concurrency**: DisallowConcurrentExecution prevents overlapping
- **Retry Logic**: Exponential backoff (2^retryCount minutes)

### 6.5 Test Retry Mechanism

#### Verify Retry Behavior
1. **Create multiple test events**:
   ```bash
   for i in {1..10}; do
     curl -X POST http://localhost:5160/test/outbox
   done
   ```

2. **Monitor retry attempts**:
   ```sql
   SELECT 
       "EventType",
       "RetryCount",
       "NextRetryAt",
       "LastError"
   FROM "OutboxEvents" 
   WHERE "IsProcessed" = false
   ORDER BY "RetryCount" DESC;
   ```

3. **Expected retry schedule**:
   - Attempt 1: Immediate
   - Attempt 2: After 2 minutes
   - Attempt 3: After 4 minutes  
   - Attempt 4: After 8 minutes
   - Max retries reached: Dead letter

### 6.6 Test Transactional Guarantees

#### Verify ACID Compliance
```http
# This should create both reservation and outbox event atomically
POST /reservations
```
**Verification:**
```sql
-- Both should exist or both should be missing
SELECT COUNT(*) FROM "Reservations" WHERE "Id" = 'reservation-id';
SELECT COUNT(*) FROM "OutboxEvents" WHERE "EventType" = 'ReservationCreated';
```

### 6.7 Performance Testing

#### Load Test Outbox Pattern
```bash
# Generate high volume of events
for i in {1..100}; do
  curl -X POST http://localhost:5160/reservations \
    -H "Content-Type: application/json" \
    -d '{
      "userId": "123e4567-e89b-12d3-a456-426614174000",
      "eventId": "456e7890-e89b-12d3-a456-426614174000",
      "expiresAt": "2024-07-15T17:00:00Z"
    }' &
done
wait
```

#### Monitor Performance Metrics
```sql
-- Check outbox table size and processing status
SELECT 
    COUNT(*) as total_events,
    COUNT(CASE WHEN "IsProcessed" = true THEN 1 END) as processed,
    COUNT(CASE WHEN "IsProcessed" = false THEN 1 END) as pending,
    AVG("RetryCount") as avg_retries
FROM "OutboxEvents";
```

---

## OUTBOX PATTERN TESTING CHECKLIST

### ✅ **Core Pattern Testing**
- [ ] Events stored in same transaction as business data
- [ ] Background job polls outbox table every 30 seconds
- [ ] Events published to message broker (RabbitMQ simulation)
- [ ] Successful events marked as processed
- [ ] Failed events retry with exponential backoff
- [ ] Dead letter handling after max retries

### ✅ **Business Event Testing**
- [ ] ReservationCreated event on reservation creation
- [ ] ReservationConfirmed event on confirmation
- [ ] ReservationCancelled event on cancellation
- [ ] PaymentProcessed event on payment
- [ ] All events contain proper event data

### ✅ **Reliability Testing**
- [ ] ACID transaction guarantees
- [ ] No lost events during failures
- [ ] No duplicate event processing
- [ ] Proper error handling and logging
- [ ] Retry mechanism with backoff

---

# REFERENCE TABLES

## Interaction Types
| Value | Type | Description |
|-------|------|-------------|
| 1 | Like | User liked the event |
| 2 | Dislike | User disliked the event |
| 3 | Saved | User saved the event to favorites |
| 4 | Attended | User attended the event |

## Ticket Types (Multi-tier Pricing)
| Value | Type | Description |
|-------|------|-------------|
| 1 | Standard | Regular admission ticket |
| 2 | VIP | VIP access ticket |
| 3 | Student | Student discount ticket |
| 4 | Senior | Senior citizen discount |

## Reservation Status
| Value | Status | Description |
|-------|--------|-------------|
| 0 | Pending | Reservation is pending |
| 1 | Confirmed | Reservation is confirmed |
| 2 | Cancelled | Reservation is cancelled |
| 3 | Expired | Reservation has expired |

## Ticket Status
| Value | Status | Description |
|-------|--------|-------------|
| 0 | Reserved | Ticket is reserved |
| 1 | Confirmed | Ticket is confirmed |
| 2 | Cancelled | Ticket is cancelled |
| 3 | CheckedIn | Ticket holder has checked in |

---

# TESTING WORKFLOW

## Core Domain Testing Order:
1. **Event Planning Context**: Create venues → Create performers → Create events → Create sessions
2. **Ticketing Context**: Create reservations → Add tickets → Process payments → Generate QR codes → Validate tickets
3. **Customer Experience**: Like/dislike events → Save to favorites → Get recommendations
4. **Outbox Pattern**: Test event creation → Monitor background job → Verify retry mechanism

## Key Features to Test:
- **Real-time Seat Locking**: Test concurrent reservation attempts
- **Dynamic Pricing**: Test different ticket types and pricing
- **QR Code Generation**: Test ticket QR code creation and validation
- **Fast Check-in**: Test ticket validation and check-in process
- **Personalized Recommendations**: Test like/dislike functionality
- **Reservation Timeouts**: Test reservation expiration
- **Multi-tier Pricing**: Test VIP, Standard, Student pricing
- **Transactional Outbox**: Test event publishing reliability with ACID guarantees
- **Eventual Consistency**: Test cross-context event communication
- **Retry Mechanism**: Test exponential backoff and dead letter handling