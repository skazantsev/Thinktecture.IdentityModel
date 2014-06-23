xcopy ..\..\build\thinktecture.identitymodel.owin.ResourceAuthorization.Mvc.dll lib\net45 /y
xcopy ..\..\build\thinktecture.identitymodel.owin.ResourceAuthorization.Mvc.pdb lib\net45 /y

NuGet.exe pack Thinktecture.IdentityModel.Owin.ResourceAuthorization.Mvc.nuspec -exclude *.cmd -OutputDirectory ..\