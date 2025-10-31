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