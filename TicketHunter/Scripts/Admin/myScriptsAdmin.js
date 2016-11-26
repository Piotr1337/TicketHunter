

//$('#bandOrArtist').on('switchChange.bootstrapSwitch', function (event, state) {
//    if (state) {
//        $('#FirstName').prop('disabled', true);
//        $('#LastName').prop('disabled', true);
//        $('#BandName').prop('disabled', false).val('');
//    } else {
//        $('#FirstName').prop('disabled', false).val('');
//        $('#LastName').prop('disabled', false).val('');
//        $('#BandName').prop('disabled', true);
//    }
//});



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
        text: "Nie będziesz w stanie odzyskać tego wydarzenia!",
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