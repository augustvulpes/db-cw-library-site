DROP TRIGGER check_status_trigger ON "Orders";
DROP FUNCTION check_status;

CREATE FUNCTION check_status() RETURNS TRIGGER AS $$
BEGIN
    IF NEW."State" = 'new' THEN
        RAISE NOTICE 'New order initiated: by user %' , New."Id";
	END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql ;

CREATE TRIGGER check_status_trigger
AFTER INSERT ON "Orders"
FOR EACH ROW
EXECUTE FUNCTION check_status();