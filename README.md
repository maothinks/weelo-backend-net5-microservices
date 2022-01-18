# Weelo Backend Microservices

The Weelo System backend has a structural organization based on Clean Architectures like Hexagonal Architecture and use a general Architecture Pattern based on Microservices:

- Microservice Api Gateway (Ocelot)
- Microservice Auth And Users
- Microservice File Storage
- Microservice Properties
- Microservice Notifications SignalR
- Microservice Notifications RabbitMQ

Each microservice has implemented a parallel xUnit Project to validate the correct functionality with Unit Tests:

- Weelo.Microservices.AuthAndUsers.APITests
- Weelo.Microservices.FileStorage.APITests
- Weelo.Microservices.Properties.Infrastructure.APITests
- Weelo.Microservices.Notifications.SignalrAPITests
- Weelo.Microservices.Notifications.RabbitMQ_Producer_APITests

Each Microservice has your own structure, architecture and repository (Database, Cloud Storage, etc) if it is required

- **Microservice AuthAndUsers** uses a **SQL SERVER Database** (See Configuration section below to **create the database** and configure the connection params)

- **Microservice Properties** uses a **SQL SERVER Database** and RabbitMQ server connection too (See Configuration section below to **create de database** and configure the connection params)

- **Microservice File Storage** uses a FIREBASE STORAGE Cloud (See Configuration section below to configure the connection params)

- **Microservice RabbitMQ** uses a RabbitMQ server connection  (See Configuration section below to configure the connection params)

- **Microservice SignalR** uses a EndPoint SignalR and RabbitMQ server connection too (See Configuration section below to configure the connection params)



## Installation

To have a functional system you need to start all microservices and has a RabbitMQ service enabled, each microservice has an appsettings.json to configure the parameters required like EndPoint, Connections Strings, JWT Secret keys, etc... 

Following we will to explain the configuration for each microservice:

- #### Microservice API Gateway

This microservice will be the service exposed to clients (React Web Application), Here we found to main JSON files:

- appsettings.json

This file contais the parameters for JWTSecretKey

                    
> "Make sure the Sercret Key matchs with Secret key in Microservice AuthAnd Users appsettins.json"

```javascript
"AppSettings": {
    "JWTSecret": "[PUT HERE SECRET KEY]"
  }
```

- ocelot.json

This file will contains the routes configuration about other microservices like Host, Port, Authetication Requires, etc...

For tests on local host is recomended use the ports defined in this file to each microservice

- Weelo.Microservices.Properties.Infrastructure.API
**Port 44301**
- Weelo.Microservices.AuthAndUsers.API
**Port 44302**
- Weelo.Microservices.FileStorage.API
**Port 44303**
- Weelo.Microservices.Notifications.RabbitMQ_Producer_API
**Port 44305**
- Weelo.Microservices.Notifications.SignalrAPI
**Port 44306**


```javascript

{
  "Routes": [
    {
      "DownstreamPathTemplate": "/users/{id}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 44302
        }
      ],
      "UpstreamPathTemplate": "/gateway/users/{id}",
      "UpstreamHttpMethod": [ "PUT", "GET", "DELETE" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      }
      //,"RouteClaimsRequirement": {
      //  "Role": "admin"
      //}
    },
	.
	.
	.
	.
	.
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:44300"
  }
}

```

- #### Microservice Auth and Users

This microservice contains the functions to Authenticate, register and CRUD operations for Users Database in the Weelo Properties System

- appsettings.json

This file contais the parameters for Database
                    
> "Make sure to creates a Database to Users on a Sql Server, run the schema script located in the root of the solution named **'WeeloDBUsers.sql'** and configured the connection string in WebApiDatabase variable"

```javascript
"AppSettings": {
    "Secret": "[PUT HERE SECRET KEY]",
    "ExpiresDays": "7"
  },
  "ConnectionStrings": {
    "WebApiDatabase": "[Put Here DB Connection String]"
  },
```

- #### Microservice File Storage

This microservice uses the **Firebase Storage Cloud** to storage the Property Images

- appsettings.json

This file contais the parameters for cloud connection

```javascript
""FirebaseStorage": {
    "firebaseStorage_ApiKey": "AIzaSyC7uuXJ8SrTsRKQY59GeYgZiDqRx2x5dfs",
    "firebaseStorage_Bucket": "weeloproject.appspot.com",
    "firebaseStorage_AuthEmail": "maothinks@gmail.com",
    "firebaseStorage_AuthPassword": "weelo123"
  }
```

- #### Microservice Properties

This microservice contains the  CRUD functions for Properties and Property Images, You have to create a SQL SErver Database

- appsettings.json

This file contais the parameters for Database
                    
> "Make sure to creates a Database to Properties on a Sql Server, run the schema script located in the root of the solution named **'WeeloDBProperties.sql'** and configured the connection string in defaultConnection variable"

```javascript
"AppSettings": {
    "Secret": "[PUT HERE SECRET KEY]",
    "ExpiresDays": "7"
  },
  "ConnectionStrings": {
    "WebApiDatabase": "[Put Here DB Connection String]"
  },
```


- #### Microservice RabbitMQ

This microservice is in charge of receiving notifications to enqueue in RabbitMQ server.

Notifications received: 

- Notify View:
Receives a request from client and queue it into **weelo-notify-view** queue. this queue is consumed by Microservice Properties to save the data into PropertiesDB.

- Notify View SignalR
Receives a request from client and queue it into **weelo-notify-view-client** queue. this queue is consumed by Microservice Signal to notify it to all connected users from react web app

- appsettings.json

This file contais the parameters for RabbitMQ server

```javascript
"RabbitMQ": {
    "Host": "amqp://guest:guest@localhost:5672",
    "NotifyViewQueue": "weelo-notify-view",
    "NotifyViewQueueSignalr": "weelo-notify-view-client"
  }
```

- #### Microservice SignalR

This microservice is in charge of send notifications to all connected client (React Web app)

- appsettings.json

This file contais the parameters for RabbitMQ server and Signal End Point (React Web App)

```javascript
"SignalR": {
    "AllowOrigin": "http://localhost:9000",
    "EndPoint": "/weelo/properties"
  },
  "RabbitMQ": {
    "Host": "amqp://guest:guest@localhost:5672",
    "NotifyViewQueue": "weelo-notify-view-client"
  }
```

## System Diagram

Weelo Properties System:

![](https://firebasestorage.googleapis.com/v0/b/weeloproject.appspot.com/o/WeeloPropertiesDiagram.png?alt=media&token=2ab10264-64c4-4cf1-9763-e93b82729069)

## Frontend Video Demos

Here you can watch some videos with the app functionality

| Use Case | Video Demo |
| ------ | ------ |
| Login and Sign-up |  <https://screencast-o-matic.com/watch/c3VF2xVoK4a> |
| Create New Property | <https://screencast-o-matic.com/watch/c3VF2gVoKaM> |
| Upload Images  | <https://screencast-o-matic.com/watch/c3VF2KVoKBq> |
| Update Property (Included update price) | <https://screencast-o-matic.com/watch/c3VF2MVoKBN> |
| Search Properties Using Filters | <https://screencast-o-matic.com/watch/c3VFouVoKG9> |
| Update Views and SignalR | <https://screencast-o-matic.com/watch/c3VFoAVoKHL> |
| Navigate to user Profile | <https://screencast-o-matic.com/watch/c3VFoUVoKm3> |
| App Logout | <https://screencast-o-matic.com/watch/c3VFowVoKmB> |

## Technical Video Demos

| Process | Video Demo |
| ------ | ------ |
| Runing Unit Tests | <https://screencast-o-matic.com/watch/c3VFD2VoK8k> |
| Start Microservices | <https://screencast-o-matic.com/watch/c3VFDYVoKPR> |
| Start Microfrontends | <https://screencast-o-matic.com/watch/c3VFDvVoKSD> |
| RabBit MQ Monitor Queues | <https://screencast-o-matic.com/watch/c3VFDmVo7VR> |



**Weelo Technical Test, Mauricio Zapata**

**Cheers!!!**
