

var EmployeeDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/Employee/Details",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

var ConfirmDeleteEmployee = function (id, sl) {
    $("#confirmDeleteModal").modal("show");
    $("#hiddenEmployeeId").val(id);
    $("#hiddenEmployeeSl").val(sl);
}


var DeleteEmployee = function () {

    var id = $("#hiddenEmployeeId").val();
    var sl = $("#hiddenEmployeeSl").val();

    $.ajax({
        type: "POST",
        url: "/Employee/Delete",
        data: { id: id },
        success: function (result) {

            $("#confirmDeleteModal").modal("hide");
            $("#row_" + sl).remove();
            $.notify("Employee Deleted", "error");

            window.location.reload(true);
        }

    });

}