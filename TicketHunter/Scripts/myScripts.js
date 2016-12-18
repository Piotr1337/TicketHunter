var fadeSpeed = 280;

$(document).ready(function () {
    function bindNavbar() {
        if ($(window).width() > 768) {
            $('.navbar-collapse .dropdown').on('mouseover', function () {
                $('.dropdown-toggle', this).next('.dropdown-menu').show();
            }).on('mouseout', function () {
                $('.dropdown-toggle', this).next('.dropdown-menu').hide();
            });
        }
        else {
            $('.navbar-collapse .dropdown').off('mouseover').off('mouseout');
        }
    }

    $(window).resize(function () {
        bindNavbar();
    });

    bindNavbar();

});


var options = {
    url: "/Event/AutoCompleteSearch",

    getValue: function (element) {
        return element.Name;
    },

    template: {
        type: "custom",
        method: function(value, item) {
            return "<i id='searchInputIcons' class='" + item.Icon + "'></i>&nbsp;" + "<p id='searchInputValue' data-item-id='" + item.Id + "' >" + value + "</p>";
        },
    },

    placeholder: "artysta, zespół, wydarzenie...",
    ajaxSettings: {
        dataType: "json",
        method: "POST",
        data: {
            dataType: "json"
        }
    },

    preparePostData: function (data) {
        data.phrase = $("#searchInput").val();
        return data;
    },
    list: {
        match: {
            enabled: true
        },
        maxNumberOfElements: 8,

        onClickEvent: function () {


            $('li').on('click', function() {
                var id = $(this).find("#searchInputValue").data("item-id");
                window.location = '/Event/ShowEvent?eventId=' + id;
            });
        }
    },

    theme: "square"

};
$("#searchInput").easyAutocomplete(options);

$(document).ready(function() {
    $('article').readmore({
        speed: 500,
        collapsedHeight: 380,
        heightMargin: 16,
        moreLink: '<a class="readmoreStyle" href="#">czytaj więcej...</a>',
        lessLink: '<a class="readmoreStyle" href="#">zwiń</a>'
    });
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



$('#userTabs a').click(function (e) {
    e.preventDefault();
    $(this).tab('show');
});

$('body').on('click', '#fillData', function () {
    $('#userDataDisplay').fadeOut(fadeSpeed, function() {
        $('#userDataEdit').fadeIn(fadeSpeed);
    });
});

$('body').on('click', '#back', function () {
    $('#userDataEdit').fadeOut(fadeSpeed, function () {
        $('#userDataDisplay').fadeIn(fadeSpeed);
        $('#fullUserDataDisplay').fadeIn(fadeSpeed);
    });
});

$('body').on('click', '#editData', function () {
    $('#fullUserDataDisplay').fadeOut(fadeSpeed, function () {
        $('#userDataEdit').fadeIn(fadeSpeed);
    });
});


$('body').on('click', '#addAddressBtn', function() {
    $('#addressModal').modal('show')
});

function activaTab(tabID) {
    $('.nav-tabs a[href="#' + tabID + '"]').tab('show');
};

$('body').on('click', '#saveAddress', function () {

});

$('body').on('click', '.removeAddress', function () {
    var addressId = $(this).closest('tr').attr('id');
    $.ajax({
        method: "POST",
        url: "/Account/DeleteAddress",
        data: {
            addressId: addressId
        }
    }).done(function () {
        var id = $("tr[id='" +addressId + "']");
        if (id.length > 0) {
            id.remove();
        }
    });



    //var id = $("tr[data-rowid='" + eventId + "']");
    //if (id.length > 0) {
    //    id.remove();
    //}
});
//$(function() {
//    $(".chosen-select").chosen({
//        width: "50%",
//        placeholder_text_multiple: "Państwo",
//        no_results_text: "Nic nie znaleziono",
//        max_selected_options: 2
//    });
//    $(".chosen").trigger('liszt:updated');
//});

//SOCIAL

$('body').on('click', '.faceLogin', function () {
    $('#fbForm').submit();
});

$('body').on('click', '.ticketTable', function () {
    ticketId = $(this).attr("data-ticket-id");
    console.log(ticketId);
    window.location.href = "/Chart/Chart?ticketId=" + ticketId
    //$.ajax({
    //    type: "POST",
    //    url: "/Chart/Chart",
    //    data: {
    //        ticketId: ticketId
    //    },
    //});
});

$('.eventThumbnail').on('click', function () {
    var id = $(this).find('input:hidden[name="EventID"]').val();
    window.location.href = "/Event/ShowEvent?eventId=" + id
})