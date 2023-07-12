const ACCESS_TOKEN = "pk.eyJ1IjoibGFpeGVyIiwiYSI6ImNsN3ZyenhsczA2M2ozdW51bHJycmN6MnEifQ.OJ3E-t5Y7N_VZYHRhO_7Aw";

export class Map {
    constructor(mapset) {
        this.mapset = mapset;

        mapboxgl.accessToken = ACCESS_TOKEN;

        var center = [5.2913, 52.1326];

        const lastCenterPosition = sessionStorage.getItem('lastCenterPosition');
        if (lastCenterPosition !== null) {
            center = [JSON.parse(lastCenterPosition).lng, JSON.parse(lastCenterPosition).lat];
        }

        this.map = new mapboxgl.Map({
            container: 'map',
            style: this.mapset.style,
            center: center,
            zoom: sessionStorage.getItem('lastZoomLevel') || 10,
            pitch: sessionStorage.getItem('lastPitchDegree') || 0,
            antialias: true,
            attributionControl: false,
        });
        this.map.addControl(
            new MapboxGeocoder({
                accessToken: mapboxgl.accessToken,
                mapboxgl: mapboxgl,
            }), "top-left"
        );
        this.map.addControl(
            new mapboxgl.GeolocateControl({
                positionOptions: {
                    enableHighAccuracy: true,
                },
                trackUserLocation: true,
                showUserHeading: true,
            }), "bottom-right"
        );
        this.map.addControl(new mapboxgl.NavigationControl(), "bottom-right");
        this.map.on('moveend', () => {
            sessionStorage.setItem('lastCenterPosition', JSON.stringify(this.map.getCenter()));
        });
        this.map.on('zoomend', () => {
            sessionStorage.setItem('lastZoomLevel', this.map.getZoom());
        });
        this.map.on('pitchend', () => {
            sessionStorage.setItem('lastPitchDegree', this.map.getPitch());
        });
        this.map.on('style.load', () => {
            const uu = sessionStorage.getItem(`active-layers-${this.mapset.id}`);

            console.log(this.mapset.layerSet);

            if (this.mapset.fenceMunicipality !== null) {
                for (const layer of this.mapset.layerSet) {
                    const updatedFilter = [
                        'all',
                        this.map.getFilter(layer.id),
                        // [
                        //     "any",
                        // [
                        //     "match",
                        //     ["get", "district_id"],
                        //     [
                        //         "gfm-d28a0a422a3d44b1884a53dffa7a1b18"
                        //     ],
                        //     true,
                        //     false
                        // ],
                        // [
                        //     "match",
                        //     ["get", "municipality_id"],
                        //     [
                        //         "gfm-24c290a57a784ca3a57b8b613ee48fd4"
                        //     ],
                        //     true,
                        //     false
                        // ],
                        // [
                        //     "match",
                        //     ["get", "neighborhood_id"],
                        //     [
                        //         "gfm-b0e4fa7e30974811810ac7e60022db1e",
                        //         "gfm-7bc9bb6497984a13a2cc95ea1a284825"
                        //     ],
                        //     true,
                        //     false
                        // ]
                        // ]
                    ];

                    updatedFilter.push([
                        "match",
                        ["get", "municipality_id"],
                        [
                            this.mapset.fenceMunicipality
                        ],
                        true,
                        false
                    ]);

                    console.log(updatedFilter);

                    this.map.setFilter(layer.id, updatedFilter);
                }
            }

            for (const layer of this.mapset.layerSet) {
                if (uu !== null) {
                    if (JSON.parse(uu).includes(layer.id)) {
                        this.setLayerVisibility(layer, true);
                    } else {
                        this.setLayerVisibility(layer, false);
                    }
                } else {
                    this.setLayerVisibility(layer, false);
                }

                if (!layer.hasEventsSet) {
                    this.map.on("mouseenter", layer.id, () => {
                        this.map.getCanvas().style.cursor = "pointer";
                    });
                    this.map.on("mouseleave", layer.id, () => {
                        this.map.getCanvas().style.cursor = "";
                    });

                    this.map.on("click", layer.id, (e) => {
                        var html = '';

                        var popup = new mapboxgl.Popup().setLngLat(turf.centroid(e.features[0]).geometry.coordinates);

                        for (const [key, value] of Object.entries(e.features[0].properties)) {
                            if (key === 'building_id') {
                                fetch(`/mapset/building/${value}`)
                                    .then(function (response) {
                                        if (!response.ok) {
                                            throw new Error('Network response was not ok');
                                        }
                                        return response.json();
                                    })
                                    .then(function (data) {
                                        data.incidentList.forEach(incident => {
                                            html += `<b>Incident:</b> ${incident.id}<br/>`;
                                        });

                                        var inquirySet = new Set();
                                        data.inquirySampleList.forEach(inquirySample => {
                                            inquirySet.add(inquirySample.inquiry);
                                        });
                                        inquirySet.forEach(inquiryId => {
                                            html += `<b>Inquiry:</b> <a href="https://app.fundermaps.com/inquiry/${inquiryId}" target="_blank">${inquiryId}</a><br/>`;
                                        });

                                        var recoverySet = new Set();
                                        data.recoverySampleList.forEach(recoverySample => {
                                            recoverySet.add(recoverySample.recovery);
                                        });
                                        recoverySet.forEach(recoveryId => {
                                            html += `<b>Recovery:</b> ${recoveryId}<br/>`;
                                        });

                                        popup.setHTML(html);
                                    })
                                    .catch(function (error) {
                                        console.error('Error:', error);
                                    });
                            }

                            if (this.canShowProperty(e.features[0].layer.id, key)) {
                                html += this.translateProperty(key, value) + '<br/>';
                            }
                        }

                        popup.setHTML(html).addTo(this.map);
                    });

                    layer.hasEventsSet = true;
                }
            }

            const hasActiveLayers = this.mapset.layerSet.filter(x => x.isVisible).length > 0;
            if (!hasActiveLayers) {
                if (this.mapset.layerSet.length > 0) {
                    var layerFirst = this.mapset.layerSet[0];

                    this.setLayerVisibility(layerFirst, true);
                }
            }
        });
    }

    setMapset(mapset) {
        this.mapset = mapset;
        this.map.setStyle(this.mapset.style);
    }

    setLayerVisibility(layer, enable) {
        if (enable) {
            this.map.setLayoutProperty(layer.id, 'visibility', 'visible');
            layer.isVisible = true;
            document.getElementById(`map-legend-${this.mapset.id}-${layer.id}`).style.display = "block";

            sessionStorage.setItem(`active-layers-${this.mapset.id}`, JSON.stringify(this.mapset.layerSet.filter(x => x.isVisible).map(x => x.id)));
        } else {
            this.map.setLayoutProperty(layer.id, 'visibility', 'none');
            layer.isVisible = false;
            document.getElementById(`map-legend-${this.mapset.id}-${layer.id}`).style.display = "none";

            sessionStorage.setItem(`active-layers-${this.mapset.id}`, JSON.stringify(this.mapset.layerSet.filter(x => x.isVisible).map(x => x.id)));
        }
    }

    layerFromId(layerId) {
        return this.mapset.layerSet.find(x => x.id === layerId);
    }

    canShowProperty(layerId, key) {
        switch (key) {
            case "neighborhood_id": return false;
            case "district_id": return false;
            case "municipality_id": return false;
        }

        switch (layerId) {
            case "foundation-type-established":
                switch (key) {
                    case "address_count":
                        return true;
                    case "foundation_type":
                        return true;
                    case "foundation_type_reliability":
                        return true;

                    default:
                        return false;
                }

            case "foundation-recovery":
                switch (key) {
                    case "recovery_type":
                        return true;

                    default:
                        return false;
                }

            case "drystand-risk":
                switch (key) {
                    case "drystand":
                        return true;
                    case "drystand_risk":
                        return true;
                    case "drystand_risk_reliability":
                        return true;

                    default:
                        return false;
                }

            case "dewatering-depth-risk":
                switch (key) {
                    case "dewatering_depth":
                        return true;
                    case "dewatering_depth_risk":
                        return true;
                    case "dewatering_depth_risk_reliability":
                        return true;

                    default:
                        return false;
                }

            case "bio-infection-risk":
                switch (key) {
                    case "bio_infection_risk":
                        return true;
                    case "bio_infection_risk_reliability":
                        return true;

                    default:
                        return false;
                }

            default:
                return true;
        }
    }

    translateProperty(key, value) {
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
            "owner": ['Eigenaar', value],
            "drystand": ['Droogstand', `${parseFloat(value).toFixed(2)} meter`],
            "drystand_risk": ['Risico droogstand', value],
            "drystand_risk_reliability": ['Risico droogstand betrouwbaarheid', value],
            "dewatering_depth": ['Ontwateringsdiepte', `${parseFloat(value).toFixed(2)}`],
            "dewatering_depth_risk": ['Risico ontwateringsdiepte', value],
            "dewatering_depth_risk_reliability": ['Risico ontwateringsdiepte betrouwbaarheid', value],
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
}