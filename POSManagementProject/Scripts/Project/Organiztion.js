


var OrganizationDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/Organization/Details",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

var ConfirmDeleteOrganization = function (id, sl) {
    $("#confirmDeleteModal").modal("show");
    $("#hiddenOrganizationId").val(id);
    $("#hiddenOrganizationSl").val(sl);
}


var DeleteOrganization = function () {

    var id = $("#hiddenOrganizationId").val();
    var sl = $("#hiddenOrganizationSl").val();

    $.ajax({
        type: "POST",
        url: "/Organization/Delete",
        data: { id: id },
        success: function (result) {

            $("#confirmDeleteModal").modal("hide");
            $("#row_" + sl).remove();
            $.notify("Organization Deleted", "error");

            window.location.reload(true);
        }

    });

}