--
-- PostgreSQL database dump
--

-- Dumped from database version 12.6 (Debian 12.6-1.pgdg100+1)
-- Dumped by pg_dump version 12.6 (Ubuntu 12.6-0ubuntu0.20.10.1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Data for Name: bundle; Type: TABLE DATA; Schema: maplayer; Owner: fundermaps
--

COPY maplayer.bundle (id, organization_id, name, create_date, update_date, delete_date, layer_configuration) FROM stdin;
\.


--
-- Data for Name: layer; Type: TABLE DATA; Schema: maplayer; Owner: fundermaps
--

COPY maplayer.layer (id, schema_name, table_name, name, markup) FROM stdin;
22bcc8a3-fd28-4184-822b-f66e3e79e4b7	maplayer	subsidence	Pandzakking	{"type": "range_num", "column": "velocity", "values": [{"max": "0", "min": "-9999", "color": "#d13b1d", "label": "< 0"}, {"max": "9999", "min": "0", "color": "#68d11d", "label": "> 0"}]}
2d151f64-778d-40b6-ac21-40675b7c5c35	maplayer	inquiry	Rapportage	{"type": "case", "column": "type", "values": [{"color": "#B54CB0", "label": "Monitoring", "match": "monitoring"}, {"color": "#8C4BB6", "label": "Notitie", "match": "note"}, {"color": "#5B4AB7", "label": "Snelle scan", "match": "quickscan"}, {"color": "#4969B8", "label": "Sloop onderzoek", "match": "demolition_research"}, {"color": "#489BB9", "label": "Second opinion", "match": "second_opinion"}, {"color": "#47BAA5", "label": "Archief onderzoek", "match": "archieve_research"}, {"color": "#4EBC77", "label": "Architectureel onderzoek", "match": "architectural_research"}, {"color": "#5CBE55", "label": "Funderingsadvies", "match": "foundation_advice"}, {"color": "#8FC05C", "label": "Inspectieput", "match": "inspectionpit"}, {"color": "#BDC262", "label": "Funderingsonderzoek", "match": "foundation_research"}, {"color": "#C4A169", "label": "Extra onderzoek", "match": "additional_research"}, {"color": "#C67E70", "label": "Grondwaterniveau onderzoek", "match": "ground_water_level_research"}, {"color": "#6A6C70", "label": "Onbekend", "match": "unknown"}]}
0f7cc50c-b831-486e-be07-c22c85943f21	maplayer	inquiry_sample_quality	Kwaliteit Funderingen	{"type": "case", "column": "overall_quality", "values": [{"color": "#67B6E4", "label": "Goed", "match": "good"}, {"color": "#4EEBA9", "label": "Redelijk", "match": "tolerable"}, {"color": "#5CF434", "label": "Acceptabel", "match": "mediocre_good"}, {"color": "#FFFF17", "label": "Twijfelachtig", "match": "mediocre"}, {"color": "#DD882F", "label": "Slecht", "match": "mediocre_bad"}, {"color": "#BE4745", "label": "Zeer slecht", "match": "bad"}]}
73258fb5-54a4-4d0c-a85f-e0ca2313e67f	maplayer	foundation_risk	Funderingsrisico	{"type": "case", "column": "foundation_risk", "values": [{"color": "#42FF33", "label": "A", "match": "a"}, {"color": "#D1FF33", "label": "B", "match": "b"}, {"color": "#FFEC33", "label": "C", "match": "c"}, {"color": "#FFAC33", "label": "D", "match": "d"}, {"color": "#FF5533", "label": "E", "match": "e"}]}
c7ccf095-cf77-4f41-813a-68b493f7c6cb	maplayer	building_built_year	Bouwjaar	{"type": "range_num", "column": "built_year", "values": [{"max": "1960", "min": "0", "color": "#293575", "label": "< 1960"}, {"max": "1970", "min": "1960", "color": "#1261A3", "label": "1960 t/m 1970"}, {"max": "1980", "min": "1970", "color": "#69A8DE", "label": "1970 t/m 1980"}, {"max": "1990", "min": "1980", "color": "#99C1E9", "label": "1980 t/m 1990"}, {"max": "2000", "min": "1990", "color": "#B378B1", "label": "1990 t/m 2000"}, {"max": "2010", "min": "2000", "color": "#bd6495", "label": "2000 t/m 2010"}, {"max": "2020", "min": "2010", "color": "#ba2351", "label": "2010 t/m 2020"}, {"max": "9999", "min": "2020", "color": "#d11313", "label": "> 2020"}]}
5742df10-53fe-44da-b449-2c0c926deab6	maplayer	incident	Meldingen	{"type": "color", "column": "meldingen123kleurtest", "values": {"color": "#bd6495"}}
307e6489-feb3-4be8-919c-4b5392fee8fb	maplayer	inquiry_sample_enforcement_term	Handhavingstermijn (jaar)	{"type": "range_num", "column": "enforcement_term", "values": [{"max": "100", "min": "25", "color": "#64DEBC", "label": "> 25"}, {"max": "25", "min": "20", "color": "#55E293", "label": "20 t/m 25"}, {"max": "20", "min": "15", "color": "#46E65F", "label": "15 t/m 20"}, {"max": "15", "min": "10", "color": "#4CEB36", "label": "10 t/m 15"}, {"max": "10", "min": "5", "color": "#77F025", "label": "5 t/m 10"}, {"max": "5", "min": "0", "color": "#AEF614", "label": "0 t/m 5"}, {"max": "0", "min": "-5", "color": "#D0E218", "label": " 0 t/m -5"}, {"max": "-5", "min": "-10", "color": "#CEB31B", "label": "-5 t/m -10"}, {"max": "-10", "min": "-15", "color": "#BB7F1E", "label": "-10 t/m -15"}, {"max": "-15", "min": "-20", "color": "#A85520", "label": "-15 t/m -20"}, {"max": "-20", "min": "-25", "color": "#973321", "label": "-20 t/m -25"}, {"max": "-25", "min": "-100", "color": "#86222A", "label": "< -25"}]}
75709b4f-d82e-4d62-9be3-07cb2ca00cec	maplayer	inquiry_sample_damage_cause	Oorzaak Schade	{"type": "case_multimatch", "column": "damage_cause", "values": [{"color": "#55B5A7", "label": "Ontwateringsdiepte", "match": ["drainage"]}, {"color": "#4B8FBF", "label": "Overbelasting", "match": ["overcharge"]}, {"color": "#4145C9", "label": "Bacteriële aantasting", "match": ["bio_fungus_infection", "bio_infection"]}, {"color": "#8936D4", "label": "Schimmelaantasting", "match": ["fungus_infection", "drystand", "bio_fungus_infection"]}, {"color": "#DE2CCF", "label": "Bodemdaling", "match": ["subsidence"]}, {"color": "#D2386F", "label": "Planten en wortels", "match": ["vegetation"]}, {"color": "#C75D43", "label": "Aardbeving", "match": ["gas", "vibrations"]}, {"color": "#BBA14F", "label": "Partieel funderingsherstel", "match": ["partial_foundation_recovery"]}, {"color": "#95B05A", "label": "Constructiefout", "match": ["construction_flaw", "foundation_flaw", "construction_heave"]}, {"color": "#6EA466", "label": "Negatieve kleef", "match": ["negative_cling", "overcharge_negative_cling"]}, {"color": "#6A6C70", "label": "Onbekend", "match": ["unknown"]}]}
782bc8e1-ff0f-48aa-9f0d-10232392ceda	maplayer	building_height	Gebouwhoogte (Beta)	\N
8beee1ea-9cd5-4999-8759-02e4cf313bd9	maplayer	building_hotspot	Hotspots	{"type": "color", "column": "meldingen123kleurtest", "values": {"color": "#d11313"}}
16cc1db0-5f09-4673-9e07-e5ac573fc1b7	maplayer	foundation_indicative	Funderingstype Indicatief	{"type": "case_multimatch", "column": "foundation_type", "values": [{"color": "#c75d43", "label": "Houten paal", "match": ["wood", "wood_rotterdam", "wood_amsterdam"]}, {"color": "#deb271", "label": "Houten paal met oplanger", "match": ["wood_charger"]}, {"color": "#6a6c70", "label": "Betonnen paal", "match": ["concrete"]}, {"color": "#ff3333", "label": "Op staal", "match": ["no_pile", "no_pile_masonry", "no_pile_strips", "no_pile_concrete_floor", "no_pile_slit", "no_pile_bearing_floor"]}]}
806d560e-2931-46d9-a06b-644491a335e8	maplayer	building_ownership	Eigendom	{"type": "color", "column": "meldingen123kleurtest", "values": {"color": "#d11313"}}
edd9bf0f-903b-43e6-be9c-2c927089b075	maplayer	incident_aggregate	Incidenten Gemeente Aantallen	{"type": "range_num", "column": "incidents", "values": [{"max": "0", "min": "0", "color": "#293575", "label": "0"}, {"max": "1", "min": "0", "color": "#293575", "label": "1"}, {"max": "2", "min": "1", "color": "#1261A3", "label": "2"}, {"max": "3", "min": "2", "color": "#69A8DE", "label": "3"}, {"max": "5", "min": "3", "color": "#99C1E9", "label": "4-5"}, {"max": "10", "min": "5", "color": "#B378B1", "label": "6-10"}, {"max": "15", "min": "10", "color": "#bd6495", "label": "16-20"}, {"max": "25", "min": "20", "color": "#ba2351", "label": "21-25"}, {"max": "100", "min": "25", "color": "#d11313", "label": "> 25"}]}
e0519b1a-ae1a-43ad-a2b9-bdbbeb0c5f86	maplayer	incident_aggregate_category	Incidenten Gemeente Category	{"type": "case", "column": "category", "values": [{"color": "#d8e7f5", "label": "Geen of nauwelijks", "match": "0"}, {"color": "#73b3d8", "label": "Enkele", "match": "1"}, {"color": "#1563aa", "label": "Meerdere", "match": "2"}]}
dfe1a361-c23f-4a1d-ba49-af8e974270b3	maplayer	subsidence_hex	Maaiveldzakking	{"type": "range_num", "column": "velocity", "values": [{"max": "9999", "min": "0", "color": "#f7fbff", "label": "> 0 mm/jaar"}, {"max": "0", "min": "-0.5", "color": "#d8e7f5", "label": "0,0 t/m -0,5 mm/jaar"}, {"max": "-0.5", "min": "-1", "color": "#b0d2e8", "label": "-0,5 t/m -1,0 mm/jaar"}, {"max": "-1", "min": "-1.5", "color": "#73b3d8", "label": "-1,0 t/m -1,5 mm/jaar"}, {"max": "-1.5", "min": "-2", "color": "#3e8ec4", "label": "-1,5 t/m -2,0 mm/jaar"}, {"max": "-2", "min": "-2.5", "color": "#1563aa", "label": "-2,0 t/m -2,5 mm/jaar"}, {"max": "-2.5", "min": "-9999", "color": "#08306b", "label": "< -2,5 mm/jaar"}]}
f42a6826-c3a0-48b1-8c96-6c9ef753ed46	maplayer	inquiry_sample_foundation_type	Funderingstype vastgesteld	{"type": "case_multimatch", "column": "foundation_type", "values": [{"color": "#c75d43", "label": "Houten paal", "match": ["wood", "weighted_pile", "wood_amsterdam", "wood_rotterdam"]}, {"color": "#deb271", "label": "Houten paal met oplanger", "match": ["wood_charger"]}, {"color": "#6a6c70", "label": "Betonnen paal", "match": ["concrete"]}, {"color": "#ff3333", "label": "Op staal", "match": ["no_pile", "no_pile_masonry", "no_pile_strips", "no_pile_concrete_floor", "no_pile_slit", "no_pile_bearing_floor"]}, {"color": "#bdbebf", "label": "Stalen paal", "match": ["steel_pile"]}, {"color": "#7192de", "label": "Verzwaarde betonpuntpaal", "match": ["weighted_pile"]}, {"color": "#b271de", "label": "Combinatie", "match": ["combined"]}, {"color": "#ffec33", "label": "Overig", "match": ["other"]}, {"color": "#71decc", "label": "Onbekend", "match": ["unknown"]}]}
9dffd130-4019-4440-b3d5-8812a961a87a	maplayer	recovery_sample_type	Hesteld	{"type": "case_multimatch", "column": "type", "values": [{"color": "#5cbe55", "label": "Volledig herstel", "match": ["table"]}, {"color": "#47baa5", "label": "Partieel herstel", "match": ["pile_in_wall"]}, {"color": "#8c4bb6", "label": "Paalkop verlaging", "match": ["pile_lowering", "beam_on_pile"]}, {"color": "#c67e70", "label": "Grondverbetering", "match": ["injection"]}, {"color": "#5B4AB7", "label": "Onbekend", "match": ["unknown"]}]}
\.


--
-- PostgreSQL database dump complete
--

