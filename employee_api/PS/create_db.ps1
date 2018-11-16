Param
(
	[String]$ClientName,
	[String]$Server,
	[String]$Username,
	[String]$Password
)

$DBBakFilePath = "E:DB\tempnew"
$RestoreMdfPath = "E:\DB\"+$ClientName+"\"+$ClientName+".mdf"
$RestoreLdfPath = "E:\DB\"+$ClientName+"\"+$ClientName+".ldf"

$Query = "RESTORE DATABASE "+$ClientName+" FROM DISK = '"+$DBBakFilePath+"' WITH MOVE 'temp' TO '"+$RestoreMdfPath+"',MOVE 'temp_log' TO '"+$RestoreLdfPath+"'"

Invoke-Sqlcmd -Database master -Password $Password -Query $Query -ServerInstance $Server -Username $Username