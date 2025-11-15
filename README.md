# Team members

| Full Name           | ID         |
|-------------------- |------------|
| Abubeker Juhar      | UGR/3857/15|
| Blen Berhanu        | UGR/8682/15|
| Eyerusalem Geremew  | UGR/2359/15|
| Nahom Zenebe        | UGR/0216/15|
| Samuel Teferi       | UGR/1535/15|

---

# Project Proposal: Smart Event & Ticket Management Platform

## 1. Problem Description
The entertainment industry especially concerts, festivals, theater shows, and large public events face a persistent set of operational challenges related to:

- Manual ticketing processes leading to fraud & duplicate entries
- Poor coordination between event scheduling, venue capacity, and ticket allocation
- Lack of real-time analytics for crowd management
- Difficulty predicting attendance, ticket demand, or overbooking
- Limited ways to manage performer bookings, venue scheduling, and attendee registration in one place

## 2. Subdomains (DDD)

### 1) Core Domain

#### 1.1 Event Ticketing & Seat Allocation
- Event listings
- Dynamic pricing
- Seat selection (cinemas, theaters, stadiums)
- VIP zones, premium seats, family seats
- Real-time seat locking to avoid double-booking

#### 1.2 Reservation & Purchase Flow
- Add-to-cart, payment flow
- Reservation timeouts
- Multi-tier pricing (regular, VIP, VVIP)

#### 1.3 Event Operations & Access Control
- QR code generation
- Fast check-in & scanning

#### 1.4 Customer Experience & Engagement
- Personalized event recommendations
- Like and dislike event

### 2) Supporting Subdomains

#### 2.1 Event Management & Scheduling
- Create/manage events
- Manage venue schedules

#### 2.2 Event Content Management
- Uploads image, agenda, metadata

### 3) Generic Subdomains

#### 3.1 Payments & Billing
- Payment gateway integration (Stripe, PayPal)
- Card processing
- Receipts and invoices

#### 3.2 Authentication & User Accounts
- Login/Signup
- Social login
- Password reset
- Role-based access

#### 3.3 Notifications
- Push notifications

#### 3.4 Reporting & Analytics Infrastructure
- Sales trends
- User activity
- Revenue dashboards
- Attendance analytics

## 3. Bounded Contexts
- **Event Planning Context** – manages creation and scheduling of events and sessions.
- **Ticketing Context** – handles ticket creation, pricing, reservations, and validation.
- **Customer Experience & Access Control Context** – manages attendee check-in, AI-driven recommendations, and engagement features.

## 4. Bounded Contexts and Their Responsibility

We define three core bounded contexts:

### 1) Event Planning Context
**Responsibility:**
- Create events, sessions, schedules
- Assign venues and performers
- Define seating categories (VIP, General, etc.)
- Manage capacity rules

### 2) Ticketing Context
**Responsibility:**
- Ticket creation, pricing, discounts
- Ticket reservation 
- QR/Barcode generation
- Ticket validation and check-in logic

### 3) Customer Experience & Access Control
**Responsibility:**
- Gate access validation (using QR code)
- Event recommendations (AI-driven)
- Like/dislike and save events to favorites

## 5. User Stories (Cross-Context, with Domain Events)

### User Story 1 — Event Creation & Ticket Generation
**As an event organizer**, I want to create an event and automatically generate ticket templates  
**So that** I don’t manually configure ticket settings.

**Contexts Involved**
- Event Planning
- Ticketing

**Domain Events**
- EventCreated (published by Event Planning)
- TicketTemplatesGenerated (published by Ticketing)

**Event Flow**
1. Event Planning publishes EventCreated.
2. Ticketing listens and generates ticket categories & templates.
3. Ticketing publishes TicketTemplatesGenerated.

**Eventual Consistency**
- Asynchronous messaging (RabbitMQ/Kafka)
- Ticketing updates may arrive slightly later, but UI will show “Generating ticket templates…”

### User Story 2 — Attendee Purchases Ticket & Gets Access Token
**As an attendee**, I want to purchase a ticket and receive a digital access pass  
**So that** I can enter the venue easily.

**Contexts Involved**
- Ticketing
- Attendance & Access Control
- Payments

**Domain Events**
- TicketPurchased
- AccessPassGenerated

**Event Flow**
1. Ticketing processes purchase → TicketPurchased.
2. Access Control listens → generates a QR pass.
3. Access Control publishes AccessPassGenerated.

**Eventual Consistency**
- Payment confirmation triggers async ticket issuing
- Access pass generation happens after the event but within seconds

### User Story 3 — Personalized Event Recommendations
**Story:** When User Interaction, the system generates AI-driven event recommendations tailored to their interests.

**Contexts Involved**
- Customer Experience & Event Recommendations – generates recommendations.
- Event Management – provides event metadata for AI analysis.

**Domain Events**
- Published: RecommendedEventsGenerated – contains personalized event suggestions.
- Subscribed: UserProfileUpdated – triggers recommendation generation.

**Eventual Consistency**
- Recommendations are generated asynchronously using a message broker (RabbitMQ/Kafka).
- Even if the recommendation service is temporarily offline, the event will be processed once connectivity is restored.

## AI-Driven Feature
**AI-Driven Feature:** Personalized Event Recommendation System

**Description:** The system uses content-based filtering to recommend events by analyzing the attributes of events a user has interacted with. It considers likes, dislikes, saved events, past attendance, and event metadata (category, venue, performers) to suggest events similar to those the user has shown interest in.

**Key Features**
- Recommend events with similar attributes to past interactions (e.g., same category, venue, or performer).
- Personalize notifications for upcoming events matching user preferences.
- Continuously adapt recommendations as user behavior evolves.
