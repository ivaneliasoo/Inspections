DROP TABLE "Inspections"."CheckListParams";
DROP TABLE "Inspections"."CheckListItems";
DROP TABLE "Inspections"."CheckLists";
DROP TABLE "Inspections"."Signatures";
DROP TABLE "Inspections"."Notes";
DROP TABLE "Inspections"."Photos";
DROP TABLE "Inspections"."Reports";
DROP TABLE "Inspections"."ReportsConfiguration";
DROP TABLE public."Users";
DROP TABLE "Inspections"."Addresses";
DROP TABLE public."Licenses"
DROP TABLE public."__EFMigrationsHistory";


DBCC CHECKIDENT ('Inspections.CheckListParams', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.CheckListItems', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.CheckLists', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Signatures', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Notes', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Photos', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Reports', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.ReportsConfiguration', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Addresses', RESEED, 0);  
