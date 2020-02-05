$('#btnaddRow').click(function () {
    var currentrow = $(this).closest("tr");
    var vat = currentrow.find('.vat').val();
    //RowSubTalSubtotal(vat, currentrow);
    //CombineTotalAndSubtotal();
    //CombineTotal();
    //CalculateVateTotal();

    var isAllValid = true;
    if ($("#mainrowgood #drpgoods").val() == "0") {
        isAllValid = false;
        //swal("Let op!", "Selecteer product", "warning");
        alert('select Product');
        return;
    }

    var isAllValid = true;
    if ($("#mainrowgood .quantity").val() == 0) {
        isAllValid = false;
        alert('Add Quantity')
        // swal("Let op!", "Voer aantal in", "warning");
        return;
    }
    var isAllValid = true;
    if ($("#mainrowgood .rate").val() == 0) {
        isAllValid = false;
        // swal("Let op!", "Voer inkoopprijs in", "warning");
        alert('Add rate')
        return;
    }

    if (isAllValid) {
        var $newRow = $("#mainrowgood").clone().removeAttr('id');
        $('#drpgoods', $newRow).val($("#drpgoods").val());
        $('.vat', $newRow).val($("#mainrowgood .vat").val());
        $('.Unit', $newRow).val($("#mainrowgood .Unit ").val());
        $("#btnaddRow", $newRow).addClass('remove').val('x').removeClass('btn-success').addClass('btn-height-Remove');
        $("#drpgoods,#Quantity,#rate,#rowsubtotal,#rownettotal,#vat", $newRow).removeAttr('id');

        $('.tbodyGood tr:last').before($newRow);
        $(".rowsubtotal").prop('disabled', true);
        $(".rownettotal").prop('disabled', true);
        //$('#mainrowgood #drpgoods').select2().select2('val', '0');
        $('#mainrowgood #drpgoods').val(0);
        $('#mainrowgood #Unit').val(0);
        $('#mainrowgood #quantity').val(0.00);
        $('#mainrowgood #UPrice').val(0.00);
        $('#mainrowgood #VAT').val(0);
        $('#mainrowgood #rownettotal').val(0.00);
        $('#mainrowgood #RowSubTotal').val(0.00);
        $('#mainrowgood #Description').val('');


        // clearfield();
    }
});


function CountTotalVat() {

    var TotalVAT = 0.00;
    var GTotal = 0.00;
    var ToatWTVAT = 0.00;

    $('#orderdetailsitems .tbodyGood tr').each(function () {

        if ($(this).find(".rownettotal").val().trim() != "") {
            GTotal = parseFloat(GTotal) + parseFloat($(this).find(".rownettotal").val());
        }
        else {
            GTotal = parseFloat(GTotal);
        }

        if ($(this).find(".RowSubTotal").val().trim() != "") {
            ToatWTVAT = parseFloat(ToatWTVAT) + parseFloat($(this).find(".RowSubTotal").val());
        }
        else {
            ToatWTVAT = parseFloat(ToatWTVAT);
        }

        TotalVAT = parseFloat(GTotal) - parseFloat(ToatWTVAT);

    });
    $('#TotalVAT').text(TotalVAT);
    $('#SubTotal').text(ToatWTVAT);
    $('#gtotal').text(GTotal);
}

function IsOneDecimalPoint(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


$(document).ready(function () {
    $('#drpgoods').change(function (e) {
        //e.preventDefault();
        var currentRow = $(this).closest("tr");
        var productId = $(this).val();
        if (productId == "AddNewProduct") {
            ISQuotationInvoice = "Yes";
            GetAllProductUnit();
            $('#Updateproduct').hide();
            $('#saveproduct').show();
            $('#modalProductGoods').modal('show');
        }
        else if (parseInt(productId) > 0) {
            ProductInfoId(productId, currentRow);
        }
        else if (parseInt(productId) == 0) {
            currentRow.find('.rate').val('0');
            currentRow.find('.Unit').val(1);
        }
    });
});

function ProductInfoId(Id, currentrow) {
    if (Id == 0) {
        return true;
    }
    else if (Id > 0)
    {      
        ajaxRequest("GET", "/Product/Edit/" + Id, "", "json").then(function (result)
        {
            if (result != "Failed") {
               // $('select').chosen();
                currentrow.find('.rate').val(result.UPrice);
                currentrow.find('#Unit').val(1);
               // $('select').trigger("chosen:updated");
            }
        });
    }
}

$(document).on('click', '.remove', function () {
    var Current = $(this).closest('tr');
    Current.remove();
    CountTotalVat();
});


$(document).on("keyup", '.rate', function (evt) {
    var Currentrow = $(this).closest("tr");
    var QTY = Currentrow.find('.quantity').val();

    if (parseInt(QTY) >= 0) {

        var Total = parseInt(QTY) * parseFloat(Currentrow.find('.rate').val());

        Currentrow.find('.RowSubTotal').val(Total);
    }

    var vat = Currentrow.find('.vat').val();
    RowSubTalSubtotal(vat, Currentrow);
    CountTotalVat();
});


$(document).on("keyup", '.quantity', function (e) {

    var Currentrow = $(this).closest("tr");
    var QTY = $(this).val();

    if (parseInt(QTY) >= 0) {

        var Total = parseInt(QTY) * parseFloat(Currentrow.find('.rate').val());

        Currentrow.find('.RowSubTotal').val(Total);
    }
    var vat = Currentrow.find('.vat').val();
    RowSubTalSubtotal(vat, Currentrow);
    CountTotalVat();
});


$(document).on("change", '.vat', function () {
    var Currentrow = $(this).closest("tr");
    var vat = Currentrow.find('.vat').val();
    RowSubTalSubtotal(vat, Currentrow);
    CountTotalVat();
});


function RowSubTalSubtotal(vat, CurrentRow) {

    Total = 0;
    Total = CurrentRow.find('.RowSubTotal').val();
    if (parseInt(vat) == 0 && typeof (vat) != "undefined" && vat != "") {
        if (!isNaN(Total) && typeof (Total) != "undefined") {
            CurrentRow.find('.rownettotal  ').val(Total);
            CurrentRow.find('.rownettotal  ').val(parseFloat(Total).toFixed(2));
            return;
        }
    }

    if (!isNaN(Total) && Total != "" && typeof (vat) != "undefined") {
        var InputVatValue = parseFloat((Total / 100) * vat);
        var ValueWTV = parseFloat(InputVatValue) + parseFloat(Total);
        CurrentRow.find('.rownettotal').val(parseFloat(ValueWTV).toFixed(2));
    }
}


function validateRow(currentRow) {

    var isvalid = true;
    var productId = 0, quantityG = 0, varrateG = 0;
    productId = currentRow.find('.product').val();
    quantity = currentRow.find('.quantity ').val();
    rate = currentRow.find('.rate ').val();
    if (parseInt(productId) == 0 || productId == "") {
        isvalid = false;
    }
    if (parseInt(quantity) == 0 || quantity == "") {
        isvalid = false;
    }
    if (parseInt(rate) == 0 || rate == "") {
        isvalid = false;
    }
    return isvalid;
}


function ValidLPO() {

    var IsValid = true;

    if ($('#Venders').val() == 0) {

        $('#Venders').css('border-color', '1px solid #BDC7BC');
        alert('Please select Vender');

        IsValid = false;
    }

    else if ($('#FromDate').val().trim() == "03/19/2018") {
        $('#FromDate').css('border-color', '1px solid #BDC7BC');
        IsValid = false;
        alert('select From Date');
    }

    else if ($('#DueDate').val().trim() == "03/19/2018") {
        $('#DueDate').css('border-color', '1px solid #BDC7BC');
        IsValid = false;
        alert('select Due Date');
    }

    return IsValid;
}


