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
-- Data for Name: premise_z; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

COPY data.premise_z (building_id, ground_00, ground_10, ground_20, ground_30, ground_40, ground_50, roof_25, rmse_25, roof_50, rmse_50, roof_75, rmse_75, roof_90, rmse_90, roof_95, rmse_95, roof_99, rmse_99, roof_flat, nr_ground_pts, nr_roof_pts, height_valid) FROM stdin;
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

