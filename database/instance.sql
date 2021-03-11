--
-- FunderMaps database environment
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
-- Name: fundermaps; Type: DATABASE; Schema: -; Owner: fundermaps
--

DROP DATABASE IF EXISTS fundermaps;


--
-- Name: fundermaps_webservice; Type: ROLE; Schema: -; Owner: postgres
--

DROP ROLE IF EXISTS fundermaps_webservice;


--
-- Name: fundermaps_webapp; Type: ROLE; Schema: -; Owner: postgres
--

DROP ROLE IF EXISTS fundermaps_webapp;


--
-- Name: fundermaps_portal; Type: ROLE; Schema: -; Owner: postgres
--

DROP ROLE IF EXISTS fundermaps_portal;


--
-- Name: fundermaps_batch; Type: ROLE; Schema: -; Owner: postgres
--

DROP ROLE IF EXISTS fundermaps_batch;


--
-- Name: fundermaps; Type: ROLE; Schema: -; Owner: postgres
--

DROP ROLE IF EXISTS fundermaps;


--
-- Name: fundermaps; Type: ROLE; Schema: -; Owner: postgres
--

CREATE ROLE fundermaps WITH NOSUPERUSER NOCREATEDB NOCREATEROLE INHERIT NOLOGIN NOREPLICATION NOBYPASSRLS;


--
-- Name: fundermaps_batch; Type: ROLE; Schema: -; Owner: postgres
--

CREATE ROLE fundermaps_batch WITH NOSUPERUSER NOCREATEDB NOCREATEROLE NOINHERIT LOGIN NOREPLICATION NOBYPASSRLS;


--
-- Name: fundermaps_portal; Type: ROLE; Schema: -; Owner: postgres
--

CREATE ROLE fundermaps_portal WITH NOSUPERUSER NOCREATEDB NOCREATEROLE NOINHERIT LOGIN NOREPLICATION NOBYPASSRLS;


--
-- Name: fundermaps_webapp; Type: ROLE; Schema: -; Owner: postgres
--

CREATE ROLE fundermaps_webapp WITH NOSUPERUSER NOCREATEDB NOCREATEROLE NOINHERIT LOGIN NOREPLICATION NOBYPASSRLS;


--
-- Name: fundermaps_webservice; Type: ROLE; Schema: -; Owner: postgres
--

CREATE ROLE fundermaps_webservice WITH NOSUPERUSER NOCREATEDB NOCREATEROLE NOINHERIT LOGIN NOREPLICATION NOBYPASSRLS;


--
-- Name: fundermaps; Type: DATABASE; Schema: -; Owner: fundermaps
--

CREATE DATABASE fundermaps WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'en_US.UTF-8' LC_CTYPE = 'en_US.UTF-8';


ALTER DATABASE fundermaps OWNER TO fundermaps;
