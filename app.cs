document.addEventListener('DOMContentLoaded', () => {
    const menuToggle = document.querySelector('.menu-toggle');
    const navContainer = document.querySelector('.nav-container');
    const navLinks = document.querySelectorAll('.nav-link');

    // Manage Mobile Navigation Menu Toggle
    if (menuToggle && navContainer) {
        menuToggle.addEventListener('click', () => {
            navContainer.classList.toggle('active');
            const isOpen = navContainer.classList.contains('active');
            menuToggle.textContent = isOpen ? '✕' : '☰';
        });
    }

    // Interactive active links highlighter on click
    navLinks.forEach(link => {
        link.addEventListener('click', () => {
            navLinks.forEach(nav => nav.classList.remove('active'));
            link.classList.add('active');
            
            // Auto close mobile menu drawer upon selection
            if (window.innerWidth <= 768) {
                navContainer.classList.remove('active');
                menuToggle.textContent = '☰';
            }
        });
    });
});
