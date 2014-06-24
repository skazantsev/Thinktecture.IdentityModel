using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace WebApp.Tests
{
    [TestClass]
    public class ChinookAuthorizationTests
    {
        static readonly ClaimsPrincipal Anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        IResourceAuthorizationManager subject;

        ClaimsPrincipal User(string name, params string[] roles)
        {
            var ci = new ClaimsIdentity("Password");
            ci.AddClaim(new Claim(ClaimTypes.Name, name));
            foreach(var role in roles)
            {
                ci.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            return new ClaimsPrincipal(ci);
        }

        [TestInitialize]
        public void Init()
        {
            subject = new ChinookAuthorization();
        }

        [TestMethod]
        public void Anonymous_Cannot_Access_Album()
        {
            var ctx = new ResourceAuthorizationContext(Anonymous,
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album);
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
            
            ctx = new ResourceAuthorizationContext(Anonymous,
                ChinookResources.AlbumActions.View,
                ChinookResources.Album);
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Anonymous_Cannot_Access_Track()
        {
            var ctx = new ResourceAuthorizationContext(Anonymous,
                ChinookResources.TrackActions.Edit,
                ChinookResources.Track);
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Can_View_Album()
        {
            var ctx = new ResourceAuthorizationContext(User("test"),
                ChinookResources.AlbumActions.View,
                ChinookResources.Album);
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_NonAdmin_Cannot_Edit_Album()
        {
            var ctx = new ResourceAuthorizationContext(User("test"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album);
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_NonAdmin_Cannot_Edit_Track()
        {
            var ctx = new ResourceAuthorizationContext(User("test"),
                ChinookResources.TrackActions.Edit,
                ChinookResources.Track);
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Admin_Can_Edit_Album()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Admin"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album);
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Admin_Can_Edit_Track()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Admin"),
                ChinookResources.TrackActions.Edit,
                ChinookResources.Track);
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

        // v2 tests
        [TestMethod]
        public void Authenticated_Manager_Can_Edit_Album()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Manager"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album);
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Manager_Cannot_Edit_Track()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Manager"),
                ChinookResources.TrackActions.Edit,
                ChinookResources.Track);
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
        }

        // v3 bob
        [TestMethod]
        public void Authenticated_Admin_NotBob_Cannot_Edit_Album_ID1()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Admin"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album,
                "1");
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Admin_NotBob_Can_Edit_Album_ID2()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Admin"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album,
                "2");
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Admin_Bob_Can_Edit_Album_ID1()
        {
            var ctx = new ResourceAuthorizationContext(User("bob", "Admin"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album,
                "1");
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Manager_NotBob_Cannot_Edit_Album_ID1()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Manager"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album,
                "1");
            Assert.IsFalse(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Manager_NotBob_Can_Edit_Album_ID2()
        {
            var ctx = new ResourceAuthorizationContext(User("test", "Manager"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album,
                "2");
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

        [TestMethod]
        public void Authenticated_Manager_Bob_Can_Edit_Album_ID1()
        {
            var ctx = new ResourceAuthorizationContext(User("bob", "Manager"),
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album,
                "1");
            Assert.IsTrue(subject.CheckAccessAsync(ctx).Result);
        }

    }
}
