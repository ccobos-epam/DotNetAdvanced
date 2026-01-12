CREATE USER "keycloakuser" WITH PASSWORD 'keycloak123';
CREATE DATABASE keycloak;
\connect keycloak
CREATE SCHEMA IF NOT EXISTS "keycloak" AUTHORIZATION "keycloakuser";
GRANT ALL ON SCHEMA "keycloak" TO "keycloakuser";
GRANT CREATE ON SCHEMA "keycloak" TO "keycloakuser";
GRANT ALL ON SCHEMA public TO "keycloakuser";