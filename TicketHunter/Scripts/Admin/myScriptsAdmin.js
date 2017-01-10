


$('#showAllEvents').click(function () {
    $('#allArtistsAdmin').slideUp("fast");
    $('#allBandsAdmin').slideUp("fast");
    $('#allEventsAdmin').slideToggle("medium");
});

$('#showAllArtists').click(function () {
    $('#allEventsAdmin').slideUp("fast");
    $('#allBandsAdmin').slideUp("fast");
    $('#allArtistsAdmin').slideToggle("medium");
});

$('#showAllBands').click(function () {
    $('#allEventsAdmin').slideUp("fast");
    $('#allArtistsAdmin').slideUp("fast");
    $('#allBandsAdmin').slideToggle("medium");
});

(function () {
    var bLazy = new Blazy({
        selector: 'img',
        offset: 100,
        success: function (element) {
            setTimeout(function () {
                var parent = element.parentNode;
                parent.className = parent.className.replace(/\bloading\b/, '');
            }, 200);
        }
    });
})();

//$('body').on('click', '#deleteEvent', function () {
//    var eventId = $(this).data('eventid');
//    swal({
//        title: "Jesteś pewny?",
//        text: "Nie będziesz w stanie odzyskać tego wydarzenia!",
//        type: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#DD6B55",
//        confirmButtonText: "Tak, usuń!",
//        cancelButtonText: "Anuluj",
//        closeOnConfirm: false
//    }, function (isConfirm) {
//        if (isConfirm) {
//            var id = $("tr[data-rowid='" + eventId + "']");
//            if (id.length > 0) {
//                id.remove();
//            }
//                $.ajax({
//                    method: "POST",
//                    url: "/Admin/Delete",
//                    data: {
//                        eventId: eventId
//                    }
//            }).done(function () {
//                swal({
//                    title: "Usunięto!",
//                    text: "Wydarzenie zostało usnięte",
//                    type: "success",
//                    confirmButtonText: "Ok",
//                    confirmButtonColor: "#337ab7",
//                });
//            });
//        }
//    });
//})

$('body').on('click', '#deleteArtist', function () {
    var artistId = $(this).data('artistid');
    swal({
        title: "Jesteś pewny?",
        text: "Nie będziesz w stanie odzyskać tego artysty/zespołu!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Tak, usuń!",
        cancelButtonText: "Anuluj",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            $.ajax({
                method: "POST",
                url: "/Admin/ArtistDelete",
                data: {
                    artistId: artistId
                }
            }).done(function () {
                swal({
                    title: "Usunięto!",
                    text: "Wydarzenie zostało usnięte",
                    type: "success",
                    confirmButtonText: "Ok",
                    confirmButtonColor: "#337ab7",
                });
                $('.sa-confirm-button-container').on('click', '.confirm', function () {
                    var id = $("tr[data-rowid='" + artistId + "']");
                    if (id.length > 0) {
                        id.remove();
                    }
                });
            });
        }
    });
})

$('body').on('click', '#deleteEvent', function () {
    var eventId = $(this).data('eventid');
    swal({
        title: "Jesteś pewny?",
        text: "Nie będziesz w stanie odzyskać tego wydarzenia!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Tak, usuń!",
        cancelButtonText: "Anuluj",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            var id = $("tr[data-rowid='" + eventId + "']");
            var pub = $("div[data-pubid='" + eventId + "']");
            if (id.length > 0) {
                id.remove();
                pub.remove();
            }
            $.ajax({
                method: "POST",
                url: "/Admin/Delete",
                data: {
                    eventId: eventId
                }
            }).done(function () {
                swal({
                    title: "Usunięto!",
                    text: "Wydarzenie zostało usnięte",
                    type: "success",
                    confirmButtonText: "Ok",
                    confirmButtonColor: "#337ab7",
                });
            });
        }
    });
})

//$('#publicate-countdown').countdown("2017/01/01", function (event) {
//    $(this).text(
//      event.strftime('%D days %H:%M:%S')
//    );
//});

countdown();

$(".eventDetails").on('click', function () {
    var id = $(this).data('eventid');
    $('#eventDetailsModal').modal('show')
    var url = $("#eventDetailsModal").data('url');
    console.log(url)
    console.log(id)
    $.get(url, { id: id }, function (data) {
        console.log(data);
        $('.eventContainer').html(data);
    })
})

$("#eventDetailsModal").on("shown.bs.modal", function () {
    countdown();
});

$('.eventThumbnail').on('click', function () {
    var id = $(".eventDetails").data('eventid');
    $('#eventDetailsModal').modal('show')
    var url = $("#eventDetailsModal").data('url');
    console.log(url)
    console.log(id)
    $.get(url, { id: id }, function (data) {
        console.log(data);
        $('.eventContainer').html(data);
    })
})

function countdown() {
    $('[data-countdown]').each(function () {
        var $this = $(this), finalDate = $(this).data('countdown');
        $this.countdown(finalDate, function (event) {
            if (event.elapsed) {
                $this.empty();

                var input = '';
                input += '<h2>'
                input += 'OPUBLIKOWANO'
                input += '</h2>'

                $this.html(input)
            }
            $this.html(event.strftime('%D dni %H:%M:%S'));
        }).on('finish.countdown', function () {
            $this.empty();

            var input = '';
            input += '<h2>'
            input += 'OPUBLIKOWANO'
            input += '</h2>'

            $this.html(input)
        });
    });
}
