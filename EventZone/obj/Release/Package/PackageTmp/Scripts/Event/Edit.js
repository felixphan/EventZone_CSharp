$(function () {
    //Datetime Picker
    $("#dtpStartTime").datetimepicker({
        keyBinds: false
    });
    $("#dtpEndTime").datetimepicker({
        useCurrent: false,
        keyBinds: false
    });
    $('#dtpEndTime').keypress(function (event) {
        event.preventDefault();
    });
    $('#dtpStartTime').keypress(function (event) {
        event.preventDefault();
    });


    $("#dtpStartTime").on("dp.change", function (e) {
        $('#dtpEndTime').data("DateTimePicker").minDate(e.date);
    });
    $("#dtpEndTime").on("dp.change", function (e) {
        $('#dtpStartTime').data("DateTimePicker").maxDate(e.date);
    });
    function htmlEncode(value) {
        //create a in-memory div, set it's inner text(which jQuery automatically encodes)
        //then grab the encoded contents back out.  The div never exists on the page.
        return $('<div/>').text(value).html();
    }


    $('#editor').on('summernote.change', function () {
        $("#event-description").val(htmlEncode($('#editor').code()).replace(/"/g, "'"));
    });
    //Binding Locations to One Location String
    $("#btnSubmit").click(function () {
        $(this).parents("form").submit();
    });

});