$('#bandOrArtist').bootstrapSwitch({

    onText: 'Zespół',
    offText: 'Artysta',
    size: 'normal',
    onInit: function (event, state) {
        if (state) {
            $('#FirstName').prop('disabled', true);
            $('#LastName').prop('disabled', true);
            $('#BandName').prop('disabled', false);
        } else {
            $('#FirstName').prop('disabled', false);
            $('#LastName').prop('disabled', false);
            $('#BandName').prop('disabled', true);
        }
    },
    onSwitchChange: function (event, state) {
        if (state) {
            $('#FirstName').prop('disabled', true);
            $('#LastName').prop('disabled', true);
            $('#BandName').prop('disabled', false).val('');
        } else {
            $('#FirstName').prop('disabled', false).val('');
            $('#LastName').prop('disabled', false).val('');
            $('#BandName').prop('disabled', true);
        }
    }
});