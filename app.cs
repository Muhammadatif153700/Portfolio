document.addEventListener('DOMContentLoaded', () => {
    const menuToggle = document.querySelector('.menu-toggle');
    const navContainer = document.querySelector('.nav-container');
    const navLinks = document.querySelectorAll('.nav-link');
    const contactForm = document.getElementById('api-contact-form');
    const feedbackMessage = document.getElementById('form-feedback-message');
    const skillsGrid = document.getElementById('skills-grid');

    // API Base URL running locally
    const API_BASE_URL = 'http://localhost:5000/api';

    // 1. Mobile Navigation Toggle
    if (menuToggle && navContainer) {
        menuToggle.addEventListener('click', () => {
            navContainer.classList.toggle('active');
            const isOpen = navContainer.classList.contains('active');
            menuToggle.textContent = isOpen ? '✕' : '☰';
        });
    }

    // 2. Navigation Active State Management
    navLinks.forEach(link => {
        link.addEventListener('click', () => {
            navLinks.forEach(nav => nav.classList.remove('active'));
            link.classList.add('active');
            if (window.innerWidth <= 768 && navContainer) {
                navContainer.classList.remove('active');
                if (menuToggle) menuToggle.textContent = '☰';
            }
        });
    });

    // ==========================================
    // 3. FETCH DYNAMIC SKILLS FROM BACKEND (GET)
    // ==========================================
    async function loadSkillsFromAPI() {
        if (!skillsGrid) return;

        try {
            const response = await fetch(`${API_BASE_URL}/skills`);
            const result = await response.json();

            if (response.ok && result.data) {
                skillsGrid.innerHTML = ''; // Clear loading message

                result.data.forEach(skill => {
                    const skillCard = document.createElement('div');
                    skillCard.className = 'skill-card';
                    skillCard.innerHTML = `
                        <div class="skill-icon" style="font-size: 2rem; margin-bottom: 0.5rem;">${skill.icon}</div>
                        <h3 style="margin-bottom: 0.5rem;">${skill.category}</h3>
                        <p>${skill.details}</p>
                    `;
                    skillsGrid.appendChild(skillCard);
                });
            } else {
                skillsGrid.innerHTML = '<p class="error-text">Failed to load skills from API server.</p>';
            }
        } catch (error) {
            skillsGrid.innerHTML = '<p class="error-text">Unable to connect to backend server. Ensure Node.js is running on port 5000.</p>';
        }
    }

    // Trigger skills fetch on load
    loadSkillsFromAPI();

    // ==========================================
    // 4. SUBMIT CONTACT FORM TO BACKEND (POST)
    // ==========================================
    if (contactForm) {
        contactForm.addEventListener('submit', async (event) => {
            event.preventDefault(); // Stop page reload
            
            feedbackMessage.textContent = "Sending message to server...";
            feedbackMessage.style.color = "#A0D4E0";

            const name = document.getElementById('form-name').value;
            const email = document.getElementById('form-email').value;
            const message = document.getElementById('form-message').value;

            try {
                // Post form payload to Node.js backend
                const response = await fetch(`${API_BASE_URL}/contact`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ name, email, message })
                });

                const result = await response.json();

                if (response.ok) {
                    feedbackMessage.textContent = result.message;
                    feedbackMessage.style.color = "#4CAF50"; // Green Success
                    contactForm.reset(); // Clear fields
                } else {
                    feedbackMessage.textContent = `Error: ${result.error}`;
                    feedbackMessage.style.color = "#FF5722"; // Orange Error
                }
            } catch (error) {
                feedbackMessage.textContent = "Could not connect to backend. Is your Node.js server running?";
                feedbackMessage.style.color = "#FF5722";
            }
        });
    }
});
