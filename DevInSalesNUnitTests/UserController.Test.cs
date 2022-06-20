using DevInSales.Context;
using DevInSales.Controllers;
using DevInSales.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DevInSalesNUnitTests
{
    public class UserControllerTests
    {
        [OneTimeSetUp]
        public void StartTest()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        [OneTimeTearDown]
        public void EndTest()
        {
            Trace.Flush();
        }

        private UserController _userController;

        private readonly DbContextOptions<SqlContext> _dbContextOp;
        public UserControllerTests()
        {
            _dbContextOp = new DbContextOptionsBuilder<SqlContext>().UseInMemoryDatabase(databaseName: "DevInSale")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;

            var sqlContext = new SqlContext(_dbContextOp);

            sqlContext.Database.EnsureDeleted();
            sqlContext.Database.EnsureCreated();
        }

        [SetUp]
        public void Setup()
        {
            _userController = new UserController(new SqlContext(_dbContextOp));
        }

        private string? getFirstListName(List<UserResponseDTO> list)
        {
            foreach (UserResponseDTO item in list)
            {
                return item.Name;
            }
            return null;
        }

        [Test]
        public async Task GetUser_ShouldReturnOk_WhenOneUserExists()
        {
            //var userController = new UserController(new SqlContext(_dbContextOp));

            string? userName = "Usuário Comum Filho";
            string? birth_date_min = null;
            string? birth_date_max = null;

            var getUserResult = await _userController.Get(userName, birth_date_min, birth_date_max);

            var expectedResult = (getUserResult.Result as ObjectResult);
            
            var listResult = expectedResult.Value as List<UserResponseDTO>;

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
            Assert.AreEqual(userName, getFirstListName(listResult));
        }

        [Test]
        public async Task GetUser_ShouldReturnOk_WhenSetDateMin()
        {
            string? userName = null;
            string? birth_date_min = "01/02/2000";
            string? birth_date_max = null;

            var getUserResult = await _userController.Get(userName, birth_date_min, birth_date_max);

            var expectedResult = (getUserResult.Result as ObjectResult);

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
        }

        [Test]
        public async Task GetUser_ShouldReturnOk_WhenSetDateMax()
        {
            string? userName = null;
            string? birth_date_min = null;
            string? birth_date_max = "01/02/2000";

            var getProductResult = await _userController.Get(userName, birth_date_min, birth_date_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
        }

        [Test]
        public async Task GetUser_ShouldReturnOk_WhenDateMatch()
        {
            string? userName = null;
            string? birth_date_min = "01/02/1999";
            string? birth_date_max = "01/02/2001";

            var getProductResult = await _userController.Get(userName, birth_date_min, birth_date_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
        }

        [Test]
        public async Task GetUser_ShouldReturnNotFound_WhenUserNotExists()
        {
            string? userName = "NO USER";
            string? birth_date_min = null;
            string? birth_date_max = null;

            var getProductResult = await _userController.Get(userName, birth_date_min, birth_date_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 404);
        }

        [Test]
        public async Task CreateUser_ShouldReturnCreated_WhenValidUser()
        {
            var userPost = new UserCreateDTO
            {
                Name = "Usuario de Tests",
                Email = "usuario@testes.com.br",
                Password = "usuario@123",
                BirthDate = "06/02/1979",
                ProfileId = 2
            };

            var getProductResult = await _userController.Create(userPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 201);
        }

        [Test]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUserNotHaveAge()
        {
            var userPost = new UserCreateDTO
            {
                Name = "Usuario de Tests",
                Email = "usuario@testes.com.br",
                Password = "usuario@123",
                BirthDate = "06/02/2019",
                ProfileId = 2
            };

            var getProductResult = await _userController.Create(userPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("O usuário deve ser maior de 18 anos."));
        }

        [Test]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUserWrongDate()
        {
            var userPost = new UserCreateDTO
            {
                Name = "Usuario de Tests",
                Email = "usuario@testes.com.br",
                Password = "usuario@123",
                BirthDate = "06/1979",
                ProfileId = 2
            };

            var getProductResult = await _userController.Create(userPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("Data inválida."));
        }

        [Test]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUserWrongPassword()
        {
            var userPost = new UserCreateDTO
            {
                Name = "Usuario de Tests",
                Email = "usuario@testes.com.br",
                Password = "aaaaaaa",
                BirthDate = "06/02/1979",
                ProfileId = 2
            };

            var getProductResult = await _userController.Create(userPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("Senha inválida. Deve-se ter pelo menos um caractere diferente dos demais."));
        }

        [Test]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUserEmailExists()
        {
            var userPost = new UserCreateDTO
            {
                Name = "Usuario de Tests",
                Email = "usuariofilho@gmail.com",
                Password = "usuario@123",
                BirthDate = "06/02/1979",
                ProfileId = 2
            };

            var getProductResult = await _userController.Create(userPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("O email informado já existe."));
        }

        [Test]
        public async Task DeleteUser_ShouldReturnOK_WhenUserExists()
        {
            int userId = 1;

            var getProductResult = await _userController.DeleteUser(userId);

            var expectedResult = (getProductResult as ObjectResult);

            Assert.True(expectedResult.StatusCode == 200);
            Assert.That(expectedResult.Value.ToString().Contains(userId.ToString()));
        }
    }
}