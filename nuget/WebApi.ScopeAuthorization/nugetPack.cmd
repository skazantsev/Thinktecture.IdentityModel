xcopy ..\..\build\Thinktecture.IdentityModel.WebApi.ScopeAuthorization.dll lib\net45 /y
xcopy ..\..\build\Thinktecture.IdentityModel.WebApi.ScopeAuthorization.pdb lib\net45 /y
NuGet.exe pack Thinktecture.IdentityModel.WebApi.ScopeAuthorization.nuspec -exclude *.cmd -OutputDirectory ..\