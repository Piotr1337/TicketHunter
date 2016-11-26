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
