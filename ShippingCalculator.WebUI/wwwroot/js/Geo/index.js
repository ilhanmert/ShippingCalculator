function getCountries(elem) {
    var id = parseInt($(elem).val());
    $.ajax({
        url: '/Geo/GetCountriesDD',
        type: 'GET',
        dataType: 'html',
        data: { id: id },
        success: function (result) {
            // console.log(result);
            $('#countries_container').html(result);
        },
        error: function (error) {
            console.log(error.status);
            console.log(error.statusText);
            console.log(error.responseText)
        }
    }); //aldıgımız degerleri generateimagepathinputs a gonderdik
}
function getCities(elem) {
    var code = $(elem).val();
    $.ajax({
        url: '/Geo/GetCitiesDD',
        type: 'GET',
        dataType: 'html',
        data: { code: code },
        success: function (result) {
            // console.log(result);
            $('#cities_container').html(result);
        },
        error: function (error) {
            console.log(error.status);
            console.log(error.statusText);
            console.log(error.responseText)
        }
    }); //aldıgımız degerleri generateimagepathinputs a gonderdik
}
function getCounties(elem) {
    var id = parseInt($(elem).val());
    $.ajax({
        url: '/Geo/GetCountiesDD',
        type: 'GET',
        dataType: 'html',
        data: { id: id },
        success: function (result) {
            // console.log(result);
            $('#counties_container').html(result);
        },
        error: function (error) {
            console.log(error.status);
            console.log(error.statusText);
            console.log(error.responseText)
        }
    }); //aldıgımız degerleri generateimagepathinputs a gonderdik
}
