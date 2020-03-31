$(document).ready(function () {
    $('input[name=\'BookQuantity\']').on('change keyup click', function () {
        booking();
        vat()
    });

    $('#UnitPrice').on('change keyup click', function () {
        booking();

        vat();
    });

    vat();

});

function booking() {

    // $('#VAT1').empty();

    var price = $('#UnitPrice').val();

    var quantity = $('.Quantity').val();

    var total = $('#TotalAmount').val(price * quantity);

    $('#gtotal').val(price * quantity);
    $('#gtotal2').text(price * quantity);
}


function vat() {



            var tt = $('#TotalAmount').val()


    $('#VAT').val();
    var vt = $('#VAT1').val();
    if (vt != 0) {

        var vat_value = ((5 * tt) / 100);
        $('#VAT').val(vat_value);
        $('#VAT2').text(vat_value);
        $('#gtotal').val(parseFloat(vat_value) + parseFloat(tt));
        $('#gtotal2').text(parseFloat(vat_value) + parseFloat(tt));
        $('#VAT1').change(function () {

            var vt2 = $('#VAT1').val();
            if (vt2 == 0) {
                var vat_value = 0;
                $('#VAT').val(vat_value);
                $('#VAT2').text(vat_value);
                $('#gtotal').val(parseFloat(vat_value) + parseFloat(tt));
                $('#gtotal2').text(parseFloat(vat_value) + parseFloat(tt));
                $('#TotalAmount').val(tt);
            }
            else {

                var vat_value = ((5 * tt) / 100);
                $('#VAT').val(vat_value);
                $('#VAT2').text(vat_value);
                $('#gtotal').val(parseFloat(vat_value) + parseFloat(tt));
                $('#gtotal2').text(parseFloat(vat_value) + parseFloat(tt));

            }

        })
    }
    else {

        var vat_value = 0;
        $('#VAT').val(vat_value);
        $('#VAT2').text(vat_value);
        $('#gtotal').val(parseFloat(vat_value) + parseFloat(tt));
        $('#gtotal2').text(parseFloat(vat_value) + parseFloat(tt));
        $('#TotalAmount').val(tt);
        $('#VAT1').change(function () {

            var vt2 = $('#VAT1').val();
            if (vt2 == 0) {
                var vat_value = 0;
                $('#VAT').val(vat_value);
                $('#VAT2').text(vat_value);
                $('#gtotal').val(parseFloat(vat_value) + parseFloat(tt));
                $('#gtotal2').text(parseFloat(vat_value) + parseFloat(tt));
                $('#TotalAmount').val(tt);
            }
            else {

                var vat_value = ((5 * tt) / 100);
                $('#VAT').val(vat_value);
                $('#VAT2').text(vat_value);
                $('#gtotal').val(parseFloat(vat_value) + parseFloat(tt));
                $('#gtotal2').text(parseFloat(vat_value) + parseFloat(tt));

            }



        })
    }

}
