$(document).ready(function () {
    $('#Quantity').keyup(function () {       
        VatCount();
    });

    $('#UnitPrice').keyup(function () {       
        VatCount();
    });

    $('#VAT1').change(function () {       
        VatCount();
    });  
});

function VatCount()
{
    var price = 0.00;
    var quantity = 0;
    var total = 0.00;
    var VatAmount = 0.00;
    var GrandTotal = 0.00;

   
    $('#BookingTable1 tbody tr').each(function () {
       // var currentRow = $(this).closest("tr");
        if ($(this).find(".Quantity").val() > 0 && $(this).find(".UnitPrice").val())
        {
            price = $(this).find(".UnitPrice").val();          
            quantity = $(this).find(".Quantity").val();
            total = price * quantity;
            if ($(this).find(".VAT1").val() > 0) {
                VatAmount = ((5 * total) / 100);
            }
           
            GrandTotal = VatAmount + total;
        }
    })

    $('#gtotal').val(GrandTotal.toFixed(2));
    $('#gtotal2').text(GrandTotal.toFixed(2));
    $('#VAT').val(VatAmount.toFixed(2));
    $('#VAT2').text(VatAmount.toFixed(2));
    $('#TotalAmount').val(total.toFixed(2));

}


