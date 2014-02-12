xcopy ..\..\build\thinktecture.identitymodel.owin.ClaimsTransformation.* lib\net45 /y
NuGet.exe pack Thinktecture.IdentityModel.Owin.ClaimsTransformation.nuspec -exclude *.cmd -OutputDirectory ..\