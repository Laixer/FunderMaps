import 'https://api.mapbox.com/mapbox-gl-js/v2.11.0/mapbox-gl.js'
import 'https://unpkg.com/@turf/turf@6/turf.min.js'

function canShowProperty(key) {
    switch (key) {
        case "inquiry_id":
            return false;

        default:
            return true;
    }
}

function translateProperty(key, value) {
    const result = ({
        "building_id": ['FunderMaps ID', value],
        "address_count": ['Adressen', value],
        "construction_year": ['Bouwjaar', value],
        "construction_year_reliability": ['Bouwjaar betrouwbaarheid', value],
        "overall_quality": ['Funderingskwaliteit', value],
        "damage_cause": ['Schadeoorzaak', value],
        "height": ['Gebouwhoogte', `${value} meter`],
        "ground_level": ['Maaiveld', `${value} meter`],
        "ground_water_level": ['Grondwaterpeil', `${value} meter`],
        "category": ['Categorie', value],
        "incidents": ['Incidenten', value],
        "restoration_costs": ['Herstelkosten', value],
        "inquiry_type": ['Rapportage', value],
        "drystand_risk": ['Risico droogstand', value],
        "drystand_risk_reliability": ['Ontwateringsdiepte betrouwbaarheid', value],
        "dewatering_depth": ['Risico bacteriele aantasting', `${parseFloat(value).toFixed(2)}`],
        "dewatering_depth_risk_reliability": ['Bacteriele aantasting betrouwbaarheid', value],
        "bio_infection_risk": ['Risico bacteriele aantasting', value],
        "bio_infection_risk_reliability": ['Bacteriele aantasting betrouwbaarheid', value],
        "foundation_risk": ['Funderingsrisico', value],
        "foundation_type": ['Funderingstype', value],
        "foundation_type_reliability": ['Betrouwbaarheid', value],
        "enforcement_term": ['Handhavingstermijn', `${value} jaar`],
        "velocity": ['Zakkingssnelheid', `${parseFloat(value).toFixed(2)}mm/jaar`],
    })[key] ?? [key, value]

    return `<b>${result[0]}</b>: ${result[1]}`;
}

export function initMap(element, style, options) {
    console.log(`Loading mapbox style ${style}`);

    let mapOptions = JSON.parse(options)

    mapboxgl.accessToken = 'pk.eyJ1IjoibGFpeGVyIiwiYSI6ImNsN3ZyenhsczA2M2ozdW51bHJycmN6MnEifQ.OJ3E-t5Y7N_VZYHRhO_7Aw';
    var map = new mapboxgl.Map(
        Object.assign({
            container: element,
            style: style,
            center: [5.2913, 52.1326],
            zoom: 10,
            antialias: true,
            attributionControl: false,
        }, mapOptions)
    );
    map.addControl(
        new MapboxGeocoder({
            accessToken: mapboxgl.accessToken,
            mapboxgl: mapboxgl,
        }), "top-left"
    );
    map.addControl(
        new mapboxgl.GeolocateControl({
            positionOptions: {
                enableHighAccuracy: true,
            },
            trackUserLocation: true,
            showUserHeading: true,
        }), "bottom-right"
    );
    map.addControl(new mapboxgl.NavigationControl(), "bottom-right");
    map.on('load', () => {
        for (const key of ['foundation-type-established', 'foundation-recovery', 'incident-schiedam', 'monitoring-schiedam']) {
            map.setLayoutProperty(key, 'visibility', 'visible');
            map.on("mouseenter", key, () => {
                map.getCanvas().style.cursor = "pointer";
            });
            map.on("mouseleave", key, () => {
                map.getCanvas().style.cursor = "";
            });
            map.on("click", key, (e) => {
                var html = '';

                for (const [key, value] of Object.entries(e.features[0].properties)) {
                    if (canShowProperty(key)) {
                        html += translateProperty(key, value) + '<br/>';
                    }
                }

                const popup = new mapboxgl.Popup()
                    .setLngLat(turf.centroid(e.features[0]).geometry.coordinates)
                    .setHTML(html)
                    .addTo(map);
            });
        }
    });

    return map;
}

export function setLayerVisibility(map, layer, on) {
    map.setLayoutProperty(layer, 'visibility', on ? 'visible' : 'none');
}
