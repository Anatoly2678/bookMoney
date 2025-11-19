Scaffold-DbContext "Host=localhost;Port=5432;Database=bookmoney;Username=postgres;Password=123" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir ModelsTemp -ContextDir Data -Context AppDbContextTemp -DataAnnotations -Force

-- client.login определение

-- Drop table

DROP TABLE IF EXISTS client.login;

CREATE TABLE client.login (
	id uuid DEFAULT gen_random_uuid() NOT NULL,
	login varchar NOT NULL,
	"password" varchar NULL,
	is_active bool DEFAULT false NOT NULL,
	date_create timestamp DEFAULT CURRENT_TIMESTAMP NOT NULL,
	CONSTRAINT login_pk PRIMARY KEY (id),
	CONSTRAINT login_unique UNIQUE (login)
);

-- client.confirm_sms определение

-- Drop table

DROP TABLE IF EXISTS client.confirm_sms;

CREATE TABLE client.confirm_sms (
	id uuid DEFAULT gen_random_uuid() NOT NULL,
	sms_code varchar NOT NULL,
	date_create timestamp DEFAULT CURRENT_TIMESTAMP NOT NULL,
	date_confirm timestamp NULL,
	login_id uuid NOT NULL,
	CONSTRAINT confirm_sms_pk PRIMARY KEY (id)
);

-- client.confirm_sms внешние включи

ALTER TABLE client.confirm_sms ADD CONSTRAINT confirm_sms_login_fk FOREIGN KEY (login_id) REFERENCES client.login(id);
ALTER TABLE client.confirm_sms ADD CONSTRAINT uk2 FOREIGN KEY (login_id) REFERENCES client.login(id);


--------------------------
-- client.info определение

-- Drop table

-- DROP TABLE client.info;

CREATE TABLE client.info (
	id uuid DEFAULT gen_random_uuid() NOT NULL, -- UUID идентификатор клиента
	last_name varchar(100) NOT NULL, -- Фамилия клиента
	first_name varchar(100) NOT NULL, -- Имя клиента
	middle_name varchar(100) NOT NULL, -- Отчество клиента
	birth_date date NOT NULL,
	email varchar(255) NOT NULL,
	password_hash varchar(255) NOT NULL,
	photo_page_image bytea NULL,
	selfie_with_passport_image bytea NULL,
	registration_page_image bytea NULL,
	passport_series bpchar(4) NOT NULL,
	passport_number bpchar(6) NOT NULL,
	issued_by text NOT NULL,
	issue_date date NOT NULL,
	department_code varchar(10) NOT NULL,
	birth_place text NOT NULL,
	snils varchar(14) NOT NULL,
	agreement_accepted bool DEFAULT false NOT NULL,
	email_verified bool DEFAULT false NOT NULL,
	status varchar(20) DEFAULT 'pending'::character varying NOT NULL,
	created_at timestamptz DEFAULT CURRENT_TIMESTAMP NOT NULL,
	updated_at timestamptz NOT NULL,
	verified_at timestamptz NULL,
	login_id uuid NOT NULL,
	CONSTRAINT chk_birth_date_reasonable CHECK (((birth_date > '1900-01-01'::date) AND (birth_date < (CURRENT_DATE - '18 years'::interval)))),
	CONSTRAINT chk_email_format CHECK (((email)::text ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'::text)),
	CONSTRAINT chk_issue_date_reasonable CHECK (((issue_date > '1990-01-01'::date) AND (issue_date <= CURRENT_DATE))),
	CONSTRAINT chk_passport_number_format CHECK ((passport_number ~ '^[0-9]{6}$'::text)),
	CONSTRAINT chk_passport_series_format CHECK ((passport_series ~ '^[0-9]{4}$'::text)),
	CONSTRAINT chk_snils_format CHECK ((((snils)::text ~ '^[0-9]{3}-[0-9]{3}-[0-9]{3} [0-9]{2}$'::text) OR ((snils)::text ~ '^[0-9]{11}$'::text))),
	CONSTRAINT info_email_key UNIQUE (email),
	CONSTRAINT info_pkey PRIMARY KEY (id),
	CONSTRAINT info_status_check CHECK (((status)::text = ANY ((ARRAY['pending'::character varying, 'verified'::character varying, 'rejected'::character varying, 'active'::character varying])::text[]))),
	CONSTRAINT unique_passport UNIQUE (passport_series, passport_number)
);
CREATE INDEX idx_clients_created_at ON client.info USING btree (created_at);
CREATE INDEX idx_clients_email ON client.info USING btree (email);
CREATE INDEX idx_clients_names ON client.info USING btree (last_name, first_name, middle_name);
CREATE INDEX idx_clients_snils ON client.info USING btree (snils);
CREATE INDEX idx_clients_status ON client.info USING btree (status);
COMMENT ON TABLE client.info IS 'Таблица для хранения информации о клиентах';

-- Column comments

COMMENT ON COLUMN client.info.id IS 'UUID идентификатор клиента';
COMMENT ON COLUMN client.info.last_name IS 'Фамилия клиента';
COMMENT ON COLUMN client.info.first_name IS 'Имя клиента';
COMMENT ON COLUMN client.info.middle_name IS 'Отчество клиента';


-- client.info внешние включи

ALTER TABLE client.info ADD CONSTRAINT info_login_fk FOREIGN KEY (login_id) REFERENCES client.login(id);