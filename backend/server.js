const express = require('express');
const cors = require('cors');
const path = require('path');
const nodemailer = require('nodemailer');
const db = require('./db');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 5000;

// ==========================================
// Middleware Setup
// ==========================================
app.use(cors()); // Enables Cross-Origin requests
app.use(express.json()); // Parses incoming JSON payloads
app.use(express.static(path.join(__dirname, 'public'))); // Serves static files (admin.html)

// ==========================================
// Nodemailer Email Transporter
// ==========================================
const transporter = nodemailer.createTransport({
    service: 'gmail',
    auth: {
        user: process.env.EMAIL_USER,
        pass: process.env.EMAIL_PASS
    },
    tls: {
        rejectUnauthorized: false // Bypasses local self-signed certificate SSL blocking
    }
});

// ==========================================
// Static / In-Memory Data (Skills)
// ==========================================
const skillsData = [
    { id: 1, category: "Mobile Development", icon: "📱", details: "Designing modern, responsive, and natively compiled applications for iOS & Android using Flutter and Dart." },
    { id: 2, category: "Software & Systems", icon: "💻", details: "Constructing scalable backend services and robust computer architectures using C# and C++." },
    { id: 3, category: "Database Management", icon: "🗄️", details: "Structuring relational engines with relational SQL databases as well as flexible document stores like MongoDB." }
];

// ==========================================
// 1. GET ROUTE: Fetch Skills
// ==========================================
app.get('/api/skills', (req, res) => {
    res.status(200).json({ success: true, data: skillsData });
});

// ==========================================
// 2. POST ROUTE: Submit Contact Message
//    (Saves to SQLite DB & Sends Email Notification)
// ==========================================
app.post('/api/contact', (req, res) => {
    const { name, email, message } = req.body;

    // Backend Input Validation Rules
    if (!name || name.trim() === "") {
        return res.status(400).json({ success: false, error: "Name is a required field." });
    }
    if (!email || !email.includes("@")) {
        return res.status(400).json({ success: false, error: "Please enter a valid email address containing '@'." });
    }
    if (!message || message.trim().length < 5) {
        return res.status(400).json({ success: false, error: "Your message must be at least 5 characters long." });
    }

    // A. Insert into SQLite Database
    const sql = 'INSERT INTO contacts (name, email, message) VALUES (?, ?, ?)';
    db.run(sql, [name.trim(), email.trim(), message.trim()], function(err) {
        if (err) {
            console.error("❌ Database Insert Error:", err.message);
            return res.status(500).json({ success: false, error: err.message });
        }

        // B. Send Email Notification via Gmail
        const mailOptions = {
            from: process.env.EMAIL_USER,
            to: process.env.EMAIL_USER, // Sends email directly to your inbox
            replyTo: email.trim(),      // Pressing 'Reply' in Gmail responds to the visitor directly!
            subject: `📩 New Contact Form Message from ${name.trim()}`,
            html: `
                <div style="font-family: Arial, sans-serif; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; max-width: 600px;">
                    <h2 style="color: #2563eb; margin-top: 0;">New Website Message Received</h2>
                    <p><strong>Name:</strong> ${name.trim()}</p>
                    <p><strong>Email:</strong> <a href="mailto:${email.trim()}">${email.trim()}</a></p>
                    <hr style="border: 0; border-top: 1px solid #eeeeee; margin: 15px 0;">
                    <p><strong>Message Body:</strong></p>
                    <blockquote style="background: #f9f9f9; padding: 12px; border-left: 4px solid #2563eb; margin: 0; line-height: 1.5;">
                        ${message.trim()}
                    </blockquote>
                </div>
            `
        };

        transporter.sendMail(mailOptions, (mailErr, info) => {
            if (mailErr) {
                console.error("⚠️ Email Sending Failed:", mailErr.message);
            } else {
                console.log("📧 Email Sent Successfully! ID:", info.messageId);
            }
        });

        // Respond to the frontend UI
        res.status(201).json({
            success: true,
            message: `Thank you, ${name.trim()}! Your message was sent successfully.`
        });
    });
});

// ==========================================
// 3. GET ROUTE: Fetch Contact Messages (SQLite)
// ==========================================
app.get('/api/contact', (req, res) => {
    db.all('SELECT * FROM contacts ORDER BY created_at DESC', [], (err, rows) => {
        if (err) {
            console.error("❌ Database Fetch Error:", err.message);
            return res.status(500).json({ success: false, error: err.message });
        }
        res.status(200).json({ success: true, data: rows });
    });
});

// ==========================================
// Start Express Server
// ==========================================
app.listen(PORT, () => {
    console.log(`=============================================================`);
    console.log(`  🚀 BACKEND RUNNING: http://localhost:${PORT}`);
    console.log(`  📥 ADMIN INBOX:     http://localhost:${PORT}/admin.html`);
    console.log(`=============================================================`);
});
