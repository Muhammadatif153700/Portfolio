# Full-Stack Portfolio Application

A modern, responsive full-stack web application built step-by-step, featuring a minimalist editorial frontend, custom API integrations, persistent SQLite database storage, and automated email notifications.

---

## 🚀 Project Evolution & Architecture

This project was developed incrementally across three distinct core phases to build a robust full-stack system:

### Phase 1: Responsive Editorial UI & Frontend Design
* Built a mobile-first, responsive portfolio website featuring a minimalist editorial aesthetic.
* Implemented custom CSS styling, typography layouts, and dynamic image cropping fixes for high-quality visual presentation.
* Integrated smooth scrolling, interactive navigation, and responsive menu behavior for seamless cross-device display.

### Phase 2: API Integration & Frontend Interactions
* Developed and wired dynamic JavaScript handlers to interact with backend endpoints.
* Connected client-side forms to asynchronous API calls (`fetch` API) with real-time UI status updates and client-side validation.
* Integrated cross-project linking and structured frontend-to-backend communication workflows.

### Phase 3: Express Backend, SQLite Database & Email Alerts
* Engineered a Node.js/Express RESTful backend server (`POST /api/contact` and `GET /api/contact`).
* Integrated an **SQLite** relational database (`app.db`) for permanent submission persistence.
* Implemented real-time transactional email notifications via **Nodemailer** for automated alert delivery upon user contact.
* Secured environment credentials using environment variables (`.env`) and `.gitignore` safety policies.

---

## 🛠️ Tech Stack

* **Frontend:** HTML5, CSS3, Modern JavaScript (ES6+)
* **Backend:** Node.js, Express.js
* **Database:** SQLite (`sqlite3`)
* **Services & Tools:** Nodemailer (SMTP), Git, GitHub

---

## 📦 Project Structure

```text
├── index.html            # Main portfolio page
├── style.css             # Responsive layout & custom editorial styling
├── script.js            # Client-side validation & API fetch requests
├── server.js            # Express server, SQLite database, & Nodemailer route
├── .env.example         # Template for environment variables
├── .gitignore           # Excludes node_modules, .env, and .db files
└── README.md            # Project documentation
⚙️ How to Run Locally
1. Prerequisites
Ensure you have Node.js installed on your machine.

2. Clone the Repository
Bash
git clone [https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git](https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git)
cd YOUR_REPO_NAME
3. Install Dependencies
Bash
npm install
4. Set Up Environment Variables
Create a .env file in the root directory based on the .env.example template:

Code snippet
PORT=5000
EMAIL_USER=your_gmail_address@gmail.com
EMAIL_PASS=your_16_character_app_password
5. Start the Server
Bash
npm start
The application will run locally at http://localhost:5000.

🔒 Security Note
Sensors and secret keys (such as email credentials and database files) are strictly excluded from git tracking using .gitignore. Use .env.example as a guide for configuring local environment secrets
