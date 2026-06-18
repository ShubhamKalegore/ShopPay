# E-Commerce Payment Module Database Schema

This PL/SQL (Oracle) database schema supports an e-commerce payment module built with ASP.NET Core and Angular, integrating Stripe for payment processing. It consists of 9 tables to manage users, products, orders, payments, subscriptions, invoices, and addresses.

## Tables and Their Purposes

1. **Users**
   - **Purpose**: Stores user information for authentication and profile management.
   - **Fields**:
     - `user_id`: Auto-generated ID (Primary Key).
     - `email`: Unique user email.
     - `password_hash`: Hashed password for security.
     - `first_name`, `last_name`: User’s name.
     - `created_at`, `updated_at`: Timestamps for record tracking.

2. **Products**
   - **Purpose**: Manages e-commerce product catalog.
   - **Fields**:
     - `product_id`: Auto-generated ID (Primary Key).
     - `name`: Product name.
     - `description`: Product details (CLOB).
     - `price`: Product price (non-negative).
     - `stock_quantity`: Available stock (non-negative).
     - `created_at`, `updated_at`: Timestamps.

3. **Stripe_Customers**
   - **Purpose**: Links users to Stripe customer IDs for payment processing.
   - **Fields**:
     - `stripe_customer_id`: Stripe customer ID (Primary Key).
     - `user_id`: References `Users(user_id)` (Foreign Key).
     - `created_at`: Timestamp.

4. **Addresses**
   - **Purpose**: Stores multiple shipping/billing addresses per user.
   - **Fields**:
     - `address_id`: Auto-generated ID (Primary Key).
     - `user_id`: References `Users(user_id)` (Foreign Key).
     - `address_type`: 'SHIPPING' or 'BILLING'.
     - `street`, `city`, `postal_code`, `country`: Address details (required).
     - `state`: Optional state/province.
     - `is_default`: 'Y' or 'N' for default address.
     - `created_at`, `updated_at`: Timestamps.

5. **Orders**
   - **Purpose**: Tracks customer orders, including address and payment details.
   - **Fields**:
     - `order_id`: Auto-generated ID (Primary Key).
     - `user_id`: References `Users(user_id)` (Foreign Key).
     - `shipping_address_id`, `billing_address_id`: References `Addresses(address_id)` (Foreign Key, nullable).
     - `total_amount`: Order total (non-negative).
     - `order_status`: 'PENDING', 'COMPLETED', or 'CANCELLED'.
     - `created_at`, `updated_at`: Timestamps.

6. **Order_Items**
   - **Purpose**: Links orders to products (many-to-many).
   - **Fields**:
     - `order_item_id`: Auto-generated ID (Primary Key).
     - `order_id`: References `Orders(order_id)` (Foreign Key).
     - `product_id`: References `Products(product_id)` (Foreign Key).
     - `quantity`: Number of items (positive).
     - `unit_price`: Price per item (non-negative).

7. **Billing**
   - **Purpose**: Records payment transactions via Stripe.
   - **Fields**:
     - `billing_id`: Auto-generated ID (Primary Key).
     - `stripe_payment_intent_id`: Unique Stripe payment intent ID.
     - `user_id`: References `Users(user_id)` (Foreign Key).
     - `amount`: Payment amount (non-negative).
     - `currency`: Currency code (default 'USD').
     - `payment_status`: 'PENDING', 'SUCCEEDED', or 'FAILED'.
     - `created_at`: Timestamp.

8. **Subscriptions**
   - **Purpose**: Manages recurring subscription plans.
   - **Fields**:
     - `subscription_id`: Auto-generated ID (Primary Key).
     - `stripe_subscription_id`: Unique Stripe subscription ID.
     - `user_id`: References `Users(user_id)` (Foreign Key).
     - `plan_name`: Subscription plan name.
     - `status`: 'ACTIVE', 'CANCELLED', or 'PAST_DUE'.
     - `start_date`, `end_date`: Subscription period.
     - `created_at`: Timestamp.

9. **Invoices**
   - **Purpose**: Tracks invoices for payments or subscriptions.
   - **Fields**:
     - `invoice_id`: Auto-generated ID (Primary Key).
     - `stripe_invoice_id`: Unique Stripe invoice ID.
     - `user_id`: References `Users(user_id)` (Foreign Key).
     - `billing_id`: References `Billing(billing_id)` (Foreign Key, nullable).
     - `subscription_id`: References `Subscriptions(subscription_id)` (Foreign Key, nullable).
     - `amount_due`, `amount_paid`: Invoice amounts (non-negative).
     - `invoice_status`: 'OPEN', 'PAID', or 'VOID'.
     - `created_at`: Timestamp.
    
# JWT Authentication with HttpOnly Cookies

This project implements secure JWT authentication using HttpOnly cookies and refresh tokens across Angular and ASP.NET Core.

## Features

### Backend (.NET)

* User Registration
* User Login
* JWT Access Token Generation
* Refresh Token Generation
* Refresh Token Storage in Database
* Token Refresh Endpoint
* Secure Logout
* JWT Bearer Authentication
* Cookie-Based Authentication Middleware

### Frontend (Angular)

* Authentication Service
* HTTP Interceptor
* Route Guard Protection
* Automatic Token Refresh Handling
* Credential-Based Requests (`withCredentials`)
* Secure Logout Integration

## API Endpoints

```http
POST /api/users/register
POST /api/users/login
POST /api/users/refresh-token
POST /api/users/logout
```

## Security Features

* HttpOnly Cookies
* Secure Cookies
* SameSite Cookie Policy
* Refresh Token Validation
* Automatic Token Renewal
* Cookie-Based JWT Authentication




## Notes
- **Total Tables**: 9
- **Cart Handling**: Cart data is managed on the frontend using Angular’s `CartService` and `localStorage`, eliminating the need for cart-related tables.
- **Indexes**: Added on foreign keys (e.g., `user_id`, `address_id`) for query performance.
- **Constraints**: Primary keys, foreign keys, and check constraints ensure data integrity.
- **Usage**: Execute the schema in Oracle using SQL Developer or DBeaver. Integrate with ASP.NET Core (using Entity Framework Core with Oracle provider) and Angular for full functionality.

For implementation details, refer to the project’s setup guides.
