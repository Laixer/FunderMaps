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
-- Data for Name: organization; Type: TABLE DATA; Schema: application; Owner: fundermaps
--

COPY application.organization (id, name, normalized_name, email, phone_number, branding_logo, home_address, home_address_number, home_address_number_postfix, home_city, home_postbox, home_zipcode, fence, create_date, update_date, delete_date, normalized_email, access_public) FROM stdin;
097fc98b-a246-4bb1-81ed-d3c23e009090	FooBar B.V.	FOOBAR B.V.	info@foobar.com	\N	\N	\N	\N	\N	\N	\N	\N	\N	2021-03-12 11:41:27.960122+00	\N	\N	INFO@FOOBAR.COM	t
62af863e-2021-4438-a5ea-730ed3db9eda	FunderMaps B.V.	FUNDERMAPS B.V.	info@fundermaps.com	0645297791	\N	\N	\N	\N	\N	\N	\N	0106000020E6100000010000000103000000010000000500000084E85F2DE74711405BC1BE40B0FB494084E85F2DE74711409CDC9B322C044A40DDE8FC67B3A111409CDC9B322C044A40DDE8FC67B3A111405BC1BE40B0FB494084E85F2DE74711405BC1BE40B0FB4940	2021-03-12 10:09:07.664108+00	\N	\N	INFO@FUNDERMAPS.COM	t
05203318-6c55-43c1-a6a6-bb8c83f930c3	Contoso B.V.	CONTOSO B.V.	info@contoso.com	\N	\N	\N	\N	\N	\N	\N	\N	0106000020E6100000010000000103000000010000000500000084E85F2DE74711405BC1BE40B0FB494084E85F2DE74711409CDC9B322C044A40DDE8FC67B3A111409CDC9B322C044A40DDE8FC67B3A111405BC1BE40B0FB494084E85F2DE74711405BC1BE40B0FB4940	2021-03-12 11:14:46.785114+00	\N	\N	INFO@CONTOSO.COM	t
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: application; Owner: fundermaps
--

COPY application."user" (id, given_name, last_name, email, normalized_email, email_confirmed, avatar, job_title, password_hash, phone_number, two_factor_enabled, access_failed_count, role, last_login, login_count, phone_number_confirmed) FROM stdin;
eda54685-a2c1-4d0e-98d8-d63faccc8a9f	Administrator	\N	admin@fundermaps.com	ADMIN@FUNDERMAPS.COM	f	\N	Administrator	AcoX6S+JSGn42iAC48JcoKYmWDUjWpb9jgbY92QvK9jTpU3G2EvFZNiU3rMUgHQ16g==	\N	f	0	administrator	\N	0	f
ff1ee265-29aa-4eff-beb1-cf02609a909c	\N	\N	Javier40@yahoo.com	JAVIER40@YAHOO.COM	f	\N	\N	AcoX6S+JSGn42iAC48JcoKYmWDUjWpb9jgbY92QvK9jTpU3G2EvFZNiU3rMUgHQ16g==	\N	f	0	user	\N	0	f
21c403fe-45fc-4106-9551-3aada1bbdec3	\N	Kihn	Freda@contoso.com	FREDA@CONTOSO.COM	f	\N	soldier	AcoX6S+JSGn42iAC48JcoKYmWDUjWpb9jgbY92QvK9jTpU3G2EvFZNiU3rMUgHQ16g==	80186700424	f	0	user	\N	0	f
1a93cfb3-f097-4697-a998-71cdd9cfaead	Lester	Bednar	lester@contoso.com	LESTER@CONTOSO.COM	f	https://cloudflare-ipfs.com/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/373.jpg	actor	AcoX6S+JSGn42iAC48JcoKYmWDUjWpb9jgbY92QvK9jTpU3G2EvFZNiU3rMUgHQ16g==	55505763052	f	0	user	\N	0	f
aadc6b80-b447-443b-b4ed-fdfcb00976f2	Patsy	Brekke	patsy@contoso.com	PATSY@CONTOSO.COM	f	\N	decorator	AcoX6S+JSGn42iAC48JcoKYmWDUjWpb9jgbY92QvK9jTpU3G2EvFZNiU3rMUgHQ16g==	59539557702	f	0	user	\N	0	f
ab403d16-e428-4a75-9eec-3dd08b294988	\N	\N	corene@contoso.com	CORENE@CONTOSO.COM	f	\N	\N	AcoX6S+JSGn42iAC48JcoKYmWDUjWpb9jgbY92QvK9jTpU3G2EvFZNiU3rMUgHQ16g==	\N	f	0	user	\N	0	f
\.


--
-- Data for Name: attribution; Type: TABLE DATA; Schema: application; Owner: fundermaps
--

COPY application.attribution (id, reviewer, creator, owner, contractor) FROM stdin;
1	21c403fe-45fc-4106-9551-3aada1bbdec3	aadc6b80-b447-443b-b4ed-fdfcb00976f2	05203318-6c55-43c1-a6a6-bb8c83f930c3	62af863e-2021-4438-a5ea-730ed3db9eda
2	21c403fe-45fc-4106-9551-3aada1bbdec3	aadc6b80-b447-443b-b4ed-fdfcb00976f2	05203318-6c55-43c1-a6a6-bb8c83f930c3	62af863e-2021-4438-a5ea-730ed3db9eda
3	21c403fe-45fc-4106-9551-3aada1bbdec3	1a93cfb3-f097-4697-a998-71cdd9cfaead	05203318-6c55-43c1-a6a6-bb8c83f930c3	62af863e-2021-4438-a5ea-730ed3db9eda
4	21c403fe-45fc-4106-9551-3aada1bbdec3	1a93cfb3-f097-4697-a998-71cdd9cfaead	05203318-6c55-43c1-a6a6-bb8c83f930c3	62af863e-2021-4438-a5ea-730ed3db9eda
\.


--
-- Data for Name: contact; Type: TABLE DATA; Schema: application; Owner: fundermaps
--

COPY application.contact (email, name, phone_number) FROM stdin;
Brady_Gorczany17@gmail.com	Ed Kuphal	\N
Jany_Ritchie24@yahoo.com	Ryan Thompson	\N
Zetta81@hotmail.com	Gary Kohler	\N
\.


--
-- Data for Name: organization_proposal; Type: TABLE DATA; Schema: application; Owner: fundermaps
--

COPY application.organization_proposal (id, name, normalized_name, email, normalized_email) FROM stdin;
\.


--
-- Data for Name: organization_user; Type: TABLE DATA; Schema: application; Owner: fundermaps
--

COPY application.organization_user (user_id, organization_id, role) FROM stdin;
eda54685-a2c1-4d0e-98d8-d63faccc8a9f	62af863e-2021-4438-a5ea-730ed3db9eda	superuser
ff1ee265-29aa-4eff-beb1-cf02609a909c	05203318-6c55-43c1-a6a6-bb8c83f930c3	superuser
21c403fe-45fc-4106-9551-3aada1bbdec3	05203318-6c55-43c1-a6a6-bb8c83f930c3	verifier
aadc6b80-b447-443b-b4ed-fdfcb00976f2	05203318-6c55-43c1-a6a6-bb8c83f930c3	writer
1a93cfb3-f097-4697-a998-71cdd9cfaead	05203318-6c55-43c1-a6a6-bb8c83f930c3	reader
ab403d16-e428-4a75-9eec-3dd08b294988	05203318-6c55-43c1-a6a6-bb8c83f930c3	reader
\.


--
-- Data for Name: product_telemetry; Type: TABLE DATA; Schema: application; Owner: fundermaps
--

COPY application.product_telemetry (user_id, organization_id, product, count, meta) FROM stdin;
\.


--
-- Name: attribution_id_seq; Type: SEQUENCE SET; Schema: application; Owner: fundermaps
--

SELECT pg_catalog.setval('application.attribution_id_seq', 10000, false);


--
-- PostgreSQL database dump complete
--

