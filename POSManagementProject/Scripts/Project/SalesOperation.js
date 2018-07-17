

$.notify.defaults({ autoHideDelay: 2000 });

//..... Start Index Page.....

$("#branchId").change(function () {
    var branchId = $("#branchId").val();
    var objData = { branchId: branchId }

    if ($("#branchId").val() == '') {
        $('#employeeId').empty();
        $("#employeeId").append($("<option></option>")
            .val('').html("---Select---"));
        $("#employeeId").attr("disabled", true);


        // Start Select list
        $('#itemName').empty();
        $("#itemName").append($("<option></option>")
            .val('').html("---Select---"));
        $("#itemName").attr("disabled", true);
        $("#itemName").trigger("chosen:updated");
        $('#itemName').prop('disabled', true);
        // End Select list


        return;
    }

    $.ajax({
        type: "POST",
        url: "/SalesOperation/GetEmployeeList",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(objData),
        dataType: "json",
        success: function (rData) {
            if (rData != undefined && rData != "") {

                $("#employeeId").removeAttr("disabled");
                $('#employeeId').empty();
                $("#employeeId").append($("<option></option>")
                    .val('').html("---Select---"));

                $.each(rData, function (key, value) {

                    $("#employeeId").append($("<option></option>")
                        .val(value.Id).html(value.Name));
                });

            }
            else {
                $("#branchId").notify("There is no employee in the branch", 'error');

                $('#employeeId').empty();
                $("#employeeId").append($("<option></option>")
                    .val('').html("---Select---"));
                $("#employeeId").attr("disabled", true);
            }
        },
        error: function (result) {
            alert('Oops, something bad happened !!!');
        }

    });

    itemAddedInDropDownList();
});


function itemAddedInDropDownList() {
    var branchId = $("#branchId").val();
    var objData = { branchId: branchId }

    if ($("#branchId").val() == '') {
        $('#itemName').empty();
        $("#itemName").append($("<option></option>")
            .val('').html("---Select---"));
        $("#itemName").attr("disabled", true);
        $("#itemName").trigger("chosen:updated");
        $('#itemName').prop('disabled', true);

        return;
    }

    $.ajax({
        type: "POST",
        url: "/SalesOperation/GetStockItemList",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(objData),
        dataType: "json",
        success: function (rData) {
            if (rData != undefined && rData != "") {

                $("#itemName").removeAttr("disabled");
                $('#itemName').empty();
                $("#itemName").trigger("chosen:updated");
                $('#itemName').prop('disabled', false);
                $("#itemName").append($("<option></option>")
                    .val('').html("---Select---"));

                $.each(rData, function (key, value) {

                    $("#itemName").append($("<option></option>")
                        .val(value.Id).html(value.Code + " - " + value.Name));
                });

                $("#itemName").trigger("chosen:updated");

            }
            else {
                $("#branchId").notify("There is no item in the branch", 'error');

                $('#itemName').empty();
                $("#itemName").append($("<option></option>")
                    .val('').html("---Select---"));
                $("#itemName").attr("disabled", true);
                $("#itemName").trigger("chosen:updated");
                $('#itemName').prop('disabled', true);
            }
        },
        error: function (result) {
            alert('Oops, something bad happened !!!');
        }

    });
    
}

//..... End Index Page.....

//.....Start Jquery Table.....

var salesArray = [];
var indexInitial = 0;
var rowSelectIndex = 0;
var updateSalesArrayIndex = 0;


$("#addSales").click(function () {

    if (checkSalesInfo() == false) {
        return;
    }

    var salesObject = getSelectedSales();

    if ($("#addSales").val() == "Add") {

        for (var i = 0; i < salesArray.length; i++) {
            var sales = salesArray[i];
            if (sales.itemValue == salesObject.itemValue) {
                sales.quantity = sales.quantity + salesObject.quantity;
                sales.LineTotal = sales.quantity * sales.unitPrice;
                salesInfoClear();
                salesInfoShow();
                $("#itemStockQuantity").val('');
                return;
            }
        }

        salesObject.index = indexInitial++;
        salesArray.push(salesObject);
    }
    else if ($("#addSales").val() == "Update") {
        salesObject.index = rowSelectIndex;
        salesArray.splice(updateSalesArrayIndex, 1);
        salesArray.splice(updateSalesArrayIndex, 0, salesObject);

        $("#addSales").val('Add');
    }

    salesInfoClear();
    salesInfoShow();
    $("#itemStockQuantity").val('');
});

$("#salesTable tbody").on("click", ".editSales", function () {
    var row = $(this).closest("tr");
    rowSelectIndex = row.find("td:eq(0)").text();

    for (var i = 0; i < salesArray.length; i++) {
        var sales = salesArray[i];
        if (sales.index == rowSelectIndex) {

            $("#itemName").val(sales.itemValue).trigger("chosen:updated");
            $("#itemQuantity").val(sales.quantity);
            $("#itemUnitPrice").val(sales.unitPrice);

            $("#addSales").val('Update');
            updateSalesArrayIndex = i;

            tempStockQuantityUpdateCheck();
            break;
        }
    }

});

$("#salesTable tbody").on("click", ".deleteSales", function () {
    var row = $(this).closest("tr");
    rowSelectIndex = row.find("td:eq(0)").text();
    
    for (var i = 0; i < salesArray.length; i++) {
        var sales = salesArray[i];
        if (sales.index == rowSelectIndex) {

            salesArray.splice(i, 1);
            salesInfoShow();
            salesInfoClear();
            $("#salesTable").notify("Item deleted",
                {
                    className: "error",
                    position: "top right"
                });
            break;
        }
    }
});

function salesInfoShow() {

    var totalSalesPrice = 0;
    $("#salesTable tbody tr").remove();

    var action = "<td class='text-center'><a class='btn btn-info btn-sm editSales' href='#'><i class='fa fa-pencil fa-lg'></i></a> &nbsp; ";
    action += "<a class='btn btn-danger btn-sm deleteSales' href='#'><i class='fa fa-times fa-lg'></i></a></td>";

    for (var i = 0; i < salesArray.length; i++) {


        var indexCell = "<td style='display: none'><input type='hidden' id='index" + i + "' value='" + salesArray[i].index + "' />" + salesArray[i].index + "</td>";
        var slCell = "<td>" + (i + 1) + "</td>";
        var itemNameCell = "<td><input type='hidden' id='itemName" + i + "' name='SalesItems[" + i + "].ItemId' value='" + salesArray[i].itemValue + "' />" + salesArray[i].itemText + "</td>";
        var itemQtyCell = "<td><input type='hidden' id='itemQty" + i + "' name='SalesItems[" + i + "].Quantity' value='" + salesArray[i].quantity + "' />" + salesArray[i].quantity + "</td>";
        var unitPriceCell = "<td><input type='hidden' id='itemUnitPrice" + i + "' name='SalesItems[" + i + "].UnitPrice' value='" + salesArray[i].unitPrice + "' />" + salesArray[i].unitPrice + "</td>";
        var lineTotalCell = "<td><input type='hidden' id='itemLinePrice" + i + "' name='SalesItems[" + i + "].LineTotal' value='" + salesArray[i].LineTotal + "' />" + salesArray[i].LineTotal + "</td>";


        var rowData = "<tr>" + indexCell + slCell + itemNameCell + itemQtyCell + unitPriceCell + lineTotalCell + action + "</tr>";

        $("#salesTable tbody").append(rowData);

        totalSalesPrice += salesArray[i].LineTotal;
    }

    var lastRowData = '<tr><td colspan="4"><b>Total</b></td><td>' + totalSalesPrice + '</td><td></td></tr>';

    //var totalAmountRow = "<tr><td colspan='4'><b>Total Amount</b></td><td><input type='hidden' id='totalAmount' name='' value='" + totalSalesPrice + "' />" + totalSalesPrice + "</td><td></td></tr>";
    //var paidAmountRow = "<tr><td colspan='4'><b>Paid Amount</b></td><td><input type='textbox' class='form-control' id='paidAmount' name='' value='" + totalSalesPrice + "' /></td><td></td></tr>";
    //var dueAmountRow = "<tr><td colspan='4'><b>Due Amount</b></td><td><label class='control-label' id='dueAmount' name='' >0</label></td><td></td></tr>";

    $("#salesTable tbody").append(lastRowData);

    $("#totalAmount").val(totalSalesPrice);
    $("#subTotalAmount").val(totalSalesPrice);
    $("#payableAmount").val(totalSalesPrice);
    $("#paidAmount").val(totalSalesPrice);
    $("#vat").val(0);
    $("#discountAmount").val(0);
    $("#dueAmount").val(0);
}

function getSelectedSales() {

    var salesObject = {
        index: 0,
        itemText: "",
        itemValue: "",
        quantity: 0,
        unitPrice: 0,
        LineTotal: 0
    };

    salesObject.itemText = $("#itemName").find("option:selected").text();
    salesObject.itemValue = $("#itemName").val();
    salesObject.quantity = parseInt($("#itemQuantity").val());
    salesObject.unitPrice = parseFloat($("#itemUnitPrice").val());
    salesObject.LineTotal = (salesObject.quantity * salesObject.unitPrice);

    return salesObject;
}

function salesInfoClear() {
    $("#itemName").val('').trigger("chosen:updated");
    $("#itemQuantity").val('');
    $("#itemUnitPrice").val('');
}

function checkSalesInfo() {

    if ($("#itemName").val() == "") {
        $("#itemName").focus();
        $('#itemName').trigger('chosen:activate');
        $("#itemName").notify("Please select the item", 'error');
        return false;
    }

    if ($("#itemQuantity").val() == "") {
        $("#itemQuantity").focus();
        $("#itemQuantity").notify("Please fill the Quantity field", 'error');
        return false;
    }

    if (!$.isNumeric($("#itemQuantity").val())) {
        $("#itemQuantity").val('');
        $("#itemQuantity").focus();
        $("#itemQuantity").notify("Please fill the correct value in Quantity field", 'error');
        return false;
    }

    if ($("#itemQuantity").val() < 1) {
        $("#itemQuantity").val('');
        $("#itemQuantity").focus();
        $("#itemQuantity").notify("Please fill the correct value in Quantity field", 'error');
        return false;
    }

    if (parseInt($("#itemQuantity").val()) > parseInt($("#itemStockQuantity").val())) {
        $("#itemQuantity").focus();
        $("#itemQuantity").notify("Selected quantity is more than stock status", 'error');
        return false;
    }

    if ($("#itemUnitPrice").val() == "") {
        $("#itemUnitPrice").focus();
        $("#itemUnitPrice").notify("Please fill the Unit Price field", 'error');
        return false;
    }

    if (!$.isNumeric($("#itemUnitPrice").val())) {
        $("#itemUnitPrice").val('');
        $("#itemUnitPrice").focus();
        $("#itemUnitPrice").notify("Please fill the correct value in Unit Price field", 'error');
        return false;
    }

    if ($("#itemUnitPrice").val() <= 0) {
        $("#itemUnitPrice").val('');
        $("#itemUnitPrice").focus();
        $("#itemUnitPrice").notify("Please fill the correct value in Unit Price field", 'error');
        return false;
    }

    return true;
}

//.....End Jquery Table.....

//.....Start Sales Calculation.....

function CalculationAmount() {

    var calTotalAmount = parseFloat($("#totalAmount").val());
    var calDiscountAmount = parseFloat($("#discountAmount").val());
    var calVat = parseFloat($("#vat").val());


    var calSubTotalAmount = (calTotalAmount - calDiscountAmount).toFixed(2);
    $("#subTotalAmount").val(calSubTotalAmount);

    var calPayableAmount = (parseFloat(calSubTotalAmount) + parseFloat(calSubTotalAmount * (calVat / 100))).toFixed(2);

    $("#payableAmount").val(calPayableAmount);
    $("#paidAmount").val(calPayableAmount);
    paidAmountCalculation();
}

$("#vat").keyup(function () {
    CalculationAmount();
});

$("#discountAmount").keyup(function () {
    CalculationAmount();
});

$("#paidAmount").keyup(function () {
    paidAmountCalculation();
});

$("#paidAmount").change(function () {
    if (parseFloat($("#paidAmount").val()) < 0 || parseFloat($("#payableAmount").val()) < parseFloat($("#paidAmount").val())) {
        $("#paidAmount").val('');
        $("#paidAmount").focus();
        $("#paidAmount").notify("Please fill the correct value");
    }
});

$("#vat").change(function () {
    if ($("#vat").val() == '') {
        $("#vat").val('0');
        CalculationAmount();
    }

    if (parseFloat($("#vat").val()) < 0 || parseFloat($("#vat").val()) > 100) {
        $("#vat").val('0');
        $("#vat").focus();
        $("#vat").notify("Please fill the correct value");
    }
});

$("#discountAmount").change(function () {
    if ($("#discountAmount").val() == '') {
        $("#discountAmount").val('0');
        CalculationAmount();
    }

    if (parseFloat($("#discountAmount").val()) < 0 || parseFloat($("#totalAmount").val()) < parseFloat($("#discountAmount").val())) {
        $("#discountAmount").val('0');
        $("#discountAmount").focus();
        $("#discountAmount").notify("Please fill the correct value");
    }
});

function paidAmountCalculation() {
    var calPayableAmount = parseFloat($("#payableAmount").val()).toFixed(2);
    var calPaidAmount = parseFloat($("#paidAmount").val()).toFixed(2);

    var calDueAmount = (calPayableAmount - calPaidAmount).toFixed(2);
    $("#dueAmount").val(calDueAmount);
}

//.....End Sales Calculation.....


$("#itemName").change(function () {

    if ($(this).val() == '') {
        $("#itemUnitPrice").val('');
        $("#itemStockQuantity").val('');
        return;
    }

    var branchId = 0;
    if ($("#branchId").val() != '') {
        branchId = $("#branchId").val();
    }

    var itemId = $("#itemName").val();
    var objData = { branchId: branchId, itemId: itemId }

    $.ajax({
        type: "POST",
        url: "/SalesOperation/GetItemStatus",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(objData),
        dataType: "json",
        success: function (rData) {
            if (rData != undefined && rData != "") {

                $("#itemUnitPrice").val(rData.ItemSalesPrice);

                var tempStockQuantity = rData.ItemStockQty - tempSalesQuantityCheck(itemId);

                if (tempStockQuantity > 0) {
                    $("#itemStockQuantity").val(tempStockQuantity);
                } else {
                    $("#itemStockQuantity").val(0);
                }
            }
            else {
                $("#itemUnitPrice").val(0);
                $("#itemStockQuantity").val(0);
            }
        },
        error: function (result) {
            alert('Oops, something bad happened !!!');
        }

    });


});

function tempSalesQuantityCheck(itemId) {

    for (var i = 0; i < salesArray.length; i++) {
        var sales = salesArray[i];
        if (sales.itemValue == itemId) {
            return sales.quantity;
        }
    }
    return 0;
}

function tempStockQuantityUpdateCheck() {

    var branchId = 0;
    if ($("#branchId").val() != '') {
        branchId = $("#branchId").val();
    }

    var itemId = $("#itemName").val();
    var objData = { branchId: branchId, itemId: itemId }

    $.ajax({
        type: "POST",
        url: "/SalesOperation/GetItemStatus",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(objData),
        dataType: "json",
        success: function (rData) {
            if (rData != undefined && rData != "") {

                var tempStockQuantity = rData.ItemStockQty;

                if (tempStockQuantity > 0) {
                    $("#itemStockQuantity").val(tempStockQuantity);
                } else {
                    $("#itemStockQuantity").val(0);
                }
            }
            else {
                $("#itemStockQuantity").val(0);
            }
        },
        error: function (result) {
            alert('Oops, something bad happened !!!');
        }

    });
}

// End Sales Op Index

// Start Sales Op Result

var salesOpExportToPdf = function () {
    var salesNo = $("#hiddenSalesNo").val();
    $.ajax({
        type: "POST",
        url: "/SalesOperation/ExportOneInfoToPdf",
        data: { salesNo: salesNo },
        success: function (response) {
            window.location.href = "/SalesOperation/Index";
        }

    });
}

$("#redirectIndex").click(function() {
    window.location.href = "/SalesOperation/Index";
});
// End Sales Op Result

// Start Sales Op Report

var salesOpReportDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/SalesOperation/SalesOpReportDetails",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

$("#salesReportExportToPdf").click(function () {

    var branchId = $("#branchId").val();
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();

    $.ajax({
        type: "POST",
        url: "/SalesOperation/ExportAllInfoToPdf",
        data: { branchId: branchId, fromDate: fromDate, toDate: toDate },
        success: function (response) {
            $.notify("Export Completed", "success");
        },
        error: function (response) {
            $.notify("Export Failed", "error");
        }

    });

});

function CheckReportValue() {

    if ($('#branchId').val() == '') {
        $('#branchId').focus();
        $('#branchId').notify("Please select the branch");
        return false;
    }

    if ($('#fromDate').val() == '') {
        $('#fromDate').focus();
        $('#fromDate').notify("Please select the date");
        return false;
    }

    if ($('#toDate').val() == '') {
        $('#toDate').focus();
        $('#toDate').notify("Please select the date");
        return false;
    }

    return true;
}

// End Sales Op Report

