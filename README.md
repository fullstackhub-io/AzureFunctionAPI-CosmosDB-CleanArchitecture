# AzureFunctionAPI-CosmosDB-CleanArchitecture
 - In Azure Portal, Create new Azure CosmosDB account Go to Data
   Explorer, create new database "famspotdb"
  
 - Create two containers in database "Audit" (Partition Key: EntityId)
   and "User" (Partition Key:    UserType & Unique Key: Email).
