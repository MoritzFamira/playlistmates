# Use the official PostgreSQL image as the base image
FROM postgres:16.1-alpine
USER postgres
# Copy the SQL script into the Docker container
ADD ./init.sql /docker-entrypoint-initdb.d/
RUN chown postgres:postgres /docker-entrypoint-initdb.d/init.sql
ENTRYPOINT ["docker-entrypoint.sh"]
# Set environment variables for PostgreSQL
#ENV POSTGRES_DB your_database_name
#ENV POSTGRES_USER your_username
ENV POSTGRES_PASSWORD password

# Expose the PostgreSQL port
EXPOSE 5432