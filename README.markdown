# ShopPay

ShopPay is a modern Full Stack E-Commerce application built using **ASP.NET Core Web API**, **Angular**, **PostgreSQL**, and **Stripe**. The project demonstrates secure authentication, order management, payment processing, and a scalable layered architecture following industry best practices.

The application uses **JWT Authentication with HttpOnly Cookies**, **Refresh Tokens**, **Entity Framework Core**, and **Stripe Payment Intents** to provide a secure checkout experience.

---

# Tech Stack

## Backend

- ASP.NET Core Web API (.NET)
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Refresh Tokens
- HttpOnly Cookies
- AutoMapper
- Repository Pattern
- Dependency Injection

---

## Frontend

- Angular
- TypeScript
- RxJS
- Angular Router
- Reactive Forms
- HTTP Interceptors
- Route Guards

---

## Database

- PostgreSQL

---

## Payment Gateway

- Stripe Payment Intents
- Stripe Elements
- Payment Verification

---

# Features

## Authentication & Security

- User Registration
- User Login
- JWT Authentication
- Refresh Tokens
- Refresh Token Rotation
- HttpOnly Cookie Authentication
- Cookie-Based Authorization
- Automatic Token Refresh
- Authentication Validation Endpoint
- Secure Logout
- Protected Routes
- Angular HTTP Interceptor

---

## Product Module

- View Products
- Product Details
- Add Product
- Update Product
- Product Images
- Responsive Product Cards

---

## Cart Module

- Add to Cart
- Remove from Cart
- Update Quantity
- Cart Total Calculation
- Local Storage Persistence

---

## Address Module

- Add Address
- Update Address
- Delete Address
- Get Address By User
- Default Shipping Address

---

## Checkout Module

- Checkout Summary
- Shipping Address Selection
- Order Preview
- Secure Payment Flow

---

## Order Module

- Create Order
- Order Items
- Order Summary
- Order Confirmation

---

## Payment Module

- Stripe Payment Intents
- Secure Card Payments
- Stripe Elements Integration
- Client Secret Generation
- Payment Confirmation
- Payment Verification
- Payment Success Page
- Payment Failed Page
- Payment Status Tracking

---

# Database Entities

Current Entities

- Users
- Products
- Addresses
- Orders
- OrderItems

Future Entities

- Payments
- Billing
- Invoices
- Subscriptions

---

# Authentication Flow

```text
User Login
      │
      ▼
Credentials Validated
      │
      ▼
JWT Access Token Generated
      │
      ▼
Refresh Token Generated
      │
      ▼
Stored in Database
      │
      ▼
Access Token stored in HttpOnly Cookie
      │
      ▼
Angular sends requests using withCredentials
      │
      ▼
HTTP Interceptor
      │
      ▼
Protected APIs
```

---

# Payment Flow

```text
User Checkout
      │
      ▼
Create Order
      │
      ▼
Create Stripe Payment Intent
      │
      ▼
Return Client Secret
      │
      ▼
Angular Stripe Elements
      │
      ▼
Confirm Payment
      │
      ▼
Stripe
      │
      ▼
Payment Success / Failed
      │
      ▼
Verify Payment Intent
      │
      ▼
Display Final Payment Status
```

---

# API Endpoints

## Authentication

```http
POST   /api/users/register
POST   /api/users/login
POST   /api/users/refresh-token
POST   /api/users/logout
GET    /api/users/validate
```

---

## Products

```http
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

---

## Addresses

```http
GET    /api/addresses/user/{userId}
POST   /api/addresses
PUT    /api/addresses/{id}
DELETE /api/addresses/{id}
```

---

## Orders

```http
POST   /api/orders
GET    /api/orders/{id}
GET    /api/orders/user/{userId}
```

---

## Stripe

```http
POST   /api/stripe/create-payment-intent
GET    /api/stripe/verify/{paymentIntentId}
```

---

# Security Features

- JWT Authentication
- HttpOnly Cookies
- Refresh Tokens
- Refresh Token Rotation
- Secure Cookies
- SameSite Cookie Policy
- Route Guards
- Angular HTTP Interceptors
- Cookie-Based Authentication
- Unauthorized Request Handling

---

# Project Structure

```text
ShopPay

Backend
│
├── Controllers
├── Services
├── Repositories
├── DTOs
├── Models
├── Data
├── Migrations
├── Middleware
└── Program.cs

Frontend
│
├── Components
├── Pages
├── Services
├── Guards
├── Interceptors
├── Models
├── Shared
└── Routes
```

---

# Current Project Status

## Completed

### Authentication

- User Registration
- User Login
- JWT Authentication
- Refresh Tokens
- HttpOnly Cookies
- Cookie-Based Authentication
- Authentication Validation
- Route Guards
- Angular HTTP Interceptor

### Products

- Product CRUD
- Product Cards
- Product Details
- Product Update

### Cart

- Cart Management
- Quantity Updates
- Local Storage

### Address

- Address CRUD
- User Address Retrieval
- Default Address

### Orders

- Order Creation
- Order Items
- Checkout Integration

### Payments

- Stripe Payment Intents
- Stripe Elements
- Payment Confirmation
- Payment Verification
- Success Page
- Failed Page

---

## In Progress

- Payment Persistence
- Order History
- Payment History

---

## Planned Features

- Stripe Webhooks
- Billing Module
- Invoice Generation
- Subscription Management
- Customer Portal
- Order Tracking
- Product Search
- Categories
- Wishlist
- Coupons
- Email Notifications
- Inventory Management
- Admin Dashboard
- Azure Deployment
- Docker Deployment

---

# Running the Project

## Backend

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Backend

```text
https://localhost:5001
```

---

## Frontend

```bash
npm install
ng serve
```

Frontend

```text
http://localhost:4200
```

---

# Future Roadmap

- Stripe Webhooks
- Payment Persistence
- Billing & Invoicing
- Subscription Billing
- Customer Portal
- Order Tracking
- Product Reviews
- Coupons
- Inventory Management
- Docker
- Azure Deployment
- CI/CD Pipeline
- Unit Testing
- Integration Testing

---

# License

This project is built for learning and demonstrating modern Full Stack development using **ASP.NET Core Web API**, **Angular**, **PostgreSQL**, **JWT Authentication**, and **Stripe Payment Integration**, following industry-standard architecture and secure coding practices.



## Stripe Webhook Integration

Implemented secure server-side payment confirmation using **Stripe Webhooks** to ensure payment status is updated based on Stripe events rather than relying only on frontend verification.

### Features
- Integrated Stripe Webhook endpoint in ASP.NET Core.
- Verified webhook signatures using the Stripe Webhook Secret.
- Processed `payment_intent.succeeded` events.
- Updated order/payment status only after receiving a valid webhook event from Stripe.
- Added support for local webhook testing using the Stripe CLI.
- Configured Stripe Secret Key, Publishable Key, and Webhook Secret through application configuration.
- Improved payment reliability by preventing frontend-only payment confirmation.

### Local Testing

1. Start the ASP.NET Core API.
2. Start the Stripe CLI and forward events:

```bash
stripe listen --forward-to https://localhost:<port>/api/stripe/webhook
```

3. Copy the generated webhook signing secret (`whsec_...`) into the application configuration.
4. Complete a test payment using Stripe test cards.
5. Verify that the webhook is received and the payment status is updated successfully.

### Stripe Events Handled

- `payment_intent.succeeded`

### Benefits

- Secure server-side payment verification.
- Prevents client-side payment spoofing.
- Reliable payment status synchronization.
- Production-ready payment confirmation flow.
