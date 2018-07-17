
$.notify.defaults({ autoHideDelay: 2000 });

$("#incomeReportExportToPdf").click(function () {

    var branchId = $("#branchId").val();
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();

    $.ajax({
        type: "POST",
        url: "/IncomeReport/ExportAllInfoToPdf",
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