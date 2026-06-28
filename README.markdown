# ShopPay

ShopPay is a modern E-Commerce web application built using **ASP.NET Core Web API**, **Angular**, **PostgreSQL**, and **Stripe**. The project follows secure authentication practices using **JWT with HttpOnly Cookies**, supports complete user and address management, and is being extended with secure payment processing using Stripe Checkout.

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
- Stripe Checkout (In Progress)

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

- Stripe Checkout Session
- Secure Payment Flow
- Payment Success Page
- Payment Cancel Page
- Stripe Webhooks (Planned)
- Payment Status Tracking
- Invoice Generation (Planned)

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

- Stripe Checkout
- Checkout Session Creation
- Payment Success Flow
- Payment Cancellation Flow
- Order Confirmation
- Payment Persistence

---

## Planned Features

- Stripe Webhooks
- Billing Module
- Invoice Generation
- Subscription Support
- Admin Dashboard
- Product Search
- Product Categories
- Wishlist
- Order Tracking
- Email Notifications

---

# Future Database Schema

Current Tables

- Users
- Products
- Addresses
- Orders
- OrderItems

Upcoming Tables

- StripeCustomers
- Payments
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
- Invoice Generation
- Subscription Billing
- Email Notifications
- Admin Dashboard
- Product Reviews
- Coupon System
- Inventory Management
- Docker Deployment
- Azure Deployment

---

# License

This project is built for learning modern Full Stack Development using **ASP.NET Core**, **Angular**, **PostgreSQL**, **JWT Authentication**, and **Stripe Payment Integration**.
