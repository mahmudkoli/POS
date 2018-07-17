

var BranchDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/Branch/Details",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

var ConfirmDeleteBranch = function (id, sl) {
    $("#confirmDeleteModal").modal("show");
    $("#hiddenBranchId").val(id);
    $("#hiddenBranchSl").val(sl);
}


var DeleteBranch = function () {

    var id = $("#hiddenBranchId").val();
    var sl = $("#hiddenBranchSl").val();

    $.ajax({
        type: "POST",
        url: "/Branch/Delete",
        data: { id: id },
        success: function (result) {

            $("#confirmDeleteModal").modal("hide");
            $("#row_" + sl).remove();
            $.notify("Branch Deleted", "error");

            window.location.reload(true);
        }

    });

}