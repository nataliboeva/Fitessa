// Dark Mode Toggle Functionality
let isDarkMode = localStorage.getItem('darkMode') === 'true';

function toggleDarkMode() {
    isDarkMode = !isDarkMode;
    localStorage.setItem('darkMode', isDarkMode);
    applyTheme();
}

function applyTheme() {
    const body = document.body;
    const themeIcon = document.getElementById('themeIcon');
    
    if (isDarkMode) {
        body.setAttribute('data-theme', 'dark');
        if (themeIcon) {
            themeIcon.className = 'fas fa-sun';
        }
    } else {
        body.removeAttribute('data-theme');
        if (themeIcon) {
            themeIcon.className = 'fas fa-moon';
        }
    }
}

// Apply theme on page load
document.addEventListener('DOMContentLoaded', function() {
    applyTheme();
    
    // Add smooth transitions for theme changes
    const style = document.createElement('style');
    style.textContent = `
        * {
            transition: background-color 0.3s ease, color 0.3s ease, border-color 0.3s ease;
        }
    `;
    document.head.appendChild(style);
});

// Auto-detect system preference on first visit
if (localStorage.getItem('darkMode') === null) {
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    isDarkMode = prefersDark;
    localStorage.setItem('darkMode', isDarkMode);
}

// Listen for system theme changes
window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function(e) {
    if (localStorage.getItem('darkMode') === null) {
        isDarkMode = e.matches;
        localStorage.setItem('darkMode', isDarkMode);
        applyTheme();
    }
});

// Enhanced dark mode for specific elements
function enhanceDarkMode() {
    const isDark = document.body.getAttribute('data-theme') === 'dark';
    
    // Enhance form controls
    const formControls = document.querySelectorAll('.form-control, .form-select');
    formControls.forEach(control => {
        if (isDark) {
            control.style.backgroundColor = 'var(--input-bg)';
            control.style.color = 'var(--text-primary)';
            control.style.borderColor = 'var(--border-color)';
        } else {
            control.style.backgroundColor = '';
            control.style.color = '';
            control.style.borderColor = '';
        }
    });
    
    // Enhance buttons
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        if (isDark && !button.classList.contains('btn-primary') && !button.classList.contains('btn-success') && 
            !button.classList.contains('btn-danger') && !button.classList.contains('btn-warning') && 
            !button.classList.contains('btn-info')) {
            button.style.backgroundColor = 'var(--bg-tertiary)';
            button.style.color = 'var(--text-primary)';
            button.style.borderColor = 'var(--border-color)';
        } else if (!isDark) {
            button.style.backgroundColor = '';
            button.style.color = '';
            button.style.borderColor = '';
        }
    });
}

// Apply enhanced dark mode after theme changes
const originalApplyTheme = applyTheme;
applyTheme = function() {
    originalApplyTheme();
    setTimeout(enhanceDarkMode, 100);
};

// Initialize enhanced dark mode
document.addEventListener('DOMContentLoaded', function() {
    setTimeout(enhanceDarkMode, 200);
}); 