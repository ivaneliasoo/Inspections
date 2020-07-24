delete from Inspections.CheckListParams
delete from Inspections.CheckListItems
delete from Inspections.CheckLists
delete from Inspections.Signatures
delete from Inspections.Notes
delete from Inspections.Photos
delete from Inspections.Reports
delete from Inspections.ReportsConfiguration
delete from Users

DBCC CHECKIDENT ('Inspections.CheckListParams', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.CheckListItems', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.CheckLists', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Signatures', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Notes', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Photos', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.Reports', RESEED, 0);  
DBCC CHECKIDENT ('Inspections.ReportsConfiguration', RESEED, 0);  
