FROM postgres:16.2

ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=postgres

RUN mkdir -p /var/lib/postgresql/data/pgdata
RUN chown -R postgres:postgres /var/lib/postgresql/data/pgdata

VOLUME /var/lib/postgresql/data/pgdata

EXPOSE 5432

ENTRYPOINT ["postgres"]