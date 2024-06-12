$(document).ready(function () {
    // Example: Add a confirmation dialog for delete actions
    $('a.btn-danger').click(function (e) {
        if (!confirm('Are you sure you want to delete this reminder?')) {
            e.preventDefault();
        }
    });
});

