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
-- Data for Name: analysis_foundation_indicative; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".analysis_foundation_indicative WITH DATA;


--
-- Data for Name: analysis_foundation_risk; Type: TABLE DATA; Schema: data; Owner: fundermaps
--

REFRESH MATERIALIZED VIEW "data".analysis_foundation_risk WITH DATA;
