$(document).ajaxStart(function () {
    // Show image container
    $('#loaderImg').modal('show');
});
$(document).ajaxComplete(function () {
    // Hide image container
    $('#loaderImg').modal('hide');
});