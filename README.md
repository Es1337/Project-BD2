# Project-BD2

1. You have to have SQL Server instance on your machine
2. In ProjectDB_create.sql change .mdf and .ldf paths to lead to your SQLServer instance's DATA folder and run script.
3. Run ProjectDB_addSchema.sql
4. Run ProjectDB_createTreesTable.sql
5. Open Solution in Visual Studio
5b. Make sure you have Linq To Sql package installed.
6. Open Project => Properties => Settings[.settings]
7. Change value of Data Source property in ProjectDBConnectionString variable to your SQLServer instance
8. Run solution, it should open a console application, if everything is done properly you should see prompt 'Connection established' and your instance's data
9. Use the application to familiarise yourself with API's capabilities
