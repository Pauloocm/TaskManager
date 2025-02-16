# TaskManager

![GitHub Repo stars](https://img.shields.io/github/stars/Pauloocm/TaskManager?style=social)
![GitHub forks](https://img.shields.io/github/forks/Pauloocm/TaskManager?style=social)
![GitHub issues](https://img.shields.io/github/issues/Pauloocm/TaskManager)
![GitHub last commit](https://img.shields.io/github/last-commit/Pauloocm/TaskManager)
![Build Status](https://img.shields.io/github/actions/workflow/status/Pauloocm/TaskManager/dotnet-desktop.yml)

TaskManager is a serverless task management system that allows users to create, update and complete tasks. Additionally, on the 1st day of each month, a PDF report is generated with various task completion metrics.

## 🚀 Tech Stack

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![AWS Lambda](https://img.shields.io/badge/AWS_Lambda-FF9900?style=for-the-badge&logo=amazonaws&logoColor=white)
![DynamoDB](https://img.shields.io/badge/AWS_DynamoDB-4053D6?style=for-the-badge&logo=amazon-dynamodb&logoColor=white)
![S3](https://img.shields.io/badge/AWS_S3-569A31?style=for-the-badge&logo=amazon-s3&logoColor=white)
![AWS EventBridge](https://img.shields.io/badge/AWS_EventBridge-8C4FFF?style=for-the-badge&logo=amazonaws&logoColor=white)
![AWS CloudWatch](https://img.shields.io/badge/AWS_CloudWatch-FF4F8B?style=for-the-badge&logo=amazonaws&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![NUnit](https://img.shields.io/badge/NUnit-25A162?style=for-the-badge&logo=nunit&logoColor=white)

## 📊 System Design

```mermaid
graph TD
    A[API Gateway] --> B[Lambda - Task API]
    B --> C[DynamoDB]
    D[EventBridge] --> E[Lambda - Report Generator]
    E --> C
    E --> F[S3 - Reports]
```

## Features

- **Task Management (CRUD)**: Create, update, delete, and complete tasks.
- **Monthly Report Generation**: Automatically generates a PDF report on the 1st day of each month.
- **Task Completion Metrics**:
  - List of completed tasks in the last month.
  - Average time to complete tasks.
  - The slowest and fastest completed tasks.
- **Serverless Architecture**:
  - Built with **.NET 8 Web API**.
  - Uses **AWS Lambda** for API hosting.
  - Stores data in **Amazon DynamoDB**.
  - Monthly report generation triggered by **AWS EventBridge**.
  - Reports are stored in **Amazon S3**.
- **Unit Testing**: Implemented using **NUnit**.

## Architecture

TaskManager follows a fully serverless architecture using AWS services:

1. **Task Management API**  
   - Built using **.NET 8 Web API**.  
   - Hosted as an **AWS Lambda function**.  
   - Uses **Amazon API Gateway** (if applicable) to expose endpoints.  
   - Stores task data in **Amazon DynamoDB**.  

2. **Monthly Report Generation**  
   - Triggered by **AWS EventBridge** on the 1st day of each month.  
   - Executed by a dedicated **AWS Lambda function**.  
   - Reads completed task data from **Amazon DynamoDB**.  
   - Generates a PDF report with task completion metrics.  
   - Stores the report in an **Amazon S3 bucket**.  



### Running Locally

1. Clone the repository:
   ```sh
   git clone https://github.com/Pauloocm/TaskManager.git
   cd TaskManager
