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
![Serilog](https://img.shields.io/badge/Serilog-1E90FF?style=for-the-badge&logo=serilog&logoColor=white)

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

- **✅ Task Management (CRUD)**  
  Effortlessly manage tasks with full **Create, Read, Update, and Delete (CRUD)** operations, including the ability to mark tasks as completed.

- **📅 Automated Monthly Reports**  
  - A **PDF report** is generated automatically on the **1st day of each month**.  
  - Provides insights into task completion trends.  
  - Reports are stored in **Amazon S3**.

- **📊 Task Completion Metrics**  
  - **📜 Completed Tasks List** – View all tasks completed in the last month.  
  - **⏳ Average Completion Time** – Analyze how long tasks typically take to complete.  
  - **🚀 Fastest & 🐢 Slowest Completed Tasks** – Identify the quickest and slowest task completions.


## Architecture

TaskManager follows a fully serverless architecture using AWS services:

### 1️⃣ Task Management API  
- Built using **.NET 8 Web API**.
- Uses **Amazon API Gateway** to expose endpoints.  
- Hosted as an **AWS Lambda function** for efficient execution.   
- Stores task data in **Amazon DynamoDB** for low-latency access.  
### 2️⃣ Monthly Report Generation  
- **⏰ Automated Trigger**: Executed by **AWS EventBridge** on the **1st day of each month**.  
- **📝 Data Processing**: Reads completed task data from **Amazon DynamoDB**.  
- **📄 PDF Report Creation**: Generates a detailed report with task completion metrics.  
- **☁️ Storage & Access**: Stores the generated report securely in an **Amazon S3 bucket**.  
- **📊 Logging & Debugging**: Logs execution details and potential failures using **CloudWatch Logs** with **Serilog**, ensuring visibility into the report generation process.




### Running Locally

1. Clone the repository:
   ```sh
   git clone https://github.com/Pauloocm/TaskManager.git
   cd TaskManager

## 📸 Screenshots

Below are some images showcasing the TaskManager in action:

### Dashboard
![image](https://github.com/user-attachments/assets/a17078c5-1879-4626-9506-50be456dbd60)


### Task Creation
![image](https://github.com/user-attachments/assets/4d2c1c87-669e-447c-ba9b-1d1806fd14ea)

