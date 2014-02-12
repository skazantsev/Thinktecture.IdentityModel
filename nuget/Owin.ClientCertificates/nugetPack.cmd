xcopy ..\..\build\thinktecture.identitymodel.owin.ClientCertificates.* lib\net45 /y
NuGet.exe pack Thinktecture.IdentityModel.Owin.ClientCertificates.nuspec -exclude *.cmd -OutputDirectory ..\