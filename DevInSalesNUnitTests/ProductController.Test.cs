using DevInSales.Context;
using DevInSales.Controllers;
using DevInSales.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DevInSalesNUnitTests
{
    public class ProductControllerTests
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

        private ProductController _productController;

        private readonly DbContextOptions<SqlContext> _dbContextOp;
        public ProductControllerTests()
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
            _productController = new ProductController(new SqlContext(_dbContextOp));
        }

        private string? getFirstListName(List<ProductGetDTO> list)
        {
            foreach (ProductGetDTO item in list)
            {
                return item.Name;
            }
            return null;
        }

        [Test]
        public async Task GetProduct_ShouldReturnOk_WhenOneCourseExists()
        {
            string? courseName = "Curso de Kotlin";
            decimal? price_min = null;
            decimal? price_max = null;

            var getProductResult = await _productController.GetProduct(courseName, price_min, price_max);

            var expectedResult = (getProductResult.Result as ObjectResult);
            
            var listResult = expectedResult.Value as List<ProductGetDTO>;

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
            Assert.AreEqual(courseName, getFirstListName(listResult));
        }

        [Test]
        public async Task GetProduct_ShouldReturnOk_WhenPriceSetPriceMin()
        {
            string? courseName = null;
            decimal? price_min = 250m;
            decimal? price_max = null;

            var getProductResult = await _productController.GetProduct(courseName, price_min, price_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
        }

        [Test]
        public async Task GetProduct_ShouldReturnOk_WhenPriceSetPriceMax()
        {
            string? courseName = null;
            decimal? price_min = null;
            decimal? price_max = 250m;

            var getProductResult = await _productController.GetProduct(courseName, price_min, price_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
        }

        [Test]
        public async Task GetProduct_ShouldReturnOk_WhenPriceMatch()
        {
            string? courseName = null;
            decimal? price_min = 250m;
            decimal? price_max = 300m;

            var getProductResult = await _productController.GetProduct(courseName, price_min, price_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNotNull(expectedResult);
            Assert.True(expectedResult.StatusCode == 200);
        }

        [Test]
        public async Task GetProduct_ShouldReturnBadRequest_WhenPriceMinIsGreaterThanPriceMax()
        {
            string? courseName = "Curso de Kotlin";
            decimal? price_min = 300m;
            decimal? price_max = 250m;

            var getProductResult = await _productController.GetProduct(courseName, price_min, price_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains($"O Preço Máximo ({price_max}) não pode ser menor que o Preço Mínimo ({price_min})."));
        }

        [Test]
        public async Task GetProduct_ShouldReturnNoContentNull_WhenCourseNotExists()
        {
            string? courseName = "Curso de Kotlin2";
            decimal? price_min = null;
            decimal? price_max = null;

            var getProductResult = await _productController.GetProduct(courseName, price_min, price_max);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNull(expectedResult);
        }

        [Test]
        public async Task PostProduct_ShouldReturnCreated_WhenValidProduct()
        {
            var productPost = new ProductPostAndPutDTO
            {
                Name = "Curso de Jython",
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PostProduct(productPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 201);
        }

        [Test]
        public async Task PostProduct_ShouldReturnBadRequest_WhenProductExists()
        {
            var productPost = new ProductPostAndPutDTO
            {
                Name = "Curso de Kotlin",
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PostProduct(productPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("Já existe um produto com este nome."));
        }

        [Test]
        public async Task PostProduct_ShouldReturnBadRequest_WhenProductPriceIsLessThenZero()
        {
            var productPost = new ProductPostAndPutDTO
            {
                Name = "Curso de Nada",
                Suggested_Price = -199m
            };

            var getProductResult = await _productController.PostProduct(productPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("O preço sugerido não pode ser menor ou igual a 0."));
        }

        [Test]
        public async Task PutProduct_ShouldReturnNoContent_WhenProductExistsAndChangeName()
        {
            int productId = 1;
            var productPut = new ProductPostAndPutDTO
            {
                Name = "Curso de Jython",
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PutProduct(productId, productPut);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNull(expectedResult);
        }

        [Test]
        public async Task PutProduct_ShouldReturnNotFound_WhenProductNotExists()
        {
            int productId = -1;
            var productPut = new ProductPostAndPutDTO
            {
                Name = "Curso de Jython",
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PutProduct(productId, productPut);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 404);
            Assert.That(expectedResult.Value.ToString().Contains("Não existe um produto com esta Id."));
        }

        [Test]
        public async Task PutProduct_ShouldReturnBadRequest_WhenProductPriceIsLessThenZero()
        {
            int productId = 1;
            var productPost = new ProductPostAndPutDTO
            {
                Name = "Curso Valor Negativo",
                Suggested_Price = -199m
            };

            var getProductResult = await _productController.PutProduct(productId, productPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("O preço sugerido não pode ser menor ou igual a 0."));
        }

        [Test]
        public async Task PutProduct_ShouldReturnBadRequest_WhenProductNameIsNull()
        {
            int productId = 1;
            var productPost = new ProductPostAndPutDTO
            {
                Name = null,
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PutProduct(productId, productPost);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.True(expectedResult.StatusCode == 400);
            Assert.That(expectedResult.Value.ToString().Contains("Nome ou Preço Sugerido são Nulos."));
        }

        [Test]
        public async Task PatchProduct_ShouldReturnNoContent_WhenValidProduct()
        {
            int productId = 1;
            var productPatch = new ProductPatchDTO
            {
                Name = "Curso de Jython",
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PatchProduct(productId, productPatch);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNull(expectedResult);
        }

        [Test]
        public async Task PatchProduct_ShouldReturnNotFound_WhenProductNotExists()
        {
            int productId = -1;
            var productPatch = new ProductPatchDTO
            {
                Name = "Curso de Jython",
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PatchProduct(productId, productPatch);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNull(expectedResult);
        }

        [Test]
        public async Task PatchProduct_ShouldReturnBadRequest_WhenProductPriceIsLessThenZero()
        {
            int productId = 1;
            var productPatch = new ProductPatchDTO
            {
                Name = "Curso Valor Negativo",
                Suggested_Price = -199m
            };

            var getProductResult = await _productController.PatchProduct(productId, productPatch);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNull(expectedResult);
        }

        [Test]
        public async Task PatchProduct_ShouldReturnBadRequest_WhenProductNameIsNull()
        {
            int productId = 1;
            var productPatch = new ProductPatchDTO
            {
                Name = null,
                Suggested_Price = 199m
            };

            var getProductResult = await _productController.PatchProduct(productId, productPatch);

            var expectedResult = (getProductResult.Result as ObjectResult);

            Assert.IsNull(expectedResult);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenProductDelete()
        {
            int productId = 1;

            var getProductResult = await _productController.DeleteProduct(productId);

            var expectedResult = (getProductResult as ObjectResult);

            Assert.IsNull(expectedResult);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductNotExists()
        {
            int productId = -1;

            var getProductResult = await _productController.DeleteProduct(productId);

            var expectedResult = (getProductResult as ObjectResult);

            Assert.True(expectedResult.StatusCode == 404);
            Assert.That(expectedResult.Value.ToString().Contains($"O Id de Produto de número {productId} não foi encontrado."));
        }
    }
}