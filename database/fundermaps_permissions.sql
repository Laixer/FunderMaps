--
-- FunderMaps database permissions
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
-- Name: TYPE storage_identifier; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.storage_identifier TO fundermaps_webapp;
GRANT ALL ON TYPE application.storage_identifier TO fundermaps_webservice;


--
-- Name: TYPE user_id; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON TYPE application.user_id TO fundermaps_webapp;
GRANT ALL ON TYPE application.user_id TO fundermaps_webservice;


--
-- Name: TYPE bag_id; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT ALL ON TYPE data.bag_id TO fundermaps_webapp;
GRANT ALL ON TYPE data.bag_id TO fundermaps_webservice;


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
-- Name: TYPE product; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT ALL ON TYPE data.product TO fundermaps_webapp;
GRANT ALL ON TYPE data.product TO fundermaps_webservice;


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
-- Name: TYPE project_sample_status; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.project_sample_status TO fundermaps_webapp;
GRANT ALL ON TYPE report.project_sample_status TO fundermaps_webservice;


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
-- Name: TYPE storage_identifier; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON TYPE report.storage_identifier TO fundermaps_webapp;
GRANT ALL ON TYPE report.storage_identifier TO fundermaps_webservice;


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
GRANT ALL ON FUNCTION application.create_geofence(json) TO fundermaps_webservice;


--
-- Name: FUNCTION create_organization(organization_id application.organization_id, email text, password_hash text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization(organization_id application.organization_id, email text, password_hash text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.create_organization(organization_id application.organization_id, email text, password_hash text) TO fundermaps_webservice;


--
-- Name: FUNCTION create_organization_proposal(organization_name text, email text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization_proposal(organization_name text, email text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.create_organization_proposal(organization_name text, email text) TO fundermaps_webservice;


--
-- Name: FUNCTION create_organization_user(organization_id application.organization_id, email text, password_hash text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text) TO fundermaps_webservice;


--
-- Name: FUNCTION create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.create_organization_user(organization_id application.organization_id, email text, password_hash text, organization_role application.organization_role) TO fundermaps_webservice;


--
-- Name: FUNCTION create_user(email text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_user(email text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.create_user(email text) TO fundermaps_webservice;


--
-- Name: FUNCTION create_user(email text, password_hash text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.create_user(email text, password_hash text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.create_user(email text, password_hash text) TO fundermaps_webservice;


--
-- Name: FUNCTION is_geometry_in_fence(user_id uuid, geom public.geometry); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.is_geometry_in_fence(user_id uuid, geom public.geometry) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.is_geometry_in_fence(user_id uuid, geom public.geometry) TO fundermaps_webservice;
GRANT ALL ON FUNCTION application.is_geometry_in_fence(user_id uuid, geom public.geometry) TO fundermaps_portal;


--
-- Name: FUNCTION normalize(text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.normalize(text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.normalize(text) TO fundermaps_webservice;
GRANT ALL ON FUNCTION application.normalize(text) TO fundermaps_portal;


--
-- Name: FUNCTION organization_email_free(email text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.organization_email_free(email text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.organization_email_free(email text) TO fundermaps_webservice;


--
-- Name: FUNCTION organization_name_free(name text); Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON FUNCTION application.organization_name_free(name text) TO fundermaps_webapp;
GRANT ALL ON FUNCTION application.organization_name_free(name text) TO fundermaps_webservice;


--
-- Name: FUNCTION get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type); Type: ACL; Schema: data; Owner: fundermaps
--

GRANT ALL ON FUNCTION data.get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type) TO fundermaps_webapp;
GRANT ALL ON FUNCTION data.get_foundation_category(type_indicative report.foundation_type, type_report report.foundation_type) TO fundermaps_webservice;


--
-- Name: FUNCTION record_update(); Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT ALL ON FUNCTION maplayer.record_update() TO fundermaps_batch;


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
GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.attribution TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE application.attribution TO fundermaps_batch;


--
-- Name: SEQUENCE attribution_id_seq; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT ALL ON SEQUENCE application.attribution_id_seq TO fundermaps_webapp;
GRANT ALL ON SEQUENCE application.attribution_id_seq TO fundermaps_webservice;


--
-- Name: TABLE contact; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.contact TO fundermaps_webapp;
GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.contact TO fundermaps_webservice;
GRANT SELECT,INSERT,REFERENCES ON TABLE application.contact TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE application.contact TO fundermaps_batch;


--
-- Name: TABLE organization; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization TO fundermaps_webapp;
GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE application.organization TO fundermaps_batch;


--
-- Name: TABLE organization_proposal; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization_proposal TO fundermaps_webapp;
GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization_proposal TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE application.organization_proposal TO fundermaps_batch;


--
-- Name: TABLE organization_user; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization_user TO fundermaps_webapp;
GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.organization_user TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE application.organization_user TO fundermaps_batch;


--
-- Name: TABLE product_telemetry; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.product_telemetry TO fundermaps_webapp;
GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application.product_telemetry TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE application.product_telemetry TO fundermaps_batch;


--
-- Name: TABLE "user"; Type: ACL; Schema: application; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application."user" TO fundermaps_webapp;
GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE application."user" TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE application."user" TO fundermaps_batch;


--
-- Name: TABLE building_geographic_region; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.building_geographic_region TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.building_geographic_region TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.building_geographic_region TO fundermaps_portal;


--
-- Name: TABLE premise_z; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.premise_z TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.premise_z TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.premise_z TO fundermaps_portal;


--
-- Name: TABLE premise_z_normalized; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.premise_z_normalized TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.premise_z_normalized TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.premise_z_normalized TO fundermaps_portal;


--
-- Name: TABLE subsidence; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.subsidence TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.subsidence TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.subsidence TO fundermaps_portal;


--
-- Name: TABLE building; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.building TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.building TO fundermaps_batch;


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

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.analysis_foundation_indicative TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.analysis_foundation_indicative TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.analysis_foundation_indicative TO fundermaps_portal;


--
-- Name: TABLE building_groundwater_level; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.building_groundwater_level TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.building_groundwater_level TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.building_groundwater_level TO fundermaps_portal;


--
-- Name: TABLE analysis_foundation_risk; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.analysis_foundation_risk TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.analysis_foundation_risk TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.analysis_foundation_risk TO fundermaps_portal;


--
-- Name: TABLE address; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.address TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.address TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.address TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.address TO fundermaps_batch;


--
-- Name: TABLE neighborhood; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.neighborhood TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.neighborhood TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.neighborhood TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.neighborhood TO fundermaps_batch;


--
-- Name: TABLE inquiry; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.inquiry TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.inquiry TO fundermaps_webservice;


--
-- Name: TABLE inquiry_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.inquiry_sample TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.inquiry_sample TO fundermaps_webservice;


--
-- Name: TABLE recovery_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.recovery_sample TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.recovery_sample TO fundermaps_webservice;


--
-- Name: TABLE analysis_address; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES ON TABLE data.analysis_address TO fundermaps_portal;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.analysis_address TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.analysis_address TO fundermaps_webservice;


--
-- Name: TABLE district; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.district TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.district TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.district TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.district TO fundermaps_batch;


--
-- Name: TABLE district_land; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.district_land TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.district_land TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.district_land TO fundermaps_portal;


--
-- Name: TABLE geographic_region; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.geographic_region TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.geographic_region TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.geographic_region TO fundermaps_portal;


--
-- Name: SEQUENCE geographic_region_id_seq; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,USAGE ON SEQUENCE data.geographic_region_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE data.geographic_region_id_seq TO fundermaps_webservice;


--
-- Name: TABLE groundwater_level; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.groundwater_level TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.groundwater_level TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.groundwater_level TO fundermaps_portal;


--
-- Name: TABLE neighborhood_land; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.neighborhood_land TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.neighborhood_land TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.neighborhood_land TO fundermaps_portal;


--
-- Name: TABLE statistics_product_buildings_restored; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_buildings_restored TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_buildings_restored TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.statistics_product_buildings_restored TO fundermaps_portal;


--
-- Name: TABLE statistics_product_construction_years; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_construction_years TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_construction_years TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.statistics_product_construction_years TO fundermaps_portal;


--
-- Name: TABLE statistics_product_data_collected; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_data_collected TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_data_collected TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.statistics_product_data_collected TO fundermaps_portal;


--
-- Name: TABLE statistics_product_foundation_risk; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_foundation_risk TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_foundation_risk TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.statistics_product_foundation_risk TO fundermaps_portal;


--
-- Name: TABLE statistics_product_foundation_type; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_foundation_type TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_foundation_type TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.statistics_product_foundation_type TO fundermaps_portal;


--
-- Name: TABLE incident; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.incident TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.incident TO fundermaps_webservice;
GRANT SELECT,INSERT,REFERENCES,TRIGGER,UPDATE ON TABLE report.incident TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE report.incident TO fundermaps_batch;


--
-- Name: TABLE statistics_product_incidents; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_incidents TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_incidents TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.statistics_product_incidents TO fundermaps_portal;


--
-- Name: TABLE statistics_product_inquiries; Type: ACL; Schema: data; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_inquiries TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE data.statistics_product_inquiries TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE data.statistics_product_inquiries TO fundermaps_portal;


--
-- Name: TABLE building_all; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_all TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_all TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_all TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_all TO fundermaps_batch;


--
-- Name: TABLE building_encoded_geom; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_encoded_geom TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_encoded_geom TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_encoded_geom TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_encoded_geom TO fundermaps_batch;


--
-- Name: TABLE building_inactive; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_inactive TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.building_inactive TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_inactive TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.building_inactive TO fundermaps_batch;


--
-- Name: TABLE country; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.country TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.country TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.country TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.country TO fundermaps_batch;


--
-- Name: TABLE municipality; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.municipality TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.municipality TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.municipality TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.municipality TO fundermaps_batch;


--
-- Name: TABLE state; Type: ACL; Schema: geocoder; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.state TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE geocoder.state TO fundermaps_webservice;
GRANT SELECT,REFERENCES ON TABLE geocoder.state TO fundermaps_portal;
GRANT SELECT,REFERENCES ON TABLE geocoder.state TO fundermaps_batch;


--
-- Name: TABLE building; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.building TO fundermaps_batch;


--
-- Name: TABLE building_built_year; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.building_built_year TO fundermaps_batch;


--
-- Name: TABLE building_height; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.building_height TO fundermaps_batch;


--
-- Name: TABLE building_hotspot; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.building_hotspot TO fundermaps_batch;


--
-- Name: TABLE bundle; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.bundle TO fundermaps_batch;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.bundle TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.bundle TO fundermaps_webservice;


--
-- Name: TABLE foundation_indicative; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.foundation_indicative TO fundermaps_batch;


--
-- Name: TABLE foundation_risk; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.foundation_risk TO fundermaps_batch;


--
-- Name: TABLE incident; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.incident TO fundermaps_batch;


--
-- Name: TABLE incident_aggregate_category; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.incident_aggregate_category TO fundermaps_batch;


--
-- Name: TABLE inquiry; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.inquiry TO fundermaps_batch;


--
-- Name: TABLE inquiry_sample_damage_cause; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.inquiry_sample_damage_cause TO fundermaps_batch;


--
-- Name: TABLE inquiry_sample_enforcement_term; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.inquiry_sample_enforcement_term TO fundermaps_batch;


--
-- Name: TABLE inquiry_sample_foundation_type; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.inquiry_sample_foundation_type TO fundermaps_batch;


--
-- Name: TABLE inquiry_sample_quality; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.inquiry_sample_quality TO fundermaps_batch;


--
-- Name: TABLE layer; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.layer TO fundermaps_batch;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.layer TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.layer TO fundermaps_webservice;


--
-- Name: TABLE recovery; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.recovery TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.recovery TO fundermaps_webservice;


--
-- Name: TABLE subsidence; Type: ACL; Schema: maplayer; Owner: fundermaps
--

GRANT SELECT,REFERENCES,TRIGGER ON TABLE maplayer.subsidence TO fundermaps_batch;


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


--
-- Name: TABLE project; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.project TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.project TO fundermaps_webservice;


--
-- Name: SEQUENCE project_id_seq; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON SEQUENCE report.project_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE report.project_id_seq TO fundermaps_webservice;


--
-- Name: TABLE project_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.project_sample TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.project_sample TO fundermaps_webservice;


--
-- Name: SEQUENCE project_sample_id_seq; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT ALL ON SEQUENCE report.project_sample_id_seq TO fundermaps_webapp;
GRANT SELECT,USAGE ON SEQUENCE report.project_sample_id_seq TO fundermaps_webservice;


--
-- Name: TABLE project_sample_inquiry_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.project_sample_inquiry_sample TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.project_sample_inquiry_sample TO fundermaps_webservice;


--
-- Name: TABLE project_sample_recovery_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.project_sample_recovery_sample TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.project_sample_recovery_sample TO fundermaps_webservice;


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
-- Name: TABLE recovery_sample_inquiry_sample; Type: ACL; Schema: report; Owner: fundermaps
--

GRANT SELECT,INSERT,REFERENCES,DELETE,TRIGGER,UPDATE ON TABLE report.recovery_sample_inquiry_sample TO fundermaps_webapp;
GRANT SELECT,REFERENCES,TRIGGER ON TABLE report.recovery_sample_inquiry_sample TO fundermaps_webservice;
