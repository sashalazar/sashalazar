function EventsRegistration(){
	
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
}