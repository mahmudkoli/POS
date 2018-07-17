
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

        return;
    }

    $.ajax({
        type: "POST",
        url: "/PurchaseOperation/GetEmployeeList",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(objData),
        dataType: "json",
        success: function (rData) {
            if (rData != undefined && rData != "") {

                $("#employeeId").removeAttr("disabled");
                $('#employeeId').empty();
                $("#employeeId").append($("<option></option>")
                    .val('').html("---Select---"));

                $.each(rData, function(key, value) {

                        $("#employeeId").append($("<option></option>")
                            .val(value.Id).html(value.Name));
                    });

            }
            else {
                $("#branchId").notify("There is no employee in the branch",'error');

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
});

//..... End Index Page.....

//.....Start Jquery Table.....

var purchaseArray = [];
var indexInitial = 0;
var rowSelectIndex = 0;
var updatePurchaseArrayIndex = 0;


$("#addPurchase").click(function () {

    if (checkPurchaseInfo() == false) {
        return;
    }

    var purchaseObject = getSelectedPurchase();

    if ($("#addPurchase").val() == "Add") {

        for (var i = 0; i < purchaseArray.length; i++) {
            var purchase = purchaseArray[i];
            if (purchase.itemValue == purchaseObject.itemValue) {
                purchase.quantity = purchase.quantity + purchaseObject.quantity;
                purchase.LineTotal = purchase.quantity * purchase.unitPrice;
                purchaseInfoClear();
                purchaseInfoShow();
                return;
            }
        }

        purchaseObject.index = indexInitial++;
        purchaseArray.push(purchaseObject);
    }
    else if ($("#addPurchase").val() == "Update") {
        purchaseObject.index = rowSelectIndex;
        purchaseArray.splice(updatePurchaseArrayIndex, 1);
        purchaseArray.splice(updatePurchaseArrayIndex, 0, purchaseObject);

        $("#addPurchase").val('Add');
    }

    purchaseInfoClear();
    purchaseInfoShow();
});

$("#purchaseTable tbody").on("click", ".editPurchase", function () {
    var row = $(this).closest("tr");
    rowSelectIndex = row.find("td:eq(0)").text();

    for (var i = 0; i < purchaseArray.length; i++) {
        var purchase = purchaseArray[i];
        if (purchase.index == rowSelectIndex) {

            $("#itemName").val(purchase.itemValue).trigger("chosen:updated");
            $("#itemQuantity").val(purchase.quantity);
            $("#itemUnitPrice").val(purchase.unitPrice);

            $("#addPurchase").val('Update');
            updatePurchaseArrayIndex = i;

            break;
        }
    }

});

$("#purchaseTable tbody").on("click", ".deletePurchase", function () {
    var row = $(this).closest("tr");
    rowSelectIndex = row.find("td:eq(0)").text();

    
    for (var i = 0; i < purchaseArray.length; i++) {
        var purchase = purchaseArray[i];
        if (purchase.index == rowSelectIndex) {
                
            purchaseArray.splice(i, 1);
            purchaseInfoShow();
            purchaseInfoClear();
            $("#purchaseTable").notify("Item deleted",
                {
                    className: "error",
                    position: "top right"
                });
            break;
        }
    }
    
});

function purchaseInfoShow() {

    var totalPurchasePrice = 0;
    $("#purchaseTable tbody tr").remove();

    var action = "<td class='text-center'><a class='btn btn-info btn-sm editPurchase' href='#'><i class='fa fa-pencil fa-lg'></i></a> &nbsp; ";
    action += "<a class='btn btn-danger btn-sm deletePurchase' href='#'><i class='fa fa-times fa-lg'></i></a></td>";

    for (var i = 0; i < purchaseArray.length; i++) {
        
        
        var indexCell = "<td style='display: none'><input type='hidden' id='index" + i + "' value='" + purchaseArray[i].index + "' />" + purchaseArray[i].index + "</td>";
        var slCell = "<td>" + (i+1) + "</td>";
        var itemNameCell = "<td><input type='hidden' id='itemName" + i + "' name='PurchaseItems[" + i + "].ItemId' value='" + purchaseArray[i].itemValue + "' />" + purchaseArray[i].itemText + "</td>";
        var itemQtyCell = "<td><input type='hidden' id='itemQty" + i + "' name='PurchaseItems[" + i + "].Quantity' value='" + purchaseArray[i].quantity + "' />" + purchaseArray[i].quantity+"</td>";
        var unitPriceCell = "<td><input type='hidden' id='itemUnitPrice" + i + "' name='PurchaseItems[" + i + "].UnitPrice' value='" + purchaseArray[i].unitPrice + "' />" + purchaseArray[i].unitPrice+"</td>";
        var lineTotalCell = "<td><input type='hidden' id='itemLinePrice" + i + "' name='PurchaseItems[" + i + "].LineTotal' value='" + purchaseArray[i].LineTotal + "' />" + purchaseArray[i].LineTotal+"</td>";


        var rowData = "<tr>" + indexCell + slCell + itemNameCell + itemQtyCell + unitPriceCell + lineTotalCell + action +"</tr>";

        $("#purchaseTable tbody").append(rowData);

        totalPurchasePrice += purchaseArray[i].LineTotal;
    }

    var lastRowData = '<tr><td colspan="4"><b>Total</b></td><td>' + totalPurchasePrice + '</td><td></td></tr>';

    //var totalAmountRow = "<tr><td colspan='4'><b>Total Amount</b></td><td><input type='hidden' id='totalAmount' name='' value='" + totalPurchasePrice + "' />" + totalPurchasePrice + "</td><td></td></tr>";
    //var paidAmountRow = "<tr><td colspan='4'><b>Paid Amount</b></td><td><input type='textbox' class='form-control' id='paidAmount' name='' value='" + totalPurchasePrice + "' /></td><td></td></tr>";
    //var dueAmountRow = "<tr><td colspan='4'><b>Due Amount</b></td><td><label class='control-label' id='dueAmount' name='' >0</label></td><td></td></tr>";

    $("#purchaseTable tbody").append(lastRowData);

    $("#totalAmount").val(totalPurchasePrice);
    $("#paidAmount").val(totalPurchasePrice);
    $("#dueAmount").val(0);
}

function getSelectedPurchase() {

    var purchaseObject = {
        index: 0,
        itemText: "",
        itemValue: "",
        quantity: 0,
        unitPrice: 0,
        LineTotal: 0
    };
    
    purchaseObject.itemText = $("#itemName").find("option:selected").text();
    purchaseObject.itemValue = $("#itemName").val();
    purchaseObject.quantity = parseInt($("#itemQuantity").val());
    purchaseObject.unitPrice = parseFloat($("#itemUnitPrice").val());
    purchaseObject.LineTotal = (purchaseObject.quantity * purchaseObject.unitPrice);

    return purchaseObject;
}

function purchaseInfoClear() {
    $("#itemName").val('').trigger("chosen:updated");
    $("#itemQuantity").val('');
    $("#itemUnitPrice").val('');
}

function checkPurchaseInfo() {

    if ($("#itemName").val() == "") {
        $("#itemName").focus();
        $('#itemName').trigger('chosen:activate');
        $("#itemName").notify("Please select the item",'error');
        return false;
    }

    if ($("#itemQuantity").val() == "") {
        $("#itemQuantity").focus();
        $("#itemQuantity").notify("Please fill the Quantity field",'error');
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


$("#paidAmount").keyup(function () {
    $("#dueAmount").val($("#totalAmount").val() - $("#paidAmount").val());
});

$("#paidAmount").change(function () {
    if (parseFloat($("#paidAmount").val()) < 0 || parseFloat($("#totalAmount").val()) < parseFloat($("#paidAmount").val())) {
        $("#paidAmount").val('');
        $("#paidAmount").focus();
        $("#paidAmount").notify("Please fill the correct value");
    }
});

$("#itemName").change(function () {

    if ($(this).val() == '') {
        $("#itemUnitPrice").val('');
        return;
    }

    var selectId = $("#itemName").val();
    var objData = { id: selectId }

    $.ajax({
        type: "POST",
        url: "../../PurchaseOperation/GetItem",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(objData),
        dataType: "json",
        success: function (rData) {
            if (rData != undefined && rData != "") {
                $("#itemUnitPrice").val(rData.CostPrice);
            }
            else {
                $("#itemUnitPrice").val(0);
            }
        }

    });

});


// Start Purchase Op Result

var purchaseOpExportToPdf = function () {
    var purchaseNo = $("#hiddenPurchaseNo").val();
    $.ajax({
        type: "POST",
        url: "/PurchaseOperation/ExportOneInfoToPdf",
        data: { purchaseNo: purchaseNo },
        success: function (response) {
            window.location.href = "/PurchaseOperation/Index";
        }

    });
}

$("#redirectIndex").click(function () {
    window.location.href = "/PurchaseOperation/Index";
});
// End Purchase Op Result

// Start Purchase Op Report

var purchaseOpReportDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/PurchaseOperation/PurchaseOpReportDetails",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

$("#purchaseReportExportToPdf").click(function() {

    var branchId = $("#branchId").val();
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();

    $.ajax({
        type: "POST",
        url: "/PurchaseOperation/ExportAllInfoToPdf",
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

// End Purchase Op Report