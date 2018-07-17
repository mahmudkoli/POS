
$.notify.defaults({ autoHideDelay: 2000 });

$("#stockReportExportToPdf").click(function () {

    var branchId = $("#branchId").val();

    $.ajax({
        type: "POST",
        url: "/StockReport/ExportAllInfoToPdf",
        data: { branchId: branchId },
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

    return true;
}