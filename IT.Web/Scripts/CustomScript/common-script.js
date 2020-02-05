//$(document).ajaxStart(function () {
//    $("#progress").modal('show');
//});
//$(document).ajaxComplete(function () {
//    $("#progress").modal('hide');
//});

function ajaxRequest(type, url, inputJSON, dataType) {
    var promiseObj = new Promise(function (resolve, reject) {
        $.ajax({
            type: type,
            url: url,
            data: inputJSON,
            contentType: "application/json; charset=utf-8",
            dataType: dataType,
            async: false,
            success: function (responseData) {

                resolve(responseData);
            },
            error: function (xhr, status, error) {
                reject("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText);
                alert("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
            }
        });
    });
    return promiseObj;
}


$(document).ready(function () {

    $('#SaveImage').click(function () {
        if ($("#input-file-now").get(0).files.length == 0) {
            alert('Please select ');
        }
        else {

            // Checking whether FormData is available in browser
            if (window.FormData !== undefined) {

                var fileUpload = $("#input-file-now").get(0);
                var files = fileUpload.files;

                // Create FormData object
                var fileData = new FormData();

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }

                fileData.append('Id', $('#Id').val());
                fileData.append('ImagesUrl', $('#ImagesUrl').val());
                fileData.append('UID', $('#UID').val());
                fileData.append('CompanyName', $('.ImageName').val());
                fileData.append('EntityName', $('#EntityName').val());
                fileData.append('ImageName', $('.ImageName').val());

                $.ajax({
                    url: '/Common/Common/UpdateImage',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result);
                        window.location.reload();

                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            } else {
                alert("FormData is not supported.");
            }
        }
    });

});


function ImageEdit(uds) {

    $('#ImageEditPreview').attr("src", uds);

    $('#ImagesUrl').val(uds);
    $('#myModal').modal('show');
}


function ImageDelete(Url) {
    $('#ImageEditPreview1').attr("src", Url);

    $('#ImagesUrl').val(Url);
    $('#myModalDeleteImage').modal('show');
}

function readURL(input,PreviewUrl) {
   
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#'+PreviewUrl).attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}


function MessagesesDriverAdmin() {
    $.ajax({
        url: '/IHome/MessagesesDriverAdmin',
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            alert(result + ' MessagesesDriverAdmin');
            //if (parseInt(result) > 0) {
            //    alert(result);
            //}

          
        }
    });
}

