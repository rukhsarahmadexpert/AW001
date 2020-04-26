$(document).ready(function () {
    var value = $('#SitesDriver').val();
    LoadOnlineDriver(value);

    $('#OnlineDriverSelectList').on('change', function () {

        var Id = $(this).val();

        if (Id < 1) {
            $('#Contact').text('');
            $('#trafficPlateNumber').text('');
            return true;
        }
        else {
            if ($('#SitesDriver').val() == "Driver") {
                ajaxRequest("POST", "/FuelTransfer/DriverAllOnlineByDriverId/" + Id, "", "json").then(function (result) {

                    if (result != "failed") {
                        $('#title').text("Driver Information");
                        $('#ContactNumber').text("Contact Number");
                        $('#AssignedVehicle').text("Assigned Vehicle");
                        $('#Contact').text(result.Contact);
                        $('#trafficPlateNumber').text(result.Nationality);

                    }
                    else {
                        alert('there is some problem');
                    }

                })
            }
            else {
                ajaxRequest("GET", "/Site/Edit/" + Id, "", "json").then(function (result) {

                    if (result != "failed") {
                        console.log(result);
                        $('#title').text("Site Information");
                        $('#ContactNumber').text("Contact Person Name");
                        $('#AssignedVehicle').text("Contact Number");
                        $('#Contact').text(result.ContactPersonName);
                        $('#trafficPlateNumber').text(result.ContactPhone);
                    }
                    else {
                        alert('there is some problem');
                    }

                })
            }
        }
    });
});


function LoadOnlineDriver(flage) {
    var Data = JSON.stringify({
        Name: flage
    })
    ajaxRequest("POST", "/AWFDriver/DriverAllOnline", Data, "json").then(function (result) {
        if (result != "failed") {
            if (result != "[]") {
                $('#OnlineDriverSelectList').empty();
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