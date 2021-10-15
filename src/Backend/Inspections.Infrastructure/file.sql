START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211008054248_add_new_opre_fields_3') THEN
    ALTER TABLE "OperationalReadings" DROP COLUMN "OverCurrentByMainBreaker";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211008054248_add_new_opre_fields_3') THEN
    ALTER TABLE "OperationalReadings" ALTER COLUMN "OverCurrentDirectActing" TYPE smallint;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211008054248_add_new_opre_fields_3') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211008054248_add_new_opre_fields_3', '5.0.6');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211008055018_add_new_opre_fields_4') THEN
    ALTER TABLE "OperationalReadings" DROP COLUMN "MainBreakerRating";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211008055018_add_new_opre_fields_4') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211008055018_add_new_opre_fields_4', '5.0.6');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211008133748_add_new_opre_fields_5') THEN
    ALTER TABLE "OperationalReadings" ADD "MainBreakerRating" smallint NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211008133748_add_new_opre_fields_5') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211008133748_add_new_opre_fields_5', '5.0.6');
    END IF;
END $$;
COMMIT;

