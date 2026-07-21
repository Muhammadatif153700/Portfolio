document.addEventListener('DOMContentLoaded', () => {
    const menuToggle = document.querySelector('.menu-toggle');
    const navContainer = document.querySelector('.nav-container');
    const navLinks = document.querySelectorAll('.nav-link');
    const contactForm = document.getElementById('api-contact-form');
    const feedbackMessage = document.getElementById('form-feedback-message');
    const skillsGrid = document.getElementById('skills-grid');

    const API_BASE_URL = 'http://localhost:5000/api';

    // 1. Mobile Menu Toggle
    if (menuToggle && navContainer) {
        menuToggle.addEventListener('click', () => {
            navContainer.classList.toggle('active');
            const isOpen = navContainer.classList.contains('active');
            menuToggle.textContent = isOpen ? '✕' : '☰';
        });
    }

    // 2. Nav Active Link Switch
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
    // 3. INTERSECTION OBSERVER FOR SCROLL REVEAL
    // ==========================================
    const observerOptions = {
        threshold: 0.1,
        rootMargin: "0px 0px -50px 0px"
    };

    const revealOnScroll = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('active');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Register static elements
    document.querySelectorAll('.scroll-reveal').forEach(el => {
        revealOnScroll.observe(el);
    });

    // ==========================================
    // 4. FETCH SKILLS WITH STAGGERED ANIMATIONS
    // ==========================================
    async function loadSkillsFromAPI() {
        if (!skillsGrid) return;

        try {
            const response = await fetch(`${API_BASE_URL}/skills`);
            const result = await response.json();

            if (response.ok && result.data) {
                skillsGrid.innerHTML = '';

                result.data.forEach((skill, index) => {
                    const skillCard = document.createElement('div');
                    // Add staggered animation delay classes
                    const delayClass = `delay-${(index % 3) + 1}`;
                    skillCard.className = `skill-card scroll-reveal ${delayClass}`;
                    
                    skillCard.innerHTML = `
                        <div class="skill-icon" style="font-size: 2.2rem; margin-bottom: 0.8rem;">${skill.icon}</div>
                        <h3 style="margin-bottom: 0.6rem; color: #f8fafc;">${skill.category}</h3>
                        <p style="color: #94a3b8; font-size: 0.95rem;">${skill.details}</p>
                    `;
                    skillsGrid.appendChild(skillCard);
                    
                    // Attach reveal observer to new card
                    revealOnScroll.observe(skillCard);
                });
            } else {
                skillsGrid.innerHTML = '<p class="error-text">Failed to load skills from API server.</p>';
            }
        } catch (error) {
            skillsGrid.innerHTML = '<p class="error-text">Unable to connect to backend server. Ensure Node.js is running on port 5000.</p>';
        }
    }

    loadSkillsFromAPI();

    // ==========================================
    // 5. CONTACT FORM SUBMISSION
    // ==========================================
    if (contactForm) {
        contactForm.addEventListener('submit', async (event) => {
            event.preventDefault();
            
            feedbackMessage.textContent = "Sending message to server...";
            feedbackMessage.style.color = "#38bdf8";

            const name = document.getElementById('form-name').value;
            const email = document.getElementById('form-email').value;
            const message = document.getElementById('form-message').value;

            try {
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
                    feedbackMessage.style.color = "#4ade80"; // Bright Green
                    contactForm.reset();
                } else {
                    feedbackMessage.textContent = `Error: ${result.error}`;
                    feedbackMessage.style.color = "#f87171"; // Bright Red
                }
            } catch (error) {
                feedbackMessage.textContent = "Could not connect to backend. Is your Node.js server running?";
                feedbackMessage.style.color = "#f87171";
            }
        });
    }
});
