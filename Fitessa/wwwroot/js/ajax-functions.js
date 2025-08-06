$(document).ready(function() {
    initializeAjaxFunctions();
});

function initializeAjaxFunctions() {
    setupExerciseSearch();
    setupWorkoutProgramFilter();
    setupProgressChart();
    setupNotificationSystem();
    setupRealTimeUpdates();
}

function setupExerciseSearch() {
    $('#exerciseSearchForm').on('submit', function(e) {
        e.preventDefault();
        
        var searchTerm = $('#exerciseSearch').val();
        var muscleGroup = $('#muscleGroupFilter').val();
        var difficulty = $('#difficultyFilter').val();
        
        $.ajax({
            url: '/Exercises/Search',
            type: 'GET',
            data: {
                search: searchTerm,
                muscleGroup: muscleGroup,
                difficulty: difficulty
            },
            success: function(data) {
                $('#exercisesList').html(data);
                updatePagination();
            },
            error: function() {
                showNotification('Error loading exercises', 'error');
            }
        });
    });
}

function setupWorkoutProgramFilter() {
    $('.workout-filter').on('change', function() {
        var difficulty = $('#difficultyFilter').val();
        var duration = $('#durationFilter').val();
        
        $.ajax({
            url: '/WorkoutPrograms/Filter',
            type: 'GET',
            data: {
                difficulty: difficulty,
                duration: duration
            },
            success: function(data) {
                $('#workoutProgramsList').html(data);
            },
            error: function() {
                showNotification('Error filtering workout programs', 'error');
            }
        });
    });
}

function setupProgressChart() {
    if ($('#progressChart').length) {
        loadProgressData();
    }
}

function loadProgressData() {
    $.ajax({
        url: '/Dashboard/GetProgressData',
        type: 'GET',
        success: function(data) {
            updateProgressChart(data);
        },
        error: function() {
            showNotification('Error loading progress data', 'error');
        }
    });
}

function updateProgressChart(data) {
    var ctx = document.getElementById('progressChart').getContext('2d');
    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Weight Progress',
                data: data.weights,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: false
                }
            }
        }
    });
}

function setupNotificationSystem() {
    $('.notification-toggle').on('click', function() {
        var notificationId = $(this).data('id');
        
        $.ajax({
            url: '/Notification/ToggleRead',
            type: 'POST',
            data: {
                id: notificationId
            },
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function() {
                updateNotificationCount();
            },
            error: function() {
                showNotification('Error updating notification', 'error');
            }
        });
    });
}

function setupRealTimeUpdates() {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();
    
    connection.start().then(function() {
        console.log("SignalR Connected");
    }).catch(function(err) {
        console.error("SignalR Connection Error: ", err);
    });
    
    connection.on("ReceiveNotification", function(message, type) {
        showNotification(message, type);
        updateNotificationCount();
    });
}

function updateNotificationCount() {
    $.ajax({
        url: '/Notification/GetUnreadCount',
        type: 'GET',
        success: function(count) {
            $('#notificationCount').text(count);
            if (count > 0) {
                $('#notificationBadge').show();
            } else {
                $('#notificationBadge').hide();
            }
        }
    });
}

function showNotification(message, type) {
    var alertClass = type === 'error' ? 'alert-danger' : 
                    type === 'success' ? 'alert-success' : 
                    type === 'warning' ? 'alert-warning' : 'alert-info';
    
    var alertHtml = `
        <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    $('#notificationContainer').append(alertHtml);
    
    setTimeout(function() {
        $('.alert').fadeOut();
    }, 5000);
}

function updatePagination() {
    $('.pagination a').on('click', function(e) {
        e.preventDefault();
        var page = $(this).data('page');
        
        $.ajax({
            url: window.location.pathname,
            type: 'GET',
            data: {
                page: page
            },
            success: function(data) {
                $('#mainContent').html(data);
                updatePagination();
            }
        });
    });
}

function loadMealPlanDetails(mealPlanId) {
    $.ajax({
        url: '/MealPlans/GetDetails',
        type: 'GET',
        data: {
            id: mealPlanId
        },
        success: function(data) {
            $('#mealPlanModal .modal-body').html(data);
            $('#mealPlanModal').modal('show');
        },
        error: function() {
            showNotification('Error loading meal plan details', 'error');
        }
    });
}

function saveProgressLog() {
    var formData = $('#progressLogForm').serialize();
    
    $.ajax({
        url: '/ProgressLog/Create',
        type: 'POST',
        data: formData,
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function(data) {
            if (data.success) {
                showNotification('Progress log saved successfully', 'success');
                $('#progressLogModal').modal('hide');
                loadProgressData();
            } else {
                showNotification('Error saving progress log', 'error');
            }
        },
        error: function() {
            showNotification('Error saving progress log', 'error');
        }
    });
}

function exportWorkoutProgram(programId) {
    $.ajax({
        url: '/WorkoutPrograms/Export',
        type: 'GET',
        data: {
            id: programId
        },
        success: function(data) {
            var blob = new Blob([data], { type: 'application/pdf' });
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = 'workout-program.pdf';
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: function() {
            showNotification('Error exporting workout program', 'error');
        }
    });
} 