$(document).ready(function () {

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
           var ticketId = getUrlParameter('ticketId');
           getArtists(ticketId);
       });

function getArtists(id) {
    $.ajax({
        url: 'GetArtists',
        method: 'GET',
        dataType: "json",
        data: {
            ticketId: id
        }
    }).success(function (result) {
        $("#artistChosen").chosen().val(result);
        $("#artistChosen").chosen().change();
        $("#artistChosen").trigger('chosen:updated');
        console.log(result);
    }).error(function () {

    });
}