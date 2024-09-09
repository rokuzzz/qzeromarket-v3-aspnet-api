using System.Net;
using Ecommerce.Domain.Models;
using Ecommerce.Services.CategoryService.DTO;
using Ecommerce.Services.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Net.Http.Headers;

namespace Ecommerce.Tests.Controllers
{
    public class CategoryControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly byte[] _filebytes;
        private readonly FormFile _file;
        public CustomWebApplicationFactory<Program> Fixture { get; }


        public CategoryControllerTest(CustomWebApplicationFactory<Program> factory, CustomWebApplicationFactory<Program> fixture)
        {
            _factory = factory;
            _filebytes = Encoding.UTF8.GetBytes("dummy image");
            _file = new FormFile(new MemoryStream(_filebytes), 0, _filebytes.Length, "Data", "image.png");
            Fixture = fixture;
        }

        [Theory]
        [InlineData("/api/v1/categories/")]
        [Trait("Category", "Integration")]
        public async Task GetAllCategory_ReturnsSuccess(string url)
        {
            var client = _factory.CreateClient();
            _factory.AddCategories();

            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var category = JsonConvert.DeserializeObject<PaginatedResult<Category, GetCategoryDto>>(result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(category);
            Assert.IsType<PaginatedResult<Category, GetCategoryDto>>(category);
            Assert.Equal(3, category.Items.Count());
            Assert.Equal(_factory.Category1.Name, category.Items.First().Name);
            Assert.Equal(_factory.Category1.CategoryImage, category.Items.First().CategoryImage);
            Assert.Equal(_factory.Category1.Id, category.Items.First().Id);
        }

        [Theory]
        [InlineData("/api/v1/categories?parentCategoryId=2")]
        [Trait("Category", "Integration")]
        public async Task GetCategoryByParentCategoryId_ReturnsSuccess(string url)
        {
            var client = _factory.CreateClient();
            _factory.AddCategories();

            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var category = JsonConvert.DeserializeObject<PaginatedResult<Category, GetCategoryDto>>(result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(category);
            Assert.IsType<PaginatedResult<Category, GetCategoryDto>>(category);
            Assert.Equal(2, category.Items.Count());
            Assert.Equal(_factory.Category2.Name, category.Items.First().Name);
            Assert.Equal(_factory.Category2.CategoryImage, category.Items.First().CategoryImage);
            Assert.Equal(_factory.Category2.Id, category.Items.First().Id);
        }

        [Theory]
        [InlineData("/api/v1/categories/1/")]
        [Trait("Category", "Integration")]
        public async Task GetOneCategory_ReturnsSuccess(string url)
        {
            var client = _factory.CreateClient();
            _factory.AddCategories();

            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var category = JsonConvert.DeserializeObject<GetCategoryDto>(result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(category);
            Assert.IsType<GetCategoryDto>(category);
            Assert.Equal(_factory.Category1.Name, category.Name);
            Assert.Equal(_factory.Category1.CategoryImage, category.CategoryImage);
            Assert.Equal(_factory.Category1.Id, category.Id);
        }

        [Theory]
        [InlineData("/api/v1/categories/100/")]
        [Trait("Category", "Integration")]
        public async Task GetOneCategory_ReturnsNotFound(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var errorResponse = JsonConvert.DeserializeObject<ProblemDetails>(result);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(errorResponse);
            Assert.IsType<ProblemDetails>(errorResponse);
            Assert.Equal("Category not found", errorResponse.Detail);
            Assert.Equal(404, errorResponse.Status);
        }

        [Theory]
        [InlineData("/api/v1/categories/")]
        [Trait("Category", "Integration")]
        public async Task CreateCategory_ReturnsCreated(string url)
        {
            var client = _factory.CreateClient();

            using var context = Fixture.CreateContext();
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();

            var category = new CreateOrUpdateCategoryDto
            {
                Name = "Category 4",
                CategoryImage = _file
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent(category.Name), "Name" },
                { new StreamContent(_file.OpenReadStream()) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "CategoryImage", _file.FileName },
            };

            var response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            var createdCategory = JsonConvert.DeserializeObject<GetCategoryDto>(result);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(createdCategory);
            Assert.IsType<GetCategoryDto>(createdCategory);
            Assert.Equal(category.Name, createdCategory.Name);
        }

        [Theory]
        [InlineData("/api/v1/categories/")]
        [Trait("Category", "Integration")]
        public async Task CreateCategory_ReturnsBadRequestIfDataNotValid(string url)
        {
            var client = _factory.CreateClient();

            using var context = Fixture.CreateContext();
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();

            var category = new CreateOrUpdateCategoryDto
            {
                Name = "n",
                CategoryImage = _file
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent(category.Name), "Name" },
                { new StreamContent(_file.OpenReadStream()) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "CategoryImage", _file.FileName },
            };

            var response = await client.PostAsync(url, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/v1/categories/1/")]
        [Trait("Category", "Integration")]
        public async Task UpdateCategory_ReturnsBadRequestIfDataNotValid(string url)
        {
            var client = _factory.CreateClient();

            using var context = Fixture.CreateContext();

            var category = new CreateOrUpdateCategoryDto
            {
                Name = "n",
                CategoryImage = _file
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent(category.Name), "Name" },
                { new StreamContent(_file.OpenReadStream()) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "CategoryImage", _file.FileName },
            };

            var response = await client.PutAsync(url, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/v1/categories/1/")]
        [Trait("Category", "Integration")]
        public async Task PatchCategory_ReturnsBadRequestIfDataNotValid(string url)
        {
            var client = _factory.CreateClient();

            using var context = Fixture.CreateContext();

            var category = new PartialUpdateCategoryDto
            {
                Name = "n",
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent(category.Name), "Name" }
            };

            var response = await client.PatchAsync(url, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/v1/categories/1/")]
        [Trait("Category", "Integration")]
        public async Task UpdateCategory_Returns(string url)
        {
            var client = _factory.CreateClient();
            using var context = _factory.CreateContext();
            _factory.AddCategories();


            var category = new CreateOrUpdateCategoryDto
            {
                Name = "Category 4",
                CategoryImage = _file
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent(category.Name), "Name" },
                { new StreamContent(_file.OpenReadStream()) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "CategoryImage", _file.FileName },
            };

            var response = await client.PutAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            var createdCategory = JsonConvert.DeserializeObject<GetCategoryDto>(result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(createdCategory);
            Assert.IsType<GetCategoryDto>(createdCategory);
            Assert.Equal(category.Name, createdCategory.Name);
            Assert.Equal(_factory.Category1.Id, createdCategory.Id);
        }

        [Theory]
        [InlineData("/api/v1/categories/100/")]
        [Trait("Category", "Integration")]
        public async Task UpdateCategory_WithNonExistingId_CreatesNewEntity(string url)
        {
            var client = _factory.CreateClient();

            using var context = Fixture.CreateContext();
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();

            var category = new CreateOrUpdateCategoryDto
            {
                Name = "Category 4",
                CategoryImage = _file
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent(category.Name), "Name" },
                { new StreamContent(_file.OpenReadStream()) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "CategoryImage", _file.FileName },
            };

            var response = await client.PutAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            var createdCategory = JsonConvert.DeserializeObject<GetCategoryDto>(result);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(createdCategory);
            Assert.IsType<GetCategoryDto>(createdCategory);
            Assert.Equal(category.Name, createdCategory.Name);
            Assert.NotEqual(100, createdCategory.Id);
        }

        [Theory]
        [InlineData("/api/v1/categories/1/")]
        [Trait("Category", "Integration")]
        public async Task DeleteCategory_ReturnsNoContent(string url)
        {
            var client = _factory.CreateClient();
            _factory.AddCategories();

            var response = await client.DeleteAsync(url);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/v1/categories/1/")]
        [Trait("Category", "Integration")]
        public async Task DeleteCategory_ReturnsNotFound(string url)
        {
            var client = _factory.CreateClient();
            using var context = _factory.CreateContext();
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();

            var response = await client.DeleteAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var errorDetails = JsonConvert.DeserializeObject<ProblemDetails>(result);

            Assert.NotNull(response.Content);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(errorDetails);
            Assert.IsType<ProblemDetails>(errorDetails);
            Assert.Equal("Category not found", errorDetails.Detail);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/v1/categories/1/")]
        [Trait("Category", "Integration")]
        public async Task PatchCategory_ReturnsSuccess(string url)
        {
            var client = _factory.CreateClient();
            using var context = _factory.CreateContext();
            _factory.AddCategories();

            var category = new PartialUpdateCategoryDto
            {
                Name = "Category 4",
                CategoryImage = _file
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent(category.Name), "Name" },
                { new StreamContent(_file.OpenReadStream()) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "CategoryImage", _file.FileName },
            };

            var response = await client.PatchAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            var updatedCategory = JsonConvert.DeserializeObject<GetCategoryDto>(result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.NotNull(updatedCategory);
            Assert.IsType<GetCategoryDto>(updatedCategory);
            Assert.Equal(category.Name, updatedCategory.Name);
            Assert.Equal(_factory.Category1.Id, updatedCategory.Id);
        }
    }
}