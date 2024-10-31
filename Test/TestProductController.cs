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

    public async Task GetProductById_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        
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

        
        // Act
        
        // Calling the GetProduct method in our controller, with Id of 1
        var result = await _prodController.GetProduct(1);

        // Assert
        
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
    
    // Test to get an error when trying to get a product via id which doesn't exist
    // Test to create product
    // Test to update product
    // Test to delete product




}