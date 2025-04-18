
--create database study_1C_db;
--drop database study_1C_db;

--drop table users_tasks;
--drop table blocks_tasks;
--drop table blocks_materials;
--drop table materials;
--drop table courses_blocks;
--drop table courses;
--drop table first_sign_in;
--drop table users;
--drop table study_states;
--drop table roles;
--drop table material_type;

create table roles(
    role_id serial constraint pk_role primary key,
    role_name text not null
);

create table material_type(
    type_id serial constraint pk_material_type primary key,
    type_name text not null
);

create table study_states(
    state_id serial constraint pk_state_type primary key,
    state_name text not null
);

create table users(
    user_id uuid constraint pk_user primary key constraint def_user_uuid default gen_random_uuid(),
    user_login text not null,
    user_hash_password text not null, 
    user_surname text not null,
    user_name text not null,
    user_patronymic text,
    user_data_create timestamptz not null constraint def_user_created default current_timestamp,
    user_role int not null,
    is_first boolean not null constraint def_user_is_first default true,
    constraint fk_user_role foreign key (user_role) references roles(role_id) on delete cascade
);

create table courses(
    course_id uuid constraint pk_course primary key constraint def_course_uuid default gen_random_uuid(),
    course_name text not null,
    course_data_create timestamptz not null constraint def_course_created default current_timestamp,
    description text,
    link text,
    author uuid not null,
    constraint fk_user_course foreign key (author) references users(user_id) on delete cascade
);

create table courses_blocks(
    block_id uuid constraint pk_block primary key constraint def_block_uuid default gen_random_uuid(),
    block_name text not null,
    block_date_created timestamptz not null constraint def_block_created default current_timestamp,
    course uuid not null,
    description text,
    block_number_of_course int, 
    constraint fk_courses_blocks foreign key (course) references courses(course_id) on delete cascade
);

create table materials(
    material_id uuid constraint pk_material primary key constraint def_material_uuid default gen_random_uuid(),
    material_name text not null,
    material_date_create timestamptz not null constraint def_material_created default current_timestamp,
    link text,
    type int not null,
    description text,
    constraint fk_materials_types foreign key (type) references material_type(type_id) on delete cascade
);

create table blocks_materials(
    bm_id uuid constraint pk_blocks_materials primary key constraint def_bm_uuid default gen_random_uuid(),
    block uuid not null,
    material uuid not null,
    bm_date_create timestamptz not null constraint def_bm_created default current_timestamp,
    note text,
    duration int,
    constraint fk_bm_blocks foreign key (block) references courses_blocks(block_id) on delete cascade,
    constraint fk_bm_materials foreign key (material) references materials(material_id) on delete cascade
);

create table blocks_tasks(
    task_id uuid constraint pk_task primary key constraint def_task_uuid default gen_random_uuid(),
    task_name text not null,
    task_date_created timestamptz not null constraint def_task_created default current_timestamp,
    duration int not null,
    block uuid not null,
    link text,
    task_number_of_block int not null,
    description text,
    constraint fk_bm_blocks foreign key (block) references courses_blocks(block_id) on delete cascade
);

create table tasks_practice (
    practice_id uuid constraint pk_practice primary key constraint def_practice_uuid default gen_random_uuid(),
    practice_name text not null,
    practice_date_created timestamptz not null constraint def_practice_created default current_timestamp,
    duration int not null,
    link text,
    task uuid not null,
    number_practice_of_task int,
    constraint fk_practice_task foreign key (task) references blocks_tasks(task_id) on delete cascade
);

create table users_tasks(
    ut_id uuid constraint pk_ut primary key constraint def_ut_uuid default gen_random_uuid(),
    auth_user uuid not null,
    task uuid,
    practice uuid,
    material uuid,
    status int not null,
    date_start timestamptz not null,
    duration_task int not null,
    duration_practice int not null,
    duration_material int not null,
    constraint fk_users_tasks_user foreign key (auth_user) references users(user_id) on delete cascade,
    constraint fk_users_tasks_task foreign key (task) references blocks_tasks(task_id) on delete cascade,
    constraint fk_users_tasks_material foreign key (material) references blocks_materials(bm_id) on delete cascade,
    constraint fk_users_tasks_practice foreign key (practice) references tasks_practice(practice_id) on delete cascade,
    constraint fk_users_tasks_status foreign key (status) references study_states(state_id) on delete cascade
);
