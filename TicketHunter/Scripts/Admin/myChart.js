
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
                extraConfig: {
                    arrayCategory: category = []
                },
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
                            input += "<strong class='reservationFonts'>Miejsce:&nbsp;</strong><span class='reservationFonts2 seats'>" + object.labels.own + "</span><br/>"
                            input += "<input type='hidden' id='currentObject' name='object' data-uuid=" + object.uuid + " data-label-parent=" + object.labels.parent + " data-label-own=" + object.labels.own + " data-label-category='" + object.category.label + "'/>"
                            input += "</div>"
                            $('.reserveBody').prepend(input);
                            $('.reserveBtn').on('click', function () {
                                var numberOfTIckets = $('.ticketCount :selected').val()
                                if (numberOfTIckets == 1) {
                                //    console.log('multiple')
                                //    multipleReservation(object, ticket, numberOfTIckets)
                                //} else {
                                    singleReservation(object, ticket)
                                }
                            })
                        });
                    }
                },
                onObjectSelected(object, selectedTicketType) {
 
                    $('#reservationInfo').one('shown.bs.modal', function () {
                        $('.ticketCount').on('change', function () {
                            var count = $(this).val();
                            var labelObjectArray = [];
                            var result = parseInt(object.labels.own);                           
                            labelObjectArray.push([object.labels.parent + '-' + object.labels.own])
                            $('body').find('.seats').html('');
                            $('body').find('.seats').append(object.labels.own);
                            for (var i = 1; i < count; i++) {
                                if (object.status == 'free') {
                                    result += 1;
                                    labelObjectArray.push([object.labels.parent + '-' + result])
                                }
                                $('body').find('.seats').append(',' + result);
                            }
                            chart.clearSelection();
                            //alert(object.status)
                            //if (object.status == 'free') {
                            chart.selectObjects(labelObjectArray)
                            //} else {
                            //    alert('brak wolnych miejsc')
                            //}
                            $('.reserveBtn').unbind().on('click', function () {
                                var labelStringArray = [];
                                $.each(labelObjectArray, function (index, value) {
                                    $.each(value, function (index, value) {
                                        labelStringArray.push(value)
                                    })
                                })
                                //labelStringArray.push(object.labels.parent + '-' + object.labels.own);
                                multipleReservation(labelStringArray, ticket)
                            })
                            $('#reservationInfo').on('hidden.bs.modal', function () {
                                chart.clearSelection();
                                $(this).off('shown.bs.modal');
                            })
                            $('.closeRsrv').on('click', function () {
                                chart.clearSelection();
                                $(this).off('shown.bs.modal');
                            })
                            
                        })
                    })
                },
                objectCategory: function (object, categories, defaultCategory, extraConfig) {
                    //extraConfig.arrayCategory.push(categories)
                }
            }).render();
            $('.bestSeats').on('click', function () {
                var url = $('#reservationInfo').data('url');
                $.get(url, function (data) {
                    $('.bestAvaiableContent').html(data);
                    $('#bestAvaiable').modal('show');
                }).success(function () {
                    $('.ticketCount').on('change', function () {
                        var numberOfTIckets = $('.ticketCount :selected').val()
                            console.log(numberOfTIckets)
                            chart.selectBestAvailable({
                                'number': numberOfTIckets,
                            });
                            $('.reserveBtn').on('click', function () {
                                BestAvaiableSeatReservation(null, ticket, numberOfTIckets)
                            });
                    });
                    $('#bestAvaiable').on('hidden.bs.modal', function () {
                        chart.clearSelection();
                    })
                })
            })
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(xhr.responseText);
            console.log(thrownError);
        }
    });
    function singleReservation(object, ticket) {
        alert('single')
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
        console.log(objectArray)
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
                $('#bestAvaiable').modal('hide');
            })
        }
    )
    }
});