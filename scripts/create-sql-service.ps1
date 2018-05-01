Write-Output "When prompted for uri, use the form sqlserver://<hostname>/<database-name>"
& cf create-user-provided-service mySqlServer -p 'uri,username,password'