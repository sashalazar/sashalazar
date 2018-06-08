
$.holdReady(true);
$.getScript("http://maps.google.com/maps/api/js?key=AIzaSyDvcGAD--dM-3LVo6md0WIeLxkTkj71jEo&sensor=false&libraries=geometry", function () {
    $.holdReady(false);

    var p1 = new google.maps.LatLng(45.463688, 9.18814);
    var p2 = new google.maps.LatLng(46.0438317, 9.75936230000002);

    //alert(calcDistance(p1, p2));
});

///////////////////////.проба //////////
//$("head").append('<script src="http://maps.google.com/maps/api/js?key=AIzaSyDvcGAD--dM-3LVo6md0WIeLxkTkj71jEo&sensor=false&libraries=geometry"></script>');

//calculates distance between two points in km's
function calcDistance(p1, p2) {
    return (google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1000).toFixed(2);
}
////////////////////////////////////////