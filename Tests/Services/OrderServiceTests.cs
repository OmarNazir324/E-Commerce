
using Application.Features.Order_ItemsFeature.DTOs;
using Application.Features.OrderFeature.DTOs;
using Application.Features.OrderFeature.Service;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Moq;
using Xunit;

namespace Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IMainInterFace<Order>> _repo;
    private readonly Mock<IUnitOfWork> _uow;
    public OrderServiceTests()
    {
        _mapper = new Mock<IMapper>();
        _repo = new Mock<IMainInterFace<Order>>();
        _uow = new Mock<IUnitOfWork>();
    }
    [Fact]
    public async Task CreateOrder_ShouldCalculateTotalPrice()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Product Test",
            Description = "Test Product",
            Price = 100,
            CategoryId = 1,
            Stock = 199
        };
        var createorderdto = new CreateOrderDTO{
            Name = "Create Order Test",
            CustomerId = 1,
            Description = "Unit Test For Calculate Total Price",
            CreateOrder_Items = new List<CreateOrder_itemsDTO>
            {
                new CreateOrder_itemsDTO
                {
                    Description = "Unit Test For Order Items",
                    Name = "Create Order Item Test",
                    ProductId = 1,
                    Quantity = 2
                }

            }
        };
        var order = new Order{
            Name = "Create Order Test",
            CustomerId = 1,
            Description = "Unit Test For Calculate Total Price",
            Order_Items = new List<Order_items>
            {
                new Order_items
                {
                    Description = "Unit Test For Order Items",
                    Name = "Create Order Item Test",
                    ProductId = 1,
                    Quantity = 2,
                    Product = product
                }

            }
        };
        _mapper.Setup(x => x.Map<Order>(createorderdto)).Returns(order);
        order.clac_TotalPrice();
        order.TotalPrice.Should().Be(200);
        order.TotalQuantity.Should().Be(2);
    }
    [Fact]
    public async Task CreateOrder_ShouldCreateOrder()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Product Test",
            Description = "Test Product",
            Price = 100,
            CategoryId = 1,
            Stock = 199
        };
        var createorderdto = new CreateOrderDTO
        {
            Name = "Create Order Test",
            CustomerId = 1,
            Description = "Unit Test For Creating New Order",
            CreateOrder_Items = new List<CreateOrder_itemsDTO>
            {
                new CreateOrder_itemsDTO
                {
                    Description = "Unit Test For Order Items",
                    Name = "Create Order Item Test",
                    ProductId = 1,
                    Quantity = 2
                }

            }
        };
        var order = new Order
        {
            Name = "Create Order Test",
            CustomerId = 1,
            Description = "Unit Test For Creating new order",
            Order_Items = new List<Order_items>
            {
                new Order_items
                {
                    Description = "Unit Test For Order Items",
                    Name = "Create Order Item Test",
                    ProductId = 1,
                    Quantity = 2,
                    Product = product
                }

            }
        };
        _mapper.Setup(x => x.Map<Order>(createorderdto)).Returns(order);
        _repo.Setup(x => x.Create(order));
        var service = new OrderService(_repo.Object, _mapper.Object, _uow.Object);
        var result = await service.Create(createorderdto);
        result.MSG.Should().Be(String.Empty);
        result.Status.Should().BeTrue();
    }
}
