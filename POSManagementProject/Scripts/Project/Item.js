

$("#categoryId").change(function() {

    var categoryId = $("#categoryId").val();

    $.ajax({
        type: "POST",
        url: "/Item/GetItemFullCode",
        data: { id: categoryId },
        success: function (result) {
            $("#itemPreCode").val(result.preCode);
            $("#itemCode").val(result.code);
        },
        error: function (result) {
            $("#itemPreCode").val('');
            $("#itemCode").val('');
        }

    });

});



var ItemDetails = function (id) {

    $.ajax({
        type: "POST",
        url: "/Item/Details",
        data: { id: id },
        success: function (response) {

            $("#detailsModalContent").html(response);

            $("#detailsModal").modal("show");

        }

    });
}

var ConfirmDeleteItem = function (id, sl) {
    $("#confirmDeleteModal").modal("show");
    $("#hiddenItemId").val(id);
    $("#hiddenItemSl").val(sl);
}


var DeleteItem = function () {

    var id = $("#hiddenItemId").val();
    var sl = $("#hiddenItemSl").val();

    $.ajax({
        type: "POST",
        url: "/Item/Delete",
        data: { id: id },
        success: function (result) {

            $("#confirmDeleteModal").modal("hide");
            $("#row_" + sl).remove();
            $.notify("Item Deleted", "error");

            window.location.reload(true);
        }

    });

}

