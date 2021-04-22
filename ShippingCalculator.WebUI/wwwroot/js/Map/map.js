var startlat = " ",
    startlon = " ",
    _firstLatLng,
    _firstPoint,
    _secondLatLng,
    _secondPoint,
    _length,
    _polyline

var options = {
    center: [startlat, startlon],
    zoom: 2
}

document.getElementById('lat').value = startlat;
document.getElementById('lon').value = startlon;

var _map = L.map('map', options);
var nzoom = 12;

L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', { attribution: 'OSM' }).addTo(_map);

_map.on('click', function (e) {

    if (!_firstLatLng) {
        _firstLatLng = e.latlng;
        _firstPoint = e.layerPoint;
        L.marker(_firstLatLng).addTo(_map);
    } else {
        _secondLatLng = e.latlng;
        _secondPoint = e.layerPoint;
        L.marker(_secondLatLng).addTo(_map);
    }

    if (_firstLatLng && _secondLatLng) {
        L.polyline([_firstLatLng, _secondLatLng], {
            color: 'red'
        }).addTo(_map);

        var markerFrom = L.circleMarker(_firstLatLng, { color: "#F00", radius: 10 });
        var markerTo = L.circleMarker(_secondLatLng, { color: "#4AFF00", radius: 10 });
        var from = markerFrom.getLatLng();
        var to = markerTo.getLatLng();
        markerFrom.bindPopup((from).toString());
        markerTo.bindPopup((to).toString());
        _map.addLayer(markerTo);
        _map.addLayer(markerFrom);
        getDistance(from, to);
    }
})


function chooseAddr(lat1, lng1) {
    var czoom = _map.getZoom();
    if (czoom < 18) { nzoom = czoom + 16; }
    if (nzoom > 18) { nzoom = 18; }
    if (czoom != 18) { _map.setView([lat1, lng1], nzoom); } else { _map.setView([lat1, lng1]); }
    document.getElementById('lat').value = lat1;
    document.getElementById('lon').value = lng1;
}

function myFunction(arr) {
    var out = "<br />";
    var i;

    if (arr.length > 0) {
        for (i = 0; i < arr.length; i++) {
            out += "<div class='address' title='Show Location and Coordinates' onclick='chooseAddr(" + arr[i].lat + ", " + arr[i].lon + ");return false;'>" + arr[i].display_name + "</div>";
        }
        document.getElementById('results').innerHTML = out;
    }
    else {
        document.getElementById('results').innerHTML = "Sorry, no results...";
    }

}

function addr_search() {
    var inp = document.getElementById("addr");
    var xmlhttp = new XMLHttpRequest();
    var url = "https://nominatim.openstreetmap.org/search?format=json&limit=3&q=" + inp.value;
    xmlhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var myArr = JSON.parse(this.responseText);
            myFunction(myArr);
        }
    };
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
}

function getDistance(from, to) {
    var container = document.getElementById('distance');
    container.innerHTML = ((from.distanceTo(to)).toFixed(0) / 1000) + ' km';
}