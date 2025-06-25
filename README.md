# 🚀 Automated Code Review System

## 📌 About This Project

This project is an **Automated Code Review System** that allows developers to receive AI-generated feedback on their Java code using a **locally hosted Large Language Model (LLM)** like DeepSeek or LLaMA, without relying on external paid APIs.

Users can register and log in securely, submit their code through a simple web interface, and view detailed suggestions for improvement. The project is fully **Dockerized** for easy deployment, and all model interactions happen locally to preserve **privacy** and eliminate **API costs**.

---

## 🛠️ Tech Stack

### **Frontend**

* **React.js**

  * Components: `Dashboard.tsx`, `LoginBoard.tsx`, `RegisterForm.tsx`, `ReviewDetail.tsx`, etc.
  * **Axios** for API communication
  * Email verification with cooldown implemented

### **Backend**

* **.NET 8 (ASP.NET Core Web API)**

  * Controllers, Services, DTOs, and Mappers used for clean architecture
  * JWT-based authentication with refresh token support
  * PostgreSQL integration
  * Email verification with cooldown logic
  * Admin user support
  * OpenAPI (Swagger) for documentation

### **LLM Integration**

* **Python model server (FastAPI)** running locally

  * Runs **DeepSeek** or **LLaMA** via **Hugging Face Transformers**
  * Receives and analyzes Java code
  * Sends back suggestions to the backend

### **Deployment**

* **Docker**

  * Separate containers for backend, frontend, and Python model
  * `.env` and `.env.production` files used for configuration
  * Ready to deploy with Render or any cloud provider

---

## 📂 How It Works

1. **User registers/logs in** and verifies their email.
2. On the **Dashboard**, user submits Java code.
3. The **.NET backend** handles the request, sends the code to the **Python model server**.
4. The **model analyzes** the code and returns feedback.
5. The backend sends the response to the **frontend**, which displays it in a readable format.
6. User can **continue the review** in a chat-style interface powered by the same model.

All operations occur **locally** on the user's machine or a private server — ensuring privacy and eliminating API costs.

---

## 🏗️ Installation

### ✅ Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [Node.js & npm](https://nodejs.org/)
* [Docker](https://www.docker.com/products/docker-desktop)
* Python 3.8+

### 🔧 1. Clone the repository

```bash
git clone https://github.com/your-username/automated-code-review.git
cd automated-code-review
```

### 🐍 2. Set up the Python model server

```bash
cd backend/PythonModelServer
pip install -r requirements.txt
uvicorn main:app --host 0.0.0.0 --port 5000
```

Requirements include:

```bash
transformers
torch
fastapi
```

### 🧱 3. Set up the .NET backend

```bash
cd backend/CodeReviewAPI
dotnet restore
dotnet run
```

Configure database and secrets in `.env` and `appsettings.json`.

### 🌐 4. Set up the frontend

```bash
cd frontend
npm install
npm run dev
```

### 🐳 5. Run with Docker (Recommended)

Make sure `.env.production` and `docker-compose.yml` are configured. Then run:

```bash
docker-compose up --build
```

---

## 🔐 Features

* ✅ Secure login with JWT tokens
* ✅ Email verification with 1-minute cooldown
* ✅ Admin user support
* ✅ Code review chat panel
* ✅ LLM feedback generation (offline, local)
* ✅ Full Docker support
* ✅ OpenAPI documentation

---

## 📄 API Documentation

Accessible via `/swagger` endpoint when the backend is running.

Example endpoints:

* `POST /api/auth/register`
* `POST /api/auth/login`
* `GET /api/users/verify-email?token=...`
* `POST /api/reviews` – Submit code for review
* `POST /api/reviews/{id}/chat` – Continue code review

---

## 🧠 LLM Integration

The project uses a Python FastAPI server to run a local DeepSeek or LLaMA model. Java code is sent as input, and the model returns feedback such as:

* Code structure and readability improvements
* Bug detection
* Security risks
* Optimization opportunities

You must set up your Python model server locally for this to work — see `PythonModelServer/README.md` if available.

---

## 📬 Contact

For questions, suggestions, or contributions, feel free to open an issue or pull request.

---
