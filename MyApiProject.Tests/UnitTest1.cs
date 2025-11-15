using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyApiProject.Tests;

public class GetCustomersTests
{
    [Fact]
    public async Task GetCustomers_ReturnsOkResult()
    {
        // Arrange
        var testServer = new TestServer(new WebHostBuilder()
            .UseTestServer()
            .ConfigureServices(services =>
            {
                services.AddRouting();
            })
            .Configure(appBuilder =>
            {
                appBuilder
                    .UseRouting()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/customers", GetCustomersHandler);
                    });
            }));

        var client = testServer.CreateClient();

        // Act
        var response = await client.GetAsync("/customers");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
        Assert.Contains("Alice", content);
        Assert.Contains("Bob", content);
        Assert.Contains("Charlie", content);
    }

    [Fact]
    public async Task GetCustomers_ReturnsThreeCustomers()
    {
        // Arrange
        var testServer = new TestServer(new WebHostBuilder()
            .UseTestServer()
            .ConfigureServices(services =>
            {
                services.AddRouting();
            })
            .Configure(appBuilder =>
            {
                appBuilder
                    .UseRouting()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/customers", GetCustomersHandler);
                    });
            }));

        var client = testServer.CreateClient();

        // Act
        var response = await client.GetAsync("/customers");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("\"id\":1", content);
        Assert.Contains("\"id\":2", content);
        Assert.Contains("\"id\":3", content);
    }

    [Fact]
    public void Customer_HasCorrectProperties()
    {
        // Arrange & Act
        var customer = new Customer(1, "Alice", "alice@example.com");

        // Assert
        Assert.Equal(1, customer.Id);
        Assert.Equal("Alice", customer.Name);
        Assert.Equal("alice@example.com", customer.Email);
    }

    private IResult GetCustomersHandler()
    {
        var customers = new[]
        {
            new Customer(1, "Alice", "alice@example.com"),
            new Customer(2, "Bob", "bob@example.com"),
            new Customer(3, "Charlie", "charlie@example.com")
        };
        return Results.Ok(customers);
    }
}

public record Customer(int Id, string Name, string Email);