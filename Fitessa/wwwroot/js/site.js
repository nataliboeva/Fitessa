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
