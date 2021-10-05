--
-- PostgreSQL database dump
--

-- Dumped from database version 12.8 (Debian 12.8-1.pgdg100+1)
-- Dumped by pg_dump version 12.8 (Ubuntu 12.8-0ubuntu0.20.04.1)

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
-- Name: application; Type: SCHEMA; Schema: -; Owner: fundermaps
--

CREATE SCHEMA application;


ALTER SCHEMA application OWNER TO fundermaps;

--
-- Name: SCHEMA application; Type: COMMENT; Schema: -; Owner: fundermaps
--

COMMENT ON SCHEMA application IS 'Schema containing information about the organisations and users of our application.';


--
-- Name: data; Type: SCHEMA; Schema: -; Owner: fundermaps
--

CREATE SCHEMA data;


ALTER SCHEMA data OWNER TO fundermaps;

--
-- Name: SCHEMA data; Type: COMMENT; Schema: -; Owner: fundermaps
--

COMMENT ON SCHEMA data IS 'Schema containing our analysis and some external data used for analysis.';


--
-- Name: geocoder; Type: SCHEMA; Schema: -; Owner: fundermaps
--

CREATE SCHEMA geocoder;


ALTER SCHEMA geocoder OWNER TO fundermaps;

--
-- Name: SCHEMA geocoder; Type: COMMENT; Schema: -; Owner: fundermaps
--

COMMENT ON SCHEMA geocoder IS 'Schema containing our own format addresses, buildings and other geospacial information.';


--
-- Name: maplayer; Type: SCHEMA; Schema: -; Owner: fundermaps
--

CREATE SCHEMA maplayer;


ALTER SCHEMA maplayer OWNER TO fundermaps;

--
-- Name: report; Type: SCHEMA; Schema: -; Owner: fundermaps
--

CREATE SCHEMA report;


ALTER SCHEMA report OWNER TO fundermaps;

--
-- Name: SCHEMA report; Type: COMMENT; Schema: -; Owner: fundermaps
--

COMMENT ON SCHEMA report IS 'Schema containing everything with regards to reports, inquiries and incidents.';


--
-- Name: postgis; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS postgis WITH SCHEMA public;


--
-- Name: EXTENSION postgis; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis IS 'PostGIS geometry, geography, and raster spatial types and functions';


--
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


--
-- Name: access_policy; Type: TYPE; Schema: application; Owner: fundermaps
--

CREATE TYPE application.access_policy AS ENUM (
    'public',
    'private'
);


ALTER TYPE application.access_policy OWNER TO fundermaps;

--
-- Name: TYPE access_policy; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TYPE application.access_policy IS 'Enum representing the access policy for some object.';


--
-- Name: email; Type: DOMAIN; Schema: application; Owner: fundermaps
--

CREATE DOMAIN application.email AS text
	CONSTRAINT at CHECK (("position"(VALUE, '@'::text) > 0));


ALTER DOMAIN application.email OWNER TO fundermaps;

--
-- Name: DOMAIN email; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON DOMAIN application.email IS 'Domain for an email address.';


--
-- Name: organization_id; Type: DOMAIN; Schema: application; Owner: fundermaps
--

CREATE DOMAIN application.organization_id AS uuid DEFAULT public.uuid_generate_v4();


ALTER DOMAIN application.organization_id OWNER TO fundermaps;

--
-- Name: DOMAIN organization_id; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON DOMAIN application.organization_id IS 'Domain for an organization identifier.';


--
-- Name: organization_role; Type: TYPE; Schema: application; Owner: fundermaps
--

CREATE TYPE application.organization_role AS ENUM (
    'superuser',
    'verifier',
    'writer',
    'reader'
);


ALTER TYPE application.organization_role OWNER TO fundermaps;

--
-- Name: TYPE organization_role; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TYPE application.organization_role IS 'Enum representing an organization role.';


--
-- Name: phone; Type: DOMAIN; Schema: application; Owner: fundermaps
--

CREATE DOMAIN application.phone AS text
	CONSTRAINT all_int CHECK ((VALUE ~* '^[0-9]+$'::text));


ALTER DOMAIN application.phone OWNER TO fundermaps;

--
-- Name: DOMAIN phone; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON DOMAIN application.phone IS 'Domain for a phone number.';


--
-- Name: role; Type: TYPE; Schema: application; Owner: fundermaps
--

CREATE TYPE application.role AS ENUM (
    'administrator',
    'user',
    'guest'
);


ALTER TYPE application.role OWNER TO fundermaps;

--
-- Name: TYPE role; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TYPE application.role IS 'Enum representing a user role.';


--
-- Name: user_id; Type: DOMAIN; Schema: application; Owner: fundermaps
--

CREATE DOMAIN application.user_id AS uuid DEFAULT public.uuid_generate_v4();


ALTER DOMAIN application.user_id OWNER TO fundermaps;

--
-- Name: DOMAIN user_id; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON DOMAIN application.user_id IS 'Domain for a user identifier.';


--
-- Name: foundation_category; Type: TYPE; Schema: data; Owner: fundermaps
--

CREATE TYPE data.foundation_category AS ENUM (
    'wood',
    'no_pile',
    'other'
);


ALTER TYPE data.foundation_category OWNER TO fundermaps;

--
-- Name: TYPE foundation_category; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON TYPE data.foundation_category IS 'Enum indicating the category in which a foundation, whether indicative or not, falls.';


--
-- Name: foundation_risk_indication; Type: TYPE; Schema: data; Owner: fundermaps
--

CREATE TYPE data.foundation_risk_indication AS ENUM (
    'a',
    'b',
    'c',
    'd',
    'e'
);


ALTER TYPE data.foundation_risk_indication OWNER TO fundermaps;

--
-- Name: TYPE foundation_risk_indication; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON TYPE data.foundation_risk_indication IS 'Enum representing the foundation risk.';


--
-- Name: reliability; Type: TYPE; Schema: data; Owner: fundermaps
--

CREATE TYPE data.reliability AS ENUM (
    'indicative',
    'established'
);


ALTER TYPE data.reliability OWNER TO fundermaps;

--
-- Name: building_type; Type: TYPE; Schema: geocoder; Owner: fundermaps
--

CREATE TYPE geocoder.building_type AS ENUM (
    'house',
    'shed',
    'houseboat',
    'mobile_home'
);


ALTER TYPE geocoder.building_type OWNER TO fundermaps;

--
-- Name: TYPE building_type; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TYPE geocoder.building_type IS 'Enum representing a building type. This is a work in progress.';


--
-- Name: data_source; Type: TYPE; Schema: geocoder; Owner: fundermaps
--

CREATE TYPE geocoder.data_source AS ENUM (
    'nl_bag',
    'osm',
    'bag_one',
    'nl_cbs'
);


ALTER TYPE geocoder.data_source OWNER TO fundermaps;

--
-- Name: TYPE data_source; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TYPE geocoder.data_source IS 'Enum representing an external data source.';


--
-- Name: geocoder_generate_id(); Type: FUNCTION; Schema: geocoder; Owner: fundermaps
--

CREATE FUNCTION geocoder.geocoder_generate_id() RETURNS text
    LANGUAGE sql PARALLEL SAFE
    AS $$SELECT 'gfm-' || REPLACE(uuid_generate_v4()::text, '-', '')$$;


ALTER FUNCTION geocoder.geocoder_generate_id() OWNER TO fundermaps;

--
-- Name: FUNCTION geocoder_generate_id(); Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON FUNCTION geocoder.geocoder_generate_id() IS 'Generates a new geocoder id.';


--
-- Name: geocoder_id; Type: DOMAIN; Schema: geocoder; Owner: fundermaps
--

CREATE DOMAIN geocoder.geocoder_id AS text DEFAULT geocoder.geocoder_generate_id();


ALTER DOMAIN geocoder.geocoder_id OWNER TO fundermaps;

--
-- Name: DOMAIN geocoder_id; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON DOMAIN geocoder.geocoder_id IS 'Domain for our internal geocoder identifier.';


--
-- Name: year; Type: DOMAIN; Schema: geocoder; Owner: fundermaps
--

CREATE DOMAIN geocoder.year AS date DEFAULT CURRENT_TIMESTAMP
	CONSTRAINT range CHECK (((date_part('Y'::text, VALUE) > (900)::double precision) AND (date_part('Y'::text, VALUE) < (2100)::double precision)));


ALTER DOMAIN geocoder.year OWNER TO fundermaps;

--
-- Name: DOMAIN year; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON DOMAIN geocoder.year IS 'Domain for a year between 900 and 2100.';


--
-- Name: audit_status; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.audit_status AS ENUM (
    'todo',
    'pending',
    'done',
    'discarded',
    'rejected',
    'pending_review'
);


ALTER TYPE report.audit_status OWNER TO fundermaps;

--
-- Name: TYPE audit_status; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.audit_status IS 'Enum representing the status of an audit.';


--
-- Name: built_year_source; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.built_year_source AS ENUM (
    'bag',
    'fundermaps'
);


ALTER TYPE report.built_year_source OWNER TO fundermaps;

--
-- Name: construction_pile; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.construction_pile AS ENUM (
    'punched',
    'broken',
    'pinched',
    'pressed',
    'perished',
    'decay',
    'root_growth'
);


ALTER TYPE report.construction_pile OWNER TO fundermaps;

--
-- Name: TYPE construction_pile; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.construction_pile IS 'Enum representing the type of construction pile that was used in the building process of a building.';


--
-- Name: construction_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.construction_type AS ENUM (
    'brick',
    'concrete'
);


ALTER TYPE report.construction_type OWNER TO fundermaps;

--
-- Name: TYPE construction_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.construction_type IS 'Enum representing the material with which the walls of a building were constructed.';


--
-- Name: crack_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.crack_type AS ENUM (
    'very_small',
    'small',
    'mediocre',
    'big'
);


ALTER TYPE report.crack_type OWNER TO fundermaps;

--
-- Name: TYPE crack_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.crack_type IS 'Enum representing the type of a crack in the facade of a building.';


--
-- Name: diameter; Type: DOMAIN; Schema: report; Owner: fundermaps
--

CREATE DOMAIN report.diameter AS numeric(5,2);


ALTER DOMAIN report.diameter OWNER TO fundermaps;

--
-- Name: DOMAIN diameter; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON DOMAIN report.diameter IS 'Domain for the diameter of an object.';


--
-- Name: enforcement_term; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.enforcement_term AS ENUM (
    'term05',
    'term510',
    'term1020',
    'term5',
    'term10',
    'term15',
    'term20',
    'term25',
    'term30',
    'term40'
);


ALTER TYPE report.enforcement_term OWNER TO fundermaps;

--
-- Name: TYPE enforcement_term; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.enforcement_term IS 'Enum representing the lifetime of something.';


--
-- Name: environment_damage_characteristics; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.environment_damage_characteristics AS ENUM (
    'subsidence',
    'sagging_sewer_connection',
    'sagging_cables_pipes',
    'flooding',
    'foundation_damage_nearby',
    'elevation',
    'increasing_traffic',
    'construction_nearby',
    'vegetation_nearby',
    'sewage_leakage',
    'low_ground_water'
);


ALTER TYPE report.environment_damage_characteristics OWNER TO fundermaps;

--
-- Name: TYPE environment_damage_characteristics; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.environment_damage_characteristics IS 'Enum representing the type of environmental damage causes.';


--
-- Name: facade; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.facade AS ENUM (
    'front',
    'sidewall_left',
    'sidewall_right',
    'rear'
);


ALTER TYPE report.facade OWNER TO fundermaps;

--
-- Name: TYPE facade; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.facade IS 'Enum representing the sides of a buildings facade.';


--
-- Name: foundation_damage_cause; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.foundation_damage_cause AS ENUM (
    'drainage',
    'construction_flaw',
    'drystand',
    'overcharge',
    'overcharge_negative_cling',
    'negative_cling',
    'bio_infection',
    'fungus_infection',
    'bio_fungus_infection',
    'foundation_flaw',
    'construction_heave',
    'subsidence',
    'vegetation',
    'gas',
    'vibrations',
    'partial_foundation_recovery'
);


ALTER TYPE report.foundation_damage_cause OWNER TO fundermaps;

--
-- Name: TYPE foundation_damage_cause; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.foundation_damage_cause IS 'Enum representing the cause of damage to a buildings foundation.';


--
-- Name: foundation_damage_characteristics; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.foundation_damage_characteristics AS ENUM (
    'jamming_door_window',
    'crack',
    'skewed',
    'crawlspace_flooding',
    'threshold_above_subsurface',
    'threshold_below_subsurface',
    'crooked_floor_wall'
);


ALTER TYPE report.foundation_damage_characteristics OWNER TO fundermaps;

--
-- Name: TYPE foundation_damage_characteristics; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.foundation_damage_characteristics IS 'Enum representing the characteristics of foundation damage.';


--
-- Name: foundation_quality; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.foundation_quality AS ENUM (
    'bad',
    'mediocre',
    'tolerable',
    'good',
    'mediocre_good',
    'mediocre_bad'
);


ALTER TYPE report.foundation_quality OWNER TO fundermaps;

--
-- Name: TYPE foundation_quality; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.foundation_quality IS 'Enum representing the quality of a buildings foundation.';


--
-- Name: foundation_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.foundation_type AS ENUM (
    'wood',
    'concrete',
    'no_pile',
    'wood_charger',
    'weighted_pile',
    'combined',
    'steel_pile',
    'other',
    'no_pile_masonry',
    'no_pile_strips',
    'no_pile_concrete_floor',
    'no_pile_slit',
    'wood_amsterdam',
    'wood_rotterdam',
    'no_pile_bearing_floor',
    'wood_rotterdam_amsterdam',
    'wood_rotterdam_arch',
    'wood_amsterdam_arch'
);


ALTER TYPE report.foundation_type OWNER TO fundermaps;

--
-- Name: TYPE foundation_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.foundation_type IS 'Enum representing the type of a buildings foundation.';


--
-- Name: height; Type: DOMAIN; Schema: report; Owner: fundermaps
--

CREATE DOMAIN report.height AS numeric(5,2);


ALTER DOMAIN report.height OWNER TO fundermaps;

--
-- Name: DOMAIN height; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON DOMAIN report.height IS 'Domain for the height of an object.';


--
-- Name: incident_question_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.incident_question_type AS ENUM (
    'buy_sell',
    'registration',
    'legal',
    'financial',
    'guidance',
    'recovery',
    'research',
    'other'
);


ALTER TYPE report.incident_question_type OWNER TO fundermaps;

--
-- Name: TYPE incident_question_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.incident_question_type IS 'Enum representing the type of a question with regards to an incident report.';


--
-- Name: inquiry_status; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.inquiry_status AS ENUM (
    'todo',
    'pending',
    'done',
    'discarded',
    'rejected',
    'pending_review'
);


ALTER TYPE report.inquiry_status OWNER TO fundermaps;

--
-- Name: TYPE inquiry_status; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.inquiry_status IS 'Enum representing the status of an inquiry.';


--
-- Name: inquiry_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.inquiry_type AS ENUM (
    'monitoring',
    'note',
    'quickscan',
    'unknown',
    'demolition_research',
    'second_opinion',
    'archieve_research',
    'architectural_research',
    'foundation_advice',
    'inspectionpit',
    'foundation_research',
    'additional_research',
    'ground_water_level_research',
    'soil_investigation'
);


ALTER TYPE report.inquiry_type OWNER TO fundermaps;

--
-- Name: TYPE inquiry_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.inquiry_type IS 'Enum representing the type of an inquiry regarding a building.';


--
-- Name: length; Type: DOMAIN; Schema: report; Owner: fundermaps
--

CREATE DOMAIN report.length AS numeric(5,2);


ALTER DOMAIN report.length OWNER TO fundermaps;

--
-- Name: DOMAIN length; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON DOMAIN report.length IS 'Domain for the length of an object.';


--
-- Name: pile_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.pile_type AS ENUM (
    'press',
    'intgernally_driven',
    'segment'
);


ALTER TYPE report.pile_type OWNER TO fundermaps;

--
-- Name: TYPE pile_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.pile_type IS 'Enum representing the type of pile.';


--
-- Name: quality; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.quality AS ENUM (
    'nil',
    'small',
    'mediocre',
    'large'
);


ALTER TYPE report.quality OWNER TO fundermaps;

--
-- Name: TYPE quality; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.quality IS 'General enum representing the quality of some object.';


--
-- Name: recovery_document_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.recovery_document_type AS ENUM (
    'permit',
    'foundation_report',
    'archive_report',
    'owner_evidence',
    'unknown'
);


ALTER TYPE report.recovery_document_type OWNER TO fundermaps;

--
-- Name: TYPE recovery_document_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.recovery_document_type IS 'Enum representing the type of document that is attached to a building foundation recovery operation.';


--
-- Name: recovery_status; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.recovery_status AS ENUM (
    'planned',
    'requested',
    'executed'
);


ALTER TYPE report.recovery_status OWNER TO fundermaps;

--
-- Name: TYPE recovery_status; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.recovery_status IS 'Enum representing the status of a recovery operation for a buildings foundation.';


--
-- Name: recovery_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.recovery_type AS ENUM (
    'table',
    'beam_on_pile',
    'pile_lowering',
    'pile_in_wall',
    'injection',
    'unknown'
);


ALTER TYPE report.recovery_type OWNER TO fundermaps;

--
-- Name: TYPE recovery_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.recovery_type IS 'Enum representing the method of recovery used for recovering the foundation of a building.';


--
-- Name: rotation_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.rotation_type AS ENUM (
    'nil',
    'very_small',
    'small',
    'mediocre',
    'big'
);


ALTER TYPE report.rotation_type OWNER TO fundermaps;

--
-- Name: TYPE rotation_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.rotation_type IS 'Enum representing the rotation type of some foundation.';


--
-- Name: size; Type: DOMAIN; Schema: report; Owner: fundermaps
--

CREATE DOMAIN report.size AS numeric(5,2);


ALTER DOMAIN report.size OWNER TO fundermaps;

--
-- Name: DOMAIN size; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON DOMAIN report.size IS 'Domain for the size of an object.';


--
-- Name: substructure; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.substructure AS ENUM (
    'basement',
    'cellar',
    'crawlspace',
    'none'
);


ALTER TYPE report.substructure OWNER TO fundermaps;

--
-- Name: TYPE substructure; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.substructure IS 'Enum representing the type of substructure of a building.';


--
-- Name: wood_encroachement; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.wood_encroachement AS ENUM (
    'fungus_infection',
    'bio_fungus_infection',
    'bio_infection'
);


ALTER TYPE report.wood_encroachement OWNER TO fundermaps;

--
-- Name: TYPE wood_encroachement; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.wood_encroachement IS 'Enum representing the encroachment (aantasting) type of some wooden structure.';


--
-- Name: wood_quality; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.wood_quality AS ENUM (
    'area1',
    'area2',
    'area3',
    'area4'
);


ALTER TYPE report.wood_quality OWNER TO fundermaps;

--
-- Name: TYPE wood_quality; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.wood_quality IS 'Enum representing the quality of wood.';


--
-- Name: wood_type; Type: TYPE; Schema: report; Owner: fundermaps
--

CREATE TYPE report.wood_type AS ENUM (
    'pine',
    'spruce'
);


ALTER TYPE report.wood_type OWNER TO fundermaps;

--
-- Name: TYPE wood_type; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TYPE report.wood_type IS 'Enum representing the type of wood.';


--
-- Name: year; Type: DOMAIN; Schema: report; Owner: fundermaps
--

CREATE DOMAIN report.year AS date DEFAULT CURRENT_TIMESTAMP
	CONSTRAINT range CHECK (((date_part('Y'::text, VALUE) > (900)::double precision) AND (date_part('Y'::text, VALUE) < (2100)::double precision)));


ALTER DOMAIN report.year OWNER TO fundermaps;

--
-- Name: DOMAIN year; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON DOMAIN report.year IS 'Domain for a year between 900 and 2100.';


--
-- Name: create_geofence(json); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.create_geofence(json) RETURNS void
    LANGUAGE sql
    AS $_$SELECT ST_Multi(ST_SetSRID(ST_GeomFromGeoJSON($1), 4326))
$_$;


ALTER FUNCTION application.create_geofence(json) OWNER TO fundermaps;

--
-- Name: FUNCTION create_geofence(json); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.create_geofence(json) IS 'Create a multi-polygon in the GPS coordinate system from GeoJSON.';


--
-- Name: create_organization(application.organization_id, text, text); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.create_organization(organization_id application.organization_id, email text, password_hash text) RETURNS application.user_id
    LANGUAGE sql
    AS $_$
WITH move_organization AS (
	DELETE FROM application.organization_proposal
	WHERE id=$1
	RETURNING *
), new_organization AS (
	INSERT INTO application.organization(id, name, normalized_name, email, normalized_email)
	SELECT * FROM move_organization
	RETURNING id
)
SELECT application.create_organization_user(
	(
		SELECT id
	 	FROM new_organization
	),
	$2,
	$3,
	'superuser'::application.organization_role);
$_$;


ALTER FUNCTION application.create_organization(organization_id application.organization_id, email text, password_hash text) OWNER TO fundermaps;

--
-- Name: FUNCTION create_organization(organization_id application.organization_id, email text, password_hash text); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.create_organization(organization_id application.organization_id, email text, password_hash text) IS 'Setup an organization from an organization proposal.';


--
-- Name: create_organization_proposal(text, text); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.create_organization_proposal(organization_name text, email text) RETURNS application.organization_id
    LANGUAGE sql
    AS $_$INSERT INTO application.organization_proposal(name, normalized_name, email, normalized_email)
VALUES ($1, application.normalize($1), $2, application.normalize($2))
RETURNING id;$_$;


ALTER FUNCTION application.create_organization_proposal(organization_name text, email text) OWNER TO fundermaps;

--
-- Name: FUNCTION create_organization_proposal(organization_name text, email text); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.create_organization_proposal(organization_name text, email text) IS 'Create new organization proposal.';


--
-- Name: create_organization_user(application.organization_id, text, text); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text) RETURNS application.user_id
    LANGUAGE sql
    AS $_$INSERT INTO application.organization_user(user_id, organization_id)
SELECT application.create_user($2, $3), $1
RETURNING user_id;$_$;


ALTER FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text) OWNER TO fundermaps;

--
-- Name: FUNCTION create_organization_user(organization_id application.organization_id, email text, password_hash text); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text) IS 'Create a new user in the organization.';


--
-- Name: create_organization_user(application.organization_id, text, text, application.organization_role); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role) RETURNS application.user_id
    LANGUAGE sql
    AS $_$INSERT INTO application.organization_user(user_id, organization_id, role)
SELECT application.create_user($2, $3), $1, $4
RETURNING user_id;$_$;


ALTER FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role) OWNER TO fundermaps;

--
-- Name: FUNCTION create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role) IS 'Create a new user in the organization with organization role.';


--
-- Name: create_user(text); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.create_user(email text) RETURNS application.user_id
    LANGUAGE sql
    AS $_$INSERT INTO application."user"(email, normalized_email)
VALUES ($1, application.normalize($1))
RETURNING id;$_$;


ALTER FUNCTION application.create_user(email text) OWNER TO fundermaps;

--
-- Name: FUNCTION create_user(email text); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.create_user(email text) IS 'Create a new user with email address.';


--
-- Name: create_user(text, text); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.create_user(email text, password_hash text) RETURNS application.user_id
    LANGUAGE sql
    AS $_$INSERT INTO application."user"(email, normalized_email, password_hash)
VALUES ($1, application.normalize($1), $2)
RETURNING id;$_$;


ALTER FUNCTION application.create_user(email text, password_hash text) OWNER TO fundermaps;

--
-- Name: FUNCTION create_user(email text, password_hash text); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.create_user(email text, password_hash text) IS 'Create a new user with email address and password hash.';


--
-- Name: log_access(application.user_id); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.log_access(id application.user_id) RETURNS void
    LANGUAGE sql
    AS $_$
UPDATE  application.user
SET     login_count = login_count + 1,
        last_login = CURRENT_TIMESTAMP
WHERE   id = $1
;$_$;


ALTER FUNCTION application.log_access(id application.user_id) OWNER TO fundermaps;

--
-- Name: normalize(text); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.normalize(text) RETURNS text
    LANGUAGE sql
    AS $_$SELECT trim(upper($1))$_$;


ALTER FUNCTION application.normalize(text) OWNER TO fundermaps;

--
-- Name: FUNCTION normalize(text); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.normalize(text) IS 'Normalize the input so it can be compared.';


--
-- Name: normalize_email(); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.normalize_email() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
	NEW.normalized_email = application.normalize(NEW.email);
	RETURN NEW;
END;
$$;


ALTER FUNCTION application.normalize_email() OWNER TO fundermaps;

--
-- Name: FUNCTION normalize_email(); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.normalize_email() IS 'Normalize email after insert.';


--
-- Name: organization_email_free(text); Type: FUNCTION; Schema: application; Owner: fundermaps
--

CREATE FUNCTION application.organization_email_free(email text) RETURNS boolean
    LANGUAGE sql
    AS $_$SELECT NOT EXISTS (
	SELECT 1 FROM application.organization 
	WHERE normalized_email = application.normalize($1)
)$_$;


ALTER FUNCTION application.organization_email_free(email text) OWNER TO fundermaps;

--
-- Name: FUNCTION organization_email_free(email text); Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON FUNCTION application.organization_email_free(email text) IS 'Check if organization email is in use.';


--
-- Name: create_clusters(); Type: PROCEDURE; Schema: data; Owner: fundermaps
--

CREATE PROCEDURE data.create_clusters()
    LANGUAGE plpgsql
    AS $$
DECLARE
	local_cluster geocoder.geocoder_id[];
	neighbors geocoder.geocoder_id[];
	cluster_id uuid;
	counter integer := 0;
BEGIN
	DROP TABLE IF EXISTS building_all;
	CREATE TEMP TABLE building_all (
		building_id geocoder.geocoder_id NOT NULL,
		CONSTRAINT building_all_pkey PRIMARY KEY (building_id)
	);

	INSERT INTO building_all
	SELECT ba.id FROM geocoder.building_active AS ba
	EXCEPT
	SELECT c.building_id FROM data.building_cluster AS c;

	LOOP
		SELECT INTO cluster_id uuid_generate_v4();
		SELECT INTO local_cluster ARRAY[building_id] FROM building_all LIMIT 1;
		
		EXIT WHEN local_cluster IS NULL;
	
		neighbors := local_cluster;
	
		LOOP
			SELECT array_agg(b2.id) INTO neighbors
			FROM geocoder.building_active b
			JOIN geocoder.building_active b2 ON st_intersects(b.geom, b2.geom) AND b.built_year = b2.built_year AND b2.id <> all(local_cluster)
			WHERE b.built_year IS NOT NULL
			AND b.id = any(neighbors);

			EXIT WHEN neighbors IS NULL;

			local_cluster := local_cluster || neighbors;
		END LOOP;

		INSERT INTO data.building_cluster
		SELECT unnest(local_cluster), cluster_id
		ON CONFLICT DO NOTHING;

		DELETE FROM building_all
		WHERE building_id = any(local_cluster);
	
		IF counter % 1000 = 0 THEN
			RAISE NOTICE 'Counter %', counter;
			COMMIT;
		END IF;

		counter := counter+1;
	END LOOP;

	DROP TABLE building_all;
END;
$$;


ALTER PROCEDURE data.create_clusters() OWNER TO fundermaps;

--
-- Name: get_established_bio_infection_risk(boolean, report.foundation_damage_cause, report.enforcement_term, report.foundation_quality, boolean); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_established_bio_infection_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) RETURNS data.foundation_risk_indication
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	WHEN has_recovery
		THEN 'a'::data.foundation_risk_indication

	WHEN damage_cause = 'bio_infection'
		AND (enforcement_term = 'term05'
		OR enforcement_term = 'term5'
		OR recovery_advised
		OR overall_quality = 'bad')
		THEN 'e'::data.foundation_risk_indication

	WHEN damage_cause = 'bio_infection'
		AND (enforcement_term = 'term510'
		OR enforcement_term = 'term10'
		OR overall_quality = 'mediocre_bad')
		THEN 'd'::data.foundation_risk_indication

	WHEN damage_cause = 'bio_infection'
		AND (enforcement_term = 'term1020'
		OR enforcement_term = 'term15'
		OR enforcement_term = 'term20'
		OR overall_quality = 'mediocre'
		OR overall_quality = 'tolerable')
		THEN 'c'::data.foundation_risk_indication

	WHEN damage_cause = 'bio_infection'
		AND (enforcement_term = 'term25'
		OR enforcement_term = 'term30'
		OR enforcement_term = 'term40'
		OR overall_quality = 'good'
		OR overall_quality = 'mediocre_good')
		THEN 'b'::data.foundation_risk_indication

	ELSE NULL::data.foundation_risk_indication
END;
$$;


ALTER FUNCTION data.get_established_bio_infection_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) OWNER TO fundermaps;

--
-- Name: get_established_dewatering_depth_risk(boolean, report.foundation_damage_cause, report.enforcement_term, report.foundation_quality, boolean); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_established_dewatering_depth_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) RETURNS data.foundation_risk_indication
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	WHEN has_recovery
		THEN 'a'::data.foundation_risk_indication

	WHEN damage_cause = 'drainage'
		AND (enforcement_term = 'term05'
		OR enforcement_term = 'term5'
		OR recovery_advised
		OR overall_quality = 'bad')
		THEN 'e'::data.foundation_risk_indication

	WHEN damage_cause = 'drainage'
		AND (enforcement_term = 'term510'
		OR enforcement_term = 'term10'
		OR overall_quality = 'mediocre_bad')
		THEN 'd'::data.foundation_risk_indication

	WHEN damage_cause = 'drainage'
		AND (enforcement_term = 'term1020'
		OR enforcement_term = 'term15'
		OR enforcement_term = 'term20'
		OR overall_quality = 'mediocre'
		OR overall_quality = 'tolerable')
		THEN 'c'::data.foundation_risk_indication

	WHEN damage_cause = 'drainage' 
		AND (enforcement_term = 'term25'
		OR enforcement_term = 'term30'
		OR enforcement_term = 'term40'
		OR overall_quality = 'good'
		OR overall_quality = 'mediocre_good')
		THEN 'b'::data.foundation_risk_indication

	ELSE NULL::data.foundation_risk_indication
END;
$$;


ALTER FUNCTION data.get_established_dewatering_depth_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) OWNER TO fundermaps;

--
-- Name: get_established_drystand_risk(boolean, report.foundation_damage_cause, report.enforcement_term, report.foundation_quality, boolean); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_established_drystand_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) RETURNS data.foundation_risk_indication
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	WHEN has_recovery
		THEN 'a'::data.foundation_risk_indication

	WHEN (damage_cause = 'drystand' OR damage_cause = 'fungus_infection' OR damage_cause = 'bio_fungus_infection')
		AND (enforcement_term = 'term05'
		OR enforcement_term = 'term5'
		OR recovery_advised
		OR overall_quality = 'bad')
		THEN 'e'::data.foundation_risk_indication

	WHEN (damage_cause = 'drystand' OR damage_cause = 'fungus_infection' OR damage_cause = 'bio_fungus_infection')
		AND (enforcement_term = 'term510'
		OR enforcement_term = 'term10'
		OR overall_quality = 'mediocre_bad')
		THEN 'd'::data.foundation_risk_indication

	WHEN (damage_cause = 'drystand' OR damage_cause = 'fungus_infection' OR damage_cause = 'bio_fungus_infection')
		AND (enforcement_term = 'term1020'
		OR enforcement_term = 'term15'
		OR enforcement_term = 'term20'
		OR overall_quality = 'mediocre'
		OR overall_quality = 'tolerable')
		THEN 'c'::data.foundation_risk_indication

	WHEN (damage_cause = 'drystand' OR damage_cause = 'fungus_infection' OR damage_cause = 'bio_fungus_infection')
		AND (enforcement_term = 'term25'
		OR enforcement_term = 'term30'
		OR enforcement_term = 'term40'
		OR overall_quality = 'good'
		OR overall_quality = 'mediocre_good')
		THEN 'b'::data.foundation_risk_indication

	ELSE NULL::data.foundation_risk_indication
END;
$$;


ALTER FUNCTION data.get_established_drystand_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) OWNER TO fundermaps;

--
-- Name: get_established_unclassified_risk(boolean, report.foundation_damage_cause, report.enforcement_term, report.foundation_quality, boolean); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_established_unclassified_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) RETURNS data.foundation_risk_indication
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	WHEN has_recovery
		THEN 'a'::data.foundation_risk_indication

	WHEN damage_cause <> 'drystand'
		AND damage_cause <> 'bio_infection'
		AND damage_cause <> 'drainage'
		AND (enforcement_term = 'term05'
		OR enforcement_term = 'term5'
		OR recovery_advised
		OR overall_quality = 'bad')
		THEN 'e'::data.foundation_risk_indication

	WHEN damage_cause <> 'drystand'
		AND damage_cause <> 'bio_infection'
		AND damage_cause <> 'drainage'
		AND (enforcement_term = 'term510'
		OR enforcement_term = 'term10'
		OR overall_quality = 'mediocre_bad')
		THEN 'd'::data.foundation_risk_indication

	WHEN damage_cause <> 'drystand'
		AND damage_cause <> 'bio_infection'
		AND damage_cause <> 'drainage'
		AND (enforcement_term = 'term1020'
		OR enforcement_term = 'term15'
		OR enforcement_term = 'term20'
		OR overall_quality = 'mediocre'
		OR overall_quality = 'tolerable')
		THEN 'c'::data.foundation_risk_indication

	WHEN damage_cause <> 'drystand'
		AND damage_cause <> 'bio_infection'
		AND damage_cause <> 'drainage'
		AND (enforcement_term = 'term25'
		OR enforcement_term = 'term30'
		OR enforcement_term = 'term40'
		OR overall_quality = 'good'
		OR overall_quality = 'mediocre_good')
		THEN 'b'::data.foundation_risk_indication

	ELSE NULL::data.foundation_risk_indication
END;
$$;


ALTER FUNCTION data.get_established_unclassified_risk(has_recovery boolean, damage_cause report.foundation_damage_cause, enforcement_term report.enforcement_term, overall_quality report.foundation_quality, recovery_advised boolean) OWNER TO fundermaps;

--
-- Name: get_foundation_category(report.foundation_type, report.foundation_type); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type) RETURNS data.foundation_category
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $_$
SELECT
CASE
	WHEN COALESCE($2, $1) = 'wood'::report.foundation_type THEN 'wood'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'concrete'::report.foundation_type THEN 'other'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'no_pile'::report.foundation_type THEN 'no_pile'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'wood_charger'::report.foundation_type THEN 'wood'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'weighted_pile'::report.foundation_type THEN 'other'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'combined'::report.foundation_type THEN 'other'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'steel_pile'::report.foundation_type THEN 'other'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'other'::report.foundation_type THEN 'other'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'no_pile_masonry'::report.foundation_type THEN 'no_pile'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'no_pile_strips'::report.foundation_type THEN 'no_pile'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'no_pile_concrete_floor'::report.foundation_type THEN 'no_pile'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'no_pile_slit'::report.foundation_type THEN 'no_pile'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'wood_amsterdam'::report.foundation_type THEN 'wood'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'wood_rotterdam'::report.foundation_type THEN 'wood'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'wood_rotterdam_amsterdam'::report.foundation_type THEN 'wood'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'wood_amsterdam_arch'::report.foundation_type THEN 'wood'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'wood_rotterdam_arch'::report.foundation_type THEN 'wood'::"data".foundation_category
	WHEN COALESCE($2, $1) = 'no_pile_bearing_floor'::report.foundation_type THEN 'no_pile'::"data".foundation_category
	ELSE 'other'::"data".foundation_category
END;
$_$;


ALTER FUNCTION data.get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type) OWNER TO fundermaps;

--
-- Name: get_indicative_bio_infection_risk(boolean, report.foundation_type, double precision); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_indicative_bio_infection_risk(has_recovery boolean, foundation_type report.foundation_type, pile_length double precision) RETURNS data.foundation_risk_indication
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	WHEN has_recovery
		THEN 'a'::data.foundation_risk_indication

	WHEN (foundation_type = 'wood' OR foundation_type = 'wood_charger' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		AND pile_length <= 12
		THEN 'd'::data.foundation_risk_indication
		
	WHEN (foundation_type = 'wood' OR foundation_type = 'wood_charger' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		AND pile_length > 12
		AND pile_length <= 15
		THEN 'c'::data.foundation_risk_indication
		
	WHEN (foundation_type = 'wood' OR foundation_type = 'wood_charger' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		AND pile_length > 15
		THEN 'b'::data.foundation_risk_indication
		
	ELSE NULL::data.foundation_risk_indication
END;
$$;


ALTER FUNCTION data.get_indicative_bio_infection_risk(has_recovery boolean, foundation_type report.foundation_type, pile_length double precision) OWNER TO fundermaps;

--
-- Name: get_indicative_dewatering_depth_risk(boolean, boolean, report.foundation_type, integer, double precision, text, integer, double precision, double precision); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_indicative_dewatering_depth_risk(has_recovery boolean, is_indicative boolean, foundation_type report.foundation_type, construction_year integer, height double precision, geographic_region text, addresses integer, velocity double precision, ground_water double precision) RETURNS data.foundation_risk_indication
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	WHEN has_recovery
		THEN 'a'::data.foundation_risk_indication

	-- "A1"
	WHEN is_indicative
		AND construction_year >= 1940
		AND construction_year < 1970
		AND addresses >= 8
		THEN 'a'::data.foundation_risk_indication
	
	-- "B2"
	WHEN is_indicative
		AND construction_year >= 1970
		THEN 'a'::data.foundation_risk_indication

	-- "F1"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water < 0.6
		then 'c'::data.foundation_risk_indication
	
	-- "F2"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water >= 0.6
		then 'b'::data.foundation_risk_indication
	
	-- "F3"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-2.0)
		and ground_water < 0.6
		then 'e'::data.foundation_risk_indication
	
	-- "F4"	
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-2.0)
		and ground_water >= 0.6
		then 'd'::data.foundation_risk_indication
		
	-- "F5"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-2.0)
		and ground_water < 0.6
		then 'd'::data.foundation_risk_indication
	
	-- "F6"	
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-2.0)
		and ground_water >= 0.6
		then 'b'::data.foundation_risk_indication

	-- "F7"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity is null
		and ground_water >= 0.6
		then 'c'::data.foundation_risk_indication
	
	-- "F8"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity is null
		and ground_water < 0.6
		then 'b'::data.foundation_risk_indication
		
	-- "F9"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity < (-1.0)
		and ground_water >= 0.6
		then 'e'::data.foundation_risk_indication

	-- "F10"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity < (-1.0)
		and ground_water < 0.6
		then 'd'::data.foundation_risk_indication

	-- "F11"
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity >= (-1.0)
		and ground_water >= 0.6
		then 'c'::data.foundation_risk_indication

	-- "F12"		
	WHEN is_indicative
		AND construction_year >= 1800
		and construction_year < 1970
		AND height < 10.5
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity >= (-1.0)
		and ground_water < 0.6
		then 'b'::data.foundation_risk_indication

	-- "J1"
	WHEN is_indicative
		AND construction_year < 1700
		and velocity is null
		and ground_water < 0.6
		then 'c'::data.foundation_risk_indication

	-- "J2"	
	WHEN is_indicative
		AND construction_year < 1700
		and velocity is null
		and ground_water >= 0.6
		then 'b'::data.foundation_risk_indication

	-- "J3"
	WHEN is_indicative
		AND construction_year < 1700
		and velocity < (-1.0)
		and ground_water < 0.6
		then 'e'::data.foundation_risk_indication

	-- "J4"
	WHEN is_indicative
		AND construction_year < 1700
		and velocity < (-1.0)
		and ground_water >= 0.6
		then 'd'::data.foundation_risk_indication

	-- "J5"
	WHEN is_indicative
		AND construction_year < 1700
		and velocity >= (-1.0)
		and ground_water < 0.6
		then 'd'::data.foundation_risk_indication

	-- "J6"
	WHEN is_indicative
		AND construction_year < 1700
		and velocity >= (-1.0)
		and ground_water >= 0.6
		then 'c'::data.foundation_risk_indication

	-- "C1"
	WHEN is_indicative
		AND construction_year >= 1700
		and construction_year < 1800
		and height < 10.5
		and velocity is null
		and ground_water < 0.6
		then 'c'::data.foundation_risk_indication

	-- "C2"
	WHEN is_indicative
		AND construction_year >= 1700
		and construction_year < 1800
		and height < 10.5
		and velocity is null
		and ground_water >= 0.6
		then 'b'::data.foundation_risk_indication
	
	-- "C3"
	WHEN is_indicative
		AND construction_year >= 1700
		and construction_year < 1800
		and height < 10.5
		and velocity < (-1.0)
		and ground_water < 0.6
		then 'e'::data.foundation_risk_indication

	-- "C4"
	WHEN is_indicative
		AND construction_year >= 1700
		and construction_year < 1800
		and height < 10.5
		and velocity >= (-1.0)
		and ground_water < 0.6
		then 'd'::data.foundation_risk_indication

	-- "C5"
	WHEN is_indicative
		AND construction_year >= 1700
		and construction_year < 1800
		and height < 10.5
		and velocity < (-1.0)
		and ground_water >= 0.6
		then 'd'::data.foundation_risk_indication

	-- "C6"
	WHEN is_indicative
		AND construction_year >= 1700
		and construction_year < 1800
		and height < 10.5
		and velocity >= (-1.0)
		and ground_water >= 0.6
		then 'c'::data.foundation_risk_indication

	-- "D1"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity is null
		and ground_water < 0.6
		then 'c'::data.foundation_risk_indication

	-- "D2"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity is null
		and ground_water >= 0.6
		then 'b'::data.foundation_risk_indication

	-- "D3"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity < (-1.0)
		and ground_water < 0.6
		then 'e'::data.foundation_risk_indication	

	-- "D4"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity < (-1.0)
		and ground_water >= 0.6
		then 'd'::data.foundation_risk_indication
	
	-- "D5"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity >= (-1.0)
		and ground_water <= 0.6
		then 'd'::data.foundation_risk_indication
	
	-- "D6"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity >= (-1.0)
		and ground_water > 0.6
		then 'b'::data.foundation_risk_indication
	
	-- "M1"
	WHEN NOT is_indicative
		AND (foundation_type = 'no_pile' or foundation_type = 'no_pile_masonry' or foundation_type = 'no_pile_strips' or foundation_type = 'no_pile_concrete_floor' or foundation_type = 'no_pile_slit')
		and velocity is null
		and ground_water < 0.6
		then 'c'::data.foundation_risk_indication
	
	-- "M2"
	WHEN NOT is_indicative
		AND (foundation_type = 'no_pile' or foundation_type = 'no_pile_masonry' or foundation_type = 'no_pile_strips' or foundation_type = 'no_pile_concrete_floor' or foundation_type = 'no_pile_slit') 
		and velocity is null
		and ground_water >= 0.6
		then 'b'::data.foundation_risk_indication
	
	-- "M3"
	WHEN NOT is_indicative
		AND (foundation_type = 'no_pile' or foundation_type = 'no_pile_masonry' or foundation_type = 'no_pile_strips' or foundation_type = 'no_pile_concrete_floor' or foundation_type = 'no_pile_slit')
		and velocity < (-2.0)
		and ground_water < 0.6
		then 'e'::data.foundation_risk_indication
	
	-- "M4"	
	WHEN NOT is_indicative
		AND (foundation_type = 'no_pile' or foundation_type = 'no_pile_masonry' or foundation_type = 'no_pile_strips' or foundation_type = 'no_pile_concrete_floor' or foundation_type = 'no_pile_slit')
		and velocity < (-2.0)
		and ground_water >= 0.6
		then 'd'::data.foundation_risk_indication
		
	-- "M5"
	WHEN NOT is_indicative
		AND (foundation_type = 'no_pile' or foundation_type = 'no_pile_masonry' or foundation_type = 'no_pile_strips' or foundation_type = 'no_pile_concrete_floor' or foundation_type = 'no_pile_slit')
		and velocity >= (-2.0)
		and ground_water < 0.6
		then 'd'::data.foundation_risk_indication
	
	-- "M6"	
	WHEN NOT is_indicative
		AND (foundation_type = 'no_pile' or foundation_type = 'no_pile_masonry' or foundation_type = 'no_pile_strips' or foundation_type = 'no_pile_concrete_floor' or foundation_type = 'no_pile_slit')
		and velocity >= (-2.0)
		and ground_water >= 0.6
		then 'b'::data.foundation_risk_indication
		
	-- "M7"
	WHEN NOT is_indicative
		AND (foundation_type = 'concrete' OR foundation_type = 'weighted_pile')
		THEN 'a'::data.foundation_risk_indication
		
	ELSE NULL::data.foundation_risk_indication
END;
$$;


ALTER FUNCTION data.get_indicative_dewatering_depth_risk(has_recovery boolean, is_indicative boolean, foundation_type report.foundation_type, construction_year integer, height double precision, geographic_region text, addresses integer, velocity double precision, ground_water double precision) OWNER TO fundermaps;

--
-- Name: get_indicative_drystand_risk(boolean, boolean, report.foundation_type, integer, double precision, text, integer, double precision, double precision); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_indicative_drystand_risk(has_recovery boolean, is_indicative boolean, foundation_type report.foundation_type, construction_year integer, height double precision, geographic_region text, addresses integer, velocity double precision, ground_water double precision) RETURNS data.foundation_risk_indication
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	WHEN has_recovery
		THEN 'a'::data.foundation_risk_indication

	-- "A1"
	WHEN is_indicative
		AND construction_year >= 1940
		AND construction_year < 1970
		AND addresses >= 8
		THEN 'a'::data.foundation_risk_indication
	
	-- "B2"
	WHEN is_indicative
		AND construction_year >= 1970
		THEN 'a'::data.foundation_risk_indication

	-- "E1"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		AND (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water >= 1.5
		then 'c'::data.foundation_risk_indication

	-- "E2"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water < 1.5
		then 'b'::data.foundation_risk_indication

	-- "E3"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-2.0)
		and ground_water >= 1.5
		then 'e'::data.foundation_risk_indication

	-- "E4"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-2.0)
		and ground_water >= 1.5
		then 'd'::data.foundation_risk_indication

	-- "E5"
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-2.0)
		and ground_water < 1.5
		then 'd'::data.foundation_risk_indication

	-- "E6"		
	WHEN is_indicative
		AND (construction_year >= 1700
		and construction_year < 1800 or construction_year is null)
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-2.0)
		and ground_water < 1.5
		then 'b'::data.foundation_risk_indication

	-- "G1"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1975
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity is null
		and ground_water >= 1.5
		then 'c'::data.foundation_risk_indication

	-- "G2"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1975
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity is null
		and ground_water < 1.5
		then 'b'::data.foundation_risk_indication

	-- "G3"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1975
		and (height >= 10.5 or height is null) 
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity < (-2.0)
		and ground_water >= 1.5
		then 'e'::data.foundation_risk_indication

	-- "G4"	
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1975
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity < (-2.0)
		and ground_water < 1.5
		then 'd'::data.foundation_risk_indication	

	-- "G5"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1975
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity >= (-2.0)
		and ground_water >= 1.5
		then 'd'::data.foundation_risk_indication

	-- "G6"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1975
		and (height >= 10.5 or height is null)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		and velocity >= (-2.0)
		and ground_water < 1.5
		then 'c'::data.foundation_risk_indication

	-- "I1"
	WHEN is_indicative
		and construction_year >= 1925
		and construction_year < 1970
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water >= 2.5
		then 'c'::data.foundation_risk_indication

	-- "I2"	
	WHEN is_indicative
		and construction_year >= 1925
		and construction_year < 1970
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water < 2.5
		then 'b'::data.foundation_risk_indication

	-- "I3"
	WHEN is_indicative
		and construction_year >= 1925
		and construction_year < 1970
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-1.0)
		and ground_water >= 2.5
		then 'e'::data.foundation_risk_indication

	-- "I4"
	WHEN is_indicative
		and construction_year >= 1925
		and construction_year < 1970
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-1.0)
		and ground_water < 2.5
		then 'c'::data.foundation_risk_indication

	-- "I5"
	WHEN is_indicative
		and construction_year >= 1925
		and construction_year < 1970
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-1.0)
		and ground_water >= 2.5
		then 'c'::data.foundation_risk_indication
	
	-- "I6"
	WHEN is_indicative
		and construction_year >= 1925
		and construction_year < 1970
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-1.0)
		and ground_water < 2.5
		then 'b'::data.foundation_risk_indication

	-- "H1"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1925
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water >= 1.5
		then 'd'::data.foundation_risk_indication

	-- "H2"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1925
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity is null
		and ground_water < 1.5
		then 'b'::data.foundation_risk_indication
	
	-- "H3"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1925
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-2.0)
		and ground_water >= 1.5
		then 'e'::data.foundation_risk_indication
	
	-- "H4"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1925
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity < (-2.0)
		and ground_water < 1.5
		then 'c'::data.foundation_risk_indication
		
	-- "H5"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1925
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-2.0)
		and ground_water >= 1.5
		then 'c'::data.foundation_risk_indication
	
	-- "H6"
	WHEN is_indicative
		and construction_year >= 1800
		and construction_year < 1925
		and (height >= 10.5 or height is null)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		and velocity >= (-2.0)
		and ground_water < 1.5
		then 'b'::data.foundation_risk_indication

	-- "N1"
	WHEN NOT is_indicative
		AND (foundation_type = 'wood' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		and velocity is null
		and ground_water >= 1.5
		then 'c'::data.foundation_risk_indication

	-- "N2"
	WHEN NOT is_indicative
		AND (foundation_type = 'wood' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		and velocity is null
		and ground_water < 1.5
		then 'b'::data.foundation_risk_indication

	-- "N3"
	WHEN NOT is_indicative
		AND (foundation_type = 'wood' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		and velocity < (-2.0)
		and ground_water >= 1.5
		then 'e'::data.foundation_risk_indication

	-- "N4"
	WHEN NOT is_indicative
		AND (foundation_type = 'wood' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		and velocity >= (-2.0)
		and ground_water >= 1.5
		then 'd'::data.foundation_risk_indication

	-- "N5"
	WHEN NOT is_indicative
		AND (foundation_type = 'wood' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		and velocity < (-2.0)
		and ground_water < 1.5
		then 'd'::data.foundation_risk_indication

	-- "N6"		
	WHEN NOT is_indicative
		AND (foundation_type = 'wood' or foundation_type = 'wood_amsterdam' or foundation_type = 'wood_rotterdam')
		and velocity >= (-2.0)
		and ground_water < 1.5
		then 'b'::data.foundation_risk_indication

	-- "O1"
	WHEN NOT is_indicative
		AND foundation_type = 'wood_charger'
		and velocity is null
		and ground_water >= 2.5
		then 'c'::data.foundation_risk_indication

	-- "O2"
	WHEN NOT is_indicative
		AND foundation_type = 'wood_charger' 
		and velocity is null
		and ground_water < 2.5
		then 'b'::data.foundation_risk_indication

	-- "O3"
	WHEN NOT is_indicative
		AND foundation_type = 'wood_charger' 
		and velocity < (-2.0)
		and ground_water >= 2.5
		then 'e'::data.foundation_risk_indication

	-- "O4"
	WHEN NOT is_indicative
		AND foundation_type = 'wood_charger'
		and velocity >= (-2.0)
		and ground_water >= 2.5
		then 'd'::data.foundation_risk_indication

	-- "O5"
	WHEN NOT is_indicative
		AND foundation_type = 'wood_charger'
		and velocity < (-2.0)
		and ground_water < 2.5
		then 'd'::data.foundation_risk_indication

	-- "O6"		
	WHEN NOT is_indicative
		AND foundation_type = 'wood_charger'
		and velocity >= (-2.0)
		and ground_water < 2.5
		then 'b'::data.foundation_risk_indication
		
	-- "O7"
	WHEN NOT is_indicative
		AND (foundation_type = 'concrete' OR foundation_type = 'weighted_pile')
		THEN 'a'::data.foundation_risk_indication
		
	ELSE NULL::data.foundation_risk_indication
END;
$$;


ALTER FUNCTION data.get_indicative_drystand_risk(has_recovery boolean, is_indicative boolean, foundation_type report.foundation_type, construction_year integer, height double precision, geographic_region text, addresses integer, velocity double precision, ground_water double precision) OWNER TO fundermaps;

--
-- Name: get_indicative_foundation_type(integer, double precision, text, integer); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_indicative_foundation_type(construction_year integer, height double precision, geographic_region text, addresses integer) RETURNS report.foundation_type
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
	-- "A"
	WHEN construction_year >= 1940
		AND construction_year < 1970
		AND addresses >= 8
		THEN 'concrete'::report.foundation_type

	-- "B"
    WHEN construction_year >= 1970
		THEN 'concrete'::report.foundation_type

	-- "C"
	WHEN construction_year >= 1700
		AND construction_year < 1800
		AND height < 10.5
		THEN 'no_pile'::report.foundation_type
		
	-- "Deze is dubbel met E en D"
	WHEN construction_year >= 1700
		AND construction_year < 1800
		AND height >= 10.5
		THEN 'wood'::report.foundation_type

	-- "D"
	WHEN construction_year >= 1700
		AND construction_year < 1800
		AND (height >= 10.5 OR height IS NULL)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		THEN 'no_pile'::report.foundation_type

	-- "E"
	WHEN construction_year >= 1700
		AND construction_year < 1800
		AND (height >= 10.5 OR height IS NULL)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		THEN 'wood'::report.foundation_type

	-- "F"
	WHEN construction_year >= 1800
		AND construction_year < 1970
		AND height < 10.5
		THEN 'no_pile'::report.foundation_type

	-- "G"
	WHEN construction_year >= 1800
		AND construction_year < 1970
		AND (height >= 10.5 OR height IS NULL)
		AND (geographic_region = 'hz'::text OR geographic_region = 'ni-hz'::text OR geographic_region = 'ni-du'::text)
		THEN 'wood'::report.foundation_type
	
	-- "H"
	WHEN construction_year >= 1800
		AND construction_year < 1925
		AND (height >= 10.5 OR height IS NULL)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		THEN 'wood'::report.foundation_type

	-- "I"
	WHEN construction_year >= 1925
		AND construction_year < 1970
		AND (height >= 10.5 OR height IS NULL)
		AND ((geographic_region <> 'hz'::text AND geographic_region <> 'ni-hz'::text AND geographic_region <> 'ni-du'::text) OR geographic_region IS NULL)
		THEN 'wood_charger'::report.foundation_type

	-- "J"
	WHEN construction_year < 1700
		THEN 'no_pile'::report.foundation_type

	-- "K"
	WHEN height >= 10.5
		THEN 'wood'::report.foundation_type

	-- "L"
	WHEN height < 10.5
		THEN 'no_pile'::report.foundation_type

	ELSE 'other'::report.foundation_type
END;
$$;


ALTER FUNCTION data.get_indicative_foundation_type(construction_year integer, height double precision, geographic_region text, addresses integer) OWNER TO fundermaps;

--
-- Name: get_restoration_cost(report.foundation_type, numeric); Type: FUNCTION; Schema: data; Owner: fundermaps
--

CREATE FUNCTION data.get_restoration_cost(foundation_type report.foundation_type, surface_area numeric) RETURNS integer
    LANGUAGE sql IMMUTABLE PARALLEL SAFE
    AS $$
SELECT
CASE
    WHEN foundation_type = 'wood'
    THEN round((surface_area * 950::double precision)::numeric, '-2'::integer)::integer
    
    WHEN foundation_type = 'no_pile'
    THEN round((surface_area * 350::double precision)::numeric, '-2'::integer)::integer

    ELSE NULL::integer
END;
$$;


ALTER FUNCTION data.get_restoration_cost(foundation_type report.foundation_type, surface_area numeric) OWNER TO fundermaps;

--
-- Name: fir_generate_id(integer); Type: FUNCTION; Schema: report; Owner: fundermaps
--

CREATE FUNCTION report.fir_generate_id(client_id integer) RETURNS text
    LANGUAGE sql
    AS $_$select 'FIR' || lpad($1::text, 2, '0') || date_part('year', CURRENT_DATE) || '-' || nextval('report.incident_id_seq');
$_$;


ALTER FUNCTION report.fir_generate_id(client_id integer) OWNER TO fundermaps;

--
-- Name: FUNCTION fir_generate_id(client_id integer); Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON FUNCTION report.fir_generate_id(client_id integer) IS 'Generate a new FIR identifier.';


--
-- Name: last_record_update(); Type: FUNCTION; Schema: report; Owner: fundermaps
--

CREATE FUNCTION report.last_record_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	NEW.update_date = CURRENT_TIMESTAMP;
	RETURN NEW;
END;
$$;


ALTER FUNCTION report.last_record_update() OWNER TO fundermaps;

--
-- Name: FUNCTION last_record_update(); Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON FUNCTION report.last_record_update() IS 'Trigger function that sets the update date when a record is updated.';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: attribution; Type: TABLE; Schema: application; Owner: fundermaps
--

CREATE TABLE application.attribution (
    id integer NOT NULL,
    reviewer application.user_id,
    creator application.user_id NOT NULL,
    owner application.organization_id NOT NULL,
    contractor application.organization_id NOT NULL,
    CONSTRAINT creator_reviewer_chk CHECK (((creator)::uuid <> (reviewer)::uuid))
);


ALTER TABLE application.attribution OWNER TO fundermaps;

--
-- Name: TABLE attribution; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TABLE application.attribution IS 'Intermediate object between an uploaded sample or report and the assigned user reviewer or owner. In case of user deletion a reference still exists, only the reference to said user or owner will be set to NULL.';


--
-- Name: attribution_id_seq; Type: SEQUENCE; Schema: application; Owner: fundermaps
--

CREATE SEQUENCE application.attribution_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE application.attribution_id_seq OWNER TO fundermaps;

--
-- Name: attribution_id_seq; Type: SEQUENCE OWNED BY; Schema: application; Owner: fundermaps
--

ALTER SEQUENCE application.attribution_id_seq OWNED BY application.attribution.id;


--
-- Name: contact; Type: TABLE; Schema: application; Owner: fundermaps
--

CREATE TABLE application.contact (
    email text NOT NULL,
    name text,
    phone_number text
);


ALTER TABLE application.contact OWNER TO fundermaps;

--
-- Name: TABLE contact; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TABLE application.contact IS 'Contains contact details.';


--
-- Name: organization; Type: TABLE; Schema: application; Owner: fundermaps
--

CREATE TABLE application.organization (
    id application.organization_id NOT NULL,
    name text NOT NULL,
    normalized_name text NOT NULL,
    email application.email NOT NULL,
    phone_number application.phone,
    branding_logo text,
    home_address text,
    home_address_number integer,
    home_address_number_postfix text,
    home_city text,
    home_postbox text,
    home_zipcode text,
    fence public.geometry(MultiPolygon,4326),
    create_date timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    update_date timestamp with time zone,
    delete_date timestamp with time zone,
    normalized_email text NOT NULL,
    access_public boolean DEFAULT true NOT NULL
);


ALTER TABLE application.organization OWNER TO fundermaps;

--
-- Name: TABLE organization; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TABLE application.organization IS 'Contains all organizations that are using FunderMaps.';


--
-- Name: COLUMN organization.create_date; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON COLUMN application.organization.create_date IS 'Timestamp of record creation, set by insert';


--
-- Name: COLUMN organization.update_date; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON COLUMN application.organization.update_date IS 'Timestamp of last record update, automatically updated on record modification';


--
-- Name: COLUMN organization.delete_date; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON COLUMN application.organization.delete_date IS 'Timestamp of soft delete';


--
-- Name: organization_proposal; Type: TABLE; Schema: application; Owner: fundermaps
--

CREATE TABLE application.organization_proposal (
    id application.organization_id NOT NULL,
    name text NOT NULL,
    normalized_name text NOT NULL,
    email application.email NOT NULL,
    normalized_email text NOT NULL,
    CONSTRAINT organization_proposal_email_chk CHECK (application.organization_email_free((email)::text))
);


ALTER TABLE application.organization_proposal OWNER TO fundermaps;

--
-- Name: TABLE organization_proposal; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TABLE application.organization_proposal IS 'Table containing organizations when they request registration. After the registration has been validated, the entry will be deleted from this table and a new entry will be created in the application.organization table.';


--
-- Name: organization_user; Type: TABLE; Schema: application; Owner: fundermaps
--

CREATE TABLE application.organization_user (
    user_id application.user_id NOT NULL,
    organization_id application.organization_id NOT NULL,
    role application.organization_role DEFAULT 'reader'::application.organization_role NOT NULL
);


ALTER TABLE application.organization_user OWNER TO fundermaps;

--
-- Name: TABLE organization_user; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TABLE application.organization_user IS 'Linking table between organizations and their users.';


--
-- Name: product_telemetry; Type: TABLE; Schema: application; Owner: fundermaps
--

CREATE TABLE application.product_telemetry (
    user_id application.user_id,
    organization_id application.organization_id NOT NULL,
    product text NOT NULL,
    count bigint DEFAULT 0 NOT NULL,
    meta jsonb
);


ALTER TABLE application.product_telemetry OWNER TO fundermaps;

--
-- Name: user; Type: TABLE; Schema: application; Owner: fundermaps
--

CREATE TABLE application."user" (
    id application.user_id NOT NULL,
    given_name text,
    last_name text,
    email application.email NOT NULL,
    normalized_email application.email NOT NULL,
    email_confirmed boolean DEFAULT false NOT NULL,
    avatar text,
    job_title text,
    password_hash text,
    phone_number application.phone,
    two_factor_enabled boolean DEFAULT false NOT NULL,
    access_failed_count integer DEFAULT 0 NOT NULL,
    role application.role DEFAULT 'user'::application.role NOT NULL,
    last_login timestamp with time zone,
    login_count integer DEFAULT 0 NOT NULL,
    phone_number_confirmed boolean DEFAULT false NOT NULL
);


ALTER TABLE application."user" OWNER TO fundermaps;

--
-- Name: TABLE "user"; Type: COMMENT; Schema: application; Owner: fundermaps
--

COMMENT ON TABLE application."user" IS 'Contains all FunderMaps users.';


--
-- Name: building_elevation; Type: TABLE; Schema: data; Owner: fundermaps
--

CREATE TABLE data.building_elevation (
    building_id geocoder.geocoder_id NOT NULL,
    ground real,
    roof real
);


ALTER TABLE data.building_elevation OWNER TO fundermaps;

--
-- Name: TABLE building_elevation; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON TABLE data.building_elevation IS 'Contains elevation data of buildings. This dataset is based on the BAG 1.0 PND dataset and has been acquired from the TU Delft. The ground units is taken from ground_50. The root unit is taken from roof_99.';


--
-- Name: building_geographic_region; Type: TABLE; Schema: data; Owner: fundermaps
--

CREATE TABLE data.building_geographic_region (
    building_id geocoder.geocoder_id NOT NULL,
    geographic_region_id integer NOT NULL,
    code text NOT NULL
);


ALTER TABLE data.building_geographic_region OWNER TO fundermaps;

--
-- Name: TABLE building_geographic_region; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON TABLE data.building_geographic_region IS 'Contains an st_intersects between geocoder.building and geographic region.';


--
-- Name: building_height; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.building_height AS
 SELECT be.building_id,
    (be.roof - be.ground) AS height
   FROM data.building_elevation be
  WHERE ((be.roof IS NOT NULL) AND (be.ground IS NOT NULL));


ALTER TABLE data.building_height OWNER TO fundermaps;

--
-- Name: VIEW building_height; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON VIEW data.building_height IS 'Absolute building height';


--
-- Name: address; Type: TABLE; Schema: geocoder; Owner: fundermaps
--

CREATE TABLE geocoder.address (
    id geocoder.geocoder_id NOT NULL,
    building_number text NOT NULL,
    postal_code text,
    street text NOT NULL,
    is_active boolean NOT NULL,
    external_id text NOT NULL,
    external_source geocoder.data_source NOT NULL,
    city text NOT NULL,
    building_id geocoder.geocoder_id
);


ALTER TABLE geocoder.address OWNER TO fundermaps;

--
-- Name: TABLE address; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TABLE geocoder.address IS 'Contains all addresses in our own format, including a tsvector column to enable full text search.';


--
-- Name: building; Type: TABLE; Schema: geocoder; Owner: fundermaps
--

CREATE TABLE geocoder.building (
    id geocoder.geocoder_id NOT NULL,
    built_year geocoder.year,
    is_active boolean NOT NULL,
    geom public.geometry(MultiPolygon,4326) NOT NULL,
    external_id text NOT NULL,
    external_source geocoder.data_source NOT NULL,
    building_type geocoder.building_type,
    neighborhood_id geocoder.geocoder_id
);


ALTER TABLE geocoder.building OWNER TO fundermaps;

--
-- Name: TABLE building; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TABLE geocoder.building IS 'Contains all buildings in our own format.';


--
-- Name: building_type; Type: MATERIALIZED VIEW; Schema: data; Owner: fundermaps
--

CREATE MATERIALIZED VIEW data.building_type AS
 SELECT building.id,
    'shed'::geocoder.building_type AS building_type
   FROM ( SELECT ba.id
           FROM geocoder.building ba
          WHERE (public.st_area((ba.geom)::public.geography, true) < (20)::double precision)
        EXCEPT
         SELECT a.building_id
           FROM geocoder.address a
          WHERE (a.building_id IS NOT NULL)) building
  WITH NO DATA;


ALTER TABLE data.building_type OWNER TO fundermaps;

--
-- Name: subsidence; Type: TABLE; Schema: data; Owner: fundermaps
--

CREATE TABLE data.subsidence (
    building_id geocoder.geocoder_id NOT NULL,
    velocity double precision NOT NULL
);


ALTER TABLE data.subsidence OWNER TO fundermaps;

--
-- Name: TABLE subsidence; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON TABLE data.subsidence IS 'Contains subsidence data for buildings. This has been acquired from multiple external data sources.';


--
-- Name: building_active; Type: VIEW; Schema: geocoder; Owner: fundermaps
--

CREATE VIEW geocoder.building_active AS
 SELECT building.id,
    building.built_year,
    building.geom,
    building.external_id,
    building.external_source,
    building.building_type,
    building.neighborhood_id
   FROM geocoder.building
  WHERE ((building.is_active = true) AND (building.geom IS NOT NULL));


ALTER TABLE geocoder.building_active OWNER TO fundermaps;

--
-- Name: VIEW building_active; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON VIEW geocoder.building_active IS 'Contains all entries from geocoder.building which have their status set to active.';


--
-- Name: analysis_foundation_indicative; Type: MATERIALIZED VIEW; Schema: data; Owner: fundermaps
--

CREATE MATERIALIZED VIEW data.analysis_foundation_indicative AS
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'concrete'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) >= (1970)::double precision))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'no_pile'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (10.5)::double precision) AND (gr.code <> 'hz'::text))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'no_pile'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (10.5)::double precision) AND (gr.code = 'hz'::text))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'wood'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (10.5)::double precision) AND (gr.code = 'hz'::text))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'wood_charger'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1925)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (10.5)::double precision) AND (gr.code <> 'hz'::text))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'wood'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1925)::double precision) AND (bh.height >= (10.5)::double precision) AND (gr.code <> 'hz'::text))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'no_pile'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height < (10.5)::double precision))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'no_pile'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) < (1700)::double precision))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    bh.height,
    b.geom,
    'wood'::report.foundation_type AS foundation_type
   FROM ((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height > (10.5)::double precision))
  WITH NO DATA;


ALTER TABLE data.analysis_foundation_indicative OWNER TO fundermaps;

--
-- Name: building_groundwater_level; Type: TABLE; Schema: data; Owner: fundermaps
--

CREATE TABLE data.building_groundwater_level (
    building_id geocoder.geocoder_id NOT NULL,
    level double precision NOT NULL
);


ALTER TABLE data.building_groundwater_level OWNER TO fundermaps;

--
-- Name: analysis_foundation_risk; Type: MATERIALIZED VIEW; Schema: data; Owner: fundermaps
--

CREATE MATERIALIZED VIEW data.analysis_foundation_risk AS
 SELECT b.id,
    b.neighborhood_id,
    gr.code,
    gwl.level AS groundwater_level,
    b.geom,
    'a'::data.foundation_risk_indication AS foundation_risk
   FROM (((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_groundwater_level gwl ON (((gwl.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE ((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) >= (1970)::double precision))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    gr.code,
    gwl.level AS groundwater_level,
    b.geom,
    'b'::data.foundation_risk_indication AS foundation_risk
   FROM (((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_groundwater_level gwl ON (((gwl.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE (((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) < (1700)::double precision) AND (gwl.level > (0.6)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height < (9.5)::double precision) AND (gwl.level > (0.6)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height > (9.5)::double precision) AND (gwl.level < (1.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height >= (9.5)::double precision) AND (gwl.level < (1.5)::double precision) AND (s.velocity > (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level > (0.6)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height > (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level < (1.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level < (1.5)::double precision) AND (s.velocity > (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1925)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height > (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level < (2.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1925)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level < (2.5)::double precision) AND (s.velocity > (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1925)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level < (1.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1925)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level < (1.5)::double precision) AND (s.velocity > (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level > (0.6)::double precision) AND (s.velocity > (- (1)::double precision))))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    gr.code,
    gwl.level AS groundwater_level,
    b.geom,
    'c'::data.foundation_risk_indication AS foundation_risk
   FROM (((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_groundwater_level gwl ON (((gwl.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE (((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) < (1700)::double precision) AND (gwl.level < (0.6)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) < (1700)::double precision) AND (gwl.level > (0.6)::double precision) AND (s.velocity > (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height < (9.5)::double precision) AND (gwl.level < (0.6)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height >= (9.5)::double precision) AND (gwl.level > (1.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level < (0.6)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level < (0.6)::double precision) AND (s.velocity > (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level > (1.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1925)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level > (2.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1925)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level > (2.5)::double precision) AND (s.velocity > (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1925)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level < (2.5)::double precision) AND (s.velocity < (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1925)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level > (1.5)::double precision) AND (s.velocity > (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1925)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level < (1.5)::double precision) AND (s.velocity < (- (2.0)::double precision))))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    gr.code,
    gwl.level AS groundwater_level,
    b.geom,
    'd'::data.foundation_risk_indication AS foundation_risk
   FROM (((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_groundwater_level gwl ON (((gwl.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE (((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1925)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level > (1.5)::double precision) AND (s.velocity IS NULL)) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level > (0.6)::double precision)) OR ((date_part('year'::text, (b.built_year)::date) < (1700)::double precision) AND (gwl.level > (0.6)::double precision) AND (s.velocity < (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) < (1700)::double precision) AND (gwl.level < (0.6)::double precision) AND (s.velocity > (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height < (9.5)::double precision) AND (gwl.level < (0.6)::double precision) AND (s.velocity > (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height >= (9.5)::double precision) AND (gwl.level > (1.5)::double precision) AND (s.velocity > (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height >= (9.5)::double precision) AND (gwl.level < (1.5)::double precision) AND (s.velocity < (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level > (0.6)::double precision) AND (s.velocity < (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height > (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level > (1.5)::double precision) AND (s.velocity > (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level < (1.5)::double precision) AND (s.velocity < (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1925)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level > (2.5)::double precision) AND (s.velocity < (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height < (9.5)::double precision) AND (gwl.level > (0.6)::double precision) AND (s.velocity < (- (1)::double precision))))
UNION ALL
 SELECT b.id,
    b.neighborhood_id,
    gr.code,
    gwl.level AS groundwater_level,
    b.geom,
    'e'::data.foundation_risk_indication AS foundation_risk
   FROM (((((geocoder.building_active b
     LEFT JOIN data.building_type bt ON ((((bt.id)::text = (b.id)::text) AND (bt.building_type = 'shed'::geocoder.building_type))))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_groundwater_level gwl ON (((gwl.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
  WHERE (((bt.id IS NULL) AND (date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level < (0.6)::double precision)) OR ((date_part('year'::text, (b.built_year)::date) < (1700)::double precision) AND (gwl.level < (0.6)::double precision) AND (s.velocity < (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height < (9.5)::double precision) AND (gwl.level > (0.6)::double precision) AND (s.velocity > (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height >= (9.5)::double precision) AND (gwl.level > (1.5)::double precision) AND (s.velocity < (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height < (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level < (0.6)::double precision) AND (s.velocity < (- (1)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1970)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code = 'hz'::text) AND (gwl.level > (1.5)::double precision) AND (s.velocity < (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1800)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1925)::double precision) AND (bh.height >= (9.5)::double precision) AND (gr.code <> 'hz'::text) AND (gwl.level > (1.5)::double precision) AND (s.velocity < (- (2.0)::double precision))) OR ((date_part('year'::text, (b.built_year)::date) > (1700)::double precision) AND (date_part('year'::text, (b.built_year)::date) < (1800)::double precision) AND (bh.height < (9.5)::double precision) AND (gwl.level < (0.6)::double precision) AND (s.velocity < (- (1.0)::double precision))))
  WITH NO DATA;


ALTER TABLE data.analysis_foundation_risk OWNER TO fundermaps;

--
-- Name: inquiry; Type: TABLE; Schema: report; Owner: fundermaps
--

CREATE TABLE report.inquiry (
    id integer NOT NULL,
    document_name text NOT NULL,
    inspection boolean DEFAULT false NOT NULL,
    joint_measurement boolean DEFAULT false NOT NULL,
    floor_measurement boolean DEFAULT false NOT NULL,
    create_date timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    update_date timestamp with time zone,
    delete_date timestamp with time zone,
    note text,
    document_date date NOT NULL,
    document_file text NOT NULL,
    attribution integer NOT NULL,
    access_policy application.access_policy DEFAULT 'private'::application.access_policy NOT NULL,
    type report.inquiry_type NOT NULL,
    standard_f3o boolean DEFAULT false NOT NULL,
    audit_status report.audit_status DEFAULT 'todo'::report.audit_status NOT NULL
);


ALTER TABLE report.inquiry OWNER TO fundermaps;

--
-- Name: TABLE inquiry; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TABLE report.inquiry IS 'Contains inquiries.';


--
-- Name: COLUMN inquiry.document_name; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.inquiry.document_name IS 'User provided document name';


--
-- Name: COLUMN inquiry.create_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.inquiry.create_date IS 'Timestamp of record creation, set by insert';


--
-- Name: COLUMN inquiry.update_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.inquiry.update_date IS 'Timestamp of last record update, automatically updated on record modification';


--
-- Name: COLUMN inquiry.delete_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.inquiry.delete_date IS 'Timestamp of soft delete';


--
-- Name: inquiry_sample; Type: TABLE; Schema: report; Owner: fundermaps
--

CREATE TABLE report.inquiry_sample (
    id integer NOT NULL,
    inquiry integer NOT NULL,
    address geocoder.geocoder_id NOT NULL,
    create_date timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    update_date timestamp with time zone,
    delete_date timestamp with time zone,
    note text,
    built_year report.year,
    substructure report.substructure,
    overall_quality report.foundation_quality,
    wood_quality report.wood_quality,
    construction_quality report.quality,
    wood_capacity_horizontal_quality report.quality,
    pile_wood_capacity_vertical_quality report.quality,
    carrying_capacity_quality report.quality,
    mason_quality report.quality,
    wood_quality_necessity boolean,
    construction_level report.height,
    wood_level report.height,
    pile_diameter_top report.diameter,
    pile_diameter_bottom report.diameter,
    pile_head_level report.height,
    pile_tip_level report.height,
    foundation_depth report.height,
    mason_level report.height,
    concrete_charger_length report.length,
    pile_distance_length report.length,
    wood_penetration_depth report.length,
    cpt text,
    monitoring_well text,
    groundwater_level_temp report.height,
    groundlevel report.height,
    groundwater_level_net report.height,
    foundation_type report.foundation_type,
    enforcement_term report.enforcement_term,
    recovery_advised boolean,
    damage_cause report.foundation_damage_cause,
    damage_characteristics report.foundation_damage_characteristics,
    construction_pile report.construction_pile,
    wood_type report.wood_type,
    wood_encroachement report.wood_encroachement,
    crack_indoor_restored boolean,
    crack_indoor_type report.crack_type,
    crack_indoor_size report.size,
    crack_facade_front_restored boolean,
    crack_facade_front_type report.crack_type,
    crack_facade_front_size report.size,
    crack_facade_back_restored boolean,
    crack_facade_back_type report.crack_type,
    crack_facade_back_size report.size,
    crack_facade_left_restored boolean,
    crack_facade_left_type report.crack_type,
    crack_facade_left_size report.size,
    crack_facade_right_restored boolean,
    crack_facade_right_type report.crack_type,
    crack_facade_right_size report.size,
    deformed_facade boolean,
    threshold_updown_skewed boolean,
    threshold_front_level report.height,
    threshold_back_level report.height,
    skewed_parallel report.length,
    skewed_perpendicular report.length,
    skewed_facade report.rotation_type,
    settlement_speed double precision,
    skewed_window_frame boolean
);


ALTER TABLE report.inquiry_sample OWNER TO fundermaps;

--
-- Name: TABLE inquiry_sample; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TABLE report.inquiry_sample IS 'Contains sample data for inquiries.';


--
-- Name: COLUMN inquiry_sample.create_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.inquiry_sample.create_date IS 'Timestamp of record creation, set by insert';


--
-- Name: COLUMN inquiry_sample.update_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.inquiry_sample.update_date IS 'Timestamp of last record update, automatically updated on record modification';


--
-- Name: COLUMN inquiry_sample.delete_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.inquiry_sample.delete_date IS 'Timestamp of soft delete';


--
-- Name: recovery_sample; Type: TABLE; Schema: report; Owner: fundermaps
--

CREATE TABLE report.recovery_sample (
    id integer NOT NULL,
    recovery integer NOT NULL,
    address geocoder.geocoder_id NOT NULL,
    create_date timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    update_date timestamp with time zone,
    delete_date timestamp with time zone,
    note text,
    status report.recovery_status,
    type report.recovery_type DEFAULT 'unknown'::report.recovery_type NOT NULL,
    pile_type report.pile_type,
    contractor application.organization_id,
    facade report.facade[],
    permit text,
    permit_date date,
    recovery_date date
);


ALTER TABLE report.recovery_sample OWNER TO fundermaps;

--
-- Name: TABLE recovery_sample; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TABLE report.recovery_sample IS 'Contains sample data for recovery operations.';


--
-- Name: COLUMN recovery_sample.create_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.recovery_sample.create_date IS 'Timestamp of record creation, set by insert';


--
-- Name: COLUMN recovery_sample.update_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.recovery_sample.update_date IS 'Timestamp of last record update, automatically updated on record modification';


--
-- Name: COLUMN recovery_sample.delete_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.recovery_sample.delete_date IS 'Timestamp of soft delete';


--
-- Name: analysis_address; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.analysis_address AS
 SELECT b.id,
    b.external_id,
    b.external_source,
    COALESCE((inqs.built_year)::date, (b.built_year)::date) AS construction_year,
        CASE
            WHEN (inqs.built_year IS NOT NULL) THEN 'fundermaps'::report.built_year_source
            ELSE 'bag'::report.built_year_source
        END AS construction_year_source,
    b.geom,
    a.id AS address_id,
    a.external_id AS address_external_id,
    a.postal_code,
    afr.neighborhood_id,
    afr.groundwater_level,
    afr.code AS soil,
    be.roof AS building_height,
    be.ground AS ground_level,
    inqs.cpt,
    inqs.monitoring_well,
    inqs.recovery_advised,
    inqs.damage_cause,
    inqs.substructure,
    i.document_name,
    i.document_date,
    i.type AS inquiry_type,
    rs.type AS recovery_type,
    rs.status AS recovery_status,
    round((public.st_area((b.geom)::public.geography, true))::numeric, 2) AS surface_area,
    round((public.st_area((b.geom)::public.geography, true))::numeric, 2) AS living_area,
    NULL::real AS foundation_bearing_layer,
        CASE
            WHEN (data.get_foundation_category(afi.foundation_type, inqs.foundation_type) = 'wood'::data.foundation_category) THEN (round(((public.st_area((b.geom)::public.geography, true) * (950)::double precision))::numeric, '-2'::integer))::double precision
            WHEN (data.get_foundation_category(afi.foundation_type, inqs.foundation_type) = 'no_pile'::data.foundation_category) THEN (round(((public.st_area((b.geom)::public.geography, true) * (350)::double precision))::numeric, '-2'::integer))::double precision
            ELSE NULL::double precision
        END AS restoration_costs,
        CASE
            WHEN ((inqs.id IS NOT NULL) AND (inqs.foundation_type IS NOT NULL)) THEN inqs.foundation_type
            ELSE afi.foundation_type
        END AS foundation_type,
        CASE
            WHEN (inqs.id IS NOT NULL) THEN 'established'::data.reliability
            ELSE 'indicative'::data.reliability
        END AS foundation_type_reliability,
        CASE
            WHEN (data.get_foundation_category(afi.foundation_type, inqs.foundation_type) = 'wood'::data.foundation_category) THEN
            CASE
                WHEN ((inqs.id IS NOT NULL) AND (inqs.wood_level IS NOT NULL)) THEN ((inqs.wood_level)::double precision - afr.groundwater_level)
                ELSE (afr.groundwater_level - (1.5)::double precision)
            END
            WHEN (data.get_foundation_category(afi.foundation_type, inqs.foundation_type) = 'other'::data.foundation_category) THEN NULL::double precision
            ELSE NULL::double precision
        END AS drystand,
    'indicative'::data.reliability AS drystand_reliability,
    afr.foundation_risk AS drystand_risk,
        CASE
            WHEN (data.get_foundation_category(afi.foundation_type, inqs.foundation_type) = 'no_pile'::data.foundation_category) THEN
            CASE
                WHEN ((inqs.id IS NOT NULL) AND (inqs.foundation_depth IS NOT NULL)) THEN ((inqs.foundation_depth)::double precision - afr.groundwater_level)
                ELSE (afr.groundwater_level - (0.6)::double precision)
            END
            ELSE NULL::double precision
        END AS dewatering_depth,
    'indicative'::data.reliability AS dewatering_depth_reliability,
    afr.foundation_risk AS dewatering_depth_risk,
    NULL::text AS bio_infection,
    'indicative'::data.reliability AS bio_infection_reliability,
    afr.foundation_risk AS bio_infection_risk
   FROM (((((((geocoder.address a
     JOIN geocoder.building_active b ON (((a.building_id)::text = (b.id)::text)))
     JOIN data.analysis_foundation_indicative afi ON (((afi.id)::text = (b.id)::text)))
     JOIN data.analysis_foundation_risk afr ON (((afr.id)::text = (b.id)::text)))
     LEFT JOIN data.building_elevation be ON (((be.building_id)::text = (b.id)::text)))
     LEFT JOIN report.inquiry_sample inqs ON (((inqs.address)::text = (a.id)::text)))
     LEFT JOIN report.inquiry i ON ((i.id = inqs.inquiry)))
     LEFT JOIN report.recovery_sample rs ON (((rs.address)::text = (a.id)::text)));


ALTER TABLE data.analysis_address OWNER TO fundermaps;

--
-- Name: building_cluster; Type: TABLE; Schema: data; Owner: fundermaps
--

CREATE TABLE data.building_cluster (
    building_id geocoder.geocoder_id NOT NULL,
    cluster_id uuid NOT NULL
);


ALTER TABLE data.building_cluster OWNER TO fundermaps;

--
-- Name: building_ownership; Type: TABLE; Schema: data; Owner: fundermaps
--

CREATE TABLE data.building_ownership (
    building_id geocoder.geocoder_id NOT NULL,
    owner text NOT NULL
);


ALTER TABLE data.building_ownership OWNER TO fundermaps;

--
-- Name: building_pleistocene; Type: TABLE; Schema: data; Owner: fundermaps
--

CREATE TABLE data.building_pleistocene (
    building_id geocoder.geocoder_id NOT NULL,
    depth double precision
);


ALTER TABLE data.building_pleistocene OWNER TO fundermaps;

--
-- Name: address_building; Type: VIEW; Schema: geocoder; Owner: fundermaps
--

CREATE VIEW geocoder.address_building AS
 SELECT addr.id AS address_id,
    ba.id AS building_id,
    ba.geom
   FROM (geocoder.address addr
     JOIN geocoder.building_active ba ON (((addr.building_id)::text = (ba.id)::text)));


ALTER TABLE geocoder.address_building OWNER TO fundermaps;

--
-- Name: analysis_complete; Type: MATERIALIZED VIEW; Schema: data; Owner: fundermaps
--

CREATE MATERIALIZED VIEW data.analysis_complete AS
 SELECT b.id AS building_id,
    b.external_id AS external_building_id,
    a.id AS address_id,
    a.external_id AS address_external_id,
    b.neighborhood_id,
    (COALESCE(established.built_year, cluster.built_year, (construction_year.construction_year)::double precision))::integer AS construction_year,
        CASE
            WHEN (established.foundation_type IS NOT NULL) THEN established.foundation_type
            WHEN (cluster.foundation_type IS NOT NULL) THEN cluster.foundation_type
            ELSE indicative_foundation_type.indicative_foundation_type
        END AS foundation_type,
        CASE
            WHEN (established.foundation_type IS NOT NULL) THEN 'established'::data.reliability
            ELSE 'indicative'::data.reliability
        END AS foundation_type_reliability,
        CASE
            WHEN (established.foundation_type IS NOT NULL) THEN data.get_restoration_cost(established.foundation_type, surface_area.surface_area)
            WHEN (cluster.foundation_type IS NOT NULL) THEN data.get_restoration_cost(cluster.foundation_type, surface_area.surface_area)
            ELSE data.get_restoration_cost(indicative_foundation_type.indicative_foundation_type, surface_area.surface_area)
        END AS restoration_costs,
        CASE
            WHEN (established_drystand_risk.established_drystand_risk IS NOT NULL) THEN established_drystand_risk.established_drystand_risk
            WHEN (cluster_drystand_risk.cluster_drystand_risk IS NOT NULL) THEN cluster_drystand_risk.cluster_drystand_risk
            ELSE drystand_risk.drystand_risk
        END AS drystand_risk,
        CASE
            WHEN (established_drystand_risk.established_drystand_risk IS NOT NULL) THEN 'established'::data.reliability
            ELSE 'indicative'::data.reliability
        END AS drystand_risk_reliability,
        CASE
            WHEN (established_bio_infection_risk.established_bio_infection_risk IS NOT NULL) THEN established_bio_infection_risk.established_bio_infection_risk
            WHEN (cluster_bio_infection_risk.cluster_bio_infection_risk IS NOT NULL) THEN cluster_bio_infection_risk.cluster_bio_infection_risk
            ELSE bio_infection_risk.bio_infection_risk
        END AS bio_infection_risk,
        CASE
            WHEN (established_bio_infection_risk.established_bio_infection_risk IS NOT NULL) THEN 'established'::data.reliability
            ELSE 'indicative'::data.reliability
        END AS bio_infection_risk_reliability,
        CASE
            WHEN (established_dewatering_depth_risk.established_dewatering_depth_risk IS NOT NULL) THEN established_dewatering_depth_risk.established_dewatering_depth_risk
            WHEN (cluster_dewatering_depth_risk.cluster_dewatering_depth_risk IS NOT NULL) THEN cluster_dewatering_depth_risk.cluster_dewatering_depth_risk
            ELSE dewatering_depth_risk.dewatering_depth_risk
        END AS dewatering_depth_risk,
        CASE
            WHEN (established_dewatering_depth_risk.established_dewatering_depth_risk IS NOT NULL) THEN 'established'::data.reliability
            ELSE 'indicative'::data.reliability
        END AS dewatering_depth_risk_reliability,
        CASE
            WHEN (established_unclassified_risk.established_unclassified_risk IS NOT NULL) THEN established_unclassified_risk.established_unclassified_risk
            WHEN (cluster_unclassified_risk.cluster_unclassified_risk IS NOT NULL) THEN cluster_unclassified_risk.cluster_unclassified_risk
            ELSE NULL::data.foundation_risk_indication
        END AS unclassified_risk,
    round((bh.height)::numeric, 2) AS height,
    round((s.velocity)::numeric, 2) AS velocity,
    bo.owner,
    established.id AS inquiry_id,
    established.inquiry_type,
    established.damage_cause,
    date_part('years'::text, age((
        CASE established.enforcement_term
            WHEN 'term05'::report.enforcement_term THEN (established.document_date + '5 years'::interval)
            WHEN 'term510'::report.enforcement_term THEN (established.document_date + '10 years'::interval)
            WHEN 'term1020'::report.enforcement_term THEN (established.document_date + '20 years'::interval)
            WHEN 'term5'::report.enforcement_term THEN (established.document_date + '5 years'::interval)
            WHEN 'term10'::report.enforcement_term THEN (established.document_date + '10 years'::interval)
            WHEN 'term15'::report.enforcement_term THEN (established.document_date + '15 years'::interval)
            WHEN 'term20'::report.enforcement_term THEN (established.document_date + '20 years'::interval)
            WHEN 'term25'::report.enforcement_term THEN (established.document_date + '25 years'::interval)
            WHEN 'term30'::report.enforcement_term THEN (established.document_date + '30 years'::interval)
            WHEN 'term40'::report.enforcement_term THEN (established.document_date + '40 years'::interval)
            ELSE NULL::timestamp without time zone
        END)::timestamp with time zone, CURRENT_TIMESTAMP)) AS enforcement_term,
    established.overall_quality,
    recovery.type AS recovery_type,
    b.geom
   FROM (((((((((geocoder.building_active b
     LEFT JOIN data.building_elevation be ON (((be.building_id)::text = (b.id)::text)))
     LEFT JOIN geocoder.address a ON (((a.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_height bh ON (((bh.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_geographic_region gr ON (((gr.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_groundwater_level gwl ON (((gwl.building_id)::text = (b.id)::text)))
     LEFT JOIN data.subsidence s ON (((s.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_ownership bo ON (((bo.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_pleistocene bp ON (((bp.building_id)::text = (b.id)::text)))
     LEFT JOIN data.building_cluster bc ON (((bc.building_id)::text = (b.id)::text))),
    LATERAL ( SELECT recovery_sample.type
           FROM report.recovery_sample
          WHERE ((recovery_sample.address)::text = (a.id)::text)
        UNION ALL
         SELECT NULL::report.recovery_type AS recovery_type
 LIMIT 1) recovery,
    LATERAL ( SELECT (array_agg(established_rows.foundation_type) FILTER (WHERE (established_rows.foundation_type IS NOT NULL)))[1] AS foundation_type,
            (array_agg(established_rows.enforcement_term) FILTER (WHERE (established_rows.enforcement_term IS NOT NULL)))[1] AS enforcement_term,
            (array_agg(established_rows.damage_cause) FILTER (WHERE (established_rows.damage_cause IS NOT NULL)))[1] AS damage_cause,
            (array_agg(established_rows.overall_quality) FILTER (WHERE (established_rows.overall_quality IS NOT NULL)))[1] AS overall_quality,
            (array_agg(established_rows.recovery_advised) FILTER (WHERE (established_rows.recovery_advised IS NOT NULL)))[1] AS recovery_advised,
            (array_agg(date_part('year'::text, established_rows.built_year)) FILTER (WHERE (established_rows.recovery_advised IS NOT NULL)))[1] AS built_year,
            (array_agg(established_rows.type) FILTER (WHERE (established_rows.type IS NOT NULL)))[1] AS inquiry_type,
            (array_agg(established_rows.document_date) FILTER (WHERE (established_rows.document_date IS NOT NULL)))[1] AS document_date,
            (array_agg(established_rows.id) FILTER (WHERE (established_rows.document_date IS NOT NULL)))[1] AS id,
            (array_agg(established_rows.recovery) FILTER (WHERE (established_rows.recovery IS NOT NULL)))[1] AS recovery
           FROM (( SELECT 1 AS group_id,
                    is2.foundation_type,
                    is2.enforcement_term,
                    is2.damage_cause,
                    is2.overall_quality,
                    is2.recovery_advised,
                    is2.built_year,
                    i.type,
                    i.document_date,
                    i.id,
                    ( SELECT (EXISTS ( SELECT 1
                                   FROM report.recovery_sample
                                  WHERE ((recovery_sample.address)::text = (ab.address_id)::text)
                                 LIMIT 1)) AS "exists") AS recovery
                   FROM (((report.inquiry_sample is2
                     JOIN report.inquiry i ON ((is2.inquiry = i.id)))
                     JOIN geocoder.address_building ab ON (((ab.address_id)::text = (is2.address)::text)))
                     JOIN geocoder.building_active ba ON (((ba.id)::text = (ab.building_id)::text)))
                  WHERE (((is2.address)::text = (a.id)::text) AND (i.document_date >= (ba.built_year)::date))
                  ORDER BY
                        CASE
                            WHEN (i.type = 'foundation_research'::report.inquiry_type) THEN 0
                            WHEN (i.type = 'inspectionpit'::report.inquiry_type) THEN 1
                            WHEN (i.type = 'second_opinion'::report.inquiry_type) THEN 2
                            WHEN (i.type = 'note'::report.inquiry_type) THEN 3
                            WHEN (i.type = 'additional_research'::report.inquiry_type) THEN 4
                            WHEN (i.type = 'demolition_research'::report.inquiry_type) THEN 5
                            WHEN (i.type = 'architectural_research'::report.inquiry_type) THEN 6
                            WHEN (i.type = 'archieve_research'::report.inquiry_type) THEN 7
                            WHEN (i.type = 'quickscan'::report.inquiry_type) THEN 8
                            ELSE 100
                        END, i.document_date DESC)
                UNION ALL
                 SELECT 1,
                    NULL::report.foundation_type AS foundation_type,
                    NULL::report.enforcement_term AS enforcement_term,
                    NULL::report.foundation_damage_cause AS foundation_damage_cause,
                    NULL::report.foundation_quality AS foundation_quality,
                    false AS bool,
                    NULL::date AS date,
                    NULL::report.inquiry_type AS inquiry_type,
                    NULL::date AS date,
                    NULL::integer AS int4,
                    false AS bool) established_rows
          GROUP BY established_rows.group_id) established,
    LATERAL ( SELECT (array_agg(cluster_rows.foundation_type) FILTER (WHERE (cluster_rows.foundation_type IS NOT NULL)))[1] AS foundation_type,
            (array_agg(cluster_rows.enforcement_term) FILTER (WHERE (cluster_rows.enforcement_term IS NOT NULL)))[1] AS enforcement_term,
            (array_agg(cluster_rows.damage_cause) FILTER (WHERE (cluster_rows.damage_cause IS NOT NULL)))[1] AS damage_cause,
            (array_agg(cluster_rows.overall_quality) FILTER (WHERE (cluster_rows.overall_quality IS NOT NULL)))[1] AS overall_quality,
            (array_agg(cluster_rows.recovery_advised) FILTER (WHERE (cluster_rows.recovery_advised IS NOT NULL)))[1] AS recovery_advised,
            (array_agg(date_part('year'::text, cluster_rows.built_year)) FILTER (WHERE (cluster_rows.recovery_advised IS NOT NULL)))[1] AS built_year,
            (array_agg(cluster_rows.recovery) FILTER (WHERE (cluster_rows.recovery IS NOT NULL)))[1] AS recovery
           FROM (( SELECT 1 AS group_id,
                    is2.foundation_type,
                    is2.enforcement_term,
                    is2.damage_cause,
                    is2.overall_quality,
                    is2.recovery_advised,
                    is2.built_year,
                    ( SELECT (EXISTS ( SELECT 1
                                   FROM report.recovery_sample
                                  WHERE ((recovery_sample.address)::text = (ab.address_id)::text)
                                 LIMIT 1)) AS "exists") AS recovery
                   FROM ((((report.inquiry_sample is2
                     JOIN report.inquiry i ON ((is2.inquiry = i.id)))
                     JOIN geocoder.address_building ab ON (((ab.address_id)::text = (is2.address)::text)))
                     JOIN geocoder.building_active ba ON ((((ba.id)::text = (ab.building_id)::text) AND ((ba.id)::text <> (b.id)::text))))
                     JOIN data.building_cluster c ON (((c.building_id)::text = (ab.building_id)::text)))
                  WHERE ((c.cluster_id = bc.cluster_id) AND (i.document_date >= (ba.built_year)::date))
                  ORDER BY
                        CASE
                            WHEN (i.type = 'foundation_research'::report.inquiry_type) THEN 0
                            WHEN (i.type = 'inspectionpit'::report.inquiry_type) THEN 1
                            WHEN (i.type = 'second_opinion'::report.inquiry_type) THEN 2
                            WHEN (i.type = 'note'::report.inquiry_type) THEN 3
                            WHEN (i.type = 'additional_research'::report.inquiry_type) THEN 4
                            WHEN (i.type = 'demolition_research'::report.inquiry_type) THEN 5
                            WHEN (i.type = 'architectural_research'::report.inquiry_type) THEN 6
                            WHEN (i.type = 'archieve_research'::report.inquiry_type) THEN 7
                            WHEN (i.type = 'quickscan'::report.inquiry_type) THEN 8
                            ELSE 100
                        END, i.document_date DESC)
                UNION ALL
                 SELECT 1,
                    NULL::report.foundation_type AS foundation_type,
                    NULL::report.enforcement_term AS enforcement_term,
                    NULL::report.foundation_damage_cause AS foundation_damage_cause,
                    NULL::report.foundation_quality AS foundation_quality,
                    false AS bool,
                    NULL::date AS date,
                    false AS bool) cluster_rows
          GROUP BY cluster_rows.group_id) cluster,
    LATERAL CAST((date_part('year'::text, (b.built_year)::date))::integer AS integer) construction_year(construction_year),
    LATERAL round((public.st_area((b.geom)::public.geography, true))::numeric, 2) surface_area(surface_area),
    LATERAL round(((be.ground - bp.depth))::numeric, 2) pile_length(pile_length),
    LATERAL ( SELECT (count(a_1.id))::integer AS count
           FROM geocoder.address a_1
          WHERE ((a_1.building_id)::text = (b.id)::text)) addresses(count),
    LATERAL data.get_indicative_foundation_type(construction_year.construction_year, (bh.height)::double precision, gr.code, addresses.count) indicative_foundation_type(indicative_foundation_type),
    LATERAL data.get_indicative_drystand_risk((recovery.type IS NOT NULL), ((established.foundation_type IS NULL) AND (cluster.foundation_type IS NULL)),
        CASE
            WHEN (established.foundation_type IS NOT NULL) THEN established.foundation_type
            ELSE cluster.foundation_type
        END, construction_year.construction_year, (bh.height)::double precision, gr.code, addresses.count, s.velocity, gwl.level) drystand_risk(drystand_risk),
    LATERAL data.get_indicative_dewatering_depth_risk((recovery.type IS NOT NULL), ((established.foundation_type IS NULL) AND (cluster.foundation_type IS NULL)),
        CASE
            WHEN (established.foundation_type IS NOT NULL) THEN established.foundation_type
            ELSE cluster.foundation_type
        END, construction_year.construction_year, (bh.height)::double precision, gr.code, addresses.count, s.velocity, gwl.level) dewatering_depth_risk(dewatering_depth_risk),
    LATERAL data.get_indicative_bio_infection_risk((recovery.type IS NOT NULL),
        CASE
            WHEN (established.foundation_type IS NOT NULL) THEN established.foundation_type
            WHEN (cluster.foundation_type IS NOT NULL) THEN cluster.foundation_type
            ELSE indicative_foundation_type.indicative_foundation_type
        END, (pile_length.pile_length)::double precision) bio_infection_risk(bio_infection_risk),
    LATERAL data.get_established_drystand_risk(cluster.recovery, cluster.damage_cause, cluster.enforcement_term, cluster.overall_quality, cluster.recovery_advised) cluster_drystand_risk(cluster_drystand_risk),
    LATERAL data.get_established_dewatering_depth_risk(cluster.recovery, cluster.damage_cause, cluster.enforcement_term, cluster.overall_quality, cluster.recovery_advised) cluster_dewatering_depth_risk(cluster_dewatering_depth_risk),
    LATERAL data.get_established_bio_infection_risk(cluster.recovery, cluster.damage_cause, cluster.enforcement_term, cluster.overall_quality, cluster.recovery_advised) cluster_bio_infection_risk(cluster_bio_infection_risk),
    LATERAL data.get_established_unclassified_risk(cluster.recovery, cluster.damage_cause, cluster.enforcement_term, cluster.overall_quality, cluster.recovery_advised) cluster_unclassified_risk(cluster_unclassified_risk),
    LATERAL data.get_established_drystand_risk(established.recovery, established.damage_cause, established.enforcement_term, established.overall_quality, established.recovery_advised) established_drystand_risk(established_drystand_risk),
    LATERAL data.get_established_dewatering_depth_risk(established.recovery, established.damage_cause, established.enforcement_term, established.overall_quality, established.recovery_advised) established_dewatering_depth_risk(established_dewatering_depth_risk),
    LATERAL data.get_established_bio_infection_risk(established.recovery, established.damage_cause, established.enforcement_term, established.overall_quality, established.recovery_advised) established_bio_infection_risk(established_bio_infection_risk),
    LATERAL data.get_established_unclassified_risk(established.recovery, established.damage_cause, established.enforcement_term, established.overall_quality, established.recovery_advised) established_unclassified_risk(established_unclassified_risk)
  WHERE ((addresses.count > 0) AND (b.building_type = 'house'::geocoder.building_type))
  WITH NO DATA;


ALTER TABLE data.analysis_complete OWNER TO fundermaps;

--
-- Name: statistics_product_buildings_restored; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.statistics_product_buildings_restored AS
 SELECT DISTINCT ON (x.neighborhood_id) x.neighborhood_id,
    x.count
   FROM ( SELECT ba.neighborhood_id,
            count(*) AS count
           FROM ((report.recovery_sample rs
             JOIN geocoder.address a ON (((rs.address)::text = (a.id)::text)))
             JOIN geocoder.building_active ba ON (((a.building_id)::text = (ba.id)::text)))
          GROUP BY ba.neighborhood_id
        UNION
         SELECT ba.neighborhood_id,
            0 AS count
           FROM geocoder.building_active ba) x;


ALTER TABLE data.statistics_product_buildings_restored OWNER TO fundermaps;

--
-- Name: VIEW statistics_product_buildings_restored; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON VIEW data.statistics_product_buildings_restored IS 'Contains statistics about how many building have been restored. This can be filtered on neighborhood_id.';


--
-- Name: statistics_product_construction_years; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.statistics_product_construction_years AS
 SELECT ba.neighborhood_id,
    (date_part('year'::text, decade.decade))::integer AS year_from,
    count(decade.decade) AS count
   FROM geocoder.building_active ba,
    LATERAL date_trunc('decade'::text, (ba.built_year)::timestamp with time zone) decade(decade)
  WHERE (ba.built_year IS NOT NULL)
  GROUP BY ba.neighborhood_id, decade.decade;


ALTER TABLE data.statistics_product_construction_years OWNER TO fundermaps;

--
-- Name: VIEW statistics_product_construction_years; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON VIEW data.statistics_product_construction_years IS 'Contains statistics on the distribution of construction years per decade per neighborhood. This can be filtered on neighborhood_id.';


--
-- Name: statistics_product_data_collected; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.statistics_product_data_collected AS
 SELECT ba.neighborhood_id,
    (count(a.id) FILTER (WHERE (i.id IS NOT NULL)) / count(a.id)) AS percentage
   FROM ((geocoder.address a
     LEFT JOIN report.inquiry_sample i ON (((i.address)::text = (a.id)::text)))
     JOIN geocoder.building_active ba ON (((a.building_id)::text = (ba.id)::text)))
  GROUP BY ba.neighborhood_id;


ALTER TABLE data.statistics_product_data_collected OWNER TO fundermaps;

--
-- Name: VIEW statistics_product_data_collected; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON VIEW data.statistics_product_data_collected IS 'Contains statistics on how much data has been collected for a given neighborhood. This can be filtered on neighborhood_id.';


--
-- Name: statistics_product_foundation_risk; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.statistics_product_foundation_risk AS
 SELECT afr.neighborhood_id,
    afr.foundation_risk,
    (((count(afr.foundation_risk))::numeric / sum(count(afr.foundation_risk)) OVER (PARTITION BY afr.neighborhood_id)) * (100)::numeric) AS percentage
   FROM data.analysis_foundation_risk afr
  GROUP BY afr.neighborhood_id, afr.foundation_risk;


ALTER TABLE data.statistics_product_foundation_risk OWNER TO fundermaps;

--
-- Name: statistics_product_foundation_type; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.statistics_product_foundation_type AS
 SELECT afi.neighborhood_id,
    afi.foundation_type,
    (((count(afi.foundation_type))::numeric / sum(count(afi.foundation_type)) OVER (PARTITION BY afi.neighborhood_id)) * (100)::numeric) AS percentage
   FROM data.analysis_foundation_indicative afi
  GROUP BY afi.neighborhood_id, afi.foundation_type;


ALTER TABLE data.statistics_product_foundation_type OWNER TO fundermaps;

--
-- Name: incident; Type: TABLE; Schema: report; Owner: fundermaps
--

CREATE TABLE report.incident (
    id text NOT NULL,
    foundation_type report.foundation_type,
    chained_building boolean NOT NULL,
    owner boolean NOT NULL,
    foundation_recovery boolean NOT NULL,
    neightbor_recovery boolean NOT NULL,
    foundation_damage_cause report.foundation_damage_cause,
    document_file text[],
    note text,
    contact text,
    create_date timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    update_date timestamp with time zone,
    delete_date timestamp with time zone,
    foundation_damage_characteristics report.foundation_damage_characteristics[],
    environment_damage_characteristics report.environment_damage_characteristics[],
    address geocoder.geocoder_id NOT NULL,
    meta jsonb,
    audit_status report.audit_status DEFAULT 'todo'::report.audit_status NOT NULL,
    internal_note text,
    question_type report.incident_question_type DEFAULT 'other'::report.incident_question_type NOT NULL
);


ALTER TABLE report.incident OWNER TO fundermaps;

--
-- Name: TABLE incident; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TABLE report.incident IS 'Contains reported incidents.';


--
-- Name: COLUMN incident.contact; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.incident.contact IS 'Link to the contact';


--
-- Name: COLUMN incident.create_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.incident.create_date IS 'Timestamp of record creation, set by insert';


--
-- Name: COLUMN incident.update_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.incident.update_date IS 'Timestamp of last record update, automatically updated on record modification';


--
-- Name: COLUMN incident.delete_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.incident.delete_date IS 'Timestamp of soft delete';


--
-- Name: statistics_product_incidents; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.statistics_product_incidents AS
 SELECT ba.neighborhood_id,
    year.year,
    count(i.id) AS count
   FROM ((report.incident i
     JOIN geocoder.address a ON (((i.address)::text = (a.id)::text)))
     JOIN geocoder.building_active ba ON (((a.building_id)::text = (ba.id)::text))),
    LATERAL CAST((date_part('year'::text, i.create_date))::integer AS integer) year(year)
  GROUP BY ba.neighborhood_id, year.year;


ALTER TABLE data.statistics_product_incidents OWNER TO fundermaps;

--
-- Name: VIEW statistics_product_incidents; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON VIEW data.statistics_product_incidents IS 'Contains statistics on the amount of incidents for a given district. This can be filtered on neighborhood_id.';


--
-- Name: statistics_product_inquiries; Type: VIEW; Schema: data; Owner: fundermaps
--

CREATE VIEW data.statistics_product_inquiries AS
 SELECT ba.neighborhood_id,
    year.year,
    count(i.id) AS count
   FROM ((report.inquiry_sample i
     JOIN geocoder.address a ON (((i.address)::text = (a.id)::text)))
     JOIN geocoder.building_active ba ON (((a.building_id)::text = (ba.id)::text))),
    LATERAL CAST((date_part('year'::text, i.create_date))::integer AS integer) year(year)
  GROUP BY ba.neighborhood_id, year.year;


ALTER TABLE data.statistics_product_inquiries OWNER TO fundermaps;

--
-- Name: VIEW statistics_product_inquiries; Type: COMMENT; Schema: data; Owner: fundermaps
--

COMMENT ON VIEW data.statistics_product_inquiries IS 'Contains statistics on the amount of inquries for a given neighborhood. This can be filtered on neighborhood_id.';


--
-- Name: country; Type: TABLE; Schema: geocoder; Owner: fundermaps
--

CREATE TABLE geocoder.country (
    id geocoder.geocoder_id NOT NULL,
    external_id text NOT NULL,
    external_source geocoder.data_source NOT NULL,
    name text NOT NULL,
    geom public.geometry(MultiPolygon,4326) NOT NULL
);


ALTER TABLE geocoder.country OWNER TO fundermaps;

--
-- Name: TABLE country; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TABLE geocoder.country IS 'Contains all countries in our own format.';


--
-- Name: district; Type: TABLE; Schema: geocoder; Owner: fundermaps
--

CREATE TABLE geocoder.district (
    id geocoder.geocoder_id NOT NULL,
    external_id text NOT NULL,
    external_source geocoder.data_source NOT NULL,
    municipality_id geocoder.geocoder_id NOT NULL,
    name text NOT NULL,
    water boolean NOT NULL,
    geom public.geometry(MultiPolygon,4326) NOT NULL
);


ALTER TABLE geocoder.district OWNER TO fundermaps;

--
-- Name: TABLE district; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TABLE geocoder.district IS 'Contains all districts in our own format.';


--
-- Name: municipality; Type: TABLE; Schema: geocoder; Owner: fundermaps
--

CREATE TABLE geocoder.municipality (
    id geocoder.geocoder_id NOT NULL,
    external_id text NOT NULL,
    external_source geocoder.data_source NOT NULL,
    name text NOT NULL,
    water boolean NOT NULL,
    geom public.geometry(MultiPolygon,4326) NOT NULL,
    state_id geocoder.geocoder_id NOT NULL
);


ALTER TABLE geocoder.municipality OWNER TO fundermaps;

--
-- Name: TABLE municipality; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TABLE geocoder.municipality IS 'Contains all municipalities in our own format.';


--
-- Name: neighborhood; Type: TABLE; Schema: geocoder; Owner: fundermaps
--

CREATE TABLE geocoder.neighborhood (
    id geocoder.geocoder_id NOT NULL,
    external_id text NOT NULL,
    external_source geocoder.data_source NOT NULL,
    district_id geocoder.geocoder_id NOT NULL,
    name text NOT NULL,
    water boolean NOT NULL,
    geom public.geometry(MultiPolygon,4326) NOT NULL
);


ALTER TABLE geocoder.neighborhood OWNER TO fundermaps;

--
-- Name: TABLE neighborhood; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TABLE geocoder.neighborhood IS 'Contains all neighborhoods in our own format.';


--
-- Name: state; Type: TABLE; Schema: geocoder; Owner: fundermaps
--

CREATE TABLE geocoder.state (
    id geocoder.geocoder_id NOT NULL,
    external_id text NOT NULL,
    external_source geocoder.data_source NOT NULL,
    country_id geocoder.geocoder_id NOT NULL,
    name text NOT NULL,
    water boolean NOT NULL,
    geom public.geometry(MultiPolygon,4326) NOT NULL
);


ALTER TABLE geocoder.state OWNER TO fundermaps;

--
-- Name: TABLE state; Type: COMMENT; Schema: geocoder; Owner: fundermaps
--

COMMENT ON TABLE geocoder.state IS 'Contains all states in our own format.';


--
-- Name: analysis_building; Type: VIEW; Schema: maplayer; Owner: fundermaps
--

CREATE VIEW maplayer.analysis_building AS
 SELECT DISTINCT ON (ac.building_id) ac.construction_year,
    ac.restoration_costs,
    ac.height,
    ac.velocity,
    ac.owner,
    ac.geom
   FROM data.analysis_complete ac;


ALTER TABLE maplayer.analysis_building OWNER TO fundermaps;

--
-- Name: analysis_foundation; Type: VIEW; Schema: maplayer; Owner: fundermaps
--

CREATE VIEW maplayer.analysis_foundation AS
 SELECT DISTINCT ON (ac.building_id) ac.foundation_type,
    ac.foundation_type_reliability,
    ac.height,
    ac.recovery_type,
    ac.geom
   FROM data.analysis_complete ac;


ALTER TABLE maplayer.analysis_foundation OWNER TO fundermaps;

--
-- Name: analysis_quality; Type: VIEW; Schema: maplayer; Owner: fundermaps
--

CREATE VIEW maplayer.analysis_quality AS
 SELECT DISTINCT ON (ac.building_id) ac.drystand_risk,
    ac.drystand_risk_reliability,
    ac.bio_infection_risk,
    ac.bio_infection_risk_reliability,
    ac.dewatering_depth_risk,
    ac.dewatering_depth_risk_reliability,
    ac.unclassified_risk,
    ac.height,
    ac.geom
   FROM data.analysis_complete ac;


ALTER TABLE maplayer.analysis_quality OWNER TO fundermaps;

--
-- Name: analysis_report; Type: VIEW; Schema: maplayer; Owner: fundermaps
--

CREATE VIEW maplayer.analysis_report AS
 SELECT DISTINCT ON (ac.building_id) ac.height,
    ac.inquiry_id,
    ac.inquiry_type,
    ac.damage_cause,
    ac.enforcement_term,
    ac.overall_quality,
    ac.geom
   FROM data.analysis_complete ac;


ALTER TABLE maplayer.analysis_report OWNER TO fundermaps;

--
-- Name: incident; Type: VIEW; Schema: maplayer; Owner: fundermaps
--

CREATE VIEW maplayer.incident AS
 WITH incident_rank AS (
         SELECT i.id,
            ab.geom,
            row_number() OVER (PARTITION BY ab.building_id ORDER BY COALESCE(i.update_date, i.create_date) DESC) AS rank
           FROM (report.incident i
             JOIN geocoder.address_building ab ON (((i.address)::text = (ab.address_id)::text)))
        )
 SELECT ik.id,
    ik.geom
   FROM incident_rank ik
  WHERE (ik.rank = 1);


ALTER TABLE maplayer.incident OWNER TO fundermaps;

--
-- Name: incident_aggregate; Type: VIEW; Schema: maplayer; Owner: fundermaps
--

CREATE VIEW maplayer.incident_aggregate AS
 SELECT COALESCE(mi.total, (0)::bigint) AS incidents,
    m.geom
   FROM (geocoder.municipality m
     LEFT JOIN ( SELECT m_1.id,
            count(m_1.id) AS total
           FROM (((((report.incident i
             JOIN geocoder.address a ON (((a.id)::text = (i.address)::text)))
             JOIN geocoder.building_active ba ON (((ba.id)::text = (a.building_id)::text)))
             JOIN geocoder.neighborhood n ON (((n.id)::text = (ba.neighborhood_id)::text)))
             JOIN geocoder.district d ON (((d.id)::text = (n.district_id)::text)))
             JOIN geocoder.municipality m_1 ON (((m_1.id)::text = (d.municipality_id)::text)))
          GROUP BY m_1.id) mi ON (((mi.id)::text = (m.id)::text)));


ALTER TABLE maplayer.incident_aggregate OWNER TO fundermaps;

--
-- Name: incident_aggregate_category; Type: VIEW; Schema: maplayer; Owner: fundermaps
--

CREATE VIEW maplayer.incident_aggregate_category AS
 SELECT
        CASE
            WHEN (mi.total IS NULL) THEN 0
            WHEN (mi.total < 3) THEN 1
            ELSE 2
        END AS category,
    m.geom
   FROM (geocoder.municipality m
     LEFT JOIN ( SELECT m_1.id,
            count(m_1.id) AS total
           FROM (((((report.incident i
             JOIN geocoder.address a ON (((a.id)::text = (i.address)::text)))
             JOIN geocoder.building_active ba ON (((ba.id)::text = (a.building_id)::text)))
             JOIN geocoder.neighborhood n ON (((n.id)::text = (ba.neighborhood_id)::text)))
             JOIN geocoder.district d ON (((d.id)::text = (n.district_id)::text)))
             JOIN geocoder.municipality m_1 ON (((m_1.id)::text = (d.municipality_id)::text)))
          GROUP BY m_1.id) mi ON (((mi.id)::text = (m.id)::text)));


ALTER TABLE maplayer.incident_aggregate_category OWNER TO fundermaps;

--
-- Name: incident_id_seq; Type: SEQUENCE; Schema: report; Owner: fundermaps
--

CREATE SEQUENCE report.incident_id_seq
    START WITH 12500
    INCREMENT BY 2
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE report.incident_id_seq OWNER TO fundermaps;

--
-- Name: inquiry_id_seq; Type: SEQUENCE; Schema: report; Owner: fundermaps
--

CREATE SEQUENCE report.inquiry_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE report.inquiry_id_seq OWNER TO fundermaps;

--
-- Name: inquiry_id_seq; Type: SEQUENCE OWNED BY; Schema: report; Owner: fundermaps
--

ALTER SEQUENCE report.inquiry_id_seq OWNED BY report.inquiry.id;


--
-- Name: inquiry_sample_id_seq; Type: SEQUENCE; Schema: report; Owner: fundermaps
--

CREATE SEQUENCE report.inquiry_sample_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE report.inquiry_sample_id_seq OWNER TO fundermaps;

--
-- Name: inquiry_sample_id_seq; Type: SEQUENCE OWNED BY; Schema: report; Owner: fundermaps
--

ALTER SEQUENCE report.inquiry_sample_id_seq OWNED BY report.inquiry_sample.id;


--
-- Name: recovery; Type: TABLE; Schema: report; Owner: fundermaps
--

CREATE TABLE report.recovery (
    id integer NOT NULL,
    create_date timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    update_date timestamp with time zone,
    delete_date timestamp with time zone,
    note text,
    attribution integer NOT NULL,
    access_policy application.access_policy DEFAULT 'private'::application.access_policy NOT NULL,
    type report.recovery_document_type DEFAULT 'unknown'::report.recovery_document_type NOT NULL,
    document_date date NOT NULL,
    document_file text NOT NULL,
    audit_status report.audit_status DEFAULT 'todo'::report.audit_status NOT NULL,
    document_name text NOT NULL
);


ALTER TABLE report.recovery OWNER TO fundermaps;

--
-- Name: TABLE recovery; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON TABLE report.recovery IS 'Contains recovery operations.';


--
-- Name: COLUMN recovery.create_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.recovery.create_date IS 'Timestamp of record creation, set by insert';


--
-- Name: COLUMN recovery.update_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.recovery.update_date IS 'Timestamp of last record update, automatically updated on record modification';


--
-- Name: COLUMN recovery.delete_date; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.recovery.delete_date IS 'Timestamp of soft delete';


--
-- Name: COLUMN recovery.document_name; Type: COMMENT; Schema: report; Owner: fundermaps
--

COMMENT ON COLUMN report.recovery.document_name IS 'User provided document name';


--
-- Name: recovery_id_seq; Type: SEQUENCE; Schema: report; Owner: fundermaps
--

CREATE SEQUENCE report.recovery_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE report.recovery_id_seq OWNER TO fundermaps;

--
-- Name: recovery_id_seq; Type: SEQUENCE OWNED BY; Schema: report; Owner: fundermaps
--

ALTER SEQUENCE report.recovery_id_seq OWNED BY report.recovery.id;


--
-- Name: recovery_sample_id_seq; Type: SEQUENCE; Schema: report; Owner: fundermaps
--

CREATE SEQUENCE report.recovery_sample_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE report.recovery_sample_id_seq OWNER TO fundermaps;

--
-- Name: recovery_sample_id_seq; Type: SEQUENCE OWNED BY; Schema: report; Owner: fundermaps
--

ALTER SEQUENCE report.recovery_sample_id_seq OWNED BY report.recovery_sample.id;


--
-- Name: attribution id; Type: DEFAULT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.attribution ALTER COLUMN id SET DEFAULT nextval('application.attribution_id_seq'::regclass);


--
-- Name: inquiry id; Type: DEFAULT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.inquiry ALTER COLUMN id SET DEFAULT nextval('report.inquiry_id_seq'::regclass);


--
-- Name: inquiry_sample id; Type: DEFAULT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.inquiry_sample ALTER COLUMN id SET DEFAULT nextval('report.inquiry_sample_id_seq'::regclass);


--
-- Name: recovery id; Type: DEFAULT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery ALTER COLUMN id SET DEFAULT nextval('report.recovery_id_seq'::regclass);


--
-- Name: recovery_sample id; Type: DEFAULT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery_sample ALTER COLUMN id SET DEFAULT nextval('report.recovery_sample_id_seq'::regclass);


--
-- Name: attribution attribution_pkey; Type: CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.attribution
    ADD CONSTRAINT attribution_pkey PRIMARY KEY (id);


--
-- Name: contact contact_pkey; Type: CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.contact
    ADD CONSTRAINT contact_pkey PRIMARY KEY (email);


--
-- Name: organization organization_pkey; Type: CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.organization
    ADD CONSTRAINT organization_pkey PRIMARY KEY (id);


--
-- Name: organization_proposal organization_proposal_pkey; Type: CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.organization_proposal
    ADD CONSTRAINT organization_proposal_pkey PRIMARY KEY (id);


--
-- Name: organization_user organization_user_pkey; Type: CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.organization_user
    ADD CONSTRAINT organization_user_pkey PRIMARY KEY (user_id, organization_id);


--
-- Name: product_telemetry product_telemetry_pkey; Type: CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.product_telemetry
    ADD CONSTRAINT product_telemetry_pkey PRIMARY KEY (organization_id, product);


--
-- Name: user user_pkey; Type: CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- Name: building_cluster building_cluster_pkey; Type: CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_cluster
    ADD CONSTRAINT building_cluster_pkey PRIMARY KEY (building_id);


--
-- Name: building_elevation building_elevation_pkey; Type: CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_elevation
    ADD CONSTRAINT building_elevation_pkey PRIMARY KEY (building_id);


--
-- Name: building_geographic_region building_geographic_region_pkey; Type: CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_geographic_region
    ADD CONSTRAINT building_geographic_region_pkey PRIMARY KEY (building_id);


--
-- Name: building_groundwater_level building_groundwater_level_pkey; Type: CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_groundwater_level
    ADD CONSTRAINT building_groundwater_level_pkey PRIMARY KEY (building_id);


--
-- Name: building_ownership building_ownership_pkey; Type: CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_ownership
    ADD CONSTRAINT building_ownership_pkey PRIMARY KEY (building_id);


--
-- Name: building_pleistocene building_pleistocene_pkey; Type: CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_pleistocene
    ADD CONSTRAINT building_pleistocene_pkey PRIMARY KEY (building_id);


--
-- Name: subsidence subsidence_pkey; Type: CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.subsidence
    ADD CONSTRAINT subsidence_pkey PRIMARY KEY (building_id);


--
-- Name: address address_pkey; Type: CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.address
    ADD CONSTRAINT address_pkey PRIMARY KEY (id);


--
-- Name: building building_pkey; Type: CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.building
    ADD CONSTRAINT building_pkey PRIMARY KEY (id);


--
-- Name: country country_pkey; Type: CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.country
    ADD CONSTRAINT country_pkey PRIMARY KEY (id);


--
-- Name: district district_pkey; Type: CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.district
    ADD CONSTRAINT district_pkey PRIMARY KEY (id);


--
-- Name: municipality municipality_pkey; Type: CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.municipality
    ADD CONSTRAINT municipality_pkey PRIMARY KEY (id);


--
-- Name: neighborhood neighborhood_pkey; Type: CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.neighborhood
    ADD CONSTRAINT neighborhood_pkey PRIMARY KEY (id);


--
-- Name: state state_pkey; Type: CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.state
    ADD CONSTRAINT state_pkey PRIMARY KEY (id);


--
-- Name: incident incident_pkey; Type: CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.incident
    ADD CONSTRAINT incident_pkey PRIMARY KEY (id);


--
-- Name: inquiry inquiry_pkey; Type: CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.inquiry
    ADD CONSTRAINT inquiry_pkey PRIMARY KEY (id);


--
-- Name: inquiry_sample inquiry_sample_pkey; Type: CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.inquiry_sample
    ADD CONSTRAINT inquiry_sample_pkey PRIMARY KEY (id);


--
-- Name: recovery recovery_pkey; Type: CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery
    ADD CONSTRAINT recovery_pkey PRIMARY KEY (id);


--
-- Name: recovery_sample recovery_sample_pkey; Type: CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery_sample
    ADD CONSTRAINT recovery_sample_pkey PRIMARY KEY (id);


--
-- Name: organization_fence_idx; Type: INDEX; Schema: application; Owner: fundermaps
--

CREATE INDEX organization_fence_idx ON application.organization USING gist (fence);


--
-- Name: organization_normalized_email_idx; Type: INDEX; Schema: application; Owner: fundermaps
--

CREATE UNIQUE INDEX organization_normalized_email_idx ON application.organization USING btree (normalized_email);


--
-- Name: organization_proposal_normalized_email_idx; Type: INDEX; Schema: application; Owner: fundermaps
--

CREATE INDEX organization_proposal_normalized_email_idx ON application.organization_proposal USING btree (normalized_email);


--
-- Name: user_normalized_email_idx; Type: INDEX; Schema: application; Owner: fundermaps
--

CREATE UNIQUE INDEX user_normalized_email_idx ON application."user" USING btree (normalized_email);


--
-- Name: analysis_complete_address_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE UNIQUE INDEX analysis_complete_address_idx ON data.analysis_complete USING btree (address_id);


--
-- Name: analysis_complete_building_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX analysis_complete_building_idx ON data.analysis_complete USING btree (building_id);


--
-- Name: analysis_complete_external_address_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE UNIQUE INDEX analysis_complete_external_address_idx ON data.analysis_complete USING btree (address_external_id);


--
-- Name: analysis_complete_external_building_id_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX analysis_complete_external_building_id_idx ON data.analysis_complete USING btree (external_building_id);


--
-- Name: analysis_foundation_indicative_foundation_type_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX analysis_foundation_indicative_foundation_type_idx ON data.analysis_foundation_indicative USING btree (foundation_type);


--
-- Name: analysis_foundation_indicative_id_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX analysis_foundation_indicative_id_idx ON data.analysis_foundation_indicative USING btree (id);


--
-- Name: analysis_foundation_indicative_neighborhood_id_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX analysis_foundation_indicative_neighborhood_id_idx ON data.analysis_foundation_indicative USING btree (neighborhood_id);


--
-- Name: analysis_foundation_risk_id_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX analysis_foundation_risk_id_idx ON data.analysis_foundation_risk USING btree (id);


--
-- Name: analysis_foundation_risk_neighborhood_id_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX analysis_foundation_risk_neighborhood_id_idx ON data.analysis_foundation_risk USING btree (neighborhood_id);


--
-- Name: building_cluster_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE UNIQUE INDEX building_cluster_idx ON data.building_cluster USING btree (cluster_id, building_id);


--
-- Name: building_elevation_available_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX building_elevation_available_idx ON data.building_elevation USING btree (building_id) WHERE ((roof IS NOT NULL) AND (ground IS NOT NULL));


--
-- Name: building_type_id_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX building_type_id_idx ON data.building_type USING btree (id);


--
-- Name: subsidence_velocity_idx; Type: INDEX; Schema: data; Owner: fundermaps
--

CREATE INDEX subsidence_velocity_idx ON data.subsidence USING btree (velocity);


--
-- Name: address_building_id_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX address_building_id_idx ON geocoder.address USING btree (building_id);


--
-- Name: address_building_number_postal_code_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX address_building_number_postal_code_idx ON geocoder.address USING btree (building_number, postal_code);


--
-- Name: address_external_id_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX address_external_id_idx ON geocoder.address USING btree (external_id);


--
-- Name: address_external_id_source_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE UNIQUE INDEX address_external_id_source_idx ON geocoder.address USING btree (external_id, external_source);


--
-- Name: address_streetname_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX address_streetname_idx ON geocoder.address USING btree (lower(street));


--
-- Name: building_active_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX building_active_idx ON geocoder.building USING btree (id) WHERE (is_active AND (geom IS NOT NULL));


--
-- Name: building_construction_year_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX building_construction_year_idx ON geocoder.building USING btree (date_part('year'::text, (built_year)::date));


--
-- Name: building_external_id_source_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE UNIQUE INDEX building_external_id_source_idx ON geocoder.building USING btree (external_id, external_source);


--
-- Name: building_geom_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX building_geom_idx ON geocoder.building USING gist (geom);


--
-- Name: building_neighborhood_active_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX building_neighborhood_active_idx ON geocoder.building USING btree (neighborhood_id) WHERE (is_active AND (geom IS NOT NULL));


--
-- Name: building_neighborhood_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX building_neighborhood_idx ON geocoder.building USING btree (neighborhood_id);


--
-- Name: country_external_id_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX country_external_id_idx ON geocoder.country USING btree (external_id, external_source);


--
-- Name: country_geom_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX country_geom_idx ON geocoder.country USING gist (geom);


--
-- Name: country_name_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX country_name_idx ON geocoder.country USING btree (name);


--
-- Name: district_external_id_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX district_external_id_idx ON geocoder.district USING btree (external_id, external_source);


--
-- Name: district_geom_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX district_geom_idx ON geocoder.district USING gist (geom);


--
-- Name: district_municipality_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX district_municipality_idx ON geocoder.district USING btree (municipality_id);


--
-- Name: district_name_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX district_name_idx ON geocoder.district USING btree (name);


--
-- Name: municipality_external_id_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX municipality_external_id_idx ON geocoder.municipality USING btree (external_id, external_source);


--
-- Name: municipality_geom_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX municipality_geom_idx ON geocoder.municipality USING gist (geom);


--
-- Name: municipality_name_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX municipality_name_idx ON geocoder.municipality USING btree (name);


--
-- Name: neighborhood_district_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX neighborhood_district_idx ON geocoder.neighborhood USING btree (district_id);


--
-- Name: neighborhood_external_id_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX neighborhood_external_id_idx ON geocoder.neighborhood USING btree (external_id);


--
-- Name: neighborhood_external_id_source_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX neighborhood_external_id_source_idx ON geocoder.neighborhood USING btree (external_id, external_source);


--
-- Name: neighborhood_geom_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX neighborhood_geom_idx ON geocoder.neighborhood USING gist (geom);


--
-- Name: neighborhood_name_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX neighborhood_name_idx ON geocoder.neighborhood USING btree (name);


--
-- Name: state_country_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX state_country_idx ON geocoder.state USING btree (country_id);


--
-- Name: state_external_id_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX state_external_id_idx ON geocoder.state USING btree (external_id, external_source);


--
-- Name: state_geom_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX state_geom_idx ON geocoder.state USING gist (geom);


--
-- Name: state_name_idx; Type: INDEX; Schema: geocoder; Owner: fundermaps
--

CREATE INDEX state_name_idx ON geocoder.state USING btree (name);


--
-- Name: inquiry_access_policy_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX inquiry_access_policy_idx ON report.inquiry USING btree (access_policy);


--
-- Name: inquiry_document_date_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX inquiry_document_date_idx ON report.inquiry USING btree (document_date);


--
-- Name: inquiry_sample_address_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX inquiry_sample_address_idx ON report.inquiry_sample USING btree (address);


--
-- Name: inquiry_type_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX inquiry_type_idx ON report.inquiry USING btree (type);


--
-- Name: recovery_access_policy_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX recovery_access_policy_idx ON report.recovery USING btree (access_policy);


--
-- Name: recovery_sample_address_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX recovery_sample_address_idx ON report.recovery_sample USING btree (address);


--
-- Name: recovery_sample_pile_type_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX recovery_sample_pile_type_idx ON report.recovery_sample USING btree (pile_type);


--
-- Name: recovery_sample_status_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX recovery_sample_status_idx ON report.recovery_sample USING btree (status);


--
-- Name: recovery_sample_type_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX recovery_sample_type_idx ON report.recovery_sample USING btree (type);


--
-- Name: recovery_type_idx; Type: INDEX; Schema: report; Owner: fundermaps
--

CREATE INDEX recovery_type_idx ON report.recovery USING btree (type);


--
-- Name: user set_normalized_email; Type: TRIGGER; Schema: application; Owner: fundermaps
--

CREATE TRIGGER set_normalized_email BEFORE INSERT OR UPDATE ON application."user" FOR EACH ROW EXECUTE FUNCTION application.normalize_email();


--
-- Name: incident update_date_record; Type: TRIGGER; Schema: report; Owner: fundermaps
--

CREATE TRIGGER update_date_record BEFORE UPDATE ON report.incident FOR EACH ROW EXECUTE FUNCTION report.last_record_update();


--
-- Name: inquiry update_date_record; Type: TRIGGER; Schema: report; Owner: fundermaps
--

CREATE TRIGGER update_date_record BEFORE UPDATE ON report.inquiry FOR EACH ROW EXECUTE FUNCTION report.last_record_update();


--
-- Name: inquiry_sample update_date_record; Type: TRIGGER; Schema: report; Owner: fundermaps
--

CREATE TRIGGER update_date_record BEFORE UPDATE ON report.inquiry_sample FOR EACH ROW EXECUTE FUNCTION report.last_record_update();


--
-- Name: recovery update_date_record; Type: TRIGGER; Schema: report; Owner: fundermaps
--

CREATE TRIGGER update_date_record BEFORE UPDATE ON report.recovery FOR EACH ROW EXECUTE FUNCTION report.last_record_update();


--
-- Name: recovery_sample update_date_record; Type: TRIGGER; Schema: report; Owner: fundermaps
--

CREATE TRIGGER update_date_record BEFORE UPDATE ON report.recovery_sample FOR EACH ROW EXECUTE FUNCTION report.last_record_update();


--
-- Name: attribution attribution_contractor_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.attribution
    ADD CONSTRAINT attribution_contractor_fkey FOREIGN KEY (contractor) REFERENCES application.organization(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: attribution attribution_creator_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.attribution
    ADD CONSTRAINT attribution_creator_fkey FOREIGN KEY (creator) REFERENCES application."user"(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: attribution attribution_owner_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.attribution
    ADD CONSTRAINT attribution_owner_fkey FOREIGN KEY (owner) REFERENCES application.organization(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: attribution attribution_reviewer_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.attribution
    ADD CONSTRAINT attribution_reviewer_fkey FOREIGN KEY (reviewer) REFERENCES application."user"(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: organization_user organization_user_organization_id_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.organization_user
    ADD CONSTRAINT organization_user_organization_id_fkey FOREIGN KEY (organization_id) REFERENCES application.organization(id) ON DELETE CASCADE;


--
-- Name: organization_user organization_user_user_id_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.organization_user
    ADD CONSTRAINT organization_user_user_id_fkey FOREIGN KEY (user_id) REFERENCES application."user"(id) ON DELETE CASCADE;


--
-- Name: product_telemetry product_telemetry_organization_id_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.product_telemetry
    ADD CONSTRAINT product_telemetry_organization_id_fkey FOREIGN KEY (organization_id) REFERENCES application.organization(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: product_telemetry product_telemetry_user_id_fkey; Type: FK CONSTRAINT; Schema: application; Owner: fundermaps
--

ALTER TABLE ONLY application.product_telemetry
    ADD CONSTRAINT product_telemetry_user_id_fkey FOREIGN KEY (user_id) REFERENCES application."user"(id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- Name: building_elevation building_elevation_building_fkey; Type: FK CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_elevation
    ADD CONSTRAINT building_elevation_building_fkey FOREIGN KEY (building_id) REFERENCES geocoder.building(id);


--
-- Name: building_geographic_region building_geographic_region_building_fk; Type: FK CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_geographic_region
    ADD CONSTRAINT building_geographic_region_building_fk FOREIGN KEY (building_id) REFERENCES geocoder.building(id);


--
-- Name: building_groundwater_level building_groundwater_level_building_fk; Type: FK CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_groundwater_level
    ADD CONSTRAINT building_groundwater_level_building_fk FOREIGN KEY (building_id) REFERENCES geocoder.building(id);


--
-- Name: building_ownership building_ownership_building_fkey; Type: FK CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_ownership
    ADD CONSTRAINT building_ownership_building_fkey FOREIGN KEY (building_id) REFERENCES geocoder.building(id);


--
-- Name: building_pleistocene building_pleistocene_building_fkey; Type: FK CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.building_pleistocene
    ADD CONSTRAINT building_pleistocene_building_fkey FOREIGN KEY (building_id) REFERENCES geocoder.building(id);


--
-- Name: subsidence subsidence_building_fk; Type: FK CONSTRAINT; Schema: data; Owner: fundermaps
--

ALTER TABLE ONLY data.subsidence
    ADD CONSTRAINT subsidence_building_fk FOREIGN KEY (building_id) REFERENCES geocoder.building(id);


--
-- Name: address address_building_fk; Type: FK CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.address
    ADD CONSTRAINT address_building_fk FOREIGN KEY (building_id) REFERENCES geocoder.building(id);


--
-- Name: building building_neighborhood_id; Type: FK CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.building
    ADD CONSTRAINT building_neighborhood_id FOREIGN KEY (neighborhood_id) REFERENCES geocoder.neighborhood(id);


--
-- Name: district district_country_fk; Type: FK CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.district
    ADD CONSTRAINT district_country_fk FOREIGN KEY (municipality_id) REFERENCES geocoder.municipality(id);


--
-- Name: neighborhood neighborhood_district_fk; Type: FK CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.neighborhood
    ADD CONSTRAINT neighborhood_district_fk FOREIGN KEY (district_id) REFERENCES geocoder.district(id);


--
-- Name: state state_country_fk; Type: FK CONSTRAINT; Schema: geocoder; Owner: fundermaps
--

ALTER TABLE ONLY geocoder.state
    ADD CONSTRAINT state_country_fk FOREIGN KEY (country_id) REFERENCES geocoder.country(id);


--
-- Name: incident incident_address_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.incident
    ADD CONSTRAINT incident_address_fkey FOREIGN KEY (address) REFERENCES geocoder.address(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: incident incident_contact_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.incident
    ADD CONSTRAINT incident_contact_fkey FOREIGN KEY (contact) REFERENCES application.contact(email) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: inquiry inquiry_attribution_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.inquiry
    ADD CONSTRAINT inquiry_attribution_fkey FOREIGN KEY (attribution) REFERENCES application.attribution(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: inquiry_sample inquiry_sample_inquiry_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.inquiry_sample
    ADD CONSTRAINT inquiry_sample_inquiry_fkey FOREIGN KEY (inquiry) REFERENCES report.inquiry(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: recovery recovery_attribution_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery
    ADD CONSTRAINT recovery_attribution_fkey FOREIGN KEY (attribution) REFERENCES application.attribution(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: recovery_sample recovery_sample_address_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery_sample
    ADD CONSTRAINT recovery_sample_address_fkey FOREIGN KEY (address) REFERENCES geocoder.address(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: inquiry_sample recovery_sample_address_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.inquiry_sample
    ADD CONSTRAINT recovery_sample_address_fkey FOREIGN KEY (address) REFERENCES geocoder.address(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: recovery_sample recovery_sample_contractor_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery_sample
    ADD CONSTRAINT recovery_sample_contractor_fkey FOREIGN KEY (contractor) REFERENCES application.organization(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- Name: recovery_sample recovery_sample_recovery_fkey; Type: FK CONSTRAINT; Schema: report; Owner: fundermaps
--

ALTER TABLE ONLY report.recovery_sample
    ADD CONSTRAINT recovery_sample_recovery_fkey FOREIGN KEY (recovery) REFERENCES report.recovery(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: SCHEMA application; Type: ACL; Schema: -; Owner: fundermaps
--

GRANT USAGE ON SCHEMA application TO fundermaps_webapp;
GRANT USAGE ON SCHEMA application TO fundermaps_webservice;
GRANT USAGE ON SCHEMA application TO fundermaps_portal;
GRANT USAGE ON SCHEMA application TO fundermaps_batch;


--
-- Name: SCHEMA data; Type: ACL; Schema: -; Owner: fundermaps
--

GRANT USAGE ON SCHEMA data TO fundermaps_webapp;
GRANT USAGE ON SCHEMA data TO fundermaps_webservice;
GRANT USAGE ON SCHEMA data TO fundermaps_portal;
GRANT USAGE ON SCHEMA data TO fundermaps_batch;


--
-- Name: SCHEMA geocoder; Type: ACL; Schema: -; Owner: fundermaps
--

GRANT USAGE ON SCHEMA geocoder TO fundermaps_webapp;
GRANT USAGE ON SCHEMA geocoder TO fundermaps_webservice;
GRANT USAGE ON SCHEMA geocoder TO fundermaps_portal;
GRANT USAGE ON SCHEMA geocoder TO fundermaps_batch;


--
-- Name: SCHEMA maplayer; Type: ACL; Schema: -; Owner: fundermaps
--

GRANT USAGE ON SCHEMA maplayer TO fundermaps_webapp;
GRANT USAGE ON SCHEMA maplayer TO fundermaps_webservice;
GRANT USAGE ON SCHEMA maplayer TO fundermaps_portal;
GRANT USAGE ON SCHEMA maplayer TO fundermaps_batch;


--
-- Name: SCHEMA report; Type: ACL; Schema: -; Owner: fundermaps
--

GRANT USAGE ON SCHEMA report TO fundermaps_webapp;
GRANT USAGE ON SCHEMA report TO fundermaps_webservice;
GRANT USAGE ON SCHEMA report TO fundermaps_portal;
GRANT USAGE ON SCHEMA report TO fundermaps_batch;


--
-- Name: TYPE access_policy; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.access_policy TO fundermaps_webapp;
GRANT ALL ON TYPE application.access_policy TO fundermaps_webservice;


--
-- Name: TYPE email; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.email TO fundermaps_webapp;
GRANT ALL ON TYPE application.email TO fundermaps_webservice;


--
-- Name: TYPE organization_id; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.organization_id TO fundermaps_webapp;
GRANT ALL ON TYPE application.organization_id TO fundermaps_webservice;


--
-- Name: TYPE organization_role; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.organization_role TO fundermaps_webapp;
GRANT ALL ON TYPE application.organization_role TO fundermaps_webservice;


--
-- Name: TYPE phone; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.phone TO fundermaps_webapp;
GRANT ALL ON TYPE application.phone TO fundermaps_webservice;


--
-- Name: TYPE role; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.role TO fundermaps_webapp;
GRANT ALL ON TYPE application.role TO fundermaps_webservice;


--
-- Name: TYPE user_id; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.user_id TO fundermaps_webapp;
GRANT ALL ON TYPE application.user_id TO fundermaps_webservice;


--
-- Name: TYPE foundation_category; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT ALL ON TYPE data.foundation_category TO fundermaps_webapp;
GRANT ALL ON TYPE data.foundation_category TO fundermaps_webservice;


--
-- Name: TYPE foundation_risk_indication; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT ALL ON TYPE data.foundation_risk_indication TO fundermaps_webapp;
GRANT ALL ON TYPE data.foundation_risk_indication TO fundermaps_webservice;


--
-- Name: TYPE reliability; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT ALL ON TYPE data.reliability TO fundermaps_webapp;
GRANT ALL ON TYPE data.reliability TO fundermaps_webservice;


--
-- Name: TYPE building_type; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT ALL ON TYPE geocoder.building_type TO fundermaps_webapp;
GRANT ALL ON TYPE geocoder.building_type TO fundermaps_webservice;


--
-- Name: TYPE data_source; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT ALL ON TYPE geocoder.data_source TO fundermaps_webapp;
GRANT ALL ON TYPE geocoder.data_source TO fundermaps_webservice;


--
-- Name: FUNCTION geocoder_generate_id(); Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT ALL ON FUNCTION geocoder.geocoder_generate_id() TO fundermaps_webapp;
GRANT ALL ON FUNCTION geocoder.geocoder_generate_id() TO fundermaps_webservice;


--
-- Name: TYPE geocoder_id; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT ALL ON TYPE geocoder.geocoder_id TO fundermaps_webapp;
GRANT ALL ON TYPE geocoder.geocoder_id TO fundermaps_webservice;


--
-- Name: TYPE year; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT ALL ON TYPE geocoder.year TO fundermaps_webapp;
GRANT ALL ON TYPE geocoder.year TO fundermaps_webservice;


--
-- Name: TYPE audit_status; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.audit_status TO fundermaps_webapp;
GRANT ALL ON TYPE report.audit_status TO fundermaps_webservice;


--
-- Name: TYPE construction_pile; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.construction_pile TO fundermaps_webapp;
GRANT ALL ON TYPE report.construction_pile TO fundermaps_webservice;


--
-- Name: TYPE construction_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.construction_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.construction_type TO fundermaps_webservice;


--
-- Name: TYPE crack_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.crack_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.crack_type TO fundermaps_webservice;


--
-- Name: TYPE diameter; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.diameter TO fundermaps_webapp;
GRANT ALL ON TYPE report.diameter TO fundermaps_webservice;


--
-- Name: TYPE enforcement_term; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.enforcement_term TO fundermaps_webapp;
GRANT ALL ON TYPE report.enforcement_term TO fundermaps_webservice;


--
-- Name: TYPE environment_damage_characteristics; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.environment_damage_characteristics TO fundermaps_webapp;
GRANT ALL ON TYPE report.environment_damage_characteristics TO fundermaps_webservice;


--
-- Name: TYPE facade; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.facade TO fundermaps_webapp;
GRANT ALL ON TYPE report.facade TO fundermaps_webservice;


--
-- Name: TYPE foundation_damage_cause; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.foundation_damage_cause TO fundermaps_webapp;
GRANT ALL ON TYPE report.foundation_damage_cause TO fundermaps_webservice;


--
-- Name: TYPE foundation_damage_characteristics; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.foundation_damage_characteristics TO fundermaps_webapp;
GRANT ALL ON TYPE report.foundation_damage_characteristics TO fundermaps_webservice;


--
-- Name: TYPE foundation_quality; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.foundation_quality TO fundermaps_webapp;
GRANT ALL ON TYPE report.foundation_quality TO fundermaps_webservice;


--
-- Name: TYPE foundation_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.foundation_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.foundation_type TO fundermaps_webservice;


--
-- Name: TYPE height; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.height TO fundermaps_webapp;
GRANT ALL ON TYPE report.height TO fundermaps_webservice;


--
-- Name: TYPE incident_question_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.incident_question_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.incident_question_type TO fundermaps_webservice;


--
-- Name: TYPE inquiry_status; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.inquiry_status TO fundermaps_webapp;
GRANT ALL ON TYPE report.inquiry_status TO fundermaps_webservice;


--
-- Name: TYPE inquiry_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.inquiry_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.inquiry_type TO fundermaps_webservice;


--
-- Name: TYPE length; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.length TO fundermaps_webapp;
GRANT ALL ON TYPE report.length TO fundermaps_webservice;


--
-- Name: TYPE pile_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.pile_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.pile_type TO fundermaps_webservice;


--
-- Name: TYPE quality; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.quality TO fundermaps_webapp;
GRANT ALL ON TYPE report.quality TO fundermaps_webservice;


--
-- Name: TYPE recovery_document_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.recovery_document_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.recovery_document_type TO fundermaps_webservice;


--
-- Name: TYPE recovery_status; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.recovery_status TO fundermaps_webapp;
GRANT ALL ON TYPE report.recovery_status TO fundermaps_webservice;


--
-- Name: TYPE recovery_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.recovery_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.recovery_type TO fundermaps_webservice;


--
-- Name: TYPE rotation_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.rotation_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.rotation_type TO fundermaps_webservice;


--
-- Name: TYPE size; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.size TO fundermaps_webapp;
GRANT ALL ON TYPE report.size TO fundermaps_webservice;


--
-- Name: TYPE substructure; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.substructure TO fundermaps_webapp;
GRANT ALL ON TYPE report.substructure TO fundermaps_webservice;


--
-- Name: TYPE wood_encroachement; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.wood_encroachement TO fundermaps_webapp;
GRANT ALL ON TYPE report.wood_encroachement TO fundermaps_webservice;


--
-- Name: TYPE wood_quality; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.wood_quality TO fundermaps_webapp;
GRANT ALL ON TYPE report.wood_quality TO fundermaps_webservice;


--
-- Name: TYPE wood_type; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.wood_type TO fundermaps_webapp;
GRANT ALL ON TYPE report.wood_type TO fundermaps_webservice;


--
-- Name: TYPE year; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.year TO fundermaps_webapp;
GRANT ALL ON TYPE report.year TO fundermaps_webservice;


--
-- Name: FUNCTION create_geofence(json); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_geofence(json) TO fundermaps_webapp;


--
-- Name: FUNCTION create_organization(organization_id application.organization_id, email text, password_hash text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization(organization_id application.organization_id, email text, password_hash text) TO fundermaps_webapp;


--
-- Name: FUNCTION create_organization_proposal(organization_name text, email text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization_proposal(organization_name text, email text) TO fundermaps_webapp;


--
-- Name: FUNCTION create_organization_user(organization_id application.organization_id, email text, password_hash text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text) TO fundermaps_webapp;


--
-- Name: FUNCTION create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role) TO fundermaps_webapp;


--
-- Name: FUNCTION create_user(email text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_user(email text) TO fundermaps_webapp;


--
-- Name: FUNCTION create_user(email text, password_hash text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_user(email text, password_hash text) TO fundermaps_webapp;


--
-- Name: FUNCTION log_access(id application.user_id); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.log_access(id application.user_id) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.log_access(id application.user_id) TO fundermaps_webservice;


--
-- Name: FUNCTION normalize(text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.normalize(text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.normalize(text) TO fundermaps_webservice;
GRANT ALL ON FUNCTION application.normalize(text) TO fundermaps_portal;
GRANT ALL ON FUNCTION application.normalize(text) TO fundermaps_batch;


--
-- Name: FUNCTION normalize_email(); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.normalize_email() TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.normalize_email() TO fundermaps_webservice;


--
-- Name: FUNCTION organization_email_free(email text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.organization_email_free(email text) TO fundermaps_webapp;


--
-- Name: FUNCTION get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type); Type: ACL; Schema: data; Owner: fundermaps
--

GRANT ALL ON FUNCTION data.get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type) TO fundermaps_webapp;
GRANT ALL ON FUNCTION data.get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type) TO fundermaps_webservice;
GRANT ALL ON FUNCTION data.get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type) TO fundermaps_portal;


--
-- Name: FUNCTION fir_generate_id(client_id integer); Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON FUNCTION report.fir_generate_id(client_id integer) TO fundermaps_webapp;
GRANT ALL ON FUNCTION report.fir_generate_id(client_id integer) TO fundermaps_webservice;
GRANT ALL ON FUNCTION report.fir_generate_id(client_id integer) TO fundermaps_portal;


--
-- Name: FUNCTION last_record_update(); Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON FUNCTION report.last_record_update() TO fundermaps_webapp;
GRANT ALL ON FUNCTION report.last_record_update() TO fundermaps_webservice;
GRANT ALL ON FUNCTION report.last_record_update() TO fundermaps_portal;


--
-- Name: TABLE attribution; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.attribution TO fundermaps_webapp;
GRANT SELECT ON TABLE application.attribution TO fundermaps_webservice;
GRANT SELECT ON TABLE application.attribution TO fundermaps_batch;
GRANT SELECT ON TABLE application.attribution TO fundermaps_portal;


--
-- Name: SEQUENCE attribution_id_seq; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON SEQUENCE application.attribution_id_seq TO fundermaps_webapp;
GRANT ALL ON SEQUENCE application.attribution_id_seq TO fundermaps_webservice;


--
-- Name: TABLE contact; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,TRIGGER,UPDATE ON TABLE application.contact TO fundermaps_webapp;
GRANT SELECT ON TABLE application.contact TO fundermaps_webservice;
GRANT SELECT,INSERT,REFERENCES,TRIGGER,UPDATE ON TABLE application.contact TO fundermaps_portal;
GRANT SELECT ON TABLE application.contact TO fundermaps_batch;


--
-- Name: TABLE organization; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization TO fundermaps_webapp;
GRANT SELECT ON TABLE application.organization TO fundermaps_webservice;
GRANT SELECT ON TABLE application.organization TO fundermaps_batch;
GRANT SELECT ON TABLE application.organization TO fundermaps_portal;


--
-- Name: TABLE organization_proposal; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization_proposal TO fundermaps_webapp;
GRANT SELECT ON TABLE application.organization_proposal TO fundermaps_webservice;
GRANT SELECT ON TABLE application.organization_proposal TO fundermaps_batch;
GRANT SELECT ON TABLE application.organization_proposal TO fundermaps_portal;


--
-- Name: TABLE organization_user; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization_user TO fundermaps_webapp;
GRANT SELECT ON TABLE application.organization_user TO fundermaps_webservice;
GRANT SELECT ON TABLE application.organization_user TO fundermaps_batch;
GRANT SELECT ON TABLE application.organization_user TO fundermaps_portal;


--
-- Name: TABLE product_telemetry; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.product_telemetry TO fundermaps_webapp;
GRANT SELECT,INSERT,UPDATE ON TABLE application.product_telemetry TO fundermaps_webservice;
GRANT SELECT ON TABLE application.product_telemetry TO fundermaps_batch;
GRANT SELECT ON TABLE application.product_telemetry TO fundermaps_portal;


--
-- Name: TABLE "user"; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application."user" TO fundermaps_webapp;
GRANT SELECT,UPDATE ON TABLE application."user" TO fundermaps_webservice;
GRANT SELECT ON TABLE application."user" TO fundermaps_batch;
GRANT SELECT ON TABLE application."user" TO fundermaps_portal;


--
-- Name: TABLE building_elevation; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.building_elevation TO fundermaps_webapp;
GRANT SELECT ON TABLE data.building_elevation TO fundermaps_webservice;
GRANT SELECT ON TABLE data.building_elevation TO fundermaps_portal;


--
-- Name: TABLE building_geographic_region; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.building_geographic_region TO fundermaps_webapp;
GRANT SELECT ON TABLE data.building_geographic_region TO fundermaps_webservice;
GRANT SELECT ON TABLE data.building_geographic_region TO fundermaps_portal;


--
-- Name: TABLE building_height; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.building_height TO fundermaps_webapp;
GRANT SELECT ON TABLE data.building_height TO fundermaps_webservice;
GRANT SELECT ON TABLE data.building_height TO fundermaps_portal;


--
-- Name: TABLE address; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.address TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.address TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.address TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.address TO fundermaps_batch;


--
-- Name: TABLE building; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.building TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.building TO fundermaps_batch;


--
-- Name: TABLE building_type; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.building_type TO fundermaps_portal;
GRANT SELECT ON TABLE data.building_type TO fundermaps_webapp;
GRANT SELECT ON TABLE data.building_type TO fundermaps_webservice;


--
-- Name: TABLE subsidence; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.subsidence TO fundermaps_webapp;
GRANT SELECT ON TABLE data.subsidence TO fundermaps_webservice;
GRANT SELECT ON TABLE data.subsidence TO fundermaps_portal;


--
-- Name: TABLE building_active; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_active TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_active TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_active TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_active TO fundermaps_batch;


--
-- Name: TABLE analysis_foundation_indicative; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.analysis_foundation_indicative TO fundermaps_portal;
GRANT SELECT ON TABLE data.analysis_foundation_indicative TO fundermaps_webapp;
GRANT SELECT ON TABLE data.analysis_foundation_indicative TO fundermaps_webservice;


--
-- Name: TABLE building_groundwater_level; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.building_groundwater_level TO fundermaps_webapp;
GRANT SELECT ON TABLE data.building_groundwater_level TO fundermaps_webservice;
GRANT SELECT ON TABLE data.building_groundwater_level TO fundermaps_portal;


--
-- Name: TABLE analysis_foundation_risk; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.analysis_foundation_risk TO fundermaps_portal;
GRANT SELECT ON TABLE data.analysis_foundation_risk TO fundermaps_webapp;
GRANT SELECT ON TABLE data.analysis_foundation_risk TO fundermaps_webservice;


--
-- Name: TABLE inquiry; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.inquiry TO fundermaps_webapp;


--
-- Name: TABLE inquiry_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.inquiry_sample TO fundermaps_webapp;
GRANT SELECT ON TABLE report.inquiry_sample TO fundermaps_portal;
GRANT SELECT ON TABLE report.inquiry_sample TO fundermaps_webservice;


--
-- Name: TABLE recovery_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.recovery_sample TO fundermaps_webapp;


--
-- Name: TABLE analysis_address; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.analysis_address TO fundermaps_webapp;
GRANT SELECT ON TABLE data.analysis_address TO fundermaps_webservice;
GRANT SELECT ON TABLE data.analysis_address TO fundermaps_portal;


--
-- Name: TABLE building_ownership; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.building_ownership TO fundermaps_portal;
GRANT SELECT ON TABLE data.building_ownership TO fundermaps_webapp;
GRANT SELECT ON TABLE data.building_ownership TO fundermaps_webservice;


--
-- Name: TABLE analysis_complete; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.analysis_complete TO fundermaps_portal;
GRANT SELECT ON TABLE data.analysis_complete TO fundermaps_webapp;
GRANT SELECT ON TABLE data.analysis_complete TO fundermaps_webservice;


--
-- Name: TABLE statistics_product_buildings_restored; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.statistics_product_buildings_restored TO fundermaps_webapp;
GRANT SELECT ON TABLE data.statistics_product_buildings_restored TO fundermaps_webservice;
GRANT SELECT ON TABLE data.statistics_product_buildings_restored TO fundermaps_portal;


--
-- Name: TABLE statistics_product_construction_years; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.statistics_product_construction_years TO fundermaps_webapp;
GRANT SELECT ON TABLE data.statistics_product_construction_years TO fundermaps_webservice;
GRANT SELECT ON TABLE data.statistics_product_construction_years TO fundermaps_portal;


--
-- Name: TABLE statistics_product_data_collected; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.statistics_product_data_collected TO fundermaps_webapp;
GRANT SELECT ON TABLE data.statistics_product_data_collected TO fundermaps_webservice;
GRANT SELECT ON TABLE data.statistics_product_data_collected TO fundermaps_portal;


--
-- Name: TABLE statistics_product_foundation_risk; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.statistics_product_foundation_risk TO fundermaps_portal;
GRANT SELECT ON TABLE data.statistics_product_foundation_risk TO fundermaps_webapp;
GRANT SELECT ON TABLE data.statistics_product_foundation_risk TO fundermaps_webservice;


--
-- Name: TABLE statistics_product_foundation_type; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.statistics_product_foundation_type TO fundermaps_portal;
GRANT SELECT ON TABLE data.statistics_product_foundation_type TO fundermaps_webapp;
GRANT SELECT ON TABLE data.statistics_product_foundation_type TO fundermaps_webservice;


--
-- Name: TABLE incident; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.incident TO fundermaps_webapp;
GRANT SELECT ON TABLE report.incident TO fundermaps_webservice;
GRANT SELECT,INSERT,REFERENCES,TRIGGER,UPDATE ON TABLE report.incident TO fundermaps_portal;


--
-- Name: TABLE statistics_product_incidents; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.statistics_product_incidents TO fundermaps_portal;
GRANT SELECT ON TABLE data.statistics_product_incidents TO fundermaps_webapp;
GRANT SELECT ON TABLE data.statistics_product_incidents TO fundermaps_webservice;


--
-- Name: TABLE statistics_product_inquiries; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT ON TABLE data.statistics_product_inquiries TO fundermaps_portal;
GRANT SELECT ON TABLE data.statistics_product_inquiries TO fundermaps_webapp;
GRANT SELECT ON TABLE data.statistics_product_inquiries TO fundermaps_webservice;


--
-- Name: TABLE country; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.country TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.country TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.country TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.country TO fundermaps_batch;


--
-- Name: TABLE district; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.district TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.district TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.district TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.district TO fundermaps_batch;


--
-- Name: TABLE municipality; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.municipality TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.municipality TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.municipality TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.municipality TO fundermaps_batch;


--
-- Name: TABLE neighborhood; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.neighborhood TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.neighborhood TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.neighborhood TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.neighborhood TO fundermaps_batch;


--
-- Name: TABLE state; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.state TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.state TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.state TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.state TO fundermaps_batch;


--
-- Name: TABLE analysis_building; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT ON TABLE maplayer.analysis_building TO fundermaps_batch;


--
-- Name: TABLE analysis_foundation; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT ON TABLE maplayer.analysis_foundation TO fundermaps_batch;


--
-- Name: TABLE analysis_quality; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT ON TABLE maplayer.analysis_quality TO fundermaps_batch;


--
-- Name: TABLE analysis_report; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT ON TABLE maplayer.analysis_report TO fundermaps_batch;


--
-- Name: TABLE incident; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT ON TABLE maplayer.incident TO fundermaps_batch;


--
-- Name: TABLE incident_aggregate; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT ON TABLE maplayer.incident_aggregate TO fundermaps_batch;


--
-- Name: TABLE incident_aggregate_category; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT ON TABLE maplayer.incident_aggregate_category TO fundermaps_batch;


--
-- Name: SEQUENCE incident_id_seq; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON SEQUENCE report.incident_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE report.incident_id_seq TO fundermaps_webservice;
GRANT ALL ON SEQUENCE report.incident_id_seq TO fundermaps_portal;


--
-- Name: SEQUENCE inquiry_id_seq; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON SEQUENCE report.inquiry_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE report.inquiry_id_seq TO fundermaps_webservice;


--
-- Name: SEQUENCE inquiry_sample_id_seq; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON SEQUENCE report.inquiry_sample_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE report.inquiry_sample_id_seq TO fundermaps_webservice;
GRANT SELECT,USAGE ON SEQUENCE report.inquiry_sample_id_seq TO fundermaps_portal;


--
-- Name: TABLE recovery; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.recovery TO fundermaps_webapp;


--
-- Name: SEQUENCE recovery_id_seq; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON SEQUENCE report.recovery_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE report.recovery_id_seq TO fundermaps_webservice;


--
-- Name: SEQUENCE recovery_sample_id_seq; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON SEQUENCE report.recovery_sample_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE report.recovery_sample_id_seq TO fundermaps_webservice;


--
-- PostgreSQL database dump complete
--

