COPY public."Authors"(
	"Id", "Name", "Country")
FROM '/var/lib/postgresql/data/tables/room.csv'
DELIMITER ','
CSV HEADER;