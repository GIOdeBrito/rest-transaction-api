
--- PostgresSQL database initialization script

--- Create the users table

CREATE TABLE USERS (
	ID SERIAL PRIMARY KEY,
	NAME TEXT NOT NULL,
	FULLNAME TEXT NOT NULL,
	MAIL TEXT NOT NULL,
	SECRET TEXT NOT NULL,
	CREATEDAT DATE NOT NULL,
	ROLE TEXT NOT NULL
);

--- Insert some pre-fabricated users
--- I have also decided to set their respective hashed passwords over
--- the insert queries for not having to memorize them

-- Pwd 123
INSERT INTO USERS (NAME, FULLNAME, SECRET, MAIL, CREATEDAT, ROLE)
VALUES ('admin', 'Admin', 'AQAAAAIAAYagAAAAEGKnL4n850kAtjZYj5C/OeWb4rJe8xAgiwlu19VQJcOy5uEUG5/C2aDCVbp3hvZJww==', 'transaction.app@mail.com', CURRENT_DATE, 'Admin');

-- Pwd yozefb27
INSERT INTO USERS (NAME, FULLNAME, SECRET, MAIL, CREATEDAT, ROLE)
VALUES ('Yozef', 'Yozef Baruch', 'AQAAAAIAAYagAAAAEFwriMntsVBpDmn5/RV86srtDidkBwajUO9r6oGDy79ZsqaR876vYgGaUbWQw+UQ9Q==', 'yozefb@mail.com', CURRENT_DATE, 'User');

-- Pwd reh1906
INSERT INTO USERS (NAME, FULLNAME, SECRET, MAIL, CREATEDAT, ROLE)
VALUES ('Robert', 'Robert Ervin', 'AQAAAAIAAYagAAAAEBsLWwl7YSDDTmkVKj+CLSvQPelHACfG2Ycz6yKWs7LgCTKW0OBy7vr2Aj0cXNul8w==', 'reh@mail.com', CURRENT_DATE, 'User');

-- Pwd ladonis1996
INSERT INTO USERS (NAME, FULLNAME, SECRET, MAIL, CREATEDAT, ROLE)
VALUES ('Lea', 'Lea Adonis', 'AQAAAAIAAYagAAAAEGFd6dnASgjinbgXR4E83EYch7Cp7eJ/TB/mgDzh5/K7SGD2gs+1MB8ZyMMtmB5+xg==', 'leadonis@mail.com', CURRENT_DATE, 'User');

--- Create the transaction history table

CREATE TABLE TRANSACTION_HISTORY (
	ID SERIAL PRIMARY KEY,
	ORIGINUSER_ID INTEGER NOT NULL,
	DESTINEDUSER_ID INTEGER NOT NULL,
	CREATEDAT DATE NOT NULL,
	AMOUNT FLOAT NOT NULL
);

--- Base transaction

INSERT INTO TRANSACTION_HISTORY (ORIGINUSER_ID, DESTINEDUSER_ID, CREATEDAT, AMOUNT)
VALUES (2, 3, CURRENT_DATE, 26.87)
