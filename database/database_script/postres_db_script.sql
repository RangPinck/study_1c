
--create database study_1C_db;
--drop database study_1C_db;

--drop table users;
--drop table roles;

create table roles(
    role_id serial constraint pk_role primary key,
    role_name text
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
    constraint fk_user_role foreign key (user_role) references roles(role_id)
);
