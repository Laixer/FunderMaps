export class MapsetManager {
    async loadData(id) {
        const response = await fetch(id !== null ? `/mapset/${id}` : "/mapset");
        this.mapSetArray = await response.json();
        this.currentMapSetIdx = 0;

        const sessionMapSetId = sessionStorage.getItem('currentMapset');
        if (sessionMapSetId !== null) {
            this.setMapsetById(sessionMapSetId);
        }

        const mapset = this.getCurrentMapset();
        sessionStorage.setItem('currentMapset', mapset.id);
    }

    setMapsetById(mapsetId) {
        const mapSetIdx = this.mapSetArray.findIndex(x => x.id == mapsetId);
        if (mapSetIdx != -1) {
            this.currentMapSetIdx = mapSetIdx;

            const mapset = this.getCurrentMapset();
            sessionStorage.setItem('currentMapset', mapset.id);
            return true;
        }
        return false;
    }

    getCurrentMapset() {
        return this.mapSetArray[this.currentMapSetIdx];
    }
}