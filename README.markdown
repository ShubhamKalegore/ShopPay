# ShopPay

ShopPay is a modern E-Commerce web application built using **ASP.NET Core Web API**, **Angular**, **PostgreSQL**, and **Stripe**. The project follows secure authentication practices using **JWT with HttpOnly Cookies**, supports complete user and address management and integrates secure payment processing using Stripe Payment Intents. The application follows a modern payment architecture and is being extended with payment verification, webhooks, billing, invoices, and subscriptions.

---

# Tech Stack

## Backend
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Refresh Tokens
- HttpOnly Cookies
- AutoMapper

## Frontend
- Angular
- TypeScript
- RxJS
- Angular Router
- HTTP Interceptors
- Reactive Forms

## Database
- PostgreSQL

## Payment Gateway
Stripe Payment Intents
Stripe Elements (In Progress)

---

# Features

## Authentication & Security

- User Registration
- User Login
- JWT Authentication
- HttpOnly Cookie Authentication
- Refresh Token Support
- Refresh Token Database Storage
- Automatic Token Refresh
- Route Protection
- Secure Logout
- Authentication Validation Endpoint

---

## Product Module

- View Products
- Product Details
- Responsive Product Cards
- Product Images
- Product Price Display

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
- Fetch User Addresses
- Default Address Selection
- Shipping Address Management

---

## Checkout Module

- Checkout Summary
- Selected Shipping Address
- Order Preview

---

## Order Module

- Create Order
- Order Items
- Order Confirmation
- Order History (Planned)

---

## Payment Module (Work In Progress)

- Stripe Payment Intents
- Secure Card Payment Flow
- Client Secret Generation
- Payment Verification
- Stripe Elements Integration (In Progress)
- Payment Status Tracking
- Billing Module (Planned)
- Invoice Generation (Planned)
- Stripe Webhooks (Planned)
- Subscription Support (Planned)

---

# Database Entities

The current backend consists of the following entities:

- Users
- Products
- Addresses
- Orders
- OrderItems

Future entities:

- Payments
- StripeCustomers
- Billing
- Invoices
- Subscriptions

---

# Authentication Flow

```
User Login
      │
      ▼
ASP.NET Core validates credentials
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

# API Endpoints

## Authentication

```
POST   /api/users/register
POST   /api/users/login
POST   /api/users/refresh-token
POST   /api/users/logout
GET    /api/users/validate
```

---

## Products

```
GET    /api/products
GET    /api/products/{id}
```

---

## Addresses

```
GET    /api/addresses/user/{userId}
POST   /api/addresses
PUT    /api/addresses/{id}
DELETE /api/addresses/{id}
```

---

## Orders

```
POST   /api/orders
GET    /api/orders/{id}
```

## Stripe

POST   /api/stripe/create-payment-intent
GET    /api/stripe/verify/{paymentIntentId}

---

# Security Features

- JWT Authentication
- HttpOnly Cookies
- Secure Cookies
- SameSite Cookie Policy
- Refresh Token Rotation
- Cookie-Based Authentication
- Route Guards
- HTTP Interceptors
- Unauthorized Request Handling

---

# Project Structure

```
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

- User Authentication
- JWT Authentication
- Refresh Tokens
- HttpOnly Cookies
- Angular Authentication Service
- HTTP Interceptor
- Route Guards
- User Registration
- User Login
- Secure Logout
- Product Listing
- Product Details
- Address CRUD
- Checkout UI
- Order Entity
- OrderItem Entity

---

## In Progress

- Stripe Payment Intents
- Stripe Elements Integration
- Payment Verification
- Payment Success Flow
- Payment Cancellation Flow
- Order Confirmation
- Payment Persistence

---

## Planned Features

- Stripe Webhooks
- Billing Module
- Invoice Generation
- Subscription Management
- Customer Portal
- Order History
- Order Tracking
- Product Search
- Product Categories
- Wishlist
- Email Notifications
- Admin Dashboard
---

# Future Database Schema

Current Tables

- Users
- Products
- Addresses
- Orders
- OrderItems

Upcoming Tables

- Payments
- StripeCustomers
- Billing
- Invoices
- Subscriptions

---

# Running the Project

## Backend

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Backend runs on:

```
https://localhost:5001
```

---

## Frontend

```bash
npm install
ng serve
```

Frontend runs on:

```
http://localhost:4200
```

---

# Future Enhancements

- Stripe Webhooks
- Billing & Invoicing
- Subscription Billing
- Customer Portal
- Order Tracking
- Email Notifications
- Coupon System
- Product Reviews
- Inventory Management
- Docker Deployment
- Azure Deployment

---

# License

This project is built for learning modern Full Stack Development using **ASP.NET Core**, **Angular**, **PostgreSQL**, **JWT Authentication**, and **Stripe Payment Integration**.
