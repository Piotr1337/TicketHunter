
$(document).ready(function () {
    var id = $('#Ticket').val();
    //$.get("/Chart/ChartInfo?ticketId=" + id, function (data) {
    //    console.log(data);
    $.ajax({
        url: "/Chart/ChartInfo",
        dataType: 'json',
        data: {
            ticketId: id
        },
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
            var ticket = $.parseJSON(data)
            var chart = new seatsio.SeatingChart({
                divId: "chart",
                publicKey: ticket.PublicKey,
                event: ticket.EventKey,
                tooltipStyle: 'color: white; background-color: black',
                messages: {
                    'STAGE': 'Podium',
                    'ORGAN': 'Orgue'
                },
                tooltipText: function (object) {
                    if (object.status !== 'free') {
                        return 'not free, sorry!';
                    } else {
                        return 'yep, can select it';
                    }
                },
                onChartRendered() {
                    $("#chart").css("border", "1px solid #bababa");
                }
            }).render();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(xhr.responseText);
            console.log(thrownError);
        }
    });
});