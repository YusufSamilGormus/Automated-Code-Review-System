# 🚀 Automated Code Review System  

## 📌 About This Project  

This project is an **Automated Code Review System** that helps developers analyze their Java code using a **locally hosted AI model**. Instead of relying on paid APIs, we set up a **DeepSeek** or **LLaMA** model on the user's machine, making the tool completely **free, private, and offline**.  

The system takes in Java code, runs an AI-powered analysis, and provides **feedback on code quality, optimizations, and security improvements**. Everything happens locally, so no data ever leaves your computer.  

---

## 🛠️ Tech Stack  

### **Backend**  
- **Java (Spring Boot)** – Handles requests, manages logic, and connects to the AI model.  
- **Python (Flask or FastAPI)** – Runs the AI model and processes code analysis.  

### **Model Hosting**  
- **DeepSeek or LLaMA** – A locally hosted LLM for analyzing Java code.  
- **Hugging Face Transformers** – Helps load and interact with the model.  

### **Frontend**  
- **React.js** – Web interface for submitting and reviewing code.  

### **Database**  
- **PostgreSQL / MongoDB** – If we decide to store user submissions.  

### **Other Tools**  
- **Docker** – To make installation easier.  
- **JWT** – For authentication.  

---

## 📂 How It Works  

1. **User submits Java code** through the frontend.  
2. **Java backend** (Spring Boot) receives the code and sends it to the **Python model server**.  
3. **Python server** runs the AI model (DeepSeek/LLaMA) to analyze the code.  
4. **AI generates feedback** and sends it back to the backend.  
5. The **frontend displays** the feedback in a structured way.  

Everything happens **locally** on the user's machine, meaning no cloud dependencies or API costs!  

---

## 🏗️ Installation  

### **1️⃣ Set Up the Python Model Server**  
Make sure you have **Python 3.8+** installed. Then, install the necessary dependencies:  

```bash
pip install transformers torch flask fastapi
