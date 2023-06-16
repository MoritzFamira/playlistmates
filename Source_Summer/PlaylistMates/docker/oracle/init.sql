BEGIN
   EXECUTE IMMEDIATE 'DROP USER pos CASCADE';
EXCEPTION
   WHEN OTHERS THEN
      IF SQLCODE != -1918 THEN
         RAISE;
      END IF;
END;
/
CREATE USER pos IDENTIFIED BY pos;
GRANT CONNECT, RESOURCE, CREATE VIEW TO pos;
GRANT UNLIMITED TABLESPACE TO pos;