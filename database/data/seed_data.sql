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
-- Data for Name: building_cluster; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.building_cluster (building_id, cluster_id) FROM stdin;
\.


--
-- Data for Name: building_elevation; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.building_elevation (building_id, ground, roof) FROM stdin;
\.


--
-- Data for Name: building_geographic_region; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.building_geographic_region (building_id, geographic_region_id, code) FROM stdin;
\.


--
-- Data for Name: building_groundwater_level; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.building_groundwater_level (building_id, level) FROM stdin;
\.


--
-- Data for Name: building_ownership; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.building_ownership (building_id, owner) FROM stdin;
\.


--
-- Data for Name: building_pleistocene; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.building_pleistocene (building_id, depth) FROM stdin;
\.


--
-- Data for Name: subsidence; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.subsidence (building_id, velocity) FROM stdin;
gfm-2ec4949bdee94ec68031e95c2577e31e	-1.7999999690800002
\.


--
-- Data for Name: subsidence_hex; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.subsidence_hex (velocity, geom) FROM stdin;
\.


--
-- PostgreSQL database dump complete
--

