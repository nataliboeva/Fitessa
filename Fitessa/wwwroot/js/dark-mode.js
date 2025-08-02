class DarkModeManager {
    constructor() {
        this.theme = localStorage.getItem('theme') || 'light';
        this.init();
    }

    init() {
        this.applyTheme();
        this.createToggleButton();
    }

    applyTheme() {
        document.documentElement.setAttribute('data-theme', this.theme);
        localStorage.setItem('theme', this.theme);
    }

    toggleTheme() {
        this.theme = this.theme === 'light' ? 'dark' : 'light';
        this.applyTheme();
        this.updateToggleButton();
    }

    createToggleButton() {
        const navbar = document.querySelector('.navbar-nav');
        if (!navbar) return;

        const toggleButton = document.createElement('li');
        toggleButton.className = 'nav-item';
        toggleButton.innerHTML = `
            <button class="btn btn-outline-light theme-toggle" onclick="darkModeManager.toggleTheme()">
                <i class="fas fa-moon" id="theme-icon"></i>
            </button>
        `;

        navbar.appendChild(toggleButton);
        this.updateToggleButton();
    }

    updateToggleButton() {
        const icon = document.getElementById('theme-icon');
        if (icon) {
            icon.className = this.theme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
        }
    }
}

const darkModeManager = new DarkModeManager();

document.addEventListener('DOMContentLoaded', function() {
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)');
    
    if (!localStorage.getItem('theme')) {
        darkModeManager.theme = prefersDark.matches ? 'dark' : 'light';
        darkModeManager.applyTheme();
    }
}); 