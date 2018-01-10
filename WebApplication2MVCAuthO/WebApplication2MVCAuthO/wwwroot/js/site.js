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

    var id2;
    var latitude2;
    var longitude2;
    var userid2;

    $("form#form_add_client_request").submit(function (e) {
        e.preventDefault();
        userid2 = this.elements["ClientRequestModel.UserId"].value;
        $.ajax({
            url: "api/ClientRequest",
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Latitude: this.elements["ClientRequestModel.Latitude"].value,
                Longitude: this.elements["ClientRequestModel.Longitude"].value, 
                User: { Id: this.elements["ClientRequestModel.UserId"].value }
            }),
            success: function(data) {
                //id2 = data.id;
                //latitude2 = data.latitude;
                //longitude2 = data.longitude;
                //updateMe(data);
            }
        });


        

    });

    function updateMe(data) {
        $.ajax({
            url: "api/ClientRequest/" + id2,
            contentType: "application/json",
            method: "PUT",
            data: JSON.stringify({
                Id:id2,
                Latitude: latitude2,
                Longitude: longitude2,
                User: { Id: userid2 }
            }),
            success: function (data2) {
                alert(data2.id);
                //addTableRow(data);
            }
        });
    }
});
