--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4 (Debian 17.4-1.pgdg120+2)
-- Dumped by pg_dump version 17.4 (Debian 17.4-1.pgdg120+2)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: public; Type: SCHEMA; Schema: -; Owner: admin
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO admin;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: admin
--

COMMENT ON SCHEMA public IS '';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: AspNetRoles; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."AspNetRoles" (
    "Id" uuid NOT NULL,
    "IsNoManipulate" boolean NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text
);


ALTER TABLE public."AspNetRoles" OWNER TO admin;

--
-- Name: AspNetUserRoles; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."AspNetUserRoles" (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL
);


ALTER TABLE public."AspNetUserRoles" OWNER TO admin;

--
-- Name: AspNetUsers; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."AspNetUsers" (
    "Id" uuid NOT NULL,
    "UserDataCreate" timestamp with time zone NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL
);


ALTER TABLE public."AspNetUsers" OWNER TO admin;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO admin;

--
-- Name: blocks_materials; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.blocks_materials (
    bm_id uuid DEFAULT gen_random_uuid() NOT NULL,
    block uuid NOT NULL,
    material uuid NOT NULL,
    bm_date_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    note text,
    duration integer
);


ALTER TABLE public.blocks_materials OWNER TO admin;

--
-- Name: blocks_tasks; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.blocks_tasks (
    task_id uuid DEFAULT gen_random_uuid() NOT NULL,
    task_name text NOT NULL,
    task_date_created timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    duration integer NOT NULL,
    block uuid NOT NULL,
    link text,
    task_number_of_block integer NOT NULL,
    description text
);


ALTER TABLE public.blocks_tasks OWNER TO admin;

--
-- Name: courses; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.courses (
    course_id uuid DEFAULT gen_random_uuid() NOT NULL,
    course_name text NOT NULL,
    course_data_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    description text,
    link text,
    author uuid NOT NULL
);


ALTER TABLE public.courses OWNER TO admin;

--
-- Name: courses_blocks; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.courses_blocks (
    block_id uuid DEFAULT gen_random_uuid() NOT NULL,
    block_name text NOT NULL,
    block_date_created timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    course uuid NOT NULL,
    description text,
    block_number_of_course integer
);


ALTER TABLE public.courses_blocks OWNER TO admin;

--
-- Name: material_type; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.material_type (
    type_id integer NOT NULL,
    type_name text NOT NULL
);


ALTER TABLE public.material_type OWNER TO admin;

--
-- Name: material_type_type_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

ALTER TABLE public.material_type ALTER COLUMN type_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.material_type_type_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: materials; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.materials (
    material_id uuid DEFAULT gen_random_uuid() NOT NULL,
    material_name text NOT NULL,
    material_date_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    link text,
    type integer NOT NULL,
    description text
);


ALTER TABLE public.materials OWNER TO admin;

--
-- Name: study_states; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.study_states (
    state_id integer NOT NULL,
    state_name text NOT NULL
);


ALTER TABLE public.study_states OWNER TO admin;

--
-- Name: study_states_state_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

ALTER TABLE public.study_states ALTER COLUMN state_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.study_states_state_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: tasks_practice; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.tasks_practice (
    practice_id uuid DEFAULT gen_random_uuid() NOT NULL,
    practice_name text NOT NULL,
    practice_date_created timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    duration integer NOT NULL,
    link text,
    task uuid NOT NULL,
    number_practice_of_task integer
);


ALTER TABLE public.tasks_practice OWNER TO admin;

--
-- Name: users; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users (
    user_id uuid DEFAULT gen_random_uuid() NOT NULL,
    user_surname text NOT NULL,
    user_name text NOT NULL,
    user_patronymic text,
    is_first boolean DEFAULT true NOT NULL
);


ALTER TABLE public.users OWNER TO admin;

--
-- Name: users_courses; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users_courses (
    cu_id uuid DEFAULT gen_random_uuid() NOT NULL,
    "CourseId" uuid NOT NULL,
    "UserId" uuid NOT NULL
);


ALTER TABLE public.users_courses OWNER TO admin;

--
-- Name: users_tasks; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users_tasks (
    ut_id uuid DEFAULT gen_random_uuid() NOT NULL,
    auth_user uuid NOT NULL,
    task uuid,
    practice uuid,
    material uuid,
    status integer NOT NULL,
    date_start timestamp with time zone NOT NULL,
    duration_task integer NOT NULL,
    duration_practice integer NOT NULL,
    duration_material integer NOT NULL
);


ALTER TABLE public.users_tasks OWNER TO admin;

--
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."AspNetRoles" ("Id", "IsNoManipulate", "Name", "NormalizedName", "ConcurrencyStamp") FROM stdin;
f47ac10b-58cc-4372-a567-0e02b2c3d479	t	Ученик	УЧЕНИК	f47ac10b-58cc-4372-a567-0e02b2c3d479
c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e	t	Куратор	КУРАТОР	c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e
f45d2396-3e72-4ec7-b892-7bd454248158	t	Администратор	АДМИНИСТРАТОР	f45d2396-3e72-4ec7-b892-7bd454248158
\.


--
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."AspNetUserRoles" ("UserId", "RoleId") FROM stdin;
33b58484-1bf5-4c42-ba72-2a53bbf67581	f45d2396-3e72-4ec7-b892-7bd454248158
0196125a-2c3e-7bc4-8dd4-5eeeff3ebc2b	c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e
019612aa-c914-7445-82d7-e451806895bb	f45d2396-3e72-4ec7-b892-7bd454248158
\.


--
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."AspNetUsers" ("Id", "UserDataCreate", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount") FROM stdin;
33b58484-1bf5-4c42-ba72-2a53bbf67581	2025-04-07 20:34:02.208429+00	admin	ADMIN	admin@admin.com	ADMIN@ADMIN.COM	t	AQAAAAIAAYagAAAAEDAflT7y8miLvkMRoQzUwPtEZdPNY4F6ovKJDKCWpOnEfbr8CDDrjFXVU/gkUawR5w==	CJQGS6WHCU7GUHDG7VQF56UU6YK2YL4W	4c125882-5b6f-44bd-b7ed-b7aceb40d048	\N	f	f	\N	t	0
0196125a-2c3e-7bc4-8dd4-5eeeff3ebc2b	2025-04-07 22:23:41.511527+00	curator@curator.com	CURATOR@CURATOR.COM	curator@curator.com	CURATOR@CURATOR.COM	t	AQAAAAIAAYagAAAAEP8gLY3ZDpJUkVmA9i24FRQJqYV2IMHE4trRtAqfrXTuIEcen53v8gC3TW/2/Mj8Mg==	3C2KGRQ3OCITZBJ4UEXFHBUTB334224T	60d4ef1d-ebf6-4ade-81e4-d76d7bfd9311	\N	f	f	\N	t	0
019612aa-c914-7445-82d7-e451806895bb	2025-04-07 23:51:44.545638+00	user@example.com	USER@EXAMPLE.COM	user@example.com	USER@EXAMPLE.COM	t	AQAAAAIAAYagAAAAEC7EMsSEFpAz+jwGrhx54/1ovW5n8ayWd7n6e6uTmxGL7ypIUb8uekL5vdDp1hfO5w==	ZSI2KJLOBM4G25472UPH46K3VSVYX4EW	4c79814b-cc5b-4676-b03e-243e1451caea	\N	f	f	\N	t	0
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250407200419_Init	9.0.2
\.


--
-- Data for Name: blocks_materials; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.blocks_materials (bm_id, block, material, bm_date_create, note, duration) FROM stdin;
d5707cd1-7f0c-4bbc-b427-72f9ab859785	da772850-eae8-4538-a31e-81a550bfae62	9bf8ed76-1bd7-4243-8ead-3be51e1b59a7	2025-04-08 09:48:07+00		480
db29812c-9445-411e-b473-f800f510426f	2e3eaa85-45c5-4457-bbcc-2aee466b901b	b96e5f8b-b37d-4d0c-8043-6479869593ec	2025-04-08 09:48:07+00		960
f0f1bdd3-cfb0-44e1-8808-d610528ee9ab	27c7f513-82d2-49ea-a494-b797bb15d7f5	68e6b362-e2ae-4c11-a021-dc2ddd99063f	2025-04-08 09:48:07+00		480
f87e99d7-9013-4578-a09a-dd7c86d58cce	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2	9f26e200-22d4-4da5-862c-35ae4b78acae	2025-04-08 09:48:07+00		1440
96f19bcd-8e63-4693-b3f9-fb5c5e8fac72	27c7f513-82d2-49ea-a494-b797bb15d7f5	e3ef10b9-6d4d-4506-827a-f4f984753349	2025-04-08 09:48:07+00		\N
2f01334e-0762-4b10-975d-c3454540852c	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2	fc514bba-adec-4030-aeb6-5b0312d53249	2025-04-08 09:48:07+00		\N
e768d4f7-f73d-4849-b0f5-22e148794e6e	2e3eaa85-45c5-4457-bbcc-2aee466b901b	c0360a37-3d6e-4d0b-8cd1-838540d6293b	2025-04-08 09:48:07+00		\N
c9e621e9-a4c2-48dd-a81e-b1bb96ef9f5d	2e3eaa85-45c5-4457-bbcc-2aee466b901b	c57fde5f-9b35-4fec-aa2f-f7baaa23fbc1	2025-04-08 09:48:07+00		\N
66d8c98e-46b9-4e1b-a7a5-f8a78833d9a9	27c7f513-82d2-49ea-a494-b797bb15d7f5	a2c833a9-9dc8-4e91-b93d-35d56010a5dc	2025-04-08 09:48:07+00	Курс кликни на меня	\N
b97cdd92-5d01-49bb-9c35-6b8cf1a43401	da772850-eae8-4538-a31e-81a550bfae62	eee6ec87-1dbd-47b5-8fc6-cf8c4d1554f7	2025-04-08 09:48:07+00	Курс кликни на меня; @mal.ru;Пароль: 2ztbtunT9O; (без седьмого блока) 	\N
7f112036-2af5-47fe-a727-ca9002ba8adc	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2	7f34a6d3-b773-4b7a-adb7-4f59e3768d98	2025-04-08 09:48:07+00	Курс кликни на меня; (кроме 16 - 18 глав).	\N
2ce91004-ac34-4923-aeb2-4509bbfc676a	da772850-eae8-4538-a31e-81a550bfae62	e3ef10b9-6d4d-4506-827a-f4f984753349	2025-04-08 09:48:07+00		\N
\.


--
-- Data for Name: blocks_tasks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.blocks_tasks (task_id, task_name, task_date_created, duration, block, link, task_number_of_block, description) FROM stdin;
93448e94-18e5-4b12-b605-cbc386645125	Начало работы с платформой «1С:Предприятие 8» (Бухгалтерию качать не нужно и 8.2 версию тоже) Только качать платформу 8.3	2025-04-08 09:48:07+00	50	27c7f513-82d2-49ea-a494-b797bb15d7f5		1	
0db07990-4d7c-43fa-b778-84238974aca1	Объекты справочной информации	2025-04-08 09:48:07+00	50	27c7f513-82d2-49ea-a494-b797bb15d7f5		2	
421a8a45-1cd3-4154-8ac6-7133c1cbf3a5	Документооборот торгового предприятия	2025-04-08 09:48:07+00	50	27c7f513-82d2-49ea-a494-b797bb15d7f5		3	
59a66a9a-a7b5-4912-83b9-0013151874c0	Регистры накопления. Проведение документов	2025-04-08 09:48:07+00	50	27c7f513-82d2-49ea-a494-b797bb15d7f5		4	
b30d8b54-0a34-4e63-b7bf-1d2639a6da2a	Виды регистров накопления, регистры сведений. Интерфейс приложения	2025-04-08 09:48:07+00	50	27c7f513-82d2-49ea-a494-b797bb15d7f5		5	
e0cbb555-0fec-44f5-872a-312e39bda062	Первая программа на платформе «1С:Предприятие 8»	2025-04-08 09:48:07+00	50	27c7f513-82d2-49ea-a494-b797bb15d7f5		6	
918f2811-6c00-41a4-b55e-f6995ccc0136	Программирование на встроенном языке «1С:Предприятие 8»	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		7	
abf6bd87-3cd9-43f3-8576-1da1d868efac	События, процедуры и функции	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		8	
da90e869-4ae3-4473-b31b-9570fbaf3a3b	Типы данных. События элементов форм	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		9	
9cb7bd99-4a37-4b7a-a6b9-dd768bc62b04	Чтение информации из базы данных, создание отчетов	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		10	
af2e5ede-e6fd-4c3b-9261-5d7a5b4fd297	Получение данных из регистров	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		11	
4a5f540e-4ec6-4681-8df4-57a82f062300	Контроль остатков и расчет себестоимости	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		12	
63dad647-bdee-4529-a025-2bbf3ea8eeae	Валовая прибыль. Создание сложных отчетов	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		13	
fb93abbe-f362-49f9-b6b6-ee2165aa190c	Основы бухгалтерского учета	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		14	
512dc4cc-6a6e-421c-97c7-009d40d2add5	Проведение документов по бух. Учету	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		15	
69d46aec-1f7a-45ce-a0f1-91d0235a4fc7	Отчеты по бухгалтерскому учету. Закрытие месяца	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		16	
d55d8cad-5434-443d-a60d-f3c1ea3b087f	Общие сведения о заработной плате. Создание объектов расчета	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		17	
1dc5b49e-a670-40a3-bb80-1e5651a7df2d	Начисление заработной платы	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		18	
54b3944e-ac15-4e9a-9afd-fe31c26e837c	Универсальные механизмы расчета. Отчеты	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		19	
5754fb1b-6baf-435c-a7db-2e6f9106f07c	Основы CRM-системы	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		20	
c4ffa710-b6d1-4e0c-b802-b3ebb93c0a2d	Воронка продаж. Бизнес-процессы и задачи	2025-04-08 09:48:07+00	60	27c7f513-82d2-49ea-a494-b797bb15d7f5		21	
258452c1-c6c6-4282-aff2-0437d79a1592	Изучаем язык 1С	2025-04-08 09:48:07+00	1035	da772850-eae8-4538-a31e-81a550bfae62		1	
79599e82-7427-498e-8833-58d63bd89b81	Пишем программы	2025-04-08 09:48:07+00	1320	da772850-eae8-4538-a31e-81a550bfae62		2	
d0bb53cf-2757-41bb-8216-cc12927684ee	Разрабатываем простейшие базы	2025-04-08 09:48:07+00	1200	da772850-eae8-4538-a31e-81a550bfae62		3	
b62eeb22-93c7-4f64-b5a0-8573c00f4dfe	Учимся извлекать данные для отчётов	2025-04-08 09:48:07+00	1140	da772850-eae8-4538-a31e-81a550bfae62		4	
b1c180db-f47c-4ac3-9310-722b82fb51ef	Извлекаем данные для отчётов из учебной базы	2025-04-08 09:48:07+00	900	da772850-eae8-4538-a31e-81a550bfae62		5	
4732c6f0-9f4f-47d0-a3fa-f34271a37f6a	Система компоновки данных для начинающих	2025-04-08 09:48:07+00	675	da772850-eae8-4538-a31e-81a550bfae62		6	
e744bfd1-a477-4d64-9918-9eb71b19bbb7	Установка учебной версии платформы «1С:Предприятие»	2025-04-08 09:48:07+00	15	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		0	
2ecc5355-3af3-47dc-8117-9dd029653c8d	Знакомство, создание информационной базы	2025-04-08 09:48:07+00	40	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		1	
9d286c35-5393-4f87-bae7-d7b319738916	Подсистемы	2025-04-08 09:48:07+00	45	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		2	
61945181-f423-4719-a46c-de362ab6897c	Справочники	2025-04-08 09:48:07+00	130	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		3	
76a8d2f3-e861-4928-b1c0-1f11686ca794	Документы	2025-04-08 09:48:07+00	90	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		4	
a0617f5f-ad36-4795-815c-0364b90b171d	Теоретическое	2025-04-08 09:48:07+00	120	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		5	
f5bd0fdd-7139-460e-b947-3f7c22cf8c44	Регистры накопления	2025-04-08 09:48:07+00	50	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		6	
518c677d-a1ce-4558-b21d-4568761f4c21	Простой отчет	2025-04-08 09:48:07+00	25	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		7	
853cf9dc-4b92-4557-8724-84abc9681ced	Макеты. Редактирование макетов	2025-04-08 09:48:07+00	40	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		8	
1710a303-266e-4956-9188-a5041552f54b	Периодические регистры сведений	2025-04-08 09:48:07+00	50	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		9	
1754bf35-15f8-4267-a5f3-91f335c3e096	Перечисления	2025-04-08 09:48:07+00	40	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		10	
002daf6a-95fe-4277-b3fb-2b1be46dbd1c	Проведение документа по нескольким регистрам	2025-04-08 09:48:07+00	80	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		11	
ef9d7994-0bcf-45de-9d4b-6af14e0ac239	Оборотные регистры накопления	2025-04-08 09:48:07+00	40	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		12	
db12ab50-b733-4b7b-97d9-35b899d5103d	Отчеты	2025-04-08 09:48:07+00	270	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		13	
081178f0-c277-4970-a34e-20e533d1c4e9	Оптимизация проведения документа "Оказание услуги"	2025-04-08 09:48:07+00	210	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		14	
de57881b-f393-4b3a-b001-1af0912b7009	План видов характеристик	2025-04-08 09:48:07+00	170	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		15	
e19c288a-2dc8-49a3-b573-4ec6d2c16d78	Поиск в базе данных	2025-04-08 09:48:07+00	40	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		16	
ca5a9986-679f-41d1-8dcc-10b0f25fa4ea	Выполнение заданий по расписанию	2025-04-08 09:48:07+00	35	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		17	
7551dbca-0730-4be4-a565-facbff699e28	Редактирование движений в форме документа	2025-04-08 09:48:07+00	40	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		18	
b8caab30-c40c-4e39-ab38-6555e6c69ddb	Список пользователей и их роли	2025-04-08 09:48:07+00	110	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		19	
54bf544d-b970-4bbf-8918-67ba235ba896	Начальная страница и настройка командного интерфейса	2025-04-08 09:48:07+00	70	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		20	
69fd2bff-b8ce-4fb4-a2af-e045d4d573cb	Обмен данными	2025-04-08 09:48:07+00	370	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		21	
783cf64c-5731-4f97-945c-7ecee08ddeb8	Функциональные опции	2025-04-08 09:48:07+00	30	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		22	
126d232a-58ae-409c-a97c-e482c0e2f4ff	Организация подбора, особенности разработки в режиме без использования модальности	2025-04-08 09:48:07+00	120	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		23	
f522c46b-eeff-474b-a7cc-200adee889b0	Приемы разработки форм	2025-04-08 09:48:07+00	130	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		24	
d7960f42-af8a-4550-a9da-c5340b70ab25	Приемы редактирования форм	2025-04-08 09:48:07+00	120	2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2		25	
983dfae0-8830-4a5b-a0ae-77b3ea1a205c	Начисление ЗП	2025-04-08 09:48:07+00	480	2e3eaa85-45c5-4457-bbcc-2aee466b901b	https//forms.yandex.ru/u/66eb1bd15056905685df13db/	1	
ab011db9-20fa-4b5e-9730-970bc4d8e7aa	на НДС	2025-04-08 09:48:07+00	960	2e3eaa85-45c5-4457-bbcc-2aee466b901b	https//forms.yandex.ru/u/66ec043b84227c0ab0bcf1a1/	2	
904b294a-0a59-4558-88be-b52040b3fdfe	Печатная форма	2025-04-08 09:48:07+00	960	2e3eaa85-45c5-4457-bbcc-2aee466b901b	https//forms.yandex.ru/u/66ec0456f47e730af62ecbe3/	3	
3a664c3e-26b8-4a17-bbb6-5cc7b0fda942	Контроль остатков	2025-04-08 09:48:07+00	1920	2e3eaa85-45c5-4457-bbcc-2aee466b901b	https//forms.yandex.ru/u/66ec04744936390a92d0e80b/	4	
7b2b973a-3a9c-4139-931b-ebcee336a62b	Задачка спеца	2025-04-08 09:48:07+00	1440	2e3eaa85-45c5-4457-bbcc-2aee466b901b	https//forms.yandex.ru/u/66ec04965056900b08f65c99/	5	
\.


--
-- Data for Name: courses; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.courses (course_id, course_name, course_data_create, description, link, author) FROM stdin;
fc770add-779d-4be5-bcd7-8a51d706f6cc	3 Месяца	2025-04-08 09:48:07+00			33b58484-1bf5-4c42-ba72-2a53bbf67581
\.


--
-- Data for Name: courses_blocks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.courses_blocks (block_id, block_name, block_date_created, course, description, block_number_of_course) FROM stdin;
27c7f513-82d2-49ea-a494-b797bb15d7f5	Основы программирования	2025-04-08 09:48:07+00	fc770add-779d-4be5-bcd7-8a51d706f6cc	Изучить в течение первой недели + начать учить сертификацию 	1
da772850-eae8-4538-a31e-81a550bfae62	Практика	2025-04-08 09:48:07+00	fc770add-779d-4be5-bcd7-8a51d706f6cc	Изучить до конца первого месяца 	2
2acc63f9-a297-4f90-a0b8-2ffddbdfb3f2	Основы разработки конфигураций	2025-04-08 09:48:07+00	fc770add-779d-4be5-bcd7-8a51d706f6cc	Изучить и выполнить до конца месяца + сдать сертификацию 	3
2e3eaa85-45c5-4457-bbcc-2aee466b901b	Практика	2025-04-08 09:48:07+00	fc770add-779d-4be5-bcd7-8a51d706f6cc	Выполнить до конца месяца + сертификация (Сдать обязательно)	4
\.


--
-- Data for Name: material_type; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.material_type (type_id, type_name) FROM stdin;
1	Теория
2	Практика
3	Профессиональная подготовка
\.


--
-- Data for Name: materials; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.materials (material_id, material_name, material_date_create, link, type, description) FROM stdin;
9bf8ed76-1bd7-4243-8ead-3be51e1b59a7	Книга 101 совет начинающим разработчикам в системе "1С:Предприятие 8"	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/xJR4Uq74xbowNQ	1	
b96e5f8b-b37d-4d0c-8043-6479869593ec	Язык запросов "1С:Предприятие 8"  E.Ю Хрусталёва	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/P19Rll4HU0MpcQ	1	
68e6b362-e2ae-4c11-a021-dc2ddd99063f	Книга Радченко "1С:Программирование для начинающих Разработка в системе 1С:П 8.3"	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/Y5t__LB6BFXs8Q	1	
9f26e200-22d4-4da5-862c-35ae4b78acae	Система компоновки данных "1С:Предприятие 8" E.Ю Хрусталёва	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/PX4FR1DuVI98_A	1	
e3ef10b9-6d4d-4506-827a-f4f984753349	Начинайте учить профку тут 	2025-04-08 09:48:07+00	http://edu.koderline-soft.ru/demand/view/?token=f1e33c87bb62f21ae5c3cb1345f0600f&eauth=012eb621e28ab71aa091f4459156ae65	3	
fc514bba-adec-4030-aeb6-5b0312d53249	Не забывай про профку :)	2025-04-08 09:48:07+00	http://edu.koderline-soft.ru/demand/view/?token=f1e33c87bb62f21ae5c3cb1345f0600f&eauth=012eb621e28ab71aa091f4459156ae65	3	
c0360a37-3d6e-4d0b-8cd1-838540d6293b	Учи профку - последний месяц :0	2025-04-08 09:48:07+00	http://edu.koderline-soft.ru/demand/view/?token=f1e33c87bb62f21ae5c3cb1345f0600f&eauth=012eb621e28ab71aa091f4459156ae65	3	
c57fde5f-9b35-4fec-aa2f-f7baaa23fbc1	Выполнение учебных задач по разработанной конфигурации по книге "Радченко".	2025-04-08 09:48:07+00		2	
a2c833a9-9dc8-4e91-b93d-35d56010a5dc	Программирование за 21 день (курсы-по-1с.рф) 20 часов вместе с дз	2025-04-08 09:48:07+00	https://курсы-по-1с.рф/free/programming-in-1c-in-21-days/final-all-in-one/	2	
eee6ec87-1dbd-47b5-8fc6-cf8c4d1554f7	Онлайн-школа 1С программирования Евгения Милькина (без седьмого блока) 	2025-04-08 09:48:07+00	https://helpme1s.ru/shkola-programmirovaniya-v-1s	2	
7f34a6d3-b773-4b7a-adb7-4f59e3768d98	Книга Радченко "Практическое пособие разработчика" (кроме 16 - 18 глав).	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/zOMO1RilllscAA	2	
\.


--
-- Data for Name: study_states; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.study_states (state_id, state_name) FROM stdin;
1	Не приступал к изучению
2	Начал изучать
3	Изучил
\.


--
-- Data for Name: tasks_practice; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.tasks_practice (practice_id, practice_name, practice_date_created, duration, link, task, number_practice_of_task) FROM stdin;
45791282-2fc5-4a74-8de0-9268954e126c	Задание 0	2025-04-08 09:48:07+00	5		e744bfd1-a477-4d64-9918-9eb71b19bbb7	0
69162b1e-3e93-4521-9858-e2e7d4b5316b	Задание 1	2025-04-08 09:48:07+00	20		2ecc5355-3af3-47dc-8117-9dd029653c8d	1
5e6f8a3d-ef61-494c-98f5-df77cf7a606e	Задание 2	2025-04-08 09:48:07+00	25		9d286c35-5393-4f87-bae7-d7b319738916	2
1b0c9f62-53ea-4f2f-add8-4c36ba79b08a	Задание 3	2025-04-08 09:48:07+00	120		61945181-f423-4719-a46c-de362ab6897c	3
765b5a00-d1b8-425d-95e2-59e44601c96f	Задание 4	2025-04-08 09:48:07+00	60		76a8d2f3-e861-4928-b1c0-1f11686ca794	4
cd4f0a45-8e62-4eae-81c2-269c13effd7b	Задание 5	2025-04-08 09:48:07+00	5		a0617f5f-ad36-4795-815c-0364b90b171d	5
b70c18d2-64c4-4cc7-ae71-5c911aa2f984	Задание 6	2025-04-08 09:48:07+00	25		f5bd0fdd-7139-460e-b947-3f7c22cf8c44	6
1e3fd68b-788b-42c7-9a57-211b53d5fa42	Задание 7	2025-04-08 09:48:07+00	15		518c677d-a1ce-4558-b21d-4568761f4c21	7
77ca02ae-8a6f-4b6f-9f59-fdfe45377156	Задание 8	2025-04-08 09:48:07+00	25		853cf9dc-4b92-4557-8724-84abc9681ced	8
7557b270-a173-4f0d-8e8d-c8d75017dd78	Задание 9	2025-04-08 09:48:07+00	25		1710a303-266e-4956-9188-a5041552f54b	9
b4834121-b9f1-426a-8dd8-d8995743fa42	Задание 10	2025-04-08 09:48:07+00	25		1754bf35-15f8-4267-a5f3-91f335c3e096	10
889da2dc-b3e4-486e-baf3-f4a57ec2822b	Задание 11	2025-04-08 09:48:07+00	40		002daf6a-95fe-4277-b3fb-2b1be46dbd1c	11
be2b5ea7-d25e-43ce-8b4a-7e6ab103e7af	Задание 12	2025-04-08 09:48:07+00	25		ef9d7994-0bcf-45de-9d4b-6af14e0ac239	12
7a53ad5c-ba37-487f-babc-8a2605923fff	Задание 13	2025-04-08 09:48:07+00	90		db12ab50-b733-4b7b-97d9-35b899d5103d	13
a066f230-d04b-444f-8a3d-d60c8a643638	Задание 14	2025-04-08 09:48:07+00	90		081178f0-c277-4970-a34e-20e533d1c4e9	14
29eb5c61-0204-4c3d-978c-01ca617ff1c9	Задание 15	2025-04-08 09:48:07+00	5		de57881b-f393-4b3a-b001-1af0912b7009	15
7a15c36a-0d74-4cd4-81c0-a95cb156e381	Задание 16	2025-04-08 09:48:07+00	5		e19c288a-2dc8-49a3-b573-4ec6d2c16d78	16
bda1e257-07cb-4e2b-bc36-6f98a214bcd4	Задание 17	2025-04-08 09:48:07+00	90		ca5a9986-679f-41d1-8dcc-10b0f25fa4ea	17
acd0c119-007b-41f6-87fe-3a1ea70a61f2	Задание 18	2025-04-08 09:48:07+00	25		7551dbca-0730-4be4-a565-facbff699e28	18
4fad4e5d-cc74-4344-bbc3-7a8773dc4633	Задание 19	2025-04-08 09:48:07+00	120		b8caab30-c40c-4e39-ab38-6555e6c69ddb	19
318529b1-5a46-4a64-b4eb-32d2e39a622c	Задание 20	2025-04-08 09:48:07+00	50		54bf544d-b970-4bbf-8918-67ba235ba896	20
2a0b7dfd-b90e-4170-ad5f-5e5d133b43c0	Задание 21	2025-04-08 09:48:07+00	180		69fd2bff-b8ce-4fb4-a2af-e045d4d573cb	21
dc0ecb5c-993b-4ab1-b81e-9441b53b0790	Задание 22	2025-04-08 09:48:07+00	25		783cf64c-5731-4f97-945c-7ecee08ddeb8	22
f68a5009-b8e9-4e00-80d5-53ce3b10409c	Задание 23	2025-04-08 09:48:07+00	5		126d232a-58ae-409c-a97c-e482c0e2f4ff	23
3bdd3630-0007-44d6-9018-93bff4867de8	Задание 24	2025-04-08 09:48:07+00	5		f522c46b-eeff-474b-a7cc-200adee889b0	24
cd73caac-7c03-4f6b-9195-504ec2c0769c	Задание 25	2025-04-08 09:48:07+00	5		d7960f42-af8a-4550-a9da-c5340b70ab25	25
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users (user_id, user_surname, user_name, user_patronymic, is_first) FROM stdin;
33b58484-1bf5-4c42-ba72-2a53bbf67581	admin	admin	\N	f
0196125a-2c3e-7bc4-8dd4-5eeeff3ebc2b	Куратор	Куратор	Куратор	f
019612aa-c914-7445-82d7-e451806895bb	user	user		f
\.


--
-- Data for Name: users_courses; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users_courses (cu_id, "CourseId", "UserId") FROM stdin;
\.


--
-- Data for Name: users_tasks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users_tasks (ut_id, auth_user, task, practice, material, status, date_start, duration_task, duration_practice, duration_material) FROM stdin;
\.


--
-- Name: material_type_type_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.material_type_type_id_seq', 3, true);


--
-- Name: study_states_state_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.study_states_state_id_seq', 3, true);


--
-- Name: AspNetRoles PK_AspNetRoles; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");


--
-- Name: AspNetUserRoles PK_AspNetUserRoles; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- Name: AspNetUsers PK_AspNetUsers; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: courses_blocks pk_block; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses_blocks
    ADD CONSTRAINT pk_block PRIMARY KEY (block_id);


--
-- Name: blocks_materials pk_blocks_materials; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT pk_blocks_materials PRIMARY KEY (bm_id);


--
-- Name: courses pk_course; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses
    ADD CONSTRAINT pk_course PRIMARY KEY (course_id);


--
-- Name: materials pk_material; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.materials
    ADD CONSTRAINT pk_material PRIMARY KEY (material_id);


--
-- Name: material_type pk_material_type; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.material_type
    ADD CONSTRAINT pk_material_type PRIMARY KEY (type_id);


--
-- Name: tasks_practice pk_practice; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.tasks_practice
    ADD CONSTRAINT pk_practice PRIMARY KEY (practice_id);


--
-- Name: study_states pk_state_type; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.study_states
    ADD CONSTRAINT pk_state_type PRIMARY KEY (state_id);


--
-- Name: blocks_tasks pk_task; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_tasks
    ADD CONSTRAINT pk_task PRIMARY KEY (task_id);


--
-- Name: users pk_user; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT pk_user PRIMARY KEY (user_id);


--
-- Name: users_courses pk_users_courses; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_courses
    ADD CONSTRAINT pk_users_courses PRIMARY KEY (cu_id);


--
-- Name: users_tasks pk_ut; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT pk_ut PRIMARY KEY (ut_id);


--
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");


--
-- Name: IX_AspNetUserRoles_RoleId; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");


--
-- Name: IX_blocks_materials_block; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_blocks_materials_block" ON public.blocks_materials USING btree (block);


--
-- Name: IX_blocks_materials_material; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_blocks_materials_material" ON public.blocks_materials USING btree (material);


--
-- Name: IX_blocks_tasks_block; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_blocks_tasks_block" ON public.blocks_tasks USING btree (block);


--
-- Name: IX_courses_author; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_courses_author" ON public.courses USING btree (author);


--
-- Name: IX_courses_blocks_course; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_courses_blocks_course" ON public.courses_blocks USING btree (course);


--
-- Name: IX_materials_type; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_materials_type" ON public.materials USING btree (type);


--
-- Name: IX_tasks_practice_task; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_tasks_practice_task" ON public.tasks_practice USING btree (task);


--
-- Name: IX_users_courses_CourseId; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_courses_CourseId" ON public.users_courses USING btree ("CourseId");


--
-- Name: IX_users_courses_UserId; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_courses_UserId" ON public.users_courses USING btree ("UserId");


--
-- Name: IX_users_tasks_auth_user; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_auth_user" ON public.users_tasks USING btree (auth_user);


--
-- Name: IX_users_tasks_material; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_material" ON public.users_tasks USING btree (material);


--
-- Name: IX_users_tasks_practice; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_practice" ON public.users_tasks USING btree (practice);


--
-- Name: IX_users_tasks_status; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_status" ON public.users_tasks USING btree (status);


--
-- Name: IX_users_tasks_task; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_task" ON public.users_tasks USING btree (task);


--
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: admin
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");


--
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: admin
--

CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: users FK_users_AspNetUsers_user_id; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT "FK_users_AspNetUsers_user_id" FOREIGN KEY (user_id) REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: blocks_materials fk_bm_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT fk_bm_blocks FOREIGN KEY (block) REFERENCES public.courses_blocks(block_id) ON DELETE CASCADE;


--
-- Name: blocks_tasks fk_bm_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_tasks
    ADD CONSTRAINT fk_bm_blocks FOREIGN KEY (block) REFERENCES public.courses_blocks(block_id) ON DELETE CASCADE;


--
-- Name: blocks_materials fk_bm_materials; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT fk_bm_materials FOREIGN KEY (material) REFERENCES public.materials(material_id) ON DELETE CASCADE;


--
-- Name: courses_blocks fk_courses_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses_blocks
    ADD CONSTRAINT fk_courses_blocks FOREIGN KEY (course) REFERENCES public.courses(course_id) ON DELETE CASCADE;


--
-- Name: users_courses fk_cu_courses; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_courses
    ADD CONSTRAINT fk_cu_courses FOREIGN KEY ("CourseId") REFERENCES public.courses(course_id) ON DELETE CASCADE;


--
-- Name: users_courses fk_cu_users; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_courses
    ADD CONSTRAINT fk_cu_users FOREIGN KEY ("UserId") REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- Name: materials fk_materials_types; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.materials
    ADD CONSTRAINT fk_materials_types FOREIGN KEY (type) REFERENCES public.material_type(type_id) ON DELETE CASCADE;


--
-- Name: tasks_practice fk_practice_task; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.tasks_practice
    ADD CONSTRAINT fk_practice_task FOREIGN KEY (task) REFERENCES public.blocks_tasks(task_id) ON DELETE CASCADE;


--
-- Name: courses fk_user_course; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses
    ADD CONSTRAINT fk_user_course FOREIGN KEY (author) REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_material; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_material FOREIGN KEY (material) REFERENCES public.blocks_materials(bm_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_practice; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_practice FOREIGN KEY (practice) REFERENCES public.tasks_practice(practice_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_status; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_status FOREIGN KEY (status) REFERENCES public.study_states(state_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_task; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_task FOREIGN KEY (task) REFERENCES public.blocks_tasks(task_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_user; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_user FOREIGN KEY (auth_user) REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: admin
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

