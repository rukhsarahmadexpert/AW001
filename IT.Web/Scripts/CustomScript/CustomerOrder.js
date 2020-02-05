$('#AddNewRow').click(function () {
    var currentrow = $(this).closest("tr");

    if (isValidRow(currentrow)) {
        var $newRow = $("#CustomerOrderTable tbody #MainRow").clone().removeAttr('id');
        $('.product', $newRow).val($("#MainRow .product").val());
        $('.Driver', $newRow).val($("#MainRow .Driver").val());
        $('.vehicle', $newRow).val($("#MainRow .vehicle").val());
        $('.Quantity', $newRow).val($("#MainRow .Quantity").val());
        $('.Note', $newRow).val($("#MainRow .Note").val());
        $("#AddNewRow", $newRow).addClass('btn btn-danger removeRow').text('x').removeClass('btn btn-success').addClass('btn');
        $("#product,#Driver,#vehicle,#Quantity #Note", $newRow).removeAttr('id');

        $('#CustomerOrderTable tbody tr:last').before($newRow);

        //$('#mainrowgood #drpgoods').select2().select2('val', '0');
        $('#MainRow #product').val(0).addClass('form-control');
        $('#MainRow #Driver').val('').addClass('form-control');
        $('#MainRow #vehicle').val(0).addClass('form-control');
        $('#MainRow #Quantity').val(0).addClass('form-control');
        $('#MainRow #Note').val(0).addClass('form-control');


        // clearfield();
    }
});

$(document).on('click', '.removeRow', function () {
    var currentRow = $(this).closest('tr');
    currentRow.remove();
});

function isValidRow(currentRow) {
    var isvalid = true;
    var TransictionTypeId = 0;
    TransictionTypeId = currentRow.find('.product').val();

    if (parseInt(TransictionTypeId) == 0 || TransictionTypeId == "") {
        isvalid = false;
    }

    return isvalid;
}

$(document).ready(function () {
    $('#SaveOrder').click(function () {
        CreateGroupOrder('/CustomerOrder/CustomerGroupOrderAdd');
    });

    $('#Site').click(function () {

        var Id = $(this).val();
        if (parseInt(Id) > 0) {
            var Data = JSON.stringify({
                Id: Id
            });

            ajaxRequest("POST", "/CustomerSites/Edit", Data, "json").then(function (result)
            {
                if (result != "Failed")
                {
                    $('#Lattitue').val(result.latitude);
                    $('#Longitude').val(result.longitude);
                    $('#LocationFullUrl').val(result.Address);
                }
            })
        }
        else {
            return true;
        }
    });

});

$('.remove').click(function () {

    var currentRow = $(this).closest("tr");

    var Data = JSON.stringify({
        Id: $('#OrderId').val(),
        OrderAsignId: currentRow.find('.Id').val(),
        Quantity: $('#RequestedQuantity').val(),
        RowQuantity: currentRow.find('.Quantity').val(),
    })
    ajaxRequest("POST", "/CustomerOrder/CustomerOrderDetailsDelete", Data, "json").then(function (result) { if (result != "Failed") { window.location.reload() } });

});

function CreateGroupOrder(Url) {

    var list = [], orderItem, CurrentRow;
    var formData = new FormData();

    var TotalQuantity = 0;

    $('#CustomerOrderTable tbody tr').each(function () {
        CurrentRow: $(this).closest("tr");

        TotalQuantity = parseInt(TotalQuantity) + parseInt($(this).find('.Quantity').val());

        orderItem = {
            Id: $(this).find('.Id').val(),
            ProductId: $(this).find('.product').val(),
            VehicleId: $(this).find('.vehicle').val(),
            DriverId: $(this).find('.Driver').val(),
            OrderQuantity: $(this).find('.Quantity').val(),
            Comments: $(this).find('.Note').val(),            
        }
        list.push(orderItem);

    });

    var empObj = {
        OrderId: $('#OrderId').val(),
        Id: $('#OrderId').val(),
        RequestedQuantity: TotalQuantity,
        CustomerNote: $('#CustomerNote').val(),
        latitude: $('#Lattitue').val(),
        Longitude: $('#Longitude').val(),
        LocationFullUrl: $('#LocationFullUrl').val(),
        SiteId: $('#Site').val(),
    };

    for (var key in empObj) {
        formData.append(key, empObj[key]);
    }

    for (var i = 0; i < list.length; i++) {
        formData.append('customerOrderViewModels[' + i + '][Id]', list[i].Id),
        formData.append('customerOrderViewModels[' + i + '][VehicleId]', list[i].VehicleId),
        formData.append('customerOrderViewModels[' + i + '][DriverId]', list[i].DriverId),
        formData.append('customerOrderViewModels[' + i + '][OrderQuantity]', list[i].OrderQuantity),
        formData.append('customerOrderViewModels[' + i + '][Comments]', list[i].Comments)
        formData.append('customerOrderViewModels[' + i + '][ProductId]', list[i].ProductId)
    }
    if (list.length > 0) {

        $.ajax({
            url: Url,
            type: "POST",
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result != "Failed") {

                    list = [];
                    //swal("Good job!", "You clicked the button!", "success").dela;
                    alert('succeess');
                    window.location.href = "/CustomerOrder/Details/" + result.Id;
                     // swal("Deleted!", "Your imaginary file has been deleted.", "success");
                }
                else {
                    alert(result);
                }
            },
            error: function (errormessage) {
                alert(errormessage);
            }
        });
    }
    else {
        alert('Please Add item to list');
    }
}

$(document).ready(function () {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(success);
    } else {
        alert("There is Some Problem on your current browser to get Geo Location!");
    }

    function success(position) {
        var lat = position.coords.latitude;
        var long = position.coords.longitude;
        var city = position.coords.locality;
        var Address = position.coords.url;
        $('#Lattitue').val(lat);
        $('#Longitude').val(long);
        $('#LocationFullUrl').val(Address);

        var LatLng = new google.maps.LatLng(lat, long);
        var mapOptions = {
            center: LatLng,
            zoom: 12,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(document.getElementById("MyMapLOC"), mapOptions);
        var marker = new google.maps.Marker({
            position: LatLng,
            title: "<div style = 'height:60px;width:200px'><b>Your location:</b><br />Latitude: "
                + lat + +"<br />Longitude: " + long
        });

        marker.setMap(map);
        var getInfoWindow = new google.maps.InfoWindow({
            content: "<b>Your Current Location</b><br/> Latitude:" +
                lat + "<br /> Longitude:" + long + ""
        });
        getInfoWindow.open(map, marker);
    }

})

$('#defaultCheckedRadio').change(function ()
{
    if ($('#CustomerOrderTable tbody tr').length > 1)
    {
        alert('Is Already in Order By Vehicle');
    }
    else {
        window.location.reload();
    }

  
    
});

$('#defaultUncheckedRadio').change(function () {

        if ($('#CustomerOrderTable tbody tr').length < 2) {
            $('#AddNewRow').remove('id').hide();
            $('#Driver').empty();
            $('#vehicle').empty();
            $('#Driver').append('<option value="1" selected="selected">Bulk Driver</option>');
            $('#vehicle').append('<option value="1" selected="selected">Bulk Vehicle</option>');
        }
        else {
            $(this).attr('checked', false);
            $('#defaultCheckedRadio').attr('checked', true);
            alert('Please remove added rows first');
        }
    
});