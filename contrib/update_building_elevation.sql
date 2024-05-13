-- TRUNCATE TABLE "data".building_elevation;

INSERT INTO "data".building_elevation(building_id, ground, roof)
SELECT b.id, p.b3_h_maaiveld, MAX(ld.b3_h_70p)
FROM public.pand p
JOIN public.lod22_2d ld on ld.identificatie = p.identificatie
JOIN geocoder.building b on b.external_id = UPPER(p.identificatie)
GROUP BY b.id, p.b3_h_maaiveld
