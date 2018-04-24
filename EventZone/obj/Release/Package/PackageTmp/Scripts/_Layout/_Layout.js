
$(z_document).ready(function () {
    $("#Keyword").toggle("slide", 0);

    $(".d_btn_search").mouseenter(function () {
        $("#Keyword").toggle("slide");
    });
});
