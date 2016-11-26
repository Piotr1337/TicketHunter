$(document).ready(function () {
    $('#OtherDetails').summernote({
        height: 300,
        toolbar:
            [
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
            ],
        lang: "pl-PL"
    });
    $('#Description').summernote({
        height: 300,
        toolbar:
            [
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
            ],
        lang: "pl-PL"
    });
});