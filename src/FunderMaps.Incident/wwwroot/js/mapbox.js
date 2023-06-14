import 'https://api.mapbox.com/mapbox-gl-js/v2.12.0/mapbox-gl.js';

mapboxgl.accessToken = 'pk.eyJ1IjoibGFpeGVyIiwiYSI6ImNraThwMWxieDA3eXkycm85OW5hbWM3aTUifQ.Ld_05yoDaHynP5VvMMLvxA';

export function addMapToElement(element) {
  return new mapboxgl.Map({
    container: element,
    style: 'mapbox://styles/mapbox/streets-v11',
    center: [4.9041, 52.3676],
    zoom: 12.5
  });
}

export function setMapCenter(map, latitude, longitude) {
  map.setCenter([longitude, latitude]);
}
