--
-- Insert or update pand (building) data from BAG
--

CREATE UNIQUE INDEX pand_identificatie_idx ON public.pand (identificatie);

INSERT INTO geocoder.building (built_year, is_active, geom, external_id, external_source, "building_type", neighborhood_id)
SELECT
	CASE
		WHEN bouwjaar > 2100 THEN NULL
		WHEN bouwjaar < 900 THEN NULL
		ELSE concat(bouwjaar, '-01-01')::date
	END,
	CASE lower(p.status)
		WHEN 'bouwvergunning verleend' THEN FALSE
		ELSE TRUE
	END,
	st_multi(ST_Transform(p.geom, 4326)),
	concat('NL.IMBAG.PAND.', p.identificatie),
	'nl_bag',
	'house',
	NULL
FROM public.pand AS p
ON CONFLICT (external_id, external_source)
DO UPDATE
SET
	built_year=EXCLUDED.built_year,
	is_active=EXCLUDED.is_active,
	geom=EXCLUDED.geom;

--
-- Insert or update ligplaats (building) data from BAG
--

CREATE UNIQUE INDEX ligplaats_identificatie_idx ON public.ligplaats (identificatie);

INSERT INTO geocoder.building (built_year, is_active, geom, external_id, external_source, "building_type", neighborhood_id)
SELECT
	NULL,
	CASE lower(l.status)
		WHEN 'plaats aangewezen' THEN TRUE
		ELSE FALSE
	END,
	st_multi(ST_Transform(l.geom, 4326)),
	concat('NL.IMBAG.LIGPLAATS.', l.identificatie),
	'nl_bag',
	'houseboat',
	NULL
FROM public.ligplaats AS l
ON CONFLICT (external_id, external_source)
DO UPDATE
SET
	is_active=EXCLUDED.is_active,
	geom=EXCLUDED.geom;

INSERT INTO geocoder.address (building_number, postal_code, street, is_active, external_id, city, building_id)
SELECT
	lower(concat(l.huisnummer, l.huisletter, l.toevoeging)),
	trim(l.postcode),
	trim(l.openbare_ruimte_naam),
	TRUE,
	concat('NL.IMBAG.NUMMERAANDUIDING.', l.nummeraanduiding_hoofdadres_identificatie),
	trim(l.woonplaats_naam),
	b.id
FROM ligplaats AS l
JOIN geocoder.building b ON b.external_id = concat('NL.IMBAG.LIGPLAATS.', l.identificatie)
ON CONFLICT (external_id)
DO NOTHING

--
-- Insert or update standplaats (building) data from BAG
--

CREATE UNIQUE INDEX standplaats_identificatie_idx ON public.standplaats (identificatie);

INSERT INTO geocoder.building (built_year, is_active, geom, external_id, external_source, "building_type", neighborhood_id)
SELECT
	NULL,
	CASE lower(s.status)
		WHEN 'plaats aangewezen' THEN TRUE
		ELSE FALSE
	END,
	st_multi(ST_Transform(s.geom, 4326)),
	concat('NL.IMBAG.STANDPLAATS.', s.identificatie),
	'nl_bag',
	'mobile_home',
	NULL
FROM public.standplaats AS s
ON CONFLICT (external_id, external_source)
DO UPDATE
SET
	is_active=EXCLUDED.is_active,
	geom=EXCLUDED.geom;

INSERT INTO geocoder.address (building_number, postal_code, street, is_active, external_id, city, building_id)
SELECT
	lower(concat(s.huisnummer, s.huisletter, s.toevoeging)),
	trim(s.postcode),
	trim(s.openbare_ruimte_naam),
	TRUE,
	concat('NL.IMBAG.NUMMERAANDUIDING.', s.nummeraanduiding_hoofdadres_identificatie),
	trim(s.woonplaats_naam),
	b.id
FROM standplaats as s
JOIN geocoder.building b ON b.external_id = concat('NL.IMBAG.STANDPLAATS.', s.identificatie)
ON CONFLICT (external_id)
DO NOTHING

--
-- Set the neighborhood id for buildings
--

UPDATE geocoder.building
SET neighborhood_id = n.id
FROM geocoder.neighborhood AS n
WHERE ST_Within(geocoder.building.geom, n.geom)
AND geocoder.building.neighborhood_id IS NULL

--
-- Insert or update nummeraanduiding (address) data from BAG
--

CREATE UNIQUE INDEX verblijfsobject_identificatie_idx ON public.verblijfsobject (identificatie);
CREATE UNIQUE INDEX verblijfsobject_nummeraanduiding_hoofdadres_identificatie_idx ON public.verblijfsobject (nummeraanduiding_hoofdadres_identificatie);
CREATE INDEX verblijfsobject_pand_identificatie_idx ON public.verblijfsobject (pand_identificatie);

DELETE FROM verblijfsobject
WHERE nummeraanduiding_hoofdadres_identificatie IS null

INSERT INTO geocoder.address (building_number, postal_code, street, is_active, external_id, city, building_id)
SELECT
	lower(concat(v.huisnummer, v.huisletter, v.toevoeging)),
	trim(v.postcode),
	trim(v.openbare_ruimte_naam),
	TRUE,
	concat('NL.IMBAG.NUMMERAANDUIDING.', v.nummeraanduiding_hoofdadres_identificatie),
	trim(v.woonplaats_naam),
	b.id
FROM verblijfsobject AS v
JOIN geocoder.building b ON b.external_id = concat('NL.IMBAG.PAND.', v.pand_identificatie)
ON CONFLICT (external_id)
do NOTHING

UPDATE geocoder.address
SET
	building_number=lower(concat(v.huisnummer, v.huisletter, v.toevoeging)),
	postal_code=trim(v.postcode),
	street=trim(v.openbare_ruimte_naam),
	is_active=true,
	city=trim(v.woonplaats_naam),
	building_id=b.id
FROM verblijfsobject AS v, geocoder.building AS b
WHERE b.external_id = concat('NL.IMBAG.PAND.', v.pand_identificatie)
AND geocoder.address.external_id = concat('NL.IMBAG.NUMMERAANDUIDING.', v.nummeraanduiding_hoofdadres_identificatie)
