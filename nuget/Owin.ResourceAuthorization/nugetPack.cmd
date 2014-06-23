xcopy ..\..\build\thinktecture.identitymodel.owin.ResourceAuthorization.dll lib\net45 /y
xcopy ..\..\build\thinktecture.identitymodel.owin.ResourceAuthorization.pdb lib\net45 /y

NuGet.exe pack Thinktecture.IdentityModel.Owin.ResourceAuthorization.nuspec -exclude *.cmd -OutputDirectory ..\