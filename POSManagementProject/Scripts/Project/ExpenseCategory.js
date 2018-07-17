

var ExpenseCategoryDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/ExpenseCategory/Details",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

var ConfirmDeleteExpenseCategory = function (id, sl) {
    $("#confirmDeleteModal").modal("show");
    $("#hiddenExpenseCategoryId").val(id);
    $("#hiddenExpenseCategorySl").val(sl);
}


var DeleteExpenseCategory = function () {

    var id = $("#hiddenExpenseCategoryId").val();
    var sl = $("#hiddenExpenseCategorySl").val();

    $.ajax({
        type: "POST",
        url: "/ExpenseCategory/Delete",
        data: { id: id },
        success: function (result) {

            $("#confirmDeleteModal").modal("hide");
            $("#row_" + sl).remove();
            $.notify("Expense Category Deleted", "error");

            window.location.reload(true);
        }

    });

}