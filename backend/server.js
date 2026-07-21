const express = require('express');
const cors = require('cors');
const app = express();
const PORT = process.env.PORT || 5000;

// Middleware configurations
app.use(cors()); // Allows frontend to communicate with this API
app.use(express.json()); // Parses incoming JSON request bodies

// Temporary in-memory databases
const skillsData = [
    { id: 1, category: "Mobile Development", icon: "📱", details: "Designing modern, responsive, and natively compiled applications for iOS & Android using Flutter and Dart." },
    { id: 2, category: "Software & Systems", icon: "💻", details: "Constructing scalable backend services and robust computer architectures using C# and C++." },
    { id: 3, category: "Database Management", icon: "🗄️", details: "Structuring relational engines with relational SQL databases as well as flexible document stores like MongoDB." }
];

const contactSubmissions = [];

// ==========================================
// 1. GET ROUTE: Fetch Skills [RESTful noun]
// ==========================================
app.get('/api/skills', (req, res) => {
    console.log("GET /api/skills - Request received");
    res.status(200).json({
        success: true,
        data: skillsData
    });
});

// ==========================================
// 2. POST ROUTE: Submit Contact Message [RESTful noun]
// ==========================================
app.post('/api/contact', (req, res) => {
    console.log("POST /api/contact - Submission attempt received:", req.body);
    const { name, email, message } = req.body;

    // --- Backend Validation Rules ---
    if (!name || name.trim() === "") {
        return res.status(400).json({ success: false, error: "Name is a required field." });
    }
    if (!email || !email.includes("@")) {
        return res.status(400).json({ success: false, error: "Please enter a valid email address containing '@'." });
    }
    if (!message || message.trim().length < 5) {
        return res.status(400).json({ success: false, error: "Your message must be at least 5 characters long." });
    }

    // Process and store validated request payload
    const submission = {
        id: contactSubmissions.length + 1,
        name: name.trim(),
        email: email.trim(),
        message: message.trim(),
        timestamp: new Date()
    };
    contactSubmissions.push(submission);

    res.status(201).json({
        success: true,
        message: `Thank you, ${submission.name}! Your message was successfully received by the backend server.`,
        data: submission
    });
});

// Start listening for incoming traffic
app.listen(PORT, () => {
    console.log(`=============================================================`);
    console.log(`  🚀 DECODELABS PROJECT 2 RUNNING: http://localhost:${PORT}`);
    console.log(`=============================================================`);
});
