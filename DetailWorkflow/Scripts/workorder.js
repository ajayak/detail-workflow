$(function() {
    $("a[data-modal]").on("click", function() {
        $("#partsModalContent").load(this.href, function() {
            $("#partsModal").modal({ keyboard: true }, "show");
        });
        return false;
    });
});