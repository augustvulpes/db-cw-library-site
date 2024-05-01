/* ACTUALLY UNUSED */
DROP TABLE IF EXISTS "Authors" CASCADE;
CREATE TABLE IF NOT EXISTS "Authors"
(
    Id INT NOT NULL GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Name text,
    Country text
);

/*INSERT INTO "Authors" (Name, Country) VALUES ('Author name', 'Country name');*/
INSERT INTO "Books" VALUES (1, 'Good book', 42, 1984, '6.-5');
INSERT INTO "Reviews" VALUES (1, 1, 1, 'Reviews content', '2020-01-01');
INSERT INTO "Users" VALUES (1, False, '8800553535', 'vasya@gmail.com', 'aboba123', 'Male', 'Vasily', 'Vasilyev', 'Vasylievich', '2001-01-02');