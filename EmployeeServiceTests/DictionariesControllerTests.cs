using EmployeeService.Data;
using EmployeeServiceProg.Controllers;
using EmployeeServiceProg.Models.Dto;
using EmployeeServiceProg.Services.Impl.Repositories;
using EmployeeServiceProg.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeServiceTests
{
    public class DictionariesControllerTests
    {
        private readonly DictionariesController _dictionariesController;
        private readonly Mock<IEmployeeTypeRepository> _mockEmployeeTypeRepository;

        public DictionariesControllerTests()
        {
            _mockEmployeeTypeRepository = new Mock<IEmployeeTypeRepository>();

            _dictionariesController = new DictionariesController(_mockEmployeeTypeRepository.Object);
        }

        [Fact]
        public void GetAllEmployeeTypes_ShouldReturnEmployeeTypes_WhenEmployeeTypesExist()
        {
            // [1] Подготовка данных
            var employeeTypes = new List<EmployeeType>
            {
                new EmployeeType { Id = 1, Description = "Manager" },
                new EmployeeType { Id = 2, Description = "Engineer" }
            };
            _mockEmployeeTypeRepository.Setup(repo => repo.GetAll()).Returns(employeeTypes);

            // [2] Вызов метода
            var result = _dictionariesController.GetAllEmployeeTypes();

            // [3] Проверка вызова метода и результатов
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var returnedEmployeeTypes = okResult.Value as IList<EmployeeTypeDto>;
            Assert.NotNull(returnedEmployeeTypes);
            Assert.Equal(2, returnedEmployeeTypes.Count);
        }

        [Fact]
        public void GetAllEmployeeTypes_ShouldReturnNotFound_WhenNoEmployeeTypesExist()
        {
            // [1] Подготовка данных
            _mockEmployeeTypeRepository.Setup(repo => repo.GetAll()).Returns(new List<EmployeeType>());

            // [2] Вызов метода
            var result = _dictionariesController.GetAllEmployeeTypes();

            // [3] Проверка вызова метода и результатов
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("No employee types found.", notFoundResult.Value);
        }

        [Theory]
        [InlineData("HR")]
        [InlineData("Developer")]
        public void CreateEmployeeType_ShouldReturnCreatedId_WhenCreationIsSuccessful(string description)
        {
            // [1] Подготовка данных
            _mockEmployeeTypeRepository.Setup(repo => repo.Create(It.IsAny<EmployeeType>())).Returns(1);

            // [2] Вызов метода
            var result = _dictionariesController.CreateEmployeeType(description);

            // [3] Проверка вызова метода и результатов
            _mockEmployeeTypeRepository.Verify(repo => repo.Create(It.IsAny<EmployeeType>()), Times.Once);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public void CreateEmployeeType_ShouldReturnBadRequest_WhenCreationFails()
        {
            // [1] Подготовка данных
            _mockEmployeeTypeRepository.Setup(repo => repo.Create(It.IsAny<EmployeeType>())).Returns(0);

            // [2] Вызов метода
            var result = _dictionariesController.CreateEmployeeType("Test");

            // [3] Проверка вызова метода и результатов
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Failed to create employee type.", badRequestResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteEmployeeType_ShouldReturnOk_WhenDeletionIsSuccessful(int id)
        {
            // [1] Подготовка данных
            _mockEmployeeTypeRepository.Setup(repo => repo.Delete(id)).Returns(true);

            // [2] Вызов метода
            var result = _dictionariesController.DeleteEmloyeeType(id);

            // [3] Проверка вызова метода и результатов
            _mockEmployeeTypeRepository.Verify(repo => repo.Delete(id), Times.Once);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public void DeleteEmployeeType_ShouldReturnNotFound_WhenDeletionFails()
        {
            // [1] Подготовка данных
            _mockEmployeeTypeRepository.Setup(repo => repo.Delete(It.IsAny<int>())).Returns(false);

            // [2] Вызов метода
            var result = _dictionariesController.DeleteEmloyeeType(1);

            // [3] Проверка вызова метода и результатов
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Failed to delete EmployeeType with Id 1.", notFoundResult.Value);
        }

        [Fact]
        public void UpdateEmployeeType_ShouldReturnOk_WhenUpdateIsSuccessful()
        {
            // [1] Подготовка данных
            var employeeTypeDto = new EmployeeTypeDto { Id = 1, Description = "Updated" };
            _mockEmployeeTypeRepository.Setup(repo => repo.Update(It.IsAny<EmployeeType>())).Returns(true);

            // [2] Вызов метода
            var result = _dictionariesController.UpdateEmployeeType(employeeTypeDto);

            // [3] Проверка вызова метода и результатов
            _mockEmployeeTypeRepository.Verify(repo => repo.Update(It.IsAny<EmployeeType>()), Times.Once);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public void UpdateEmployeeType_ShouldReturnNotFound_WhenUpdateFails()
        {
            // [1] Подготовка данных
            var employeeTypeDto = new EmployeeTypeDto { Id = 1, Description = "Updated" };
            _mockEmployeeTypeRepository.Setup(repo => repo.Update(It.IsAny<EmployeeType>())).Returns(false);

            // [2] Вызов метода
            var result = _dictionariesController.UpdateEmployeeType(employeeTypeDto);

            // [3] Проверка вызова метода и результатов
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal($"Failed to update EmployeeType with Id {employeeTypeDto.Id}.", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1)]
        public void GetEmployeeTypeById_ShouldReturnEmployeeType_WhenEmployeeTypeExists(int id)
        {
            // [1] Подготовка данных
            var employeeType = new EmployeeType { Id = id, Description = "Test Type" };
            _mockEmployeeTypeRepository.Setup(repo => repo.GetById(id)).Returns(employeeType);

            // [2] Вызов метода
            var result = _dictionariesController.GetEmployeeTypeById(id);

            // [3] Проверка вызова метода и результатов
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var returnedEmployeeType = okResult.Value as EmployeeTypeDto;
            Assert.NotNull(returnedEmployeeType);
            Assert.Equal(id, returnedEmployeeType.Id);
        }

        [Fact]
        public void GetEmployeeTypeById_ShouldReturnNotFound_WhenEmployeeTypeDoesNotExist()
        {
            // [1] Подготовка данных
            _mockEmployeeTypeRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((EmployeeType)null);

            // [2] Вызов метода
            var result = _dictionariesController.GetEmployeeTypeById(1);

            // [3] Проверка вызова метода и результатов
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("EmployeeType with Id 1 not found.", notFoundResult.Value);
        }
    }
}
