DELETE FROM "Inspections"."Photos" WHERE "ReportId" IN (SELECT "Id" FROM "Inspections"."Reports");
DELETE FROM "Inspections"."Notes" WHERE "ReportId" IN (SELECT "Id" FROM "Inspections"."Reports");
DELETE FROM "Inspections"."Signatures" WHERE "ReportId" IN (SELECT "Id" FROM "Inspections"."Reports");
DELETE FROM "Inspections"."CheckListItems" WHERE "CheckListId" IN (SELECT "CheckListId" FROM "Inspections"."CheckLists" WHERE "ReportId" IN (SELECT "Id" FROM "Inspections"."Reports"));
DELETE FROM "Inspections"."CheckLists" WHERE "ReportId" IN (SELECT "Id" FROM "Inspections"."Reports");
DELETE FROM "Inspections"."Reports";