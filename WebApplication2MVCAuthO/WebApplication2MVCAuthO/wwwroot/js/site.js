// Write your JavaScript code.
$(document).ready(function () {
    OnDocumentLoad();
});

$(document).ajaxComplete(function () {
    OnDocumentLoad();
});


var sbmObj;
function GetClientGeoLocation(obj) {
    sbmObj = obj;
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(clientGeoSuccess, error);
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false);
    }
}

var sbmDrObj;
function GetDriverGeoLocation(obj) {
    sbmDrObj = obj;
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(driverGeoSuccess, error);
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false);
    }
}

function clientGeoSuccess(position) {
    var pos = {
        lat: position.coords.latitude,
        lng: position.coords.longitude
    };
    //if (pos.lat && pos.lng)     
    $.ajax({
        url: "api/ClientRequest",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            Latitude: pos.lat,
            Longitude: pos.lng,
            User: { Id: sbmObj.elements["ClientRequestModel.UserId"].value }
        }),
        success: function (data) {
            getDriverLocationsTimeOut(data.id);
        }
    });
}

function driverGeoSuccess(position) {
    var pos = {
        lat: position.coords.latitude,
        lng: position.coords.longitude
    };
    //if (pos.lat && pos.lng)     
    $.ajax({
        url: "DriverLocation/Create",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            Latitude: pos.lat,
            Longitude: pos.lng,
            User: { Id: sbmDrObj.elements["DriverLocationModel.UserId"].value }
        }),
        success: function (data) {
            //id2 = data.id;
            //latitude2 = data.latitude;
            //longitude2 = data.longitude;
            $(div_body_content).html(data);
        }
    });
}

function error() {
    handleLocationError(true);
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    console.log(browserHasGeolocation
        ? 'Error: The Geolocation service failed.'
        : 'Error: Your browser doesn\'t support geolocation.');
}

var request_count = 0;
var max_request_count = 15;
var timerId = null;

function getDriverLocationsTimeOut(requestId) {
    if (isUrl(requestId)) {
        getAjaxRequestByUrl(requestId);
    } else {
        getDriverLocations(requestId);
    }
    request_count++;
    if (max_request_count >= request_count) {
        timerId = setTimeout(getDriverLocationsTimeOut, 20000, requestId);
    } else {
        clearTimeout(timerId);
    }
}

function isUrl(param) {
    if (param.url) {
        return true;
    }

    return false;
}

function getDriverLocations(requestId) {
    $.ajax({
        url: `DriverLocation/Index/${requestId}`,
        contentType: "text/plain",
        method: "GET",
        success: function (driverList) {
            $(div_body_content).html(driverList);

        }
    });
}

function getAjaxRequestByUrl(param) {
    $.ajax({
        url: param.url,
        data: "id=" + param.reqid,
        contentType: "text/plain",
        method: "GET",
        success: function (htmlResult) {
            $(div_body_content).html(htmlResult);

        }
    });
}

$.urlParam = function (name) {
    var res = null;
    var results = new RegExp('[\?&]' + name + '=([^&#]*)')
        .exec(window.location.href);

    if (results) res = results[1];
    return  res || 0;
}

var div_body_content = "div.container.body-content>div#body_content";
var ActiveMode = null;
var UrlMode = null;

function OnDocumentLoad() {
    var tabsObj = $(".tabs_menu a");
    UrlMode = $.urlParam("mode");

    tabsObj.click(function (e, data) {
        e.preventDefault();
        var tab;
        $('.tabs_menu .active').removeClass('active');
        
        if (data) {
            tab = data;
            //let reqStr = ".tabs_menu li>a[href='" + data +"']";
            //$(reqStr).addClass('active');
        } else {
            tab = $(this).attr('href');
            //$(this).addClass('active');
        }

        if (localStorage) {
            if (tab === "#tab1") {
                localStorage.setItem("ActiveMode", "Client");
            } else {
                localStorage.setItem("ActiveMode", "Driver");
                // todo -- Jquery Ajax instead of location.replace
                if (!data && (!UrlMode || UrlMode !== "Driver")) {
                    location.replace("/?mode=Driver");
                    return;
                }
            }
        }

        if (data) {
            let reqStr = ".tabs_menu li>a[href='" + data + "']";
            $(reqStr).addClass('active');
        } else {
            $(this).addClass('active');
        }

        $('.tab').not(tab).css({ 'display': 'none' });
        $(tab).fadeIn(400);
    });

    if (tabsObj) {
        ActiveMode = localStorage.getItem("ActiveMode");
        if (UrlMode) ActiveMode = UrlMode;
        if (!ActiveMode) ActiveMode = "Client";
        if (ActiveMode === "Client") {
            tabsObj.trigger("click", "#tab1");
        } else {
            tabsObj.trigger("click", "#tab2");
        }
    }
    
    //alert();

    //$(".phone").keyup(function () {
    //    var strVal = $(this).val().toString();
    //    //alert(strVal);
    //    strVal = strVal.replace(/^(\+38)\(?(\d{3})\)?[-. ]?(\d{3})[-. ]?(\d{4})$/, "$1($2)$3-$4");
    //    //alert(strVal);
    //    $(this).val(strVal);
    //});

    $(".phone").mask("+38(n00) 000-0000",
        {
            translation:
            {
                'n': {
                    pattern: /[0]/,
                    fallback: '0'
                }
            }
        });

    var id2;
    var latitude2;
    var longitude2;
    var userid2;

    $("form#form_add_driver_location").submit(function (e) {
        e.preventDefault();
        GetDriverGeoLocation(this);
    });


    $("form#form_add_client_request").submit(function (e) {
        e.preventDefault();
        GetClientGeoLocation(this);
        //$.ajax({
        //    url: "api/ClientRequest",
        //    contentType: "application/json",
        //    method: "POST",
        //    data: JSON.stringify({
        //        Latitude: this.elements["ClientRequestModel.Latitude"].value,
        //        Longitude: this.elements["ClientRequestModel.Longitude"].value, 
        //        User: { Id: this.elements["ClientRequestModel.UserId"].value }
        //    }),
        //    success: function(data) {
        //        getDriverLocationsTimeOut(data.id);
        //    }
        //});
    });

    //update client request status to Closed
    //and return to home index view
    function closeClientRequest(data) {
        $.ajax({
            url: `/api/ClientRequest/${data.reqid}`,
            contentType: "application/json",
            method: "PUT",
            data: JSON.stringify({
                Id: data.reqid,
                Status:"Closed"
            }),
            success: function (result) {
                document.location = data.url;
            }
        });
    }

    //return from driver details page to the driver locations list
    //location - Views\DriverLocation\Details.cshtml
    $('a#backToDrList').click(function (e) {              
        var reqid = $(this).attr("data-reqid");
        var url = $(this).attr("data-url");
        var param = { reqid: reqid, url: url };
        if (url) {
            getDriverLocationsTimeOut(param);
        }

    });

    //cancel client request - return from driver list to client home page
    //location - Views\DriverLocation\Index.cshtml
    $('a#cancelClientRequest').click(function (e) {
        var reqid = $(this).attr("data-reqid");
        var url = $(this).attr("data-url");
        var param = { reqid: reqid, url: url };
        if (url) {
            closeClientRequest(param);
        }

    });

    //cancel Driver Location - update driver location status to Closed
    $('a#updateDriverLocation').click(function (e) {
        var reqid = $(this).attr("data-reqid");
        var status = $(this).attr("data-status");
        var param = { reqid: reqid, status: status };
        if (status) {
            if (status.indexOf("Closed") > -1) {
                clearTimeout(locTimerId);
                UpdDriverGeoLocation(param);
            } else {
                updateDriverLocationTimeOut(param);
            }
        }

    });

    //create client order - returns confirmation + driver details 
    //location - Views\DriverLocation\Details.cshtml
    $('a#createOrder').click(function (e) {
        var reqid = $(this).attr("data-reqid");
        var drlocid = $(this).attr("data-drlocid");
        var url = $(this).attr("data-url");
        var param = { reqid: reqid, drlocid: drlocid, url: url };
        if (url) {
            CreateOrder(param);
        }

    });

    function onclick1() {
        
    }
}

function CreateOrder(param) {
    $.ajax({
        url: param.url,
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            DriverLocation: { Id: param.drlocid },
            ClientRequest: { Id: param.reqid },
            Status: "Open"
        }),
        success: function (data) {
            $(div_body_content).html(data);
        }
    });
}

var locTimerId = null;
var location_count = 0;

function updateDriverLocationTimeOut(params) {
    if (params.status !== "Closed") {
        UpdDriverGeoLocation(params);
    }
    else {
        clearTimeout(locTimerId);
        return;
    }

    locTimerId = setTimeout(updateDriverLocationTimeOut, 20000, params);
}

var updDrObj;
function UpdDriverGeoLocation(obj) {
    updDrObj = obj;
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(updDriverGeoSuccess, error);
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false);
    }
}

function updDriverGeoSuccess(position) {
    var pos = {
        lat: position.coords.latitude,
        lng: position.coords.longitude
    };
    //if (pos.lat && pos.lng)     
    updateDriverLocation(updDrObj, pos);
}
//update driver location status and other
//and return to the same view
function updateDriverLocation(data, pos) {
    $.ajax({
        url: `DriverLocation/Edit/${data.reqid}`,
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            Id: data.reqid,
            Status: data.status,
            Latitude: pos.lat,
            Longitude: pos.lng
        }),
        success: function (htmlResult) {
            $(div_body_content).html(htmlResult);
        },
        error: function (response, q, t) {
            var r = jQuery.parseJSON(response.responseText);
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        //handle error here
        var ppp = jqXHR.responseJSON.message;
    });

}
