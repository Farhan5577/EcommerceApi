# 🛒 Ecommerce API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=c-sharp)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16.0-4169E1?style=flat-square&logo=postgresql)
![EF Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=flat-square)
![JWT](https://img.shields.io/badge/JWT-Authentication-black?style=flat-square&logo=json-web-tokens)
![Cloudinary](https://img.shields.io/badge/Cloudinary-Image%20Hosting-3B5998?style=flat-square&logo=cloudinary)

A robust RESTful API backend for a multi-vendor e-commerce platform that enables users to manage stores, product catalogs, automated cloud image uploads, and ACID-compliant multi-item checkouts.

---

## 📌 About The Project

This project was built using **ASP.NET Core Web API** following the layered **Controller-Service Architecture Pattern**. The core focus is delivering high performance, strict database transaction integrity, and secure JWT-based identity management.

### Key Features
- **Authentication & Security:** User registration, login, JWT token generation, and claims-based authorization.
- **Store Management (Multi-Vendor):** P2P store creation for registered sellers.
- **Product Catalog:** Full CRUD operations for products with direct **Cloudinary** cloud image uploading.
- **Order & Checkout System:** Supports purchasing items from multiple stores in a single checkout operation, complete with automatic stock deduction and database transaction safety (rollback on failure).
- **Merchant Order Processing:** Store owners can update order statuses specifically for items sold from their store.

---

## 🛠️ Tech Stack

- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Database:** PostgreSQL (Neon Serverless DB)
- **ORM:** Entity Framework Core 8
- **Authentication:** JWT (JSON Web Token)
- **Storage:** Cloudinary SDK (`CloudinaryDotNet`)
- **API Documentation:** Swagger / OpenAPI

---

## 🔗 API Endpoints

### 🔑 Authentication
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :---: |
| `POST` | `/api/Auth/register` | Register a new user account | ❌ |
| `POST` | `/api/Auth/login` | Authenticate user & receive JWT token | ❌ |

### 🏪 Store
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :---: |
| `POST` | `/api/Store` | Create a new store | 🔒 |
| `GET` | `/api/Store/my-store` | Retrieve current user's store details | 🔒 |

### 📦 Products
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :---: |
| `GET` | `/api/Product` | Fetch all products | ❌ |
| `GET` | `/api/Product/{id}` | Fetch product details by ID | ❌ |
| `POST` | `/api/Product` | Create product with image upload to Cloudinary | 🔒 |
| `PUT` | `/api/Product/{id}` | Update product information | 🔒 |
| `DELETE` | `/api/Product/{id}` | Delete a product | 🔒 |

### 🛍️ Orders
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :---: |
| `POST` | `/api/Order` | Perform multi-item order checkout | 🔒 |
| `GET` | `/api/Order/my-orders` | Fetch current buyer's order history | 🔒 |
| `GET` | `/api/Order/{id}` | Get specific order details by ID | 🔒 |
| `PUT` | `/api/Order/{id}/status` | Update order status (Store Owners only) | 🔒 |

---

## 🚀 Getting Started

### Prerequisites

1. [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. PostgreSQL Database
3. [Cloudinary](https://cloudinary.com/) Account (For media uploads)

### Environment Setup

Create or update the `appsettings.json` file in the root directory:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=YOUR_POSTGRES_HOST;Database=YOUR_DB;Username=YOUR_USER;Password=YOUR_PASSWORD;SSL Mode=Require;"
  },
  "Jwt": {
    "Issuer": "EcommerceApi",
    "Audience": "EcommerceApiUser",
    "Key": "YOUR_SUPER_SECRET_KEY_MINIMUM_32_CHARACTERS"
  },
  "CloudinaryOptions": {
    "CloudName": "YOUR_CLOUD_NAME",
    "ApiKey": "YOUR_API_KEY",
    "ApiSecret": "YOUR_API_SECRET"
  }
}
```
Running the Project
Clone the repository:
```
git clone https://github.com/Farhan5577/EcommerceApi
cd EcommerceApi
dotnet restore
```
Apply Database Migrations:
```
dotnet af database update
```
Run Project:
```
dotnet run
```
📄 License
This project is open-source and available for educational and portfolio purposes.
