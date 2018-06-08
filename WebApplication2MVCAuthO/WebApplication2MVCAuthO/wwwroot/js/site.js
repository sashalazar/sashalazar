// Write your JavaScript code.
jQuery(document).ready(function () {
    OnDocumentLoad();
});

jQuery(document).ajaxComplete(function () {
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
    jQuery.ajax({
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

var pre;

function driverGeoSuccess(position) {
    var pos = {
        lat: position.coords.latitude,
        lng: position.coords.longitude
    };
    //if (pos.lat && pos.lng)     
    jQuery.ajax({
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
            pre.close();
            var jqStr = ".page.page-current div.container.body-content>div#body_content_taxInfoPage";
            jQuery(jqStr).html(data);
            //app.router.forward(app.router.getPageEl(data));
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
var max_request_count = 150000;
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
    jQuery.ajax({
        url: `DriverLocation/Index/${requestId}`,
        contentType: "text/plain",
        method: "GET",
        success: function (driverList) {
            app.dialog.close();
            var jqStr = ".page.page-current div.container.body-content>div#body_content_carsListPage";
            jQuery(jqStr).html(driverList);

        }
    });
}

function getAjaxRequestByUrl(param) {
    jQuery.ajax({
        url: param.url,
        data: "id=" + param.reqid,
        contentType: "text/plain",
        method: "GET",
        success: function (htmlResult) {
            jQuery(div_body_content).html(htmlResult);

        }
    });
}

jQuery.urlParam = function (name) {
    var res = null;
    var results = new RegExp('[\?&]' + name + '=([^&#]*)')
        .exec(window.location.href);

    if (results) res = results[1];
    return  res || 0;
}

function getAjaxInfoPage(obj) {
    jQuery.ajax({
        url: obj.url,
        data: obj.param,
        contentType: "text/plain",
        method: "GET",
        success: function (htmlResult) {
            jQuery("div#tab2.tab div.block").html(htmlResult);

        }
    });
}

var div_body_content = "div.container.body-content>div#body_content";
var ActiveMode = null;
var UrlMode = null;

function OnDocumentLoad() {
    var tabsObj = jQuery(".tabs_menu a");
    UrlMode = jQuery.urlParam("mode");

    tabsObj.click(function (e, data) {
        e.preventDefault();
        var tab;
        jQuery('.tabs_menu .active').removeClass('active');
        jQuery('.tabs_menu .tab-link-active').removeClass('tab-link-active');
        
        if (data) {
            tab = data;
            //let reqStr = ".tabs_menu li>a[href='" + data +"']";
            //$(reqStr).addClass('active');
        } else {
            tab = jQuery(this).attr('href');
            //$(this).addClass('active');
        }

        if (localStorage) {
            if (tab === "#tab1") {
                localStorage.setItem("ActiveMode", "Client");
            } else {
                localStorage.setItem("ActiveMode", "Driver");
                
                var isDriverTabActive = jQuery("div#tab2.tab");
                // мы находимся на водительской вкладке и показана информация о водителе
                if (isDriverTabActive.length) {
                    var driverPhoneNum = jQuery("div#tab2 p#drPhNum");
                    var carType = jQuery("div#tab2 input#HiddenCarType");
                    // если телефон или машина не определены - редирект - заполнить эти поля
                    if ((driverPhoneNum.length && driverPhoneNum.text().trim() === "") || (carType.length && carType.val().trim() === "")) {
                        getAjaxInfoPage({ url:"\\", param:"mode=Driver"});
                    }
                }
                //return;
            }
        }

        if (data) {
            let reqStr = ".tabs_menu a[href='" + data + "']";
            jQuery(reqStr).addClass('active');
            jQuery(reqStr).addClass('tab-link-active');
        } else {
            jQuery(this).addClass('active');
            jQuery(this).addClass('tab-link-active');
        }
        // framework7 app
        app.toolbar.setHighlight(jQuery(".toolbar.tabbar"));

        jQuery('.tab').not(tab).css({ 'display': 'none' });
        jQuery(tab).fadeIn(400);
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

    jQuery(".phone").mask("+38(n00) 000-0000",
        {
            translation:
            {
                'n': {
                    pattern: /[0]/,
                    fallback: '0'
                }
            }
        });


    jQuery("form#form_add_client_request").unbind('submit');
    jQuery("form#form_add_client_request").submit(function (e) {
        e.preventDefault();
        GetClientGeoLocation(this);
    });

    jQuery("form#form_add_driver_location").unbind('submit');
    jQuery("form#form_add_driver_location").submit(function (e) {
        e.preventDefault();
        GetDriverGeoLocation(this);
    });

    //return from driver details page to the driver locations list
    //location - Views\DriverLocation\Details.cshtml
    jQuery("a#backToDrList").unbind('click');
    jQuery('a#backToDrList').click(function (e) {              
        var reqid = jQuery(this).attr("data-reqid");
        var url = jQuery(this).attr("data-url");
        var param = { reqid: reqid, url: url };
        if (url) {
            getDriverLocationsTimeOut(param);
        }

    });

    //cancel client request - return from driver list to client home page
    //location - Views\DriverLocation\Index.cshtml
    jQuery("a#cancelClientRequest").unbind('click');
    jQuery('a#cancelClientRequest').click(function (e) {
        var reqid = jQuery(this).attr("data-reqid");
        var url = jQuery(this).attr("data-url");
        var param = { reqid: reqid, url: url };
        if (url) {
            closeClientRequest(param);
        }

    });

    //cancel Driver Location - update driver location status to Closed
    jQuery("a#updateDriverLocation").unbind('click');
    jQuery('a#updateDriverLocation').click(function (e) {
        var reqid = jQuery(this).attr("data-reqid");
        var status = jQuery(this).attr("data-status");
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
    jQuery("a#createOrder").unbind('click');
    jQuery('a#createOrder').click(function (e) {
        var reqid = jQuery(this).attr("data-reqid");
        var drlocid = jQuery(this).attr("data-drlocid");
        var url = jQuery(this).attr("data-url");
        var param = { reqid: reqid, drlocid: drlocid, url: url };
        if (url) {
            CreateOrder(param);
        }

    });

    function onclick1() {
        
    }
}

//const element = document.querySelector('form#form_add_driver_location');
//element.addEventListener('submit', event => {
//    event.preventDefault();
//    GetDriverGeoLocation(element);
//    // actual logic, e.g. validate the form
//    //console.log('Form submission cancelled.');
//});

function CreateOrder(param) {
    jQuery.ajax({
        url: param.url,
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            DriverLocation: { Id: param.drlocid },
            ClientRequest: { Id: param.reqid },
            Status: "Open"
        }),
        success: function (data) {
            jQuery(div_body_content).html(data);
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
    jQuery.ajax({
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
            var jqStr = ".page.page-current div.container.body-content>div#body_content_taxInfoPage";
            jQuery(jqStr).html(htmlResult);
        },
        error: function (response, q, t) {
            var r = jQuery.parseJSON(response.responseText);
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        //handle error here
        var ppp = jqXHR.responseJSON.message;
    });

}

//update client request status to Closed
//and return to home index view
function closeClientRequest(data) {
    jQuery.ajax({
        url: `/api/ClientRequest/${data.reqid}`,
        contentType: "application/json",
        method: "PUT",
        data: JSON.stringify({
            Id: data.reqid,
            Status: "Closed"
        }),
        success: function (result) {
            //document.location = data.url;
        }
    });
}



function alertStopTaxing(){
    setTimeout(alertStopTaxingRun, 0);
}

function alertStopTaxingRun() {
    app.dialog.alert('<h4>Работа такси остановлена!</h4>',
        'Предупреждение',
        function () {
            params.status = "Closed";
            clearTimeout(locTimerId);
            UpdDriverGeoLocation(params);
            locTimerId = null;
        });
}

function alertStopClientRequest(){
    setTimeout(alertStopClientRequestRun, 0);
}

function alertStopClientRequestRun() {
    app.dialog.alert('<h4>Запрос на поиск такси отменен!</h4>',
        'Предупреждение',
        function () {
            clearTimeout(timerId);
            closeClientRequest(reqParams);
            timerId = null;
        });
}