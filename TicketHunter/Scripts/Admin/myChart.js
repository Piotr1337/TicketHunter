
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
                language: 'pl',
                showLegend: true,
                tooltipText: function (object) {
                    if (object.status !== 'free') {
                        return 'Miejsce zajęte!';
                    } else {
                        return 'yep, can select it';
                    }
                },
                onChartRendered() {
                    $("#chart").css("border", "1px solid #bababa");
                    //console.log($('html').find('#chartContainer'))
                    //$('#chartContainer').bind('wheel mousewheel', function (e) {
                    //    var delta;
                    //    alert('dd')
                    //    if (e.originalEvent.wheelDelta !== undefined)
                    //        delta = e.originalEvent.wheelDelta;
                    //    else
                    //        delta = e.originalEvent.deltaY * -1;

                    //    if (delta > 0) {
                    //        $("#chartContainer").css("width", "+=10");
                    //    }
                    //    else {
                    //        $("#chartContainer").css("width", "-=10");
                    //    }
                    //});
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
                        input += "<strong class='reservationFonts'>Sektor:&nbsp;" + object.id + "</strong>"
                        input += "</div>"
                        console.log(input)
                        $('.reserveBody').prepend(input);
                        $('body').on('click', '.reserveBtn', function () {
                            var numberOfTIckets = $('.ticketCount :selected').val()
                            if (numberOfTIckets > 1)
                            {
                                multipleReservation(object, ticket, numberOfTIckets)
                            } else {
                                singleReservation(object, ticket)
                            }
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

    function singleReservation(object, ticket) {
        $.ajax({
            type: "POST",
            url: "https://app.seats.io/api/reservationToken/829ec0d2-42b6-481e-86d4-b23b7f8f7691/create",
        }).success(function (reservationToken) {
            $.ajax({
                type: "POST",
                url: "https://app.seats.io/api/reserve",
                data: JSON.stringify({
                    "objects": [object.id],
                    "event": ticket.EventKey,
                    "publicKey": ticket.PublicKey,
                    "reservationToken": reservationToken
                }),
            }).success(function () {
                $('#reservationInfo').modal('hide');
            })
        });
    }

    function multipleReservation(object, ticket, number) {
        $.ajax({
            type: "POST",
            url: "https://app.seats.io/api/changeStatus",
            data: JSON.stringify({
                "bestAvailable": { number: number },
                "eventKey": ticket.EventKey,
                'secretKey': '9081cdd9-d70c-43e8-87ba-41ec8778c518',
                "status": 'reserved'
            }),
        }).success(function () {
            $('#reservationInfo').modal('hide');
        })
    }
   
});