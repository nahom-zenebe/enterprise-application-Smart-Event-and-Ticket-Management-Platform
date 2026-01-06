# API Testing Guide - Step by Step

**Base URL:** `http://localhost:5159`

**Note:** Make sure your backend is running before testing. You can use tools like:
- Postman
- cURL
- Thunder Client (VS Code extension)
- Swagger UI (available at `http://localhost:5159/swagger` when running)

---

## 📋 Table of Contents
1. [Tickets API](#1-tickets-api)
2. [Events API](#2-events-api)
3. [Venues API](#3-venues-api)
4. [Performers API](#4-performers-api)
5. [Sessions API](#5-sessions-api)
6. [Payments API](#6-payments-api)
7. [Auth API](#7-auth-api)

---

## 1. Tickets API

### 1.1 Create Ticket
**POST** `/tickets`

**Request Body:**
```json
{
  "type": 0,
  "price": 50.00,
  "discountCode": "SAVE10",
  "reservationID": "00000000-0000-0000-0000-000000000000",
  "qrCode": null
}
```

**TicketType Values:**
- `0` = Regular
- `1` = VIP
- `2` = Student

**Example Response:**
```json
{
  "ticketID": "123e4567-e89b-12d3-a456-426614174000",
  "type": 0,
  "price": 50.00,
  "discountCode": "SAVE10",
  "status": 0,
  "reservationID": "00000000-0000-0000-0000-000000000000",
  "qrCode": null,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Status Values:**
- `0` = Reserved
- `1` = Confirmed
- `2` = Cancelled
- `3` = CheckedIn

---

### 1.2 Get All Tickets
**GET** `/tickets`

**No request body needed**

**Example Response:**
```json
[
  {
    "ticketID": "123e4567-e89b-12d3-a456-426614174000",
    "type": 0,
    "price": 50.00,
    "discountCode": "SAVE10",
    "status": 0,
    "reservationID": "00000000-0000-0000-0000-000000000000",
    "qrCode": null,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

---

### 1.3 Get Ticket by ID
**GET** `/tickets/{id}`

**Replace `{id}` with the ticket GUID**

**Example:**
```
GET /tickets/123e4567-e89b-12d3-a456-426614174000
```

---

### 1.4 Update Ticket
**PUT** `/tickets/{id}`

**Request Body:**
```json
{
  "type": 1,
  "price": 75.00,
  "discountCode": "VIP20",
  "reservationID": "00000000-0000-0000-0000-000000000000",
  "qrCode": null
}
```

**Response:** `204 No Content`

---

### 1.5 Delete Ticket
**DELETE** `/tickets/{id}`

**No request body needed**

**Response:** `204 No Content`

---

### 1.6 Confirm Ticket
**POST** `/tickets/{id}/confirm`

**No request body needed**

**Response:** `204 No Content`

**Note:** Only tickets with status "Reserved" can be confirmed.

---

### 1.7 Cancel Ticket
**POST** `/tickets/{id}/cancel`

**No request body needed**

**Response:** `204 No Content`

**Note:** Checked-in tickets cannot be cancelled.

---

### 1.8 Check In Ticket
**POST** `/tickets/{id}/checkin`

**No request body needed**

**Response:** `204 No Content`

**Note:** Only confirmed tickets can be checked in.

---

### 1.9 Get QR Code for Ticket
**GET** `/tickets/{id}/qrcode`

**Replace `{id}` with the ticket GUID**

**Example Response:**
```json
{
  "qrCode": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA..."
}
```

**Note:** If QR code doesn't exist, it will be generated automatically.

---

### 1.10 Validate Ticket (Entry Point)
**POST** `/tickets/validate`

**Request Body (Option 1 - Using Ticket ID):**
```json
{
  "ticketId": "123e4567-e89b-12d3-a456-426614174000",
  "qrCode": null
}
```

**Request Body (Option 2 - Using QR Code):**
```json
{
  "ticketId": null,
  "qrCode": "123e4567-e89b-12d3-a456-426614174000"
}
```

**Success Response (200 OK):**
```json
{
  "isValid": true,
  "message": "Ticket validated and checked in successfully",
  "ticket": {
    "ticketID": "123e4567-e89b-12d3-a456-426614174000",
    "type": 0,
    "price": 50.00,
    "discountCode": "SAVE10",
    "status": 3,
    "reservationID": "00000000-0000-0000-0000-000000000000",
    "qrCode": "data:image/png;base64,...",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
}
```

**Error Response (400 Bad Request):**
```json
{
  "isValid": false,
  "message": "Ticket has already been checked in",
  "ticket": { ... }
}
```

**Note:** This endpoint automatically checks in the ticket if validation is successful.

---

## 2. Events API

### 2.1 Create Event
**POST** `/events`

**Request Body:**
```json
{
  "name": "Summer Music Festival 2024",
  "description": "A fantastic music festival with multiple artists",
  "startDateUtc": "2024-07-15T10:00:00Z",
  "endDateUtc": "2024-07-15T22:00:00Z",
  "category": "Music",
  "venue": "Central Park"
}
```

**Example Response:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "Summer Music Festival 2024",
  "description": "A fantastic music festival with multiple artists",
  "startDateUtc": "2024-07-15T10:00:00Z",
  "endDateUtc": "2024-07-15T22:00:00Z",
  "category": "Music",
  "venue": "Central Park"
}
```

---

### 2.2 List Events
**GET** `/events`

**Query Parameters (all optional):**
- `dateUtc` - Filter by date (ISO 8601 format)
- `category` - Filter by category
- `venue` - Filter by venue

**Examples:**
```
GET /events
GET /events?category=Music
GET /events?venue=Central Park
GET /events?dateUtc=2024-07-15T00:00:00Z&category=Music
```

**Example Response:**
```json
[
  {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Summer Music Festival 2024",
    "description": "A fantastic music festival",
    "startDateUtc": "2024-07-15T10:00:00Z",
    "endDateUtc": "2024-07-15T22:00:00Z",
    "category": "Music",
    "venue": "Central Park"
  }
]
```

---

### 2.3 Get Event by ID
**GET** `/events/{id}`

**Replace `{id}` with the event GUID**

---

### 2.4 Update Event
**PUT** `/events/{id}`

**Request Body:**
```json
{
  "name": "Summer Music Festival 2024 - Updated",
  "description": "Updated description",
  "startDateUtc": "2024-07-15T10:00:00Z",
  "endDateUtc": "2024-07-15T22:00:00Z",
  "category": "Music",
  "venue": "Central Park"
}
```

---

### 2.5 Delete Event
**DELETE** `/events/{id}`

**Response:** `204 No Content`

---

## 3. Venues API

### 3.1 Get All Venues
**GET** `/api/venues`

**No request body needed**

**Example Response:**
```json
[
  {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Central Park",
    "location": "New York, NY",
    "capacity": 10000,
    "seatingLayoutId": null,
    "contactInfo": "contact@centralpark.com",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": null
  }
]
```

---

### 3.2 Create Venue
**POST** `/api/venues`

**Request Body:**
```json
{
  "name": "Central Park",
  "location": "New York, NY",
  "capacity": 10000,
  "seatingLayoutId": null,
  "contactInfo": "contact@centralpark.com"
}
```

**Example Response:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "Central Park",
  "location": "New York, NY",
  "capacity": 10000,
  "seatingLayoutId": null,
  "contactInfo": "contact@centralpark.com",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": null
}
```

---

### 3.3 Update Venue
**PUT** `/api/venues/{id}`

**Request Body:**
```json
{
  "name": "Central Park - Updated",
  "location": "New York, NY",
  "capacity": 15000,
  "seatingLayoutId": null,
  "contactInfo": "newcontact@centralpark.com"
}
```

---

## 4. Performers API

### 4.1 Create Performer
**POST** `/api/Performers`

**Request Body:**
```json
{
  "name": "John Doe",
  "bio": "A talented musician with 10 years of experience",
  "imageUrl": "https://example.com/johndoe.jpg",
  "performanceType": "Singer",
  "socialLinks": {
    "twitter": "https://twitter.com/johndoe",
    "instagram": "https://instagram.com/johndoe"
  }
}
```

**Example Response:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "John Doe",
  "bio": "A talented musician",
  "imageUrl": "https://example.com/johndoe.jpg",
  "performanceType": "Singer",
  "socialLinks": {
    "twitter": "https://twitter.com/johndoe",
    "instagram": "https://instagram.com/johndoe"
  },
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": null
}
```

---

### 4.2 Get All Performers
**GET** `/api/Performers`

**No request body needed**

---

### 4.3 Get Performer by ID
**GET** `/api/Performers/{id}`

**Replace `{id}` with the performer GUID**

---

### 4.4 Update Performer
**PUT** `/api/Performers/{id}`

**Request Body:**
```json
{
  "name": "John Doe - Updated",
  "bio": "Updated bio",
  "imageUrl": "https://example.com/johndoe-new.jpg",
  "performanceType": "Singer",
  "socialLinks": {
    "twitter": "https://twitter.com/johndoe",
    "facebook": "https://facebook.com/johndoe"
  }
}
```

---

### 4.5 Delete Performer
**DELETE** `/api/Performers/{id}`

**Response:** `204 No Content`

---

## 5. Sessions API

### 5.1 Create Session
**POST** `/api/events/{eventId}/sessions`

**Replace `{eventId}` with the event GUID**

**Request Body:**
```json
{
  "name": "Opening Ceremony",
  "startTime": "2024-07-15T10:00:00Z",
  "endTime": "2024-07-15T11:00:00Z",
  "performerIds": [
    "123e4567-e89b-12d3-a456-426614174000",
    "223e4567-e89b-12d3-a456-426614174001"
  ],
  "capacity": 5000,
  "seatingMapId": null
}
```

**Note:** `startTime` must be before `endTime`

**Example Response:**
```json
{
  "id": "323e4567-e89b-12d3-a456-426614174002",
  "eventId": "123e4567-e89b-12d3-a456-426614174000",
  "name": "Opening Ceremony",
  "startTime": "2024-07-15T10:00:00Z",
  "endTime": "2024-07-15T11:00:00Z",
  "performerIds": [
    "123e4567-e89b-12d3-a456-426614174000",
    "223e4567-e89b-12d3-a456-426614174001"
  ],
  "capacity": 5000,
  "seatingMapId": null,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": null
}
```

---

### 5.2 Get Sessions for Event
**GET** `/api/events/{eventId}/sessions`

**Replace `{eventId}` with the event GUID**

**Example Response:**
```json
[
  {
    "id": "323e4567-e89b-12d3-a456-426614174002",
    "eventId": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Opening Ceremony",
    "startTime": "2024-07-15T10:00:00Z",
    "endTime": "2024-07-15T11:00:00Z",
    "performerIds": ["..."],
    "capacity": 5000,
    "seatingMapId": null,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": null
  }
]
```

---

### 5.3 Update Session
**PUT** `/api/sessions/{id}`

**Request Body:**
```json
{
  "name": "Opening Ceremony - Updated",
  "startTime": "2024-07-15T10:30:00Z",
  "endTime": "2024-07-15T11:30:00Z",
  "performerIds": [
    "123e4567-e89b-12d3-a456-426614174000"
  ],
  "capacity": 6000,
  "seatingMapId": null
}
```

---

## 6. Payments API

### 6.1 Create Payment
**POST** `/api/payments`

**Request Body:**
```json
{
  "ticketId": 1,
  "userId": 1,
  "amount": 50.00,
  "method": 0
}
```

**PaymentMethod Values:**
- `0` = CreditCard
- `1` = DebitCard
- `2` = PayPal
- `3` = Stripe
- `4` = Cash

**Example Response:**
```json
{
  "message": "Payment processed successfully"
}
```

---

## 7. Auth API

### 7.1 Public Endpoint
**GET** `/api/public`

**No authentication required**

**Example Response:**
```
"Public endpoint"
```

---

### 7.2 Secure Endpoint
**GET** `/api/secure`

**Requires authentication (JWT token)**

**Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Example Response:**
```
"Authenticated user"
```

---

### 7.3 Admin Endpoint
**GET** `/api/admin`

**Requires authentication with Admin role**

**Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Example Response:**
```
"Admin only endpoint"
```

---

## 🧪 Complete Testing Workflow Example

Here's a complete workflow to test the ticket system:

### Step 1: Create a Ticket
```bash
POST http://localhost:5159/tickets
Content-Type: application/json

{
  "type": 0,
  "price": 50.00,
  "discountCode": "SAVE10",
  "reservationID": "00000000-0000-0000-0000-000000000000"
}
```
**Save the `ticketID` from the response**

### Step 2: Get the Ticket
```bash
GET http://localhost:5159/tickets/{ticketID}
```

### Step 3: Confirm the Ticket
```bash
POST http://localhost:5159/tickets/{ticketID}/confirm
```

### Step 4: Get QR Code
```bash
GET http://localhost:5159/tickets/{ticketID}/qrcode
```
**Save the `qrCode` value (or extract the ticket ID from it)**

### Step 5: Validate Ticket (Entry Point)
```bash
POST http://localhost:5159/tickets/validate
Content-Type: application/json

{
  "ticketId": "{ticketID}",
  "qrCode": null
}
```

**OR using QR Code:**
```bash
POST http://localhost:5159/tickets/validate
Content-Type: application/json

{
  "ticketId": null,
  "qrCode": "{ticketID}"
}
```

**Expected Result:** Ticket should be validated and automatically checked in (status changes to CheckedIn)

### Step 6: Try to Validate Again (Should Fail)
```bash
POST http://localhost:5159/tickets/validate
Content-Type: application/json

{
  "ticketId": "{ticketID}",
  "qrCode": null
}
```

**Expected Result:** Should return error "Ticket has already been checked in"

---

## 📝 Notes

1. **GUIDs**: All IDs are GUIDs (UUIDs) in the format: `123e4567-e89b-12d3-a456-426614174000`

2. **Dates**: All dates should be in ISO 8601 format (UTC): `2024-07-15T10:00:00Z`

3. **Status Codes:**
   - `200 OK` - Success
   - `201 Created` - Resource created
   - `204 No Content` - Success with no content
   - `400 Bad Request` - Invalid request
   - `404 Not Found` - Resource not found

4. **Swagger UI**: When the application is running, you can access Swagger UI at:
   ```
   http://localhost:5159/swagger
   ```
   This provides an interactive API documentation and testing interface.

5. **Database**: Make sure PostgreSQL is running and the connection string in `appsettings.json` is correct (password: `1904`)

---

## 🔧 Testing Tools

### Using cURL (Command Line)

**Example - Create Ticket:**
```bash
curl -X POST http://localhost:5159/tickets \
  -H "Content-Type: application/json" \
  -d '{
    "type": 0,
    "price": 50.00,
    "discountCode": "SAVE10",
    "reservationID": "00000000-0000-0000-0000-000000000000"
  }'
```

### Using PowerShell (Windows)

**Example - Create Ticket:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5159/tickets" `
  -Method POST `
  -ContentType "application/json" `
  -Body '{
    "type": 0,
    "price": 50.00,
    "discountCode": "SAVE10",
    "reservationID": "00000000-0000-0000-0000-000000000000"
  }'
```

---

## ✅ Checklist

- [ ] Backend is running on `http://localhost:5159`
- [ ] PostgreSQL is running and accessible
- [ ] Database connection is configured correctly
- [ ] Test ticket creation
- [ ] Test ticket retrieval
- [ ] Test ticket confirmation
- [ ] Test QR code generation
- [ ] Test ticket validation
- [ ] Test event creation
- [ ] Test venue creation
- [ ] Test performer creation
- [ ] Test session creation

---

**Happy Testing! 🚀**

