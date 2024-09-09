# E-commerce API Schema

- [Auth](#auth)
- [Users](#users)
- [Categories](#categories)
- [Cart](#cart)
- [Orders](#orders)
- [Products](#products)
- [Reviews](#reviews)

## API Description

### Default Permissions
- Authenticated users only

### Request Headers 

#### For protected routes
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Authorization` | `Bearer {jwt_token}`          |

#### For requests with bodies (if not otherwise specified)
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `application/json`          |

---

## Auth

### Route Group Permissions
- Anonymous users only

### Endpoints

#### Register a User

```http
POST /api/v1/auth/register
```

##### Request Body
| Field      | Type     | Required |
| :--------- | :------- | :------- |
| `email`    | `string` | Yes      |
| `firstName`| `string` | Yes      |
| `lastName` | `string` | Yes      |
| `password` | `string` | Yes      |
| `avatar`   | `file` | No       |

##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |

##### Sample Request (without avatar)
```json
{
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "password": "test123"
}
```

##### Sample Response (201)
```json
{
  "message": "User registered successfully",
  "userId": 2
}
```

##### Status Codes
- 201 Created (User created successfully)
- 400 Bad Request (Invalid input)
- 409 Conflict (Email already exists)

---

#### Login

```http
POST /api/v1/auth/login
```

##### Request Body
| Field      | Type     | Required |
| :--------- | :------- | :------- |
| `email`    | `string` | Yes      |
| `password` | `string` | Yes      |

##### Sample Request
```json
{
  "email": "john.doe@example.com",
  "password": "test123"
}
```

##### Sample Response (200)
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

##### Status Codes
- 200 OK (Successfully logged in)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid credentials)

---

## Users

### Route Group Permissions
- Admin or resource owner.

### Endpoints

#### Get All Users

```http
GET /api/v1/users?page={page}&limit={limit}&role={role}
```

##### Endpoint Permissions
- Admin only

##### Request Body
No request body required

##### Query Parameters
| Parameter | Description              |
| :-------- | :----------------------- |
| `page`    | Page number for pagination |
| `limit`   | Number of users per page |
| `role`    | Filter users by role      |

##### Sample Response (200)
```json
{  "totalItems": 1,
   "itemsPerPage": 10,
   "currentPage": 1,
   "items": [
    {
      "userId": 1,
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "role": "User",
      "avatar": "https://example.com/avatars/johndoe.jpg"
    },
    {
      "userId": 2,
      "email": "jane.smith@example.com",
      "firstName": "Jane",
      "lastName": "Smith",
      "role": "User",
      "avatar": null
    }
  ]
}
```

##### Status Codes
- 200 OK (Successfully retrieved users)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)

---

#### Get a User by ID

```http
GET /api/v1/users/{userId}
```

##### Request Body
No request body required

##### Sample Response (200)
```json
{
  "userId": 1,
  "email": "jane.doe@example.com",
  "firstName": "Jane",
  "lastName": "Doe",
  "role": "User",
  "avatar": "https://example.com/avatars/janedoe.jpg"
}
```

##### Status Codes
- 200 OK (Successfully retrieved user information)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (User not found)

---

#### Update or Create a User

```http
PUT /api/v1/users/{userId}
```

##### Request Body
| Field      | Type     | Required |
| :--------- | :------- | :------- |
| `email`    | `string` | Yes      |
| `firstName`| `string` | Yes      |
| `lastName` | `string` | Yes      |
| `password` | `string` | Yes      |
| `avatar`   | `file` | No       |

##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |

##### Sample Request
```json
{
  "email": "jane.updated@example.com",
  "firstName": "Jane",
  "lastName": "UpdatedDoe",
  "password": "new_password",
  "role": "User",
  "avatar": "https://example.com/avatars/janedoe.jpg"
}
```

##### Sample Response (200, 201)
```json
{
  "userId": 1,
  "email": "jane.updated@example.com",
  "firstName": "Jane",
  "lastName": "UpdatedDoe",
  "role": "User",
  "avatar": "https://example.com/avatars/janedoe.jpg"
}
```

##### Status Codes
- 200 OK (Successfully updated user)
- 201 Created (User created successfully)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)

---

#### Partially Update a User

```http
PATCH /api/v1/users/{userId}
```

##### Request Body
| Field      | Type     | Required |
| :--------- | :------- | :------- |
| `email`    | `string` | No       |
| `firstName`| `string` | No       |
| `lastName` | `string` | No       |
| `password` | `string` | No       |
| `avatar`   | `file` | No       |

##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |

##### Sample Request
```json
{
  "email": "jane.updated@example.com"
}
```

##### Sample Response (200)
```json
{
  "userId": 1,
  "email": "jane.updated@example.com",
  "firstName": "Jane",
  "lastName": "Doe",
  "role": "User",
  "avatar": "https://example.com/avatars/janedoe.jpg"
}
```

##### Status Codes
- 200 OK (Successfully updated user)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (User not found)

---

#### Delete a User

```http
DELETE /api/v1/users/{userId}
```

##### Request Body
No request body required

##### Sample Response (204)
- No content (User deleted successfully)

##### Status Codes
- 204 No Content (User deleted successfully)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (User not found)

---

## Categories

### Route Group Permissions
- Admin only

### Endpoints

#### Create a Category

```http
POST /api/v1/categories
```

##### Request Body
| Field             | Type      | Required |
| :---------------- | :-------- | :------- |
| `name`            | `string`  | Yes      |
| `categoryImage`   | `file`  | Yes      |
| `parentCategoryId`| `integer` | No       |

##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |

##### Sample Request
```json
{
  "name": "Fashion",
  "categoryImage": "media/0bbdac35-ac83-4569-8ced-710bd9515c7e.webp",
  "parentCategoryId": null
}
```

##### Sample Response (201)
```json
{
  "categoryId": 1,
  "name": "Fashion",
  "categoryImage": "media/0bbdac35-ac83-4569-8ced-710bd9515c7e.webp",
  "parentCategoryId": null
}
```

##### Status Codes
- 201 Created (Category created successfully)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 409 Conflict (Category already exists)

---

#### List All Categories

```http
GET /api/v1/categories?page={page}&limit={limit}&parentCategoryId={parentCategoryId}
```

##### Endpoint Permissions
- All users

##### Query Parameters
| Parameter         | Description                         |
| :---------------- | :---------------------------------- |
| `page`    | Page number for pagination |
| `limit`   | Number of users per page |
| `parentCategoryId` | Filter categories by parent category |

##### Request Body
No request body required

##### Sample Response (200)
```json
{
  "totalItems": 1,
  "itemsPerPage": 10,
  "currentPage": 1,
  "items": [
    {
      "categoryId": 1,
      "name": "Fashion",
      "categoryImage": "media/0bbdac35-ac83-4569-8ced-710bd9515c7e.webp",
      "parentCategoryId": null
    },
    {
      "categoryId": 2,
      "name": "Electronics",
      "categoryImage": "media/5c2c2983-0f99-4421-b0de-13824ff95e2c.webp",
      "parentCategoryId": null
    }
  ]
}
```

##### Status Codes
- 200 OK (Successfully retrieved categories)
- 401 Unauthorized (Invalid or missing token)

---

#### Get Category by ID

```http
GET /api/v1/categories/{categoryId}
```
##### Endpoint Permissions
- All users

##### Request Body
No request body required

##### Sample Response (200)
```json
{
  "categoryId": 1,
  "name": "Fashion",
  "categoryImage": "media/0bbdac35-ac83-4569-8ced-710bd9515c7e.webp",
  "parentCategoryId": null
}
```

##### Status Codes
- 200 OK (Successfully retrieved category information)
- 401 Unauthorized (Invalid or missing token)
- 404 Not Found (Category not found)

---

#### Update or Create a Category

```http
PUT /api/v1/categories/{categoryId}
```

##### Request Body
| Field             | Type      | Required |
| :---------------- | :-------- | :------- |
| `name`            | `string`  | Yes      |
| `categoryImage`   | `file`  | Yes      |
| `parentCategoryId`| `integer` | No       |

##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |

##### Sample Request
```json
{
  "name": "Updated Fashion",
  "categoryImage": "media/newimage.webp",
  "parentCategoryId": null
}
```

##### Sample Response (200, 201)
```json
{
  "categoryId": 1,
  "name": "Updated Fashion",
  "categoryImage": "media/newimage.webp",
  "parentCategoryId": null
}
```

##### Status Codes
- 200 OK (Successfully updated category)
- 201 Created (Category created successfully)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 409 Conflict (Category already exists)

---

#### Partially Update a Category

```http
PATCH /api/v1/categories/{categoryId}
```

##### Request Body
| Field             | Type      | Required |
| :---------------- | :-------- | :------- |
| `name`            | `string`  | No       |
| `categoryImage`   | `file`  | No       |
| `parentCategoryId`| `integer` | No       |

##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |

##### Sample Request
```json
{
  "name": "Updated Fashion"
}
```

##### Sample Response (200)
```json
{
  "categoryId": 1,
  "name": "Updated Fashion",
  "categoryImage": "media/0bbdac35-ac83-4569-8ced-710bd9515c7e.webp",
  "parentCategoryId": null
}
```

##### Status Codes
- 200 OK (Successfully updated category)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Category not found)

---

#### Delete a Category

```http
DELETE /api/v1/categories/{categoryId}
```

##### Request Body
No request body required

##### Sample Response (204)
- No content (Category deleted successfully)

##### Status Codes
- 204 No Content (Category deleted successfully)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Category not found)

---

## Cart

### Route Group Permissions
- Authenticated users only

### Endpoints

#### Create Cart

```http
POST /api/v1/cart-items
```

##### Request Body
| Field      | Type     | Required |
| :--------- | :------- | :------- |
| `productId`| `int` | `yes` |
| `quantity`| `int` | `yes` |

##### Sample Request
```json
{
  "productId": 2, 
  "quantity": 3,
  "userId"
}
```

##### Sample Response (201)
```json
{
  "cartItemId": 1, 
  "productId": 2, 
  "quantity": 3,
  "userId": 1
}
```

##### Status Codes
- 201 Created (Cart item created successfully)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (User in the token is different from the user in the request)
- 404: Not Found (if product or user don't exist)

---

#### Partially Update a Cart Item

```http
PATCH /api/v1/cart-items/{cartItemId}
```

##### Endpoint Permission:
- Admin or resource owner

##### Request Body
| Field             | Type      | Required |
| :---------------- | :-------- | :------- |
| `quantity`            | `int`  | Yes       |


##### Sample Request
```json
{
  "quantity": 5
}
```

##### Sample Response (200)
```json
{
  "cartItemId": 1, 
  "productId": 2, 
  "quantity": 5,
  "userId": 1
}
```

##### Status Codes
- 200 OK (Successfully updated cart item)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Cart item not found)

---

#### List All Cart Items

```http
GET /api/v1/cart-items?page={page}&limit={limit}&userId={userId}
```

##### Endpoint Permissions
- Admin or resource owner

##### Query Parameters
| Parameter         | Description                         |
| :---------------- | :---------------------------------- |
| `page`    | Page number for pagination |
| `limit`   | Number of users per page |
| `userId` | Required |

##### Request Body
No request body required

##### Sample Response (200)
```json
{
  {
    "totalItems": 1,
    "itemsPerPage": 10,
    "currentPage": 1,
    "items": [
      {
        "cartItemId": 7,
        "quantity": 1,
        "userId": 2,
        "productId": 3
      }
    ]
  }
}
```

##### Status Codes
- 200 OK (Successfully retrieved cart items)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)

---

#### Get Cart Item by ID

```http
GET /api/v1/cart-item/{cartItemId}
```
##### Endpoint Permissions
- Admin or resource owner

##### Request Body
No request body required

##### Sample Response (200)
```json
 {
  "cartItemId": 7,
  "quantity": 1,
  "userId": 2,
  "productId": 3
}
```

##### Status Codes
- 200 OK (Successfully retrieved cart item information)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Cart item not found)

---

#### Delete Cart Item

```http
DELETE /api/v1/cart-items/{cartItemId}
```

##### Endpoint Permissions
- Admin or resource owner

##### Request Body
No request body required

##### Sample Response (204)
- No content (Cart item deleted successfully)

##### Status Codes
- 204 No Content (Cart item deleted successfully)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Cart item not found)
---

## Orders

### Route Group Permissions
- Authenticated users only

### Endpoints

#### Place an Order

```http
POST /api/v1/orders
```

#### Sample Request

```json
{
  "userId": 1
}
```

##### Sample Response (201)
```json
{
  "orderId": 1,
  "userId": 1,
  "items": [
    { "productId": 2, "quantity": 3 },
    { "productId": 5, "quantity": 1 }
  ],
  "orderDate": "2023-10-05T14:30:00Z"
}
```

##### Status Codes
- 201 Created (Order created successfully)
- 400 Bad Request (Invalid data (User`s cart is empty, insufficient stock or validation error))
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (User in the token is different from the user in the request)

---

> [!NOTE]
> To convert the shopping cart items to order items at the database level, an SQL procedure can be used here.
---

#### Get All Orders

##### Endpoint Permissions
- Admin only

##### Request Body
No request body required

##### Sample Response (200)
```json
{ 
  "totalItems": 1,
  "itemsPerPage": 10,
  "currentPage": 1,
  "items": [
    {
      "orderId": 1,
      "userId": 1,
      "items": [
        { "productId": 2, "quantity": 3 },
        { "productId": 5, "quantity": 1 }
      ],
      "orderDate": "2023-10-05T14:30:00Z"
    }
  ]
}
```

##### Status Codes
- 200 OK (Successfully retrieved orders)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)

---

#### Get Orders By Id

##### Endpoint Permissions
- Admin or resource owner

##### Request Body
No request body required

##### Sample Response (200)
```json
{ 
  {
    "orderId": 1,
    "userId": 1,
    "items": [
      { "productId": 2, "quantity": 3 },
      { "productId": 5, "quantity": 1 }
    ],
    "orderDate": "2023-10-05T14:30:00Z"
  }
}
```

##### Status Codes
- 200 OK (Successfully retrieved order)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Order not found)

---



#### Delete Order

```http
DELETE /api/v1/orders/{orderId}
```

##### Endpoint Permissions
- Admin only

##### Request Body
No request body required

##### Sample Response (204)
- No content (Order deleted successfully)

##### Status Codes
- 204 No Content (Order deleted successfully)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Order not found)
---

## Products

### Endpoints

#### Get all Products

```http
GET /api/v1/products?page={page}&limit={limit}&categoryId={categoryId}
```
##### Query Parameters
| Parameter | Description              |
| :-------- | :----------------------- |
| `page`    | Page number for pagination |
| `limit`   | Number of products per page |
| `categoryId` | Product category |

##### Request body
No request body required.

##### Sample Response (200)
```json
{
  "totalItems": 1,
  "itemsPerPage": 10,
  "currentPage": 1,   
  "products": [
    {
      "productId": 1,
      "title": "Sample Product",
      "description": "This is a sample product.",
      "price": 99.99,
      "stock": 50,
      "categoryId": 2,
      "category": {
        "categoryId": 2,
        "name": "Electronics"
      },
      "reviews": [],
      "productImages": []
    }
  ]
}
```

##### Status codes:
- 200 OK
- 401 Unauthorized (if user is not authenticated)

---

#### Get a single Product by ID

```http
GET /api/v1/products/{productId}
```

##### Request body
No request body required.

##### Sample Response (200)
```json
{
  "productId": 1,
  "title": "Sample Product",
  "description": "This is a sample product.",
  "price": 99.99,
  "stock": 50,
  "categoryId": 2,
  "category": {
    "categoryId": 2,
    "name": "Electronics"
  },
  "reviews": [],
  "productImages": []
}
```

##### Status codes:
- 200 OK
- 401 Unauthorized (if user is not authenticated)
- 404 Not Found (if the product does not exist)

---

#### Create a new Product
```http
POST /api/v1/products
```

##### Endpoint Permission
- Admin only.

##### Request body
| Value        | Type     | Required |
|--------------|----------|----------|
| `title`      | `string` | Yes      |
| `description`| `string` | Yes      |
| `price`      | `decimal`| Yes      |
| `stock`      | `integer`    | Yes      |
| `categoryId` | `integer`    | Yes      |
| `productImages`     | `[file]` | No      |


##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |


##### Sample request
```json
{
  "title": "New Product",
  "description": "Description of the new product",
  "price": 49.99,
  "stock": 100,
  "categoryId": 2
}
```

##### Sample Response (201)
```json
{
  "productId": 2,
  "title": "New Product",
  "description": "Description of the new product",
  "price": 49.99,
  "stock": 100,
  "categoryId": 2
}
```

##### Status codes:
- 201 Created
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)

---

#### Update or Create a Product

```http
PUT /api/v1/products/{productId}
```

##### Request body
| Value        | Type     | Required |
|--------------|----------|----------|
| `title`      | `string` | Yes       |
| `description`| `string` | Yes       |
| `price`      | `decimal`| Yes       |
| `stock`      | `integer`    | Yes       |
| `categoryid` | `integer`    | Yes       |
| `productImages`     | `[file]` | No      |


##### Request Headers
| Key             | Value                        |
| :-------------- | :--------------------------- |
| `Content-Type` | `multipart/form-data`          |


##### Sample request
```json
{
  "title": "Updated Product",
  "description": "Updated description of the product",
  "price": 59.99,
  "stock": 80,
  "categoryId": 3
}
```

##### Sample Response (200, 201)
```json
{
  "productId": 1
  "title": "Updated Product",
  "description": "Updated description of the product",
  "price": 59.99,
  "stock": 80,
  "categoryId": 3
}
```

##### Status codes:
- 200 OK (Successfully updated product)
- 201 Created (Product created successfully)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)

---

#### Partial Update an existing Product

```http
PATCH /api/v1/products/{productId}
```

##### Request body
| Value        | Type     | Required |
|--------------|----------|----------|
| `title`      | `string` | No       |
| `description`| `string` | No       |
| `price`      | `decimal`| No       |
| `stock`      | `integer`    | No       |
| `categoryId` | `integer`    | No       |

##### Sample request
```json
{
  "price": 69.99,
  "stock": 75
}
```

##### Sample request
```json
{
  "productId": 1
  "title": "Updated Product",
  "description": "Updated description of the product",
  "price": 69.99,
  "stock": 75,
  "categoryId": 3
}
```

##### Status codes:
- 200 OK (Successfully updated product)
- 400 Bad Request (Invalid input)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Product not found)

---

#### Delete a Product

```http
DELETE /api/v1/products/{productId}
```

##### Request body
No request body required.

##### Sample Response
- No content (Product deleted successfully)

##### Status codes:
- 204 No Content (Product deleted successfully)
- 401 Unauthorized (Invalid or missing token)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (Product not found)

---

## Reviews

### Endpoint Group Permission
- Authenticated users only

### Endpoints

#### Get all Reviews for a Product

```http
GET /api/v1/products/{{productId}}/reviews?page={{page}}&limit={{limit}}
```
##### Endpoint Permissions
- All users

##### Query Parameters
| Parameter | Description              |
| :-------- | :----------------------- |
| `page`    | Page number for pagination |
| `limit`   | Number of reviews per page |

##### Request body
No request body required.

##### Sample Response
```json
{
  "totalItems": 1,
  "itemsPerPage": 10,
  "currentPage": 1, 
  "reviews": [
    {
      "reviewId": 1,
      "title": "Great Product",
      "description": "I really enjoyed this product!",
      "rating": 5,
      "userId": 3,
      "productId": 1,
      "user": {
        "userId": 3,
        "firstName": "Jane",
        "lastName": "Doe"
      }
    }
  ]
}
```

##### Status codes:
- 200 OK
- 401 Unauthorized (if user is not authenticated)

---

#### Get a single Review by ID for a Product

```http
GET /api/v1/products/{{productId}}/reviews/{{reviewId}}
```
##### Endpoint Permissions
- All users

##### Request body
No request body required.

##### Sample Response
```json
{
  "reviewId": 1,
  "title": "Great Product",
  "description": "I really enjoyed this product!",
  "rating": 5,
  "userId": 3,
  "productId": 1,
  "user": {
    "userId": 3,
    "firstName": "Jane",
    "lastName": "Doe"
  }
}
```

##### Status codes:
- 200 OK
- 401 Unauthorized (if user is not authenticated)
- 404 Not Found (if the review or product do not exist)

---

#### Create a new Review for a Product

```http
POST /api/v1/products/{{productId}}/reviews
```

##### Request body
| Value        | Type     | Required |
|--------------|----------|----------|
| `title`      | `string` | Yes      |
| `description`| `string` | Yes      |
| `rating`     | `integer`    | Yes      |

##### Sample request
```json
{
  "title": "Amazing product!",
  "description": "This product exceeded my expectations.",
  "rating": 5,
}
```

##### Sample Response (201)
```json
{
  "reviewId": 1,
  "title": "Amazing product!",
  "description": "This product exceeded my expectations.",
  "rating": 5,
  "userId": 3,
  "productId": 1,
  "user": {
    "userId": 3,
    "firstName": "Jane",
    "lastName": "Doe"
  }
}
```

##### Status codes:
- 201 Created
- 400 Bad Request (invalid input)
- 401 Unauthorized (if user is not authenticated)
- 404 Not Found (Product not found)

---

#### Update or Create Review for a Product

```http
PUT /api/v1/products/{{productId}}/reviews/{{reviewId}}
```
##### Endpoint Permission
- Admin or resource owner 

##### Request body
| Value        | Type     | Required |
|--------------|----------|----------|
| `title`      | `string` | Yes       |
| `description`| `string` | Yes       |
| `rating`     | `integer`    | Yes       |

##### Sample request
```json
{
  "title": "Updated Review",
  "description": "Updated description of the review",
  "rating": 4
}
```

##### Sample Response (200, 201)
```json
{
  "reviewId": 1,
  "title": "Updated Review",
  "description": "Updated description of the review",
  "rating": 4,
  "userId": 3,
  "productId": 1,
  "user": {
    "userId": 3,
    "firstName": "Jane",
    "lastName": "Doe"
  }
}
```

##### Status codes:
- 200 OK
- 201 Created
- 400 Bad Request (invalid input)
- 401 Unauthorized (if user is not authenticated)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (if the product does not exist)

---

#### Partial Update an existing Review for a Product

```http
PATCH /api/v1/products/{{productId}}/reviews/{{reviewId}}
```
##### Endpoint Permission
- Admin or resource owner

##### Request body
| Value        | Type     | Required |
|--------------|----------|----------|
| `title`      | `string` | No       |
| `description`| `string` | No       |
| `rating`     | `integer`    | No       |

##### Sample request
```json
{
  "rating": 4
}
```

##### Sample Response (200)
```json
{
  "reviewId": 1,
  "title": "Amazing product!",
  "description": "This product exceeded my expectations.",
  "rating": 4,
  "userId": 3,
  "productId": 1,
  "user": {
    "userId": 3,
    "firstName": "Jane",
    "lastName": "Doe"
  }
}
```

##### Status codes:
- 200 OK
- 400 Bad Request (invalid input)
- 401 Unauthorized (if user is not authenticated)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (if the product or review do not exist)

---

#### Delete a Review for a Product

```http
DELETE /api/v1/products/{{productId}}/reviews/{{reviewId}}
```
##### Endpoint Permission
- Admin or resource owner

##### Request body
No request body required.

##### Sample Response (204)
 - No Content (review deleted successfully).
##### Status codes:
- 204 No Content (review deleted successfully)
- 401 Unauthorized (if user is not authenticated)
- 403 Forbidden (Insufficient permissions)
- 404 Not Found (if the review does not exist)

---
