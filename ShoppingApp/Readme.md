How to create migratin:
ex.:
dotnet ef migrations add Products_Module_Init --startup-project ../../../ShoppingApp.Bootstraper/ --context ProductsDbContext -o ./Postgres/Migrations