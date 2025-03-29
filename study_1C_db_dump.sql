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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: blocks_materials; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.blocks_materials (
    bm_id uuid DEFAULT gen_random_uuid() NOT NULL,
    bm_date_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    note text,
    block integer NOT NULL,
    material uuid NOT NULL
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
    link_for_save_progress text,
    block integer NOT NULL
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
    block_id integer NOT NULL,
    block_name text NOT NULL,
    block_date_created timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    description text,
    course uuid NOT NULL
);


ALTER TABLE public.courses_blocks OWNER TO admin;

--
-- Name: courses_blocks_block_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.courses_blocks_block_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.courses_blocks_block_id_seq OWNER TO admin;

--
-- Name: courses_blocks_block_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.courses_blocks_block_id_seq OWNED BY public.courses_blocks.block_id;


--
-- Name: first_sign_in; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.first_sign_in (
    sign_in_id uuid DEFAULT gen_random_uuid() NOT NULL,
    auth_user uuid NOT NULL,
    is_first boolean NOT NULL
);


ALTER TABLE public.first_sign_in OWNER TO admin;

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

CREATE SEQUENCE public.material_type_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.material_type_type_id_seq OWNER TO admin;

--
-- Name: material_type_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.material_type_type_id_seq OWNED BY public.material_type.type_id;


--
-- Name: materials; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.materials (
    material_id uuid DEFAULT gen_random_uuid() NOT NULL,
    material_name text NOT NULL,
    material_date_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    link text,
    type integer NOT NULL
);


ALTER TABLE public.materials OWNER TO admin;

--
-- Name: roles; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.roles (
    role_id integer NOT NULL,
    role_name text NOT NULL
);


ALTER TABLE public.roles OWNER TO admin;

--
-- Name: roles_role_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.roles_role_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.roles_role_id_seq OWNER TO admin;

--
-- Name: roles_role_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.roles_role_id_seq OWNED BY public.roles.role_id;


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

CREATE SEQUENCE public.study_states_state_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.study_states_state_id_seq OWNER TO admin;

--
-- Name: study_states_state_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.study_states_state_id_seq OWNED BY public.study_states.state_id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users (
    user_id uuid DEFAULT gen_random_uuid() NOT NULL,
    user_login text NOT NULL,
    user_hash_password text NOT NULL,
    user_surname text NOT NULL,
    user_name text NOT NULL,
    user_patronymic text,
    user_data_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    user_role integer NOT NULL
);


ALTER TABLE public.users OWNER TO admin;

--
-- Name: users_tasks; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users_tasks (
    ut_id uuid DEFAULT gen_random_uuid() NOT NULL,
    auth_user uuid NOT NULL,
    task uuid NOT NULL,
    status integer NOT NULL,
    date_start timestamp with time zone NOT NULL,
    duration integer NOT NULL
);


ALTER TABLE public.users_tasks OWNER TO admin;

--
-- Name: courses_blocks block_id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses_blocks ALTER COLUMN block_id SET DEFAULT nextval('public.courses_blocks_block_id_seq'::regclass);


--
-- Name: material_type type_id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.material_type ALTER COLUMN type_id SET DEFAULT nextval('public.material_type_type_id_seq'::regclass);


--
-- Name: roles role_id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.roles ALTER COLUMN role_id SET DEFAULT nextval('public.roles_role_id_seq'::regclass);


--
-- Name: study_states state_id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.study_states ALTER COLUMN state_id SET DEFAULT nextval('public.study_states_state_id_seq'::regclass);


--
-- Data for Name: blocks_materials; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.blocks_materials (bm_id, bm_date_create, note, block, material) FROM stdin;
\.


--
-- Data for Name: blocks_tasks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.blocks_tasks (task_id, task_name, task_date_created, duration, link_for_save_progress, block) FROM stdin;
\.


--
-- Data for Name: courses; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.courses (course_id, course_name, course_data_create, description, link, author) FROM stdin;
\.


--
-- Data for Name: courses_blocks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.courses_blocks (block_id, block_name, block_date_created, description, course) FROM stdin;
\.


--
-- Data for Name: first_sign_in; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.first_sign_in (sign_in_id, auth_user, is_first) FROM stdin;
\.


--
-- Data for Name: material_type; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.material_type (type_id, type_name) FROM stdin;
\.


--
-- Data for Name: materials; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.materials (material_id, material_name, material_date_create, link, type) FROM stdin;
\.


--
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.roles (role_id, role_name) FROM stdin;
\.


--
-- Data for Name: study_states; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.study_states (state_id, state_name) FROM stdin;
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users (user_id, user_login, user_hash_password, user_surname, user_name, user_patronymic, user_data_create, user_role) FROM stdin;
\.


--
-- Data for Name: users_tasks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users_tasks (ut_id, auth_user, task, status, date_start, duration) FROM stdin;
\.


--
-- Name: courses_blocks_block_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.courses_blocks_block_id_seq', 1, false);


--
-- Name: material_type_type_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.material_type_type_id_seq', 1, false);


--
-- Name: roles_role_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.roles_role_id_seq', 1, false);


--
-- Name: study_states_state_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.study_states_state_id_seq', 1, false);


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
-- Name: first_sign_in pk_first_sign_in; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.first_sign_in
    ADD CONSTRAINT pk_first_sign_in PRIMARY KEY (sign_in_id);


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
-- Name: roles pk_role; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT pk_role PRIMARY KEY (role_id);


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
-- Name: users_tasks pk_ut; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT pk_ut PRIMARY KEY (ut_id);


--
-- Name: blocks_materials fk_bm_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT fk_bm_blocks FOREIGN KEY (block) REFERENCES public.courses_blocks(block_id);


--
-- Name: blocks_tasks fk_bm_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_tasks
    ADD CONSTRAINT fk_bm_blocks FOREIGN KEY (block) REFERENCES public.courses_blocks(block_id);


--
-- Name: blocks_materials fk_bm_materials; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT fk_bm_materials FOREIGN KEY (material) REFERENCES public.materials(material_id);


--
-- Name: courses_blocks fk_courses_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses_blocks
    ADD CONSTRAINT fk_courses_blocks FOREIGN KEY (course) REFERENCES public.courses(course_id);


--
-- Name: materials fk_materials_types; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.materials
    ADD CONSTRAINT fk_materials_types FOREIGN KEY (type) REFERENCES public.material_type(type_id);


--
-- Name: courses fk_user_course; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses
    ADD CONSTRAINT fk_user_course FOREIGN KEY (author) REFERENCES public.users(user_id);


--
-- Name: first_sign_in fk_user_first_sign_in; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.first_sign_in
    ADD CONSTRAINT fk_user_first_sign_in FOREIGN KEY (auth_user) REFERENCES public.users(user_id);


--
-- Name: users fk_user_role; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT fk_user_role FOREIGN KEY (user_role) REFERENCES public.roles(role_id);


--
-- Name: users_tasks fk_users_tasks_status; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_status FOREIGN KEY (status) REFERENCES public.study_states(state_id);


--
-- Name: users_tasks fk_users_tasks_task; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_task FOREIGN KEY (task) REFERENCES public.blocks_tasks(task_id);


--
-- Name: users_tasks fk_users_tasks_user; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_user FOREIGN KEY (auth_user) REFERENCES public.users(user_id);


--
-- PostgreSQL database dump complete
--

