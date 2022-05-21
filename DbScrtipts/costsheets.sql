DROP TABLE current;

CREATE TABLE current (
    id SERIAL,
    circuit VARCHAR(30),
    start_date VARCHAR(20),
    end_date VARCHAR(20),
    current_data JSONB,
    CONSTRAINT current_pk PRIMARY KEY (id)
);
