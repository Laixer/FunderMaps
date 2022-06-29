--
-- FunderMaps database precompute classes
--

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
-- Data for Name: building_type; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".building_type WITH DATA;


--
-- Data for Name: analysis_foundation_risk; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".analysis_foundation_risk WITH DATA;


--
-- Data for Name: analysis_complete; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".analysis_complete WITH DATA;


--
-- Data for Name: statistics_product_buildings_restored; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".statistics_product_buildings_restored WITH DATA;


--
-- Data for Name: statistics_product_construction_years; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".statistics_product_construction_years WITH DATA;


--
-- Data for Name: statistics_product_data_collected; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".statistics_product_data_collected WITH DATA;


--
-- Data for Name: statistics_product_foundation_risk; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".statistics_product_foundation_risk WITH DATA;


--
-- Data for Name: statistics_product_foundation_type; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".statistics_product_foundation_type WITH DATA;


--
-- Data for Name: statistics_product_incidents; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".statistics_product_incidents WITH DATA;


--
-- Data for Name: statistics_product_inquiries; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".statistics_product_inquiries WITH DATA;
