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
c7ccf095-cf77-4f41-813a-68b493f7c6cb	maplayer	building	Gebouw	{"type": "range_num", "column": "built_year", "values": [{"max": "1960", "min": "0", "color": "#293575", "label": "< 1960"}, {"max": "1970", "min": "1960", "color": "#1261A3", "label": "1960-1970"}, {"max": "1980", "min": "1970", "color": "#69A8DE", "label": "1970-1980"}, {"max": "1990", "min": "1980", "color": "#99C1E9", "label": "1980-1990"}, {"max": "2000", "min": "1990", "color": "#B378B1", "label": "1990-2000"}, {"max": "2010", "min": "2000", "color": "#bd6495", "label": "2000-2010"}, {"max": "2020", "min": "2010", "color": "#ba2351", "label": "2010-2020"}, {"max": "9999", "min": "2020", "color": "#d11313", "label": "> 2020"}]}
5742df10-53fe-44da-b449-2c0c926deab6	maplayer	incident	Meldingen	{"type": "color", "column": "meldingen123kleurtest", "values": {"color": "red"}}
0f7cc50c-b831-486e-be07-c22c85943f21	maplayer	inquiry_sample_quality	Kwaliteit Funderingen	{"type": "case", "column": "overall_quality", "values": [{"color": "#67B6E4", "label": "Goed", "match": "good"}, {"color": "#4EEBA9", "label": "Redelijk", "match": "tolerable"}, {"color": "#5CF434", "label": "Acceptabel", "match": "mediocre_good"}, {"color": "#FFFF17", "label": "Twijfelachtig", "match": "mediocre"}, {"color": "#DD882F", "label": "Slecht", "match": "mediocre_bad"}, {"color": "#BE4745", "label": "Zeer slecht", "match": "bad"}]}
16cc1db0-5f09-4673-9e07-e5ac573fc1b7	maplayer	foundation_indicative	Funderingstype Indicatief	{"type": "case_multimatch", "column": "foundation_type", "values": [{"color": "#de7171", "label": "Houten paalfundering", "match": ["wood", "wood_rotterdam", "wood_amsterdam"]}, {"color": "#ded971", "label": "Houten paalfundering met betonoplanger", "match": ["wood_charger"]}, {"color": "#97de71", "label": "Betonnen paalfundering", "match": ["concrete"]}, {"color": "#71decc", "label": "Niet onderheid", "match": ["no_pile", "no_pile_masonry", "no_pile_strips", "no_pile_concrete_floor", "no_pile_slit", "no_pile_bearing_floor"]}, {"color": "#7192de", "label": "Verzwaarde betonpuntpaal", "match": ["weighted_pile"]}, {"color": "#b271de", "label": "Combinatie", "match": ["combined"]}, {"color": "#de71c5", "label": "Stalen paalfundering", "match": ["steel_pile"]}, {"color": "#8a918a", "label": "Onbekend", "match": ["unknown"]}, {"color": "#00877a", "label": "Anders", "match": ["other"]}]}
73258fb5-54a4-4d0c-a85f-e0ca2313e67f	maplayer	foundation_risk	Funderingsrisico	{"type": "case", "column": "foundation_risk", "values": [{"color": "#42FF33", "label": "A", "match": "a"}, {"color": "#D1FF33", "label": "B", "match": "b"}, {"color": "#FFEC33", "label": "C", "match": "c"}, {"color": "#FFAC33", "label": "D", "match": "d"}, {"color": "#FF5533", "label": "E", "match": "e"}]}
307e6489-feb3-4be8-919c-4b5392fee8fb	maplayer	inquiry_sample_enforcement_term	Handhavingstermijn (jaar)	{"type": "range_num", "column": "enforcement_term", "values": [{"max": "100", "min": "25", "color": "#64DEBC", "label": "> 25"}, {"max": "25", "min": "20", "color": "#55E293", "label": "20 - 25"}, {"max": "20", "min": "15", "color": "#46E65F", "label": "15 - 20"}, {"max": "15", "min": "10", "color": "#4CEB36", "label": "10 - 15"}, {"max": "10", "min": "5", "color": "#77F025", "label": "5 - 10"}, {"max": "5", "min": "0", "color": "#AEF614", "label": "0 - 5"}, {"max": "0", "min": "-5", "color": "#D0E218", "label": " 0 - -5"}, {"max": "-5", "min": "-10", "color": "#CEB31B", "label": "-5 - -10"}, {"max": "-10", "min": "-15", "color": "#BB7F1E", "label": "-10 - -15"}, {"max": "-15", "min": "-20", "color": "#A85520", "label": "-15 - -20"}, {"max": "-20", "min": "-25", "color": "#973321", "label": "-20 - -25"}, {"max": "-25", "min": "-100", "color": "#86222A", "label": "< -25"}]}
f42a6826-c3a0-48b1-8c96-6c9ef753ed46	maplayer	inquiry_sample_foundation_type	Vastgestelde funderingstype	{"type": "case_multimatch", "column": "foundation_type", "values": [{"color": "#C75D43", "label": "Houten paal", "match": ["wood", "wood_charger", "weighted_pile", "wood_amsterdam", "wood_rotterdam"]}, {"color": "#6A6C70", "label": "Betonnen paal", "match": ["concrete"]}, {"color": "#FF5533", "label": "Op staal", "match": ["no_pile", "no_pile_masonry", "no_pile_strips", "no_pile_concrete_floor", "no_pile_slit", "no_pile_bearing_floor"]}, {"color": "#FFAC33", "label": "Stalen paal", "match": ["steel_pile"]}, {"color": "#FFEC33", "label": "Overig", "match": ["other", "combined"]}, {"color": "#8936D4", "label": "Onbekend", "match": ["unknown"]}]}
cc1cc6ca-6408-4a01-b16c-850c09cdb9c5	maplayer	incident_aggregate_category	Incidenten Gemeente Category	{"type": "case", "column": "category", "values": [{"color": "gray", "label": "Geen of nauwelijks", "match": "0"}, {"color": "yellow", "label": "Enkele", "match": "1"}, {"color": "blue", "label": "Meerdere", "match": "2"}]}
75709b4f-d82e-4d62-9be3-07cb2ca00cec	maplayer	inquiry_sample_damage_cause	Oorzaak Schade	{"type": "case_multimatch", "column": "damage_cause", "values": [{"color": "#55B5A7", "label": "Ontwateringsdiepte", "match": ["drainage"]}, {"color": "#4B8FBF", "label": "Overbelasting", "match": ["overcharge"]}, {"color": "#4145C9", "label": "Bacteriële aantasting", "match": ["bio_fungus_infection", "bio_infection"]}, {"color": "#8936D4", "label": "Schimmelaantasting", "match": ["fungus_infection", "drystand", "bio_fungus_infection"]}, {"color": "#DE2CCF", "label": "Bodemdaling", "match": ["subsidence"]}, {"color": "#D2386F", "label": "Planten en wortels", "match": ["vegetation"]}, {"color": "#C75D43", "label": "Aardbeving", "match": ["gas", "vibrations"]}, {"color": "#BBA14F", "label": "Partieel funderingsherstel", "match": ["partial_foundation_recovery"]}, {"color": "#95B05A", "label": "Constructiefout", "match": ["construction_flaw", "foundation_flaw", "construction_heave"]}, {"color": "#6EA466", "label": "Negatieve kleef", "match": ["negative_cling", "overcharge_negative_cling"]}, {"color": "#6A6C70", "label": "Onbekend", "match": ["unknown"]}]}
\.


--
-- PostgreSQL database dump complete
--

