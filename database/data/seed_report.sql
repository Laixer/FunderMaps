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
FIR122021-1	wood	f	t	t	t	drystand	{deposit.dwf,adapter_e_enable.viv}	\N	Brady_Gorczany17@gmail.com	2021-03-19 15:50:26.566311+00	\N	\N	{crack,jamming_door_window,skewed}	{sagging_sewer_connection,vegetation_nearby}	gfm-351cc5645ab7457b92d3629e8c163f0b	{"SessionUser": "1a93cfb3-f097-4697-a998-71cdd9cfaead", "SessionOrganization": "05203318-6c55-43c1-a6a6-bb8c83f930c3"}	todo	\N	financial
FIR122021-2	wood	t	t	f	t	bio_fungus_infection	{payment_credit_card_account_dynamic.jsonld,tasty_steel_keyboard_input_b2b.xdm}	\N	Jany_Ritchie24@yahoo.com	2021-03-19 15:50:38.136926+00	\N	\N	{threshold_below_subsurface}	{increasing_traffic,vegetation_nearby,sewage_leakage,sagging_cables_pipes,low_ground_water,construction_nearby,foundation_damage_nearby,elevation}	gfm-351cc5645ab7457b92d3629e8c163f0b	{"SessionUser": "1a93cfb3-f097-4697-a998-71cdd9cfaead", "SessionOrganization": "05203318-6c55-43c1-a6a6-bb8c83f930c3"}	todo	\N	research
FIR122021-3	no_pile_strips	t	t	t	f	drainage	{circuit_object_based_sas.svgz,valley_deposit_synthesizing.qwd,background.fxp}	sunt	Zetta81@hotmail.com	2021-03-19 15:50:46.847066+00	\N	\N	{crawlspace_flooding,threshold_below_subsurface,skewed}	{flooding,sagging_cables_pipes,subsidence,construction_nearby,increasing_traffic,sagging_sewer_connection,low_ground_water,vegetation_nearby,sewage_leakage,foundation_damage_nearby}	gfm-351cc5645ab7457b92d3629e8c163f0b	{"SessionUser": "1a93cfb3-f097-4697-a998-71cdd9cfaead", "SessionOrganization": "05203318-6c55-43c1-a6a6-bb8c83f930c3"}	todo	\N	buy_sell
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
1	2021-03-19 15:53:29.756599+00	2021-03-19 15:53:30.01+00	\N	\N	1	public	archive_report	1409-08-15	https://gerardo.info/branding.pdf	pending	rubber_homogeneous.opml
2	2021-03-19 15:58:20.489823+00	2021-03-19 15:58:20.762597+00	\N	Delectus id est id praesentium nemo iusto eius quo. At voluptatem laudantium reprehenderit quis illo maxime provident. Eligendi laboriosam voluptatem praesentium. Rem dolor voluptatem placeat. Enim est cumque consequatur voluptatem.	2	private	owner_evidence	1838-07-04	https://maximilian.name/reactive/info-mediaries/sweden.pdf	pending	liberian_dollar.mpc
\.


--
-- Data for Name: recovery_sample; Type: TABLE DATA; Schema: report; Owner: fundermaps
--

COPY report.recovery_sample (id, recovery, address, create_date, update_date, delete_date, note, status, type, pile_type, contractor, facade, permit, permit_date, recovery_date) FROM stdin;
1	1	gfm-351cc5645ab7457b92d3629e8c163f0b	2021-03-19 15:53:29.845684+00	2021-03-19 15:53:30.005427+00	\N	Aliquam harum aut.	requested	beam_on_pile	press	62af863e-2021-4438-a5ea-730ed3db9eda	{sidewall_right,front,sidewall_left}	Fish	1996-03-02	2003-12-27
2	2	gfm-351cc5645ab7457b92d3629e8c163f0b	2021-03-19 15:58:20.582106+00	2021-03-19 15:58:20.757614+00	\N	Voluptatem exercitationem et deleniti. Molestiae eveniet et dolorem non est. Recusandae facere voluptatem earum ullam ut rerum totam. Nulla dolor facere dolorem. Rerum dolor ut officia numquam est nisi eos et.	requested	unknown	press	62af863e-2021-4438-a5ea-730ed3db9eda	{sidewall_right}	Pizza	2001-04-11	2006-10-09
\.


--
-- Name: incident_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.incident_id_seq', 10000, false);


--
-- Name: inquiry_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.inquiry_id_seq', 10000, false);


--
-- Name: inquiry_sample_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.inquiry_sample_id_seq', 10000, false);


--
-- Name: project_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.project_id_seq', 10000, false);


--
-- Name: project_sample_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.project_sample_id_seq', 10000, false);


--
-- Name: recovery_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.recovery_id_seq', 10000, true);


--
-- Name: recovery_sample_id_seq; Type: SEQUENCE SET; Schema: report; Owner: fundermaps
--

SELECT pg_catalog.setval('report.recovery_sample_id_seq', 10000, true);


--
-- PostgreSQL database dump complete
--

