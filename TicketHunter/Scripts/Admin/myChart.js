
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
                    'STAGE': 'Scena',
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
                },
                onObjectSelected(object, selectedTicketType) {
                    console.log(object)
                    var url = $('#reservationInfo').data('url');
                    $.get(url, function (data) {
                        $('.reservationContent').html(data);                       
                        $('#reservationInfo').modal('show');
                    }).success(function () {
                        var input = "";
                        input += "<div>"
                        input += "<h3>Sektor:&nbsp;" + object.id + "<h3>"
                        input += object.label
                        input += object.status
                        input += "</div>"
                        console.log(input)
                        $('.reserveBody').append(input);
                        $('body').on('click', '.reserveBtn', function () {
                            $.ajax({
                                type: "POST",
                                url: "https://app.seats.io/api/reservationToken/829ec0d2-42b6-481e-86d4-b23b7f8f7691/create",
                            }).success(function (data) {
                                var reservationToken = data;
                                console.log(object.id, ticket.EventKey, ticket.PublicKey, reservationToken)
                                $.ajax({
                                    type: "POST",
                                    url: "https://app.seats.io/api/reserve",
                                    data: JSON.stringify({
                                        "objects": [object.id],
                                        "event": ticket.EventKey,
                                        "publicKey": ticket.PublicKey,
                                        "reservationToken": reservationToken
                                    }),
                                })
                            });
                        })
                    });

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