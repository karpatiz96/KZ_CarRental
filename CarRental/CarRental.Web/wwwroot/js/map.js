$(function () {

    var key = '@Model.MapKey'
    var latitude = '@Model.Address.Latitude';
    var longitude = '@Model.Address.Longitude';

        var map = new atlas.Map('map', {
            center: [latitude, longitude],
            zoom: 16,
            authOptions: {
                authType: 'subscriptionKey',
                subscriptionKey: key
            }

        });

        map.events.add('ready', function () {

            var centerMarker = new atlas.HtmlMarker({
                color: 'DodgerBlue',
                text: '10',
                position: [latitude, longitude],
                popup: new atlas.Popup({
                    content: '<div style="padding:10px">Hello World</div>',
                    pixelOffset: [0, -30]
                })
            });

            map.markers.add(centerMarker);

            map.events.add('click', centerMarker, () => {
                marker.togglePopup();
            });
        });
});