

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
        url: "/ExpenseOperation/GetEmployeeList",
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
});

//..... End Index Page.....

//.....Start Jquery Table.....

var expenseArray = [];
var indexInitial = 0;
var rowSelectIndex = 0;
var updateExpenseArrayIndex = 0;


$("#addExpense").click(function () {

    if (checkExpenseInfo() == false) {
        return;
    }

    var expenseObject = getSelectedExpense();

    if ($("#addExpense").val() == "Add") {

        for (var i = 0; i < expenseArray.length; i++) {
            var expense = expenseArray[i];
            if (expense.itemValue == expenseObject.itemValue) {
                expense.quantity = expense.quantity + expenseObject.quantity;
                expense.LineTotal = expense.quantity * expense.amount;
                expenseInfoClear();
                expenseInfoShow();
                return;
            }
        }

        expenseObject.index = indexInitial++;
        expenseArray.push(expenseObject);
    }
    else if ($("#addExpense").val() == "Update") {
        expenseObject.index = rowSelectIndex;
        expenseArray.splice(updateExpenseArrayIndex, 1);
        expenseArray.splice(updateExpenseArrayIndex, 0, expenseObject);

        $("#addExpense").val('Add');
    }

    expenseInfoClear();
    expenseInfoShow();
});

$("#expenseTable tbody").on("click", ".editExpense", function () {
    var row = $(this).closest("tr");
    rowSelectIndex = row.find("td:eq(0)").text();

    for (var i = 0; i < expenseArray.length; i++) {
        var expense = expenseArray[i];
        if (expense.index == rowSelectIndex) {

            $("#itemName").val(expense.itemValue).trigger("chosen:updated");
            $("#itemQuantity").val(expense.quantity);
            $("#itemAmount").val(expense.amount);
            $("#itemReason").val(expense.reason);

            $("#addExpense").val('Update');
            updateExpenseArrayIndex = i;

            break;
        }
    }

});

$("#expenseTable tbody").on("click", ".deleteExpense", function () {
    var row = $(this).closest("tr");
    rowSelectIndex = row.find("td:eq(0)").text();
    
    for (var i = 0; i < expenseArray.length; i++) {
        var expense = expenseArray[i];
        if (expense.index == rowSelectIndex) {

            expenseArray.splice(i, 1);
            expenseInfoShow();
            expenseInfoClear();
            $("#expenseTable").notify("Item deleted",
                {
                    className: "error",
                    position: "top right"
                });
            break;
        }
    }
    
});

function expenseInfoShow() {

    var totalExpensePrice = 0;
    $("#expenseTable tbody tr").remove();

    var action = "<td class='text-center'><a class='btn btn-info btn-sm editExpense' href='#'><i class='fa fa-pencil fa-lg'></i></a> &nbsp; ";
    action += "<a class='btn btn-danger btn-sm deleteExpense' href='#'><i class='fa fa-times fa-lg'></i></a></td>";

    for (var i = 0; i < expenseArray.length; i++) {


        var indexCell = "<td style='display: none'><input type='hidden' id='index" + i + "' value='" + expenseArray[i].index + "' />" + expenseArray[i].index + "</td>";
        var slCell = "<td>" + (i + 1) + "</td>";
        var itemNameCell = "<td><input type='hidden' id='itemName" + i + "' name='ExpenseItems[" + i + "].ItemId' value='" + expenseArray[i].itemValue + "' />" + expenseArray[i].itemText + "</td>";
        var reasonCell = "<td><input type='hidden' id='itemReason" + i + "' name='ExpenseItems[" + i + "].Reason' value='" + expenseArray[i].reason + "' />" + expenseArray[i].reason + "</td>";
        var itemQtyCell = "<td><input type='hidden' id='itemQty" + i + "' name='ExpenseItems[" + i + "].Quantity' value='" + expenseArray[i].quantity + "' />" + expenseArray[i].quantity + "</td>";
        var amountCell = "<td><input type='hidden' id='itemAmount" + i + "' name='ExpenseItems[" + i + "].Amount' value='" + expenseArray[i].amount + "' />" + expenseArray[i].amount + "</td>";
        var lineTotalCell = "<td><input type='hidden' id='itemLinePrice" + i + "' name='ExpenseItems[" + i + "].LineTotal' value='" + expenseArray[i].LineTotal + "' />" + expenseArray[i].LineTotal + "</td>";


        var rowData = "<tr>" + indexCell + slCell + itemNameCell + reasonCell + itemQtyCell + amountCell + lineTotalCell + action + "</tr>";

        $("#expenseTable tbody").append(rowData);

        totalExpensePrice += expenseArray[i].LineTotal;
    }

    var lastRowData = '<tr><td colspan="5"><b>Total</b></td><td>' + totalExpensePrice + '</td><td></td></tr>';

    //var totalAmountRow = "<tr><td colspan='4'><b>Total Amount</b></td><td><input type='hidden' id='totalAmount' name='' value='" + totalExpensePrice + "' />" + totalExpensePrice + "</td><td></td></tr>";
    //var paidAmountRow = "<tr><td colspan='4'><b>Paid Amount</b></td><td><input type='textbox' class='form-control' id='paidAmount' name='' value='" + totalExpensePrice + "' /></td><td></td></tr>";
    //var dueAmountRow = "<tr><td colspan='4'><b>Due Amount</b></td><td><label class='control-label' id='dueAmount' name='' >0</label></td><td></td></tr>";

    $("#expenseTable tbody").append(lastRowData);

    $("#totalAmount").val(totalExpensePrice);
    $("#paidAmount").val(totalExpensePrice);
    $("#dueAmount").val(0);
}

function getSelectedExpense() {

    var expenseObject = {
        index: 0,
        itemText: "",
        itemValue: "",
        reason:"",
        quantity: 0,
        amount: 0,
        LineTotal: 0
    };

    expenseObject.itemText = $("#itemName").find("option:selected").text();
    expenseObject.itemValue = $("#itemName").val();
    expenseObject.reason = $("#itemReason").val();
    expenseObject.quantity = parseInt($("#itemQuantity").val());
    expenseObject.amount = parseFloat($("#itemAmount").val());
    expenseObject.LineTotal = (expenseObject.quantity * expenseObject.amount);

    return expenseObject;
}

function expenseInfoClear() {
    $("#itemName").val('').trigger("chosen:updated");
    $("#itemQuantity").val('');
    $("#itemAmount").val('');
    $("#itemReason").val('');
}

function checkExpenseInfo() {

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

    if ($("#itemAmount").val() == "") {
        $("#itemAmount").focus();
        $("#itemAmount").notify("Please fill the Amount field", 'error');
        return false;
    }

    if (!$.isNumeric($("#itemAmount").val())) {
        $("#itemAmount").val('');
        $("#itemAmount").focus();
        $("#itemAmount").notify("Please fill the correct value in Amount field", 'error');
        return false;
    }

    if ($("#itemAmount").val() <= 0) {
        $("#itemAmount").val('');
        $("#itemAmount").focus();
        $("#itemAmount").notify("Please fill the correct value in Amount field", 'error');
        return false;
    }

    if ($("#itemReason").val() == "") {
        $("#itemReason").focus();
        $("#itemReason").notify("Please fill the Reason field", 'error');
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

// Start Expense Op Result

var expenseOpExportToPdf = function () {
    var expenseNo = $("#hiddenExpenseNo").val();
    $.ajax({
        type: "POST",
        url: "/ExpenseOperation/ExportOneInfoToPdf",
        data: { expenseNo: expenseNo },
        success: function (response) {
            window.location.href = "/ExpenseOperation/Index";
        }

    });
}

$("#redirectIndex").click(function () {
    window.location.href = "/ExpenseOperation/Index";
});
// End Expense Op Result

// Start Purchase Op Report

var expenseOpReportDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/ExpenseOperation/ExpenseOpReportDetails",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

$("#expenseReportExportToPdf").click(function () {

    var branchId = $("#branchId").val();
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();

    $.ajax({
        type: "POST",
        url: "/ExpenseOperation/ExportAllInfoToPdf",
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