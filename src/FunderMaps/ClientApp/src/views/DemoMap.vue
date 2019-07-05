<template>
  <div class="MapBox">
    <MglMap
      class="MapBox__wrapper" 
      :accessToken="accessToken"
      :mapStyle.sync="mapStyle"
      @load="onMapLoaded" />

    <div class="MapBox__tools">
      <Form @submit="() => null">
        <FormField 
          v-model="select.value"
          v-bind="select"
          @input="toggleLayer" />
      </Form>
    </div>
  </div>
</template>

<script>
import { MglMap } from 'vue-mapbox';

import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'

import { mapGetters, mapActions } from 'vuex'

export default {
  components: {
    MglMap, Form, FormField
  },
  data() {
    return {
      accessToken: 'pk.eyJ1Ijoiam91cm5leXdvcmtzIiwiYSI6ImNqeDd6Nm5oNDBlZGkzc3A3Zm40ZzVhb3YifQ.7Pi9HnHW3r-PU-koxWLYGw',
      mapStyle: 'mapbox://styles/journeyworks/cjx7z8ar10m081cm6gq6uyjez',

      select: {
        type: 'select',
        label: 'Selecteer een laag',
        value: 'all',
        novalidate: true,
        options: [
          {
            value: 'all',
            text: 'Alle lagen'
          },
          // {
          //   value: 'samples',
          //   text: 'Op basis van rapportages'
          // },
          {
            value: 'samples2',
            text: 'Historische fundering'
          },
          {
            value: 'samples3',
            text: 'Mogelijk houten- of betonpalen'
          },
          {
            value: 'samples4',
            text: 'Mogelijk houtenpalen of niet onderheid'
          },
          {
            value: 'samples5',
            text: 'Zeer hoge kans op beton'
          }
        ]
      }
    }
  },
  computed: {
    ...mapGetters('map', [
      'hasMapData',
      'mapData'
    ])
  },
  created() {
    // TODO: Use the endpoint instead of static file
    // this.getMapData()
  },
  methods: {
    ...mapActions('map', [
      'getMapData'
    ]),
    onMapLoaded(event) {
      // Note: a reference to the map has to be stored in a non-reactive manner.
      this.$store.map = event.map;

      this.addSamplePoints();
    },
    addSamplePoints() {
      this.$store.map.addSource('sample-source', {
        type: 'geojson',
        data: '/mapData.json'
      });
      this.$store.map.addSource('historic-foundation', {
        type: 'geojson',
        data: '/historic_foundation.json'
      });
      this.$store.map.addSource('wood-concrete', {
        type: 'geojson',
        data: '/wood_or_concrete.json'
      });
      this.$store.map.addSource('no-pile', {
        type: 'geojson',
        data: '/no_pile.json'
      });
      this.$store.map.addSource('concrete', {
        type: 'geojson',
        data: '/concrete.json'
      });
      this.$store.map.addLayer({
        "id": "samples",
        "type": "circle",
        "source": 'sample-source',
        'layout': {
          'visibility': 'none',
        },
        "paint": {
            'circle-radius': 8,
            "circle-color": '#2da798',
            "circle-opacity": 0.8,
            "circle-stroke-width": 0,
        }
      });
      this.$store.map.addLayer({
        "id": "samples2",
        "type": "circle",
        "source": 'historic-foundation',
        'layout': {
          'visibility': 'visible',
        },
        "paint": {
            'circle-radius': 8,
            "circle-color": '#005ce6',
            "circle-opacity": 0.8,
            "circle-stroke-width": 0,
        }
      });
      this.$store.map.addLayer({
        "id": "samples3",
        "type": "circle",
        "source": 'wood-concrete',
        'layout': {
          'visibility': 'visible',
        },
        "paint": {
            'circle-radius': 8,
            "circle-color": '#e60000',
            "circle-opacity": 0.8,
            "circle-stroke-width": 0,
        }
      });
      this.$store.map.addLayer({
        "id": "samples4",
        "type": "circle",
        "source": 'no-pile',
        'layout': {
          'visibility': 'visible',
        },
        "paint": {
            'circle-radius': 8,
            "circle-color": '#00e64d',
            "circle-opacity": 0.8,
            "circle-stroke-width": 0,
        }
      });
      this.$store.map.addLayer({
        "id": "samples5",
        "type": "circle",
        "source": 'concrete',
        'layout': {
          'visibility': 'visible',
        },
        "paint": {
            'circle-radius': 8,
            "circle-color": '#ac00e6',
            "circle-opacity": 0.8,
            "circle-stroke-width": 0,
        }
      });
    },
    toggleLayer(selectedLayer) {
      [
        'samples', 'samples2', 'samples3', 'samples4', 'samples5'
      ].forEach(layer => {
        let state = (layer === selectedLayer || selectedLayer === 'all') 
          ? 'visible' 
          : 'none'
        this.$store.map.setLayoutProperty(layer, 'visibility', state);
      })
    }
  }
}
</script>

<style lang="scss">
  .mapboxgl-canvas-container, .mapboxgl-canvas {
    &:active, &:focus {
      outline: none;
    }
  }

  .mapboxgl-marker {
    position: absolute;
    cursor: pointer;
  }

  .mapboxgl-map {
    width: 100% !important;
    height: 100% !important;
  }

  // Hide MapBox logo
  .mapboxgl-ctrl.mapboxgl-ctrl-attrib, .mapboxgl-ctrl {
    // display: none !important;
  }
  .MapBox, .MapBox__wrapper {
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    user-select: none;
  }
  .MapBox__tools {
    position: absolute;
    top: 4px;
    right: 4px;
    
    background: white;
    padding: 10px 15px 5px;
    box-shadow: 0px -1px 0px 0px rgba(223,226,229,1) inset;
  }

</style>
