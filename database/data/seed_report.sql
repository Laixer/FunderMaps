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
-- Data for Name: incident; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.incident (id, foundation_type, chained_building, owner, foundation_recovery, neightbor_recovery, foundation_damage_cause, document_file, note, contact, create_date, update_date, delete_date, foundation_damage_characteristics, environment_damage_characteristics, address, meta, audit_status, internal_note, question_type) FROM stdin;
\.


--
-- Data for Name: inquiry; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.inquiry (id, document_name, inspection, joint_measurement, floor_measurement, create_date, update_date, delete_date, note, document_date, document_file, attribution, access_policy, type, standard_f3o, audit_status) FROM stdin;
\.


--
-- Data for Name: inquiry_sample; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.inquiry_sample (id, inquiry, address, create_date, update_date, delete_date, note, built_year, substructure, overall_quality, wood_quality, construction_quality, wood_capacity_horizontal_quality, pile_wood_capacity_vertical_quality, carrying_capacity_quality, mason_quality, wood_quality_necessity, construction_level, wood_level, pile_diameter_top, pile_diameter_bottom, pile_head_level, pile_tip_level, foundation_depth, mason_level, concrete_charger_length, pile_distance_length, wood_penetration_depth, cpt, monitoring_well, groundwater_level_temp, groundlevel, groundwater_level_net, foundation_type, enforcement_term, recovery_advised, damage_cause, damage_characteristics, construction_pile, wood_type, wood_encroachement, crack_indoor_restored, crack_indoor_type, crack_indoor_size, crack_facade_front_restored, crack_facade_front_type, crack_facade_front_size, crack_facade_back_restored, crack_facade_back_type, crack_facade_back_size, crack_facade_left_restored, crack_facade_left_type, crack_facade_left_size, crack_facade_right_restored, crack_facade_right_type, crack_facade_right_size, deformed_facade, threshold_updown_skewed, threshold_front_level, threshold_back_level, skewed_parallel, skewed_perpendicular, skewed_facade, settlement_speed, skewed_window_frame) FROM stdin;
\.


--
-- Data for Name: project; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.project (id, dossier, note, start_date, end_date, create_date, update_date, delete_date, adviser, creator, lead) FROM stdin;
\.


--
-- Data for Name: project_sample; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.project_sample (id, project, create_date, update_date, delete_date, note, status, contact, address) FROM stdin;
\.


--
-- Data for Name: recovery; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.recovery (id, create_date, update_date, delete_date, note, attribution, access_policy, type, document_date, document_file, audit_status, document_name) FROM stdin;
\.


--
-- Data for Name: recovery_sample; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.recovery_sample (id, recovery, address, create_date, update_date, delete_date, note, status, type, pile_type, contractor, facade, permit, permit_date, recovery_date) FROM stdin;
\.


--
-- Name: incident_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.incident_id_seq', 12500, false);


--
-- Name: inquiry_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.inquiry_id_seq', 1, false);


--
-- Name: inquiry_sample_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.inquiry_sample_id_seq', 1, false);


--
-- Name: project_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.project_id_seq', 1, false);


--
-- Name: project_sample_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.project_sample_id_seq', 1, false);


--
-- Name: recovery_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.recovery_id_seq', 1, false);


--
-- Name: recovery_sample_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.recovery_sample_id_seq', 1, false);


--
-- PostgreSQL database dump complete
--

