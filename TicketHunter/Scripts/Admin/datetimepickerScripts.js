jQuery.validator.methods["date"] = function (value, element) { return true; }
$(function () {
    $('#DateOfEvent').datetimepicker({ format: 'DD-MM-YYYY', locale: 'pl' });
    $('#TimeOfEvent').datetimepicker({ format: 'LT', locale: 'pl', stepping: 30 });
});