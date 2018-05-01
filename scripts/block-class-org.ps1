Param(
	[parameter(Mandatory=$True)]
	[string]$Org
)

& cf target -o $Org
If ($? -ne "True") {Pause; Exit}

& cf spaces | select -Skip 3 | foreach {
	& cf bind-security-group block-internal $Org $_
}

Write-Output "Make sure to have class restart apps"