using API.Controllers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Frameworks;

namespace Test;
// Arrange -> Act -> Assert

public class TestProductController
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductsController _prodController;

    public TestProductController()
    {
        _mockRepository = new Mock<IProductRepository>();
        _prodController = new ProductsController(_mockRepository.Object);

    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        //// Arrange step -> Setup necessary precondition and inputs
        
        var mockProds = new List<Product>
        {
            // Data we expect to receive when we call GetProductsAsync
            new Product
            {
                Id = 1,
                Name = "Product1",
                Price = 1010,
                Description = "TestDescription",
                FuelType = "TestFuelType",
                Gearbox = "TestAutomatic",
                Mileage = 100,
                Model = "TestModel",
                Brand = "TestBrand",
                PictureUrl = "TestPictureUrl",
                QuantityInStock = 1
            },
            new Product
            {
                Id = 2,
                Name = "Product2",
                Price = 10102,
                Description = "TestDescription2",
                FuelType = "TestFuelType2",
                Gearbox = "TestAutomatic2",
                Mileage = 1002,
                Model = "TestModel2",
                Brand = "TestBrand2",
                PictureUrl = "TestPictureUrl2",
                QuantityInStock = 2
            }
        };
        
        // Setting up mock repository to return the mockProducts we created above
        // Calling GetProductsAsync with Null, since we don't want to apply any sort (can still simulate repo behaviour)
        _mockRepository.Setup(rep => rep.GetProductsAsync(null, null, null))
            .ReturnsAsync(mockProds);
        
        //// Act stage -> Performing the action we want to test
        
        // Simulate Client calling the API endpoint (again without filters hence Null)
        var result = await _prodController.GetProducts(null, null, null);
        
        //// Assert stage -> Check if output is what we are expecting
        
        // Verify controller successfully returned an object data -> check for status 200 OK.
        var resultOk = Assert.IsType<OkObjectResult>(result.Result);
        
        // Verify the data within the 200 OK response is a List<Product> 
        var returnProducts = Assert.IsType<List<Product>>(resultOk.Value);
        
        // Verify number of items in our list is equal to 2 (number of products in our mockProducts)
        Assert.Equal(2, returnProducts.Count);
    }
    
    [Fact]
    public async Task GetProductById_ReturnsProduct_WhenProductExists()
    {
        //// Arrange
        
        // Create Mock product to retrieve
        var mockProduct = new Product
        {
            Id = 1,
            Name = "Existing Product",
            Description = "Existing Product Test By Id",
            Price = 101,
            Year = 101,
            FuelType = "FuelTypeTest",
            Gearbox = "GearBoxTest",
            Mileage = 10101,
            Model = "ModelTest",
            Brand = "BrandTest",
            PictureUrl = "PictureUrl",
            QuantityInStock = 131
        };
        
        // Mock our repo -> To retrieve our existing product we created when our GetProductByIdAsync is called with argument of 1 (for id)
        _mockRepository.Setup(rep => rep.GetProductByIdAsync(1)).ReturnsAsync(mockProduct);
        
        //// Act
        
        // Calling the GetProduct method in our controller, with Id of 1
        var result = await _prodController.GetProduct(1);

        //// Assert
        
        // Verify the HTTP response if 200 Ok
        var resultOk = Assert.IsType<OkObjectResult>(result.Result);
        
        // Verify our product is of type Product
        var product = Assert.IsType<Product>(resultOk.Value);
        
        // Ver ify our product we retrieved matches the mockProduct we created
        Assert.Equal(mockProduct.Id, product.Id);
        Assert.Equal(mockProduct.Name, product.Name);
        Assert.Equal(mockProduct.Price, product.Price);
        Assert.Equal(mockProduct.Description, product.Description);
        Assert.Equal(mockProduct.Year, product.Year);
        Assert.Equal(mockProduct.Model, product.Model);
        Assert.Equal(mockProduct.Brand, product.Brand);
        Assert.Equal(mockProduct.FuelType, product.FuelType);
        Assert.Equal(mockProduct.Gearbox, product.Gearbox);
        Assert.Equal(mockProduct.Mileage, product.Mileage);
        Assert.Equal(mockProduct.PictureUrl, product.PictureUrl);
        Assert.Equal(mockProduct.QuantityInStock, product.QuantityInStock);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
    //// Arrange
    // Configure Mock repo to pull a product, and .ReturnsAsync from Moq library allow us to define what should be returned. 
    // We CAST null to type Product so we can return a Product Object (don't have to create Task<Product> above)
    _mockRepository.Setup(rep => rep.GetProductByIdAsync(99))
        .ReturnsAsync((Product)null);

    //// Act
    var result = await _prodController.GetProduct(99);

    //// Assert
    // Verify that we get HTTP Response of 404 (Not Found) 
    Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedAtActionResult_WhenProductIsCreatedSuccessfully()
    {
        //// Arrange
        
        // Create new mock product
        var mockProduct = new Product
        {
            Id = 1,
            Name = "Existing Product",
            Description = "Existing Product Test By Id",
            Price = 101,
            Year = 101,
            FuelType = "FuelTypeTest",
            Gearbox = "GearBoxTest",
            Mileage = 10101,
            Model = "ModelTest",
            Brand = "BrandTest",
            PictureUrl = "PictureUrl",
            QuantityInStock = 121
        };
        
        // Simulate the mock repo saving changes
        _mockRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(true);

        //// Act
        
        // Simulate Client request to create a new product (we pass same field values as the mock we created above)
        var result = await _prodController.CreateProduct(mockProduct);


        //// Assert

        // Verify if we get a CreatedAtActionResult -> Meaning product was created
        var createdActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        
        // Verify new created product location in response header is as expected (201 response)
        Assert.Equal("GetProduct", createdActionResult.ActionName);
        
        // Verify the product we created in Act matches the product fields we gave it in arrange 
        Assert.Equal(mockProduct.Id, ((Product)createdActionResult.Value!).Id);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        
        // Mock product to update
        var mockProduct = new Product
        {
            Id = 1,
            Name = "Existing Product",
            Description = "Existing Product Test By Id",
            Price = 101,
            Year = 101,
            FuelType = "FuelTypeTest",
            Gearbox = "GearBoxTest",
            Mileage = 10101,
            Model = "ModelTest",
            Brand = "BrandTest",
            PictureUrl = "PictureUrl",
            QuantityInStock = 121
        };
        
        // Mock repo to return our mockProduct with its Id of 1 -> Will return true
        _mockRepository.Setup(rep => rep.ProductExists(mockProduct.Id)).Returns(true);
        
        // Mock repo to save changes when we update
        _mockRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(true);
        
        // Act
        
        // Simulate client request to update specific product
        var result = await _prodController.UpdateProduct(mockProduct.Id, mockProduct);

        // Assert
        
        // Verify outcome is NoContentResult -> what we would expect in a successful update (204 status code)
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenDeleteIsSuccessful()
    {
        var mockProduct = new Product
        {
            Id = 11,
            Name = "Existing Product",
            Description = "Existing Product Test By Id",
            Price = 101,
            Year = 101,
            FuelType = "FuelTypeTest",
            Gearbox = "GearBoxTest",
            Mileage = 10101,
            Model = "ModelTest",
            Brand = "BrandTest",
            PictureUrl = "PictureUrl",
            QuantityInStock = 121
        };

        // Simulates Repo retrieving our specified product via Id and returns it 
        _mockRepository.Setup(rep => rep.GetProductByIdAsync(mockProduct.Id)).ReturnsAsync(mockProduct);

        // simulate a successful delete in our DB
        _mockRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(true);

        var result = await _prodController.DeleteProduct(mockProduct.Id);

        Assert.IsType<NoContentResult>(result);
    }
    

}