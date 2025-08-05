// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (window.signalR === undefined) {
    var script = document.createElement('script');
    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/3.1.18/signalr.min.js';
    script.onload = function () {
        setupSignalR();
    };
    document.head.appendChild(script);
} else {
    setupSignalR();
}

function setupSignalR() {
    if (!window.signalR) return;
    var connection = new signalR.HubConnectionBuilder()
        .withUrl('/notificationHub')
        .build();
    connection.on('ReceiveNotification', function (message) {
        alert('Notification: ' + message);
    });
    connection.start().catch(function (err) {
        return console.error(err.toString());
    });
}

$(document).ready(function() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);

    $('a[href^="#"]').on('click', function(event) {
        var target = $(this.getAttribute('href'));
        if (target.length) {
            event.preventDefault();
            $('html, body').stop().animate({
                scrollTop: target.offset().top - 70
            }, 1000);
        }
    });

    $('form').on('submit', function() {
        var $form = $(this);
        var $submitBtn = $form.find('button[type="submit"]');
        
        if ($form[0].checkValidity()) {
            $submitBtn.prop('disabled', true);
            $submitBtn.html('<i class="fas fa-spinner fa-spin me-2"></i>Processing...');
        }
    });

    initializeNotifications();
});

let notificationHub;

function initializeNotifications() {
    notificationHub = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .withAutomaticReconnect()
        .build();

    notificationHub.on("ReceiveNotification", function (message, type) {
        showNotification(message, type);
    });

    notificationHub.start().then(function () {
        console.log("Connected to notification hub");
        
        if (typeof currentUserId !== 'undefined') {
            notificationHub.invoke("JoinUserGroup", currentUserId);
        }
    }).catch(function (err) {
        console.error("Error connecting to notification hub: ", err);
    });
}

function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px; max-width: 400px; box-shadow: 0 4px 12px rgba(0,0,0,0.15);';
    
    const icon = getNotificationIcon(type);
    
    notification.innerHTML = `
        <div class="d-flex align-items-center">
            <i class="${icon} me-2"></i>
            <div class="flex-grow-1">
                ${message}
            </div>
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    document.body.appendChild(notification);
    
    // Auto-remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

function getNotificationIcon(type) {
    switch (type) {
        case 'success':
            return 'fas fa-check-circle text-success';
        case 'error':
        case 'danger':
            return 'fas fa-exclamation-circle text-danger';
        case 'warning':
            return 'fas fa-exclamation-triangle text-warning';
        case 'info':
        default:
            return 'fas fa-info-circle text-info';
    }
}

function sendGlobalNotification(message, type = 'info') {
    if (notificationHub) {
        notificationHub.invoke("SendNotification", message, type);
    }
}

function sendPersonalNotification(userId, message, type = 'info') {
    if (notificationHub) {
        notificationHub.invoke("SendPersonalNotification", userId, message, type);
    }
}

function showSuccessNotification(message) {
    showNotification(message, 'success');
}

function showErrorNotification(message) {
    showNotification(message, 'danger');
}

function showWarningNotification(message) {
    showNotification(message, 'warning');
}

function showInfoNotification(message) {
    showNotification(message, 'info');
}

function enhanceForm(formSelector) {
    $(formSelector).on('submit', function(e) {
        const $form = $(this);
        const $submitBtn = $form.find('button[type="submit"]');
        const originalText = $submitBtn.html();
        
        if ($form[0].checkValidity()) {
            $submitBtn.prop('disabled', true);
            $submitBtn.html('<i class="fas fa-spinner fa-spin me-2"></i>Processing...');
            
            setTimeout(() => {
                $submitBtn.prop('disabled', false);
                $submitBtn.html(originalText);
            }, 3000);
        }
    });
}

function enhanceTable(tableSelector) {
    $(tableSelector).addClass('table-hover');
    
    if ($(tableSelector).length) {
        const searchInput = $('<input type="text" class="form-control mb-3" placeholder="Search...">');
        $(tableSelector).before(searchInput);
        
        searchInput.on('keyup', function() {
            const value = $(this).val().toLowerCase();
            $(tableSelector + ' tbody tr').filter(function() {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
            });
        });
    }
}

$(document).ready(function() {
    enhanceForm('form');
    enhanceTable('.table');
});
