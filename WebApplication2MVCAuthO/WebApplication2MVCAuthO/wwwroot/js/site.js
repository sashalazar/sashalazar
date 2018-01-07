// Write your JavaScript code.
$(document).ready(function () {
    $('.tabs_menu a').click(function (e) {
        e.preventDefault();
        $('.tabs_menu .active').removeClass('active');
        $(this).addClass('active');
        var tab = $(this).attr('href');
        $('.tab').not(tab).css({ 'display': 'none' });
        $(tab).fadeIn(400);
    });


    $(".phone").keyup(function () {
        var strVal = $(this).val().toString();
        //alert(strVal);
        strVal = strVal.replace(/^(\+38)\(?(\d{3})\)?[-. ]?(\d{3})[-. ]?(\d{4})$/, "$1($2)$3-$4");
        //alert(strVal);
        $(this).val(strVal);
    });
});
