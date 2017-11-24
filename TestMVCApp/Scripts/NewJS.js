$(".nonactive_row").on("click",
    function(event) {
        $(event.target).closest("tr").toggleClass("active_row");
    });
function show() {

    $("#window").show();
}

function clWindow() {
    $("#window").hide();
}


function clAlarm() {
    $("#alarm").hide();
}


$("#newData").click(function() {
    var a = $("#insertedData").val();
    if (isNaN(parseFloat(a)) || !isFinite(a) || parseFloat(a) <= 0 | $(".active_row").length <= 0) {
        $("#alarm").show();
    } else {

        var mass = [];
        
        $(".active_row").each(function() {
            mass.push($(this).attr("id"));
        });
        var jsonData={Massiv:mass,Price:a}
        $.ajax({
                type: "POST",
                url: "/home/NewAjaxRequest",
                dataType: "json",
                data: jsonData
            })
            .always(function() {
                location.reload();});
    }
});




   
