# AzureFunctionAPI-CosmosDB-CleanArchitecture
 - In Azure Portal, Create new Azure CosmosDB account Go to Data
   Explorer, create new database "famspotdb"
  
 - Create two containers in database "Audit" (Partition Key: EntityId)
   and "User" (Partition Key:    UserType & Unique Key: Email).

- Go to "Keys" in "Settings" section, Get the "URI" and "PRIMARY KEY" and replace the values in af-userapi -> local.settings.json file's ConnectionString section.

```
  "ConnectionStrings": {
    "FamspotDB": {
      "EndpointUrl": "https://XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX:443/",
      "PrimaryKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
      "DatabaseName": "famspotdb",
      "Containers": [
        {
          "Name": "Audit",
          "PartitionKey": "/EntityId"
        },
        {
          "Name": "User",
          "PartitionKey": "/UserType"
        }
      ]
    }
  }
  ```
