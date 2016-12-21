$(function () {
    $('.datetimepicker1').datetimepicker({ format: 'DD-MM-YYYY', locale: 'pl' });
    $('.datetimepicker2').datetimepicker({ format: 'DD-MM-YYYY', locale: 'pl' });
    $('.datetimepicker3').datetimepicker({ format: 'DD-MM-YYYY HH:mm', locale: 'pl' });
});


Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
};
var list = [];

var date = $('#EventStartDateTime').val().substring(0, 10);
var date2 = $('#EventEndDateTime').val().substring(0, 10);
var dataParts;
var dataParts2;
if (date.includes("-") && date2.includes("-")) {
    var start = date.replace(/-/g, ".");
    var end = date2.replace(/-/g, ".");
    date = start;
    date2 = end;
}
dataParts = date.split(".");
dataParts2 = date2.split(".");

var dateObjectForStart = new Date(dataParts[2], dataParts[1] - 1, dataParts[0]);
var dateObjectForEnd = new Date(dataParts2[2], dataParts2[1] - 1, dataParts2[0]);

while (dateObjectForStart <= dateObjectForEnd) {

    dateObjectForStart = dateObjectForStart.addDays(1);
    var myNewDate = (new Date(dateObjectForStart)).toISOString().slice(0, 10);
    list.push(myNewDate);
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

var eventId = getUrlParameter('eventId');

$(document).ready(function () {
    $('#calendar')
        .fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay,listWeek'
            },
            buttonText: {
                today: 'dziś',
                month: 'miesiąc',
                week: 'tydzień',
                day: 'dzień',
                list: 'lista'
            },
            allDayText: 'cały dzień',
            defaultView: 'month',
            allDaySlot: true,
            selectable: true,
            editable: false,
            slotMinutes: 15,
            locale: 'pl',
            timeFormat: 'H:mm',
            axisFormat: 'H:mm',
            eventBorderColor: '#51dfd4',
            ignoreTimezone: false,
            eventLimit: true,
            events: 'GetTickets?eventID='+ eventId,
            views: {
                agenda: {
                    eventLimit: 4
                }
            },
            dayRender: function (date, cell) {

                var getNormalDate = new Date(date);
                var myNewDate = (new Date(getNormalDate)).toISOString().slice(0, 10);

                list.forEach(function(item) {
                    if (myNewDate === item) {
                        cell.css("background-color", "#eafde4");
                    }
                });
            },
            eventRender: function (event, element, view) {
                console.log(event.chartKey)
                //Popover
                var url = "/Admin/GetImage?artistId=" + event.artistID;
                var input = "<h6>";
                input += event.artistName;
                input += "</h6>";
                input += '<img src="https://app.seats.io/api/chart/' + event.chartKey + '/thumbnail"/>';
                input += "<p class='bg-info' id='customBgPop' style='padding: 5px;margin-top: 10px;'>";
                input += "<span class='glyphicon glyphicon-calendar'></span>";
                input += "&nbsp; 2016-09-15 (22:00)";
                input += "</p>";
                input += "<a id='editInPop'><span class='glyphicon glyphicon-pencil' style='color: blue'></span>Edytuj</a>";
                input += "<a id='cancelInPop'><span class='glyphicon glyphicon-remove' style='color: red'></span>Anuluj</a>";

                if (view.name === "agendaWeek" || view.name === "agendaDay") {
                    if (event.allDay === true) {
                        element.find('.fc-title')
                            .append("<br/>" +
                                "<span class='glyphicon glyphicon-home'></span>" +
                                " " +
                                event.roomName +
                                "<br/>" +
                                "<span class='glyphicon glyphicon-user'></span>" +
                                " " +
                                event.creatorName);
                    } else {
                        element.find('.fc-title')
                            .append("<br/>" +
                                "<span class='glyphicon glyphicon-home'></span>" +
                                event.roomName +
                                " " +
                                "<span class='glyphicon glyphicon-user'></span>" +
                                event.creatorName);
                    }
                }
                element.popover({
                    title: event.title,
                    html: true,
                    animation: true,
                    placement: 'left',

                    content: input,
                    container: 'body'
                });

                $('body').on('click', function (e) {
                    if (!element.is(e.target) && element.has(e.target).length === 0 && $('.popover').has(e.target).length === 0)
                        element.popover('hide');
                });

                element.bind('dblclick',
                    function() {
                        var url = "/Admin/TicketEdit?ticketId=" + event.id;
                        window.location.href = url;
                    });

            },
            dayClick: dayClickCallback,
        });

    var slotDate;

    function dayClickCallback(date) {
        slotDate = date;
        $("#calendar").on("mousemove", forgetSlot);
    }

    function eventRenderCallback(event, element) {
        element.on("dblclick",
            function () {
                dblClickFunction(event.start);
            });
    }

    function forgetSlot() {
        slotDate = null;
        $("#calendar").off("mousemove", forgetSlot);
    }


    function dblClickFunction(date) {
        var clickedDate = new Date(date._d.getFullYear(), date._d.getMonth(), date._d.getDate());
        var readyClickedDate = clickedDate.getFullYear() + "-" + ("0" + (clickedDate.getMonth() +1)).slice(-2) + "-" +  ("0" + (clickedDate.getDate())).slice(-2)
        var found = $.inArray(readyClickedDate, list) > -1;
        console.log(readyClickedDate)
        console.log(list)
        console.log(found)
        if (found) {
            $('#ticketModal').modal('show');
            $('#ticketModal').on('hidden.bs.modal', function () {
                $(this).find('form')[0].reset();
                $(".chosen").trigger('chosen:updated');
            });
            $('#ticketModal').on('shown.bs.modal', function() {
                $('#Title').focus();
                $('#DateOfEvent').val(date.format("DD-MM-YYYY"));
            });
            $('#ticketModal').on('click', ".saveTicket", function (e) {
                e.preventDefault();
                $.ajax({
                    type: "POST",
                    url: "/Admin/TicketEdit",
                    data: $("#span").serialize(),
                    success: function () {
                        $('#ticketModal').modal('hide');
                        $('#calendar').fullCalendar("refetchEvents");
                    }
                });
            });
        } else {
            alert("Można dodać bilet tylko na zielonych polach");
        }
    }


    $("#calendar")
        .dblclick(function () {
            if (slotDate) {
                dblClickFunction(slotDate);
            }
        });
});
