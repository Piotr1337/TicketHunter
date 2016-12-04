
$(document).ready(function () {
    var id = $('#Ticket').val();
    $.ajax({
        url: "/Chart/ChartInfo",
        dataType: 'json',
        data: {
            ticketId: id
        },
        success: function (data) {
            var ticket = $.parseJSON(data)
            var chart = new seatsio.SeatingChart({
                divId: "chart",
                publicKey: ticket.PublicKey,
                event: ticket.EventKey,
                showRowLines: false,
                selectedObjectsInputName: 'chosenTickets',
                objectColor: function (object, defaultColor, extraConfig) {
                    if (object.selected) {
                        $(this).css('border-solor', 'green')
                        return 'white';
                    }
                    return defaultColor;
                },
                messages: {
                    'STAGE': 'Scena',
                    'ORGAN': 'Orgue'
                },
                language: 'pl',
                showLegend: true,
                tooltipText: function (object) {
                    boj = object;
                    var tooltip = "";
                    if (object.labels.section !== null)
                    {
                        tooltip += '<strong>Sektor:</strong>&nbsp;' + object.labels.section + '<br/>';
                    }
                    tooltip += '<strong>Rząd:</strong>&nbsp;' + object.labels.parent + '<br/>';
                    tooltip += '<strong>Miejsce:</strong>&nbsp;' + object.labels.own;

                    return tooltip;
                },
                tooltipStyle:
                    'max-width: 400px; max-height: 200px; font-size: 20px; color: black;background-color: white; border: 2px solid black',   
                onChartRendered: function (chart) {
                    $("#chart").css("border", "1px solid #bababa");
                },
                
                onObjectClicked(object, selectedTicketType) {
                    if (object.status === 'free') {
                        var url = $('#reservationInfo').data('url');
                        $.get(url, function (data) {
                            $('.reservationContent').html(data);
                            $('#reservationInfo').modal('show');
                        }).success(function () {
                            var input = "";
                            input += "<div id='reserveDetails'>"
                            if (object.labels.section !== null) {
                                input += "<strong class='reservationFonts'>Sektor:&nbsp;</strong><span class='reservationFonts2'>" + object.labels.section + "</span><br/>"
                            }
                            input += "<strong class='reservationFonts'>Rząd:&nbsp;</strong><span class='reservationFonts2'>" + object.labels.parent + "</span><br/>"
                            input += "<strong class='reservationFonts'>Miejsce:&nbsp;</strong><span class='reservationFonts2'>" + object.labels.own + "</span><br/>"
                            input += "<input type='hidden' id='currentObject' name='object' data-uuid=" + object.uuid + " data-label-parent=" + object.labels.parent + " data-label-own=" + object.labels.own + " data-label-category='" + object.category.label + "'/>"
                            input += "</div>"
                            $('.reserveBody').prepend(input);
                            $('.reserveBtn').on('click', function () {
                                var numberOfTIckets = $('.ticketCount :selected').val()
                                if (numberOfTIckets == 1) {
                                //    console.log('multiple')
                                //    multipleReservation(object, ticket, numberOfTIckets)
                                //} else {
                                    console.log(numberOfTIckets)
                                    console.log('single')
                                    singleReservation(object, ticket)
                                }
                            })
                        });
                    }
                },
                onObjectSelected(object, selectedTicketType) {
                    $('#reservationInfo').one('shown.bs.modal', function () {
                        //var rowNumber = $('#reserveDetails').find('#currentObject').attr('data-label-parent');
                        //var seatNumber = $('#reserveDetails').find('#currentObject').attr('data-label-own');
                        //var category = $('#reserveDetails').find('#currentObject').attr('data-label-category');
                        //var uuid = $('#reserveDetails').find('#currentObject').attr('data-uuid');
                        $('.ticketCount').on('change', function () {
                            var count = $(this).val();
                            console.log('drop' + count)
                            var labelObjectArray = [];
                            var result = parseInt(object.labels.own);
                            for (var i = 0; i < count; i++) {
                                result += 1;
                                console.log(result)
                                labelObjectArray.push([object.labels.parent + '-' + result])
                            }
                            console.log(labelObjectArray)
                            chart.clearSelection();
                            chart.selectObjects(labelObjectArray)
                            $('.reserveBtn').on('click', function () {
                                var labelStringArray = [];
                                $.each(labelObjectArray, function (index, value) {
                                    $.each(value, function (index, value) {
                                        labelStringArray.push(value)
                                    })
                                })
                                multipleReservation(labelStringArray, ticket)
                            })
                            
                        })
                    })
                    console.log(object)
                }
            }).render();
            $('#reservationInfo').on('hidden.bs.modal', function () {
                chart.clearSelection();
            })
            $('.closeRsrv').on('click', function () {
                chart.clearSelection();
            })
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
    function multipleReservation(objectArray, ticket) {
        $.ajax({
            type: "POST",
            url: "https://app.seats.io/api/reservationToken/829ec0d2-42b6-481e-86d4-b23b7f8f7691/create",
        }).success(function (reservationToken) {
            $.ajax({
                type: "POST",
                url: "https://app.seats.io/api/reserve",
                data: JSON.stringify({
                    "objects": objectArray,
                    "event": ticket.EventKey,
                    "publicKey": ticket.PublicKey,
                    "reservationToken": reservationToken
                }),
            }).success(function () {
                $('#reservationInfo').modal('hide');
            })
        });
    }


    function BestAvaiableSeatReservation(object, ticket, number) {
        $.ajax({
            type: "POST",
            url: "https://app.seats.io/api/reservationToken/829ec0d2-42b6-481e-86d4-b23b7f8f7691/create",
        }).success(function (reservationToken) {
            $.ajax({
                type: "POST",
                url: "https://app.seats.io/api/reserve",
                data: JSON.stringify({
                    "bestAvailable": { number: number },
                    "event": ticket.EventKey,
                    "publicKey": ticket.PublicKey,
                    "reservationToken": reservationToken
                }),
            }).success(function () {
                $('#reservationInfo').modal('hide');
            })
        }
    )
    }

});