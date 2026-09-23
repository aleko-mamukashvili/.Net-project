using BAL.Services;
using DAL.Repository.Person;
using DAL.UnitOfWork;
using Moq;
using Shared.Models;

namespace UnitTests
{
    public class PersonServiceTests
    {
        private readonly IPersonService _personService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public PersonServiceTests()
        {
            var test = new Mock<IPersonRepository>();
            
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Persons).Returns(test.Object);

            _personService = new PersonService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetPersonAsync_ShouldReturnPerson_WhenPersonExists()
        {
            // Arrange
            var personId = 1;
            var expectedPerson = new Person
            {
                PersonId = 1,
                Name = "John",
                Surname = "Doe",
                Gender = "ქალი",
                BirthDate = new DateTime(1990, 1, 1),
                City = new City { CityId = 1, CityName = "Test" },
                Image = "test",
                PersonalNumber = "11111111111",
                PhoneNumber = new List<PhoneNumber> { new PhoneNumber { PhoneId = 1, Number = "52312", NumberType = "სახლის" } },
                RelatedPerson = new List<RelatedPerson> { new RelatedPerson { RelatedPersonId = 1, PersonType = "სხვა" } }
            };
            _unitOfWorkMock.Setup(uow => uow.Persons.GetByIdAsync(personId)).ReturnsAsync(expectedPerson);

            // Act
            var result = await _personService.GetPersonAsync(personId);

            // Assert
            Assert.Equal(expectedPerson, result);
        }

        [Fact]
        public async Task GetAllPersons_ShouldReturnAllPersons()
        {
            var person1 = new Person
            {
                PersonId = 1,
                Name = "John",
                Surname = "Doe",
                Gender = "ქალი",
                BirthDate = new DateTime(1990, 1, 1),
                City = new City { CityId = 1, CityName = "Test" },
                Image = "test",
                PersonalNumber = "11111111111",
                PhoneNumber = new List<PhoneNumber> { new PhoneNumber { PhoneId = 1, Number = "52312", NumberType = "სახლის" } },
                RelatedPerson = new List<RelatedPerson> { new RelatedPerson { RelatedPersonId = 1, PersonType = "სხვა" },}
            };

            var person2 = new Person
            {
                PersonId = 2,
                Name = "Jane",
                Surname = "Doe",
                Gender = "ქალი",
                BirthDate = new DateTime(1990, 1, 1),
                City = new City { CityId = 1, CityName = "Test2" },
                Image = "test2",
                PersonalNumber = "11111111111",
                PhoneNumber = new List<PhoneNumber> { new PhoneNumber { PhoneId = 1, Number = "52312", NumberType = "სახლის" } },
                RelatedPerson = new List<RelatedPerson> { new RelatedPerson { RelatedPersonId = 1, PersonType = "სხვა" } }
            };





            // Arrange
            var expectedPersons = new List<Person> {person1, person2 };

            _unitOfWorkMock.Setup(uow => uow.Persons.GetAllAsync()).ReturnsAsync(expectedPersons);

            // Act
            var result = await _personService.GetAllPersons();

            // Assert
            Assert.Equal(expectedPersons, result);
        }


        [Fact]
        public async Task UpdatePersonAsync_ShouldUpdatePerson()
        {
            // Arrange
            var updatedPerson = new Person
            {
                PersonId = 1,
                Name = "John",
                Surname = "Doe",
                Gender = "ქალი",
                BirthDate = new DateTime(1990, 1, 1),
                City = new City { CityId = 1, CityName = "Test" },
                Image = "test",
                PersonalNumber = "11111111111",
                PhoneNumber = new List<PhoneNumber> { new PhoneNumber { PhoneId = 1, Number = "52312", NumberType = "სახლის" } },
                RelatedPerson = new List<RelatedPerson> { new RelatedPerson { RelatedPersonId = 1, PersonType = "სხვა" } }
            };
            // Act
            await _personService.UpdatePersonAsync(updatedPerson);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Persons.UpdateAsync(updatedPerson), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldDeletePerson()
        {
            // Arrange
            var personId = 1;

            // Act
            await _personService.DeletePersonAsync(personId);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Persons.DeleteAsync(personId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}