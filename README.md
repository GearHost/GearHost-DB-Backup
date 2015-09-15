# gearhost-db-backup
GearHost offline database backup utility

1. Login to your account at https://my.gearhost.com
2. Ensure you have API key at https://my.gearhost.com/ApiConfig
3. Using your [APIKEY] run the utlity: ghbackup.exe -apiKey [APIKEY] -db [DBNAME TO BACKUP]
4. Optionally you can specify location to store files using -path parameter. Default location is "Downloads" folder under the app folder.
5. Example: ghbackup.exe -apiKey=123abc -dbName=products -path=C:\Backups 
