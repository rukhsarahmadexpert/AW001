$(document).ready(function () {

    LoadOnlineDriver();

    $('#OnlineDriverSelectList').on('change', function () {

        var Id = $(this).val();

        if (Id < 1) {
            $('#Contact').text('');
            $('#trafficPlateNumber').text('');
            return true;
        }
        else {


            ajaxRequest("POST", "/FuelTransfer/DriverAllOnlineByDriverId/" + Id, "", "json").then(function (result) {

                if (result != "failed") {
                    //console.log(result);
                    $('#Contact').text(result.Contact);
                    $('#trafficPlateNumber').text(result.Nationality);

                }
                else {
                    alert('there is some problem');
                }

            })
        }

    });

});


function LoadOnlineDriver() {
    ajaxRequest("POST", "/AWFDriver/DriverAllOnline", "", "json").then(function (result) {
        if (result != "failed") {
            if (result != "[]") {

                console.log(result)
                $('#OnlineDriverSelectList').empty();
                $('#OnlineDriverSelectList').append('<option value="' + 0 + '">' + "select Driver" + '</option>');
                $.each(result, function (item, value) {
                    $('#OnlineDriverSelectList').append('<option value="' + value.Id + '">' + value.Name + '</option>');
                });
            }
        }
        else {
            alert('there is some problem');
        }
    })
}