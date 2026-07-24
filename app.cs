document.addEventListener('DOMContentLoaded', () => {
    const menuToggle = document.querySelector('.menu-toggle');
    const navContainer = document.querySelector('.nav-container');
    const navLinks = document.querySelectorAll('.nav-link');
    const contactForm = document.getElementById('api-contact-form');
    const feedbackMessage = document.getElementById('form-feedback-message');
    const skillsGrid = document.getElementById('skills-grid');

    const API_BASE_URL = 'http://localhost:5000/api';

    // 1. Mobile Navigation Menu Toggle
    if (menuToggle && navContainer) {
        menuToggle.addEventListener('click', () => {
            navContainer.classList.toggle('active');
            const isOpen = navContainer.classList.contains('active');
            menuToggle.textContent = isOpen ? '✕' : '☰';
        });
    }

    // 2. Navigation Link Active State Handler
    navLinks.forEach(link => {
        link.addEventListener('click', () => {
            navLinks.forEach(nav => nav.classList.remove('active'));
            link.classList.add('active');
            if (window.innerWidth <= 960 && navContainer) {
                navContainer.classList.remove('active');
                if (menuToggle) menuToggle.textContent = '☰';
            }
        });
    });

    // 3. Smooth Scroll Reveal Animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: "0px 0px -40px 0px"
    };

    const revealOnScroll = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('active');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    document.querySelectorAll('.scroll-reveal').forEach(el => {
        revealOnScroll.observe(el);
    });

    // 4. Dynamically Load Skills from API Server
    async function loadSkillsFromAPI() {
        if (!skillsGrid) return;

        try {
            const response = await fetch(`${API_BASE_URL}/skills`);
            const result = await response.json();

            if (response.ok && result.data) {
                skillsGrid.innerHTML = '';

                result.data.forEach((skill) => {
                    const skillCard = document.createElement('div');
                    skillCard.className = 'skill-card scroll-reveal';
                    
                    skillCard.innerHTML = `
                        <div class="skill-icon">${skill.icon}</div>
                        <h3>${skill.category}</h3>
                        <p>${skill.details}</p>
                    `;
                    skillsGrid.appendChild(skillCard);
                    revealOnScroll.observe(skillCard);
                });
            } else {
                skillsGrid.innerHTML = '<p style="color: #d90429;">Failed to load skills from API.</p>';
            }
        } catch (error) {
            skillsGrid.innerHTML = '<p style="color: #8b5a2b; font-weight: 500;">Backend server disconnected. Ensure Node.js is running on port 5000.</p>';
        }
    }

    loadSkillsFromAPI();

    // 5. Contact Form Submission Handling
    if (contactForm) {
        contactForm.addEventListener('submit', async (event) => {
            event.preventDefault();
            
            feedbackMessage.textContent = "Sending message...";
            feedbackMessage.style.color = "#8b5a2b";

            const name = document.getElementById('form-name').value;
            const email = document.getElementById('form-email').value;
            const message = document.getElementById('form-message').value;

            try {
                const response = await fetch(`${API_BASE_URL}/contact`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ name, email, message })
                });

                const result = await response.json();

                if (response.ok) {
                    feedbackMessage.textContent = "Message sent successfully!";
                    feedbackMessage.style.color = "#2b9348";
                    contactForm.reset();
                } else {
                    feedbackMessage.textContent = `Error: ${result.error}`;
                    feedbackMessage.style.color = "#d90429";
                }
            } catch (error) {
                feedbackMessage.textContent = "Could not connect to backend server.";
                feedbackMessage.style.color = "#d90429";
            }
        });
    }
});
