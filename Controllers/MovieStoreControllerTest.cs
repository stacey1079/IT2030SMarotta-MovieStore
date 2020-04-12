namespace Unit_Tests.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using MovieStore.Controllers;
    using MovieStore.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Mvc;
    using MovieStore.Data;

    [TestClass]
    public class MovieStoreControllerTest
    {
        [TestMethod]
        public void MovieStore_Index_TestView()
        {
            //Arrange
            MoviesController controller = new MoviesController();

            //Act
            ViewResult result = controller.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_ListOfMovies()
        {
            //Arrange
            MoviesController controller = new MoviesController();

            //Act
            List<Movie> result = controller.ListOfMovies();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected: "Iron Man 1", actual: result[0].Title);
            Assert.AreEqual(expected: "Iron Man 2", actual: result[1].Title);
            Assert.AreEqual(expected: "Iron Man 3", actual: result[2].Title);
        }

        [TestMethod]
        public void MovieStore_IndexRedirect_Success()
        {
            //Arrange
            MoviesController controller = new MoviesController();

            //Act
            RedirectToRouteResult result = controller.IndexRedirect(id: 1) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected: "Create", actual: result.RouteValues["action"]);
            Assert.AreEqual(expected: "HomeController", actual: result.RouteValues["controller"]);
        }

        [TestMethod]
        public void MovieStore_IndexRedirect_BadRequest()
        {
            // Arrange
            MoviesController controller = new MoviesController();

            // Act
            HttpStatusCodeResult result = controller.IndexRedirect(id: 0) as HttpStatusCodeResult;

            // Assert
            Assert.AreEqual(expected: HttpStatusCode.BadRequest, actual: (HttpStatusCode)result.StatusCode);
        }

        [TestMethod]
        public void MovieStore_ListFromDb()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Superman 1"},
                new Movie() {MovieId = 2, Title = "Superman 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            ViewResult result = controller.ListFromDb() as ViewResult;
            List<Movie> resultMovies = result.Model as List<Movie>;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_Details_Success()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Superman 1"},
                new Movie() {MovieId = 2, Title = "Superman 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            ViewResult result = controller.Details(id: 1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_Details_MovieIsNull()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Superman 1"},
                new Movie() {MovieId = 2, Title = "Superman 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            Movie movie = null;

            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(movie);

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            HttpStatusCodeResult result = controller.Details(id: 0) as HttpStatusCodeResult;

            // Assert
            Assert.AreEqual(expected: HttpStatusCode.NotFound, actual: (HttpStatusCode)result.StatusCode);
        }

        [TestMethod]
        public void MovieStore_Edit_Success()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Spiderman 1"},
                new Movie() {MovieId = 2, Title = "Spiderman 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            ViewResult result = controller.Details(id: 1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_Edit_MovieIsNull()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Spiderman 1"},
                new Movie() {MovieId = 2, Title = "Spiderman 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            Movie movie = null;

            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(movie);

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            HttpStatusCodeResult result = controller.Edit(id: 0) as HttpStatusCodeResult;

            // Assert
            Assert.AreEqual(expected: HttpStatusCode.NotFound, actual: (HttpStatusCode)result.StatusCode);
        }

        [TestMethod]
        public void MovieStore_Create_TestView()
        {
            //Arrange
            MoviesController controller = new MoviesController();

            //Act
            ViewResult result = controller.Create() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_Create_MovieIsNull()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Captain America 1"},
                new Movie() {MovieId = 2, Title = "Captain America 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            Movie movie = null;

            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(movie);

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            RedirectToRouteResult result = controller.Create(movie) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            // Assert.AreEqual(expected: "Create", actual: result.RouteValues["action"]);
            // Assert.AreEqual(expected: "MoviesController", actual: result.RouteValues["controller"]);
        }
        [TestMethod]
        public void MovieStore_Delete()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Trolls 1"},
                new Movie() {MovieId = 2, Title = "Trolls 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            ViewResult result = controller.Details(id: 1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void MovieStore_DeletePost()
        {
            // Goal: Query from our own list instead of the database.

            // Step 1
            var list = new List<Movie>
            {
                new Movie() {MovieId = 1, Title = "Superman 1"},
                new Movie() {MovieId = 2, Title = "Superman 2"},
            }.AsQueryable();

            // Step 2
            Mock<MovieStoreDbContext> mockContext = new Mock<MovieStoreDbContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

            // Step 3
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            Movie movie = null;

            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(movie);

            // Step 4
            mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            HttpStatusCodeResult result = controller.Delete(id: 0) as HttpStatusCodeResult;

            // Assert
            Assert.AreEqual(expected: HttpStatusCode.NotFound, actual: (HttpStatusCode)result.StatusCode);
        }
    }
}
