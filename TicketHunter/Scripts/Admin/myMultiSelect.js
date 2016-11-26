$(function () {
    $("#artistChosen").chosen({
        max_selected_options: 3, width: "100%", placeholder_text_multiple: " ",
        no_results_text: "Nic nie znaleziono"
    });
    $(".chosen-deselect").chosen({ allow_single_deselect: true });
    $("#artistChosen").chosen().change();
    $("#artistChosen").trigger('liszt:updated');
});

$(function() {
    $(".countryChosen").chosen({
        width: "100%",
        placeholder_text_multiple: "Państwo",
        no_results_text: "Nic nie znaleziono",
        max_selected_options: 1
    });
    $(".countryChosen").trigger('liszt:updated');
});