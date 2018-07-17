


var PartyDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/Party/Details",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

var ConfirmDeleteParty = function (id, sl) {
    $("#confirmDeleteModal").modal("show");
    $("#hiddenPartyId").val(id);
    $("#hiddenPartySl").val(sl);
}


var DeleteParty = function () {

    var id = $("#hiddenPartyId").val();
    var sl = $("#hiddenPartySl").val();

    $.ajax({
        type: "POST",
        url: "/Party/Delete",
        data: { id: id },
        success: function (result) {

            $("#confirmDeleteModal").modal("hide");
            $("#row_" + sl).remove();
            $.notify("Party Deleted", "error");

            window.location.reload(true);
        }

    });

}
