

--- PostgresSQL database start

--- Create the users table

CREATE TABLE USERS (
	ID SERIAL PRIMARY KEY,
	NAME TEXT NOT NULL,
	MAIL TEXT NOT NULL,
	SECRET TEXT NOT NULL,
	ROLE TEXT NOT NULL
);

-- Insert some pre-fabricated users

INSERT INTO USERS (NAME, SECRET, MAIL, ROLE) VALUES ('CLOUD', 'omnislash', 'cloud@mail.com', 'User');
INSERT INTO USERS (NAME, SECRET, MAIL, ROLE) VALUES ('TIFA', 'finalheaven', 'tifa@mail.com', 'User');
INSERT INTO USERS (NAME, SECRET, MAIL, ROLE) VALUES ('CID', 'highwind', 'cid@mail.com', 'User');

--- Create the transaction history table

CREATE TABLE TRN_HISTORY (
	ID SERIAL PRIMARY KEY,
	NAME TEXT NOT NULL,
	DATEAT DATE NOT NULL
);

