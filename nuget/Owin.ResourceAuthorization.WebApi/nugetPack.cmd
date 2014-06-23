xcopy ..\..\build\thinktecture.identitymodel.owin.ResourceAuthorization.WebApi.dll lib\net45 /y
xcopy ..\..\build\thinktecture.identitymodel.owin.ResourceAuthorization.WebApi.pdb lib\net45 /y

NuGet.exe pack Thinktecture.IdentityModel.Owin.ResourceAuthorization.WebApi.nuspec -exclude *.cmd -OutputDirectory ..\