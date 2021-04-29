var startlat = "38.734802",
    startlon = "35.467987",
    _firstLatLng,
    _firstPoint,
    _secondLatLng,
    _secondPoint,
    _length,
    _polyline

var options = {
    center: [startlat, startlon],
    zoom: 5
}

var _map = L.map('map', options);
var nzoom = 12;

L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', { attribution: 'OSM' }).addTo(_map);

function getAddresses(inputId, ContainerId) {

    var container = $('#' + ContainerId); //sonucların gosterilecegi container bulunuyor
    var out = "<br />"; //geri donulecek html olusturuluyor
    var _url = "https://nominatim.openstreetmap.org/search?format=json&limit=3&q=" + $('#' + inputId).val(); //herkese acik openstreetmap apisinden adresleri getirmek icin url olusturuluyor
    //ajax istegi atiliyor
    $.ajax({
        method: "GET",
        url: _url,
    }).done(function (result) {
        //sonuc gelirse
        if (result.length > 0) {
            for (var i = 0; i < result.length; i++) { //sonuc icersinde gez
                out += "<div class='address' title='Adresi Göster' onclick='chooseAddr2(" + result[i].lat + "," + result[i].lon + ",\"" + result[i].display_name + "\",\"" + inputId + "\",\"" + ContainerId + "\");'>" + result[i].display_name + "</div>"; //bulunan sonuclari gostermek icin divler olusturulup out htmle ekleniyor
            }
        } else {
            out = 'Adres bulunamadı...';
        }
        container.html(out);
    }).fail(function (result) {
        console.log("başarısız!!!1");
    });
}

function chooseAddr2(lat1, lng1, addr, inputId, containerId) {
    var latLong = {};
    latLong.lat = lat1;
    latLong.lng = lng1;

    var czoom = _map.getZoom();
    if (czoom < 18) { nzoom = czoom + 16; }
    if (nzoom > 18) { nzoom = 18; }
    if (czoom != 18) { _map.setView([lat1, lng1], nzoom); } else { _map.setView([lat1, lng1]); }

    if (!_firstLatLng) {
        _firstLatLng = latLong;
    }
    else {
        _secondLatLng = latLong;

    }
    var adMarker = L.marker(latLong).addTo(_map);
    adMarker.bindPopup(addr);
    adMarker.openPopup();
    $('#' + containerId).html('');
    $('#' + inputId).val(addr);
    $('#' + inputId).prop('disabled', true);
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
}

function getDistance(from, to) {
    var dv = ((from.distanceTo(to)).toFixed(0) / 1000);
    $('#distance').val(dv);
    var container = document.getElementById('distance');
    container.innerHTML = ((from.distanceTo(to)).toFixed(0) / 1000) + ' km';
}

function packagePriceCalculate() {
    var _distance = parseInt($('#distance').val());
    if (!_distance) {
        Swal.fire({
            title: 'Adres Seçmelisiniz...',
            confirmButtonText: 'Tamam',
        });
        return;
    }
    var _package = {}; //package nesnesi
    $('#desi_form').serializeArray().map(function (x) { _package[x.name] = x.value; }); //desiformdaki inputları package nesnesine cevirir.
    if (_package != null) {
        $.ajax({
            method: "POST",
            url: "Home/PackageCalculate",
            data: { distance: _distance, package: _package },
        }).done(function (result) {

            //istek basarili ise burasi calisir
            $('#price_result').val(result + " ₺");
            $('#price_result_container').removeClass('d-none');

        }).fail(function (result) {
            //istek basarisiz ise burasi calisir.
            console.log(result);
            console.log(Object.keys(result));
        });
    }
    else {
        Swal.fire({
            title: 'Lütfen Ölçüleri Eksiksiz Giriniz...',
            confirmButtonText: 'Tamam',
        });
        return;
    }
    
}

function documentPriceCalculate() {
    var _distance = parseInt($('#distance').val());
    if (!_distance) {
        Swal.fire({
            title: 'Adres Seçmelisiniz...',
            confirmButtonText: 'Tamam',
        });
        return;
    }
    $.ajax({
        method: "POST",
        url: "Home/DocumentCalculate",
        data: { distance: _distance },
    }).done(function (result) {
        Swal.fire({
            title: 'Dosya/Zarf Gönderilerinizde Ağırlık En Fazla 1000gr Olmalıdır.',
            confirmButtonText: 'Tamam',
        });
        //istek basarili ise burasi calisir
        $('#price_result').val(result + " ₺");
        $('#price_result_container').removeClass('d-none');

    }).fail(function (result) {
        //istek basarisiz ise burasi calisir.
        console.log(result);
        console.log(Object.keys(result));
    });
}

function getPackageDelivery() {
    $('#package_delivery_container').removeClass('d-none');
}

function addNewPackage() {
    var package = {}; //package nesnesi
    $('#desi_form').serializeArray().map(function (x) { package[x.name] = x.value; }); //desiformdaki inputları package nesnesine cevirir.
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: 'Home/GenerateNewPackage',
        data: { package: package },
    }).done(function (result) {
        $('#package_delivery_container').html(result);
    }).fail(function (result) {
        //istek basarisiz ise burasi calisir.
        console.log(result);
        console.log(Object.keys(result));
        Swal.fire({
            icon: 'error',
            title: "Hata",
            html: 'Beklenmedik Bir Hata Oluştu!',
            confirmButtonText: 'Tamam'
        });
    });
}

function htmlContentExample() {
    Swal.fire({
        title: 'Hesaplanıyor...',
        html: 'Bu işlem <strong>bir kaç dakika </strong> sürebilir. Lütfen bekleyin...',// add html attribute if you want or remove
        allowOutsideClick: false,
        showConfirmButton: false,
        onBeforeOpen: () => {
            Swal.showLoading()
        },
    });
    $.ajax({
        method: "GET",
        url: "Home/CargoCompaniesList",
    }).done(function (result) {
        $('#cargo_companies_container').html(result);
        Swal.close();
    }).fail(function (result) {
        //istek basarisiz ise burasi calisir.
        console.log(result);
        console.log(Object.keys(result));
        Swal.fire({
            icon: 'error',
            title: "Hata",
            html: 'Beklenmedik Bir Hata Oluştu',
            confirmButtonText: 'Tamam'
        });
    });
}

/*var inp = document.getElementById("searchbar");*/

//_map.on('click', function (e) {
//    var txtaddress = $('#searchbar').val();
//    if (!_firstLatLng) {
//        _firstLatLng = e.latlng;
//        _firstPoint = e.layerPoint;
//        L.marker(_firstLatLng).addTo(_map);
//        $('#shipperaddress').val(txtaddress);
//        $('#shipperaddress_container').removeClass('d-none');
//    } else {
//        _secondLatLng = e.latlng;
//        _secondPoint = e.layerPoint;
//        L.marker(_secondLatLng).addTo(_map);
//        $('#deliveryaddress').val(txtaddress);
//        $('#deliveryaddress_container').removeClass('d-none');
//    }

//    if (_firstLatLng && _secondLatLng) {
//        L.polyline([_firstLatLng, _secondLatLng], {
//            color: 'red'
//        }).addTo(_map);

//        var markerFrom = L.circleMarker(_firstLatLng, { color: "#F00", radius: 10 });
//        var markerTo = L.circleMarker(_secondLatLng, { color: "#4AFF00", radius: 10 });
//        var from = markerFrom.getLatLng();
//        var to = markerTo.getLatLng();
//        markerFrom.bindPopup((from).toString());
//        markerTo.bindPopup((to).toString());
//        _map.addLayer(markerTo);
//        _map.addLayer(markerFrom);
//        getDistance(from, to);
//    }
//})

//function chooseAddr(lat1, lng1) {
//    var czoom = _map.getZoom();
//    if (czoom < 18) { nzoom = czoom + 16; }
//    if (nzoom > 18) { nzoom = 18; }
//    if (czoom != 18) { _map.setView([lat1, lng1], nzoom); } else { _map.setView([lat1, lng1]); }
//}

//function myFunction(arr) {
//    var out = "<br />";
//    var i;

//    if (arr.length > 0) {
//        for (i = 0; i < arr.length; i++) {
//            out += "<div class='address' title='Adresi Göster' onclick='chooseAddr(" + arr[i].lat + ", " + arr[i].lon + ");return false;'>" + arr[i].display_name + "</div>";
//        }
//        document.getElementById('results').innerHTML = out;
//    }
//    else {
//        document.getElementById('results').innerHTML = "Adres Bulunamadı...";
//    }

//}
//inp.addEventListener('keyup', function addr_search() {
//    var xmlhttp = new XMLHttpRequest();
//    var url = "https://nominatim.openstreetmap.org/search?format=json&limit=3&q=" + inp.value;
//    xmlhttp.onreadystatechange = function () {
//        if (this.readyState == 4 && this.status == 200) {
//            var myArr = JSON.parse(this.responseText);
//            myFunction(myArr);
//        }
//    };
//    xmlhttp.open("GET", url, true);
//    xmlhttp.send();
//});