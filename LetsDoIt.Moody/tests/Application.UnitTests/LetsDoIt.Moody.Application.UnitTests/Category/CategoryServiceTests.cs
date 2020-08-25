﻿using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Web.Entities.Requests;
    using Application.Category;
    using VersionHistory;
    using Domain;    
    using Persistance.Repositories.Base;

    public class CategoryServiceTests
    {
        private readonly CategoryService _testing;
        private readonly Mock<IEntityRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IEntityRepository<VersionHistory>> _mockVersionHistoryRepository;
        private readonly Mock<IVersionHistoryService> _mockVersionHistoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<IEntityRepository<Category>>();
            _mockVersionHistoryRepository = new Mock<IEntityRepository<VersionHistory>>();
            _mockVersionHistoryService = new Mock<IVersionHistoryService>();

            _testing = new CategoryService(
                    _mockCategoryRepository.Object,
                    _mockVersionHistoryRepository.Object,
                    _mockVersionHistoryService.Object);
        }

        [Fact]
        public async Task InsertAsync_GivenNoException_ShouldInvokeRepositoryAddAsyncAndInvokeVersion()
        {
            var name = "asd";
            var order = 5;
            var image = "USrCELxGejBZI4W/Llsvmw==\r\n";
            var byteImage = Convert.FromBase64String(image);

            await _testing.InsertAsync(name, order, byteImage);

            _mockCategoryRepository.Verify(ur =>
                    ur.AddAsync(It.Is<Category>(x => x.Name == name)), Times.Once);
           
            _mockVersionHistoryService.Verify(ur =>
                ur.CreateNewVersionAsync(), Times.Once);
        }
    }

    
}
