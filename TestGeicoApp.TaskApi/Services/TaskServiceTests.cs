using GeicoApp.Data.Entities;
using GeicoApp.Models;
using GeicoApp.Services;
using GeicoApp.TaskApi.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestGeicoApp.TaskApi.Services
{
    public class TaskServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IDataService> mockDataService;

        public TaskServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockDataService = this.mockRepository.Create<IDataService>();
        }

        private TaskService CreateService(List<TaskModel> taskModels)
        {
         //   var iquerable = new IQueryable<TaskModel>();
            var iquerable = new GTask[] { new GTask { Id = 2, Description = "Description", DueDate = DateTime.UtcNow.Date, EndDate = DateTime.Now.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New } }.AsQueryable();

            this.mockDataService.Setup(x=>x.AddTask(It.IsAny<AddTaskModel>())).Returns(Task.CompletedTask);
            this.mockDataService.Setup(x => x.UpdateTask(It.IsAny<TaskModel>())).Returns(Task.CompletedTask);
            this.mockDataService.Setup(x => x.GetTasks()).Returns(Task.FromResult(taskModels));
            this.mockDataService.Setup(x => x.GetQuerableTasks()).Returns(iquerable);
            return new TaskService(
                this.mockDataService.Object);
        }

        private TaskService CreateService(List<TaskModel> taskModels, List<GTask> querableTasks)
        {
            //   var iquerable = new IQueryable<TaskModel>();
            var iquerable = querableTasks.AsQueryable();

            this.mockDataService.Setup(x => x.AddTask(It.IsAny<AddTaskModel>())).Returns(Task.CompletedTask);
            this.mockDataService.Setup(x => x.UpdateTask(It.IsAny<TaskModel>())).Returns(Task.CompletedTask);
            this.mockDataService.Setup(x => x.GetTasks()).Returns(Task.FromResult(taskModels));
            this.mockDataService.Setup(x => x.GetQuerableTasks()).Returns(iquerable);
            return new TaskService(
                this.mockDataService.Object);
        }

        [Fact]
        public async Task GetTasks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date, EndDate = DateTime.Now.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };
            var taskList = new List<TaskModel> { task };
            var service = this.CreateService(taskList);

            // Act
            var result = await service.GetTasks();

            // Assert
            this.mockDataService.Verify(x => x.GetTasks(), Times.Once);
        }

        [Fact]
        public async Task AddTask_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date.Date, EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };
            var taskList = new List<TaskModel> { task };
            var service = this.CreateService(taskList);

            AddTaskModel newTask = new() { Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(1), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.Now.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };

            // Act
            await service.AddTask(
                newTask);

            // Assert
            this.mockDataService.Verify(x => x.AddTask(newTask), Times.Once);

        }

        [Fact]
        public async Task UpdateTask_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date, EndDate = DateTime.Now.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };
            var taskList = new List<TaskModel> { task };
            var service = this.CreateService(taskList);

            TaskModel updateTask = new TaskModel { Id = 1,  Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(1), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.Now.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };

            // Act
            await service.UpdateTask(
                updateTask);

            // Assert
            this.mockDataService.Verify(x => x.UpdateTask(updateTask), Times.Once);
        }


        [Theory]
        [InlineData(10000, GTaskPriority.High, GTaskStatus.Finished)]
        [InlineData(10000, GTaskPriority.Low, GTaskStatus.Finished)]
        [InlineData(10000, GTaskPriority.Middle, GTaskStatus.Finished)]
        [InlineData(10000, GTaskPriority.Low, GTaskStatus.New)]
        [InlineData(10000, GTaskPriority.Middle, GTaskStatus.New)]
        [InlineData(10000, GTaskPriority.Low, GTaskStatus.InProcess)]
        [InlineData(10000, GTaskPriority.Middle, GTaskStatus.InProcess)]
        [InlineData(10, GTaskPriority.High, GTaskStatus.Finished)]
        [InlineData(900, GTaskPriority.Low, GTaskStatus.Finished)]
        [InlineData(10, GTaskPriority.Middle, GTaskStatus.Finished)]
        [InlineData(100, GTaskPriority.Low, GTaskStatus.New)]
        [InlineData(100, GTaskPriority.Middle, GTaskStatus.New)]
        [InlineData(100, GTaskPriority.Low, GTaskStatus.InProcess)]
        [InlineData(100, GTaskPriority.Middle, GTaskStatus.InProcess)]
        [InlineData(99, GTaskPriority.High, GTaskStatus.New)]
        [InlineData(99, GTaskPriority.High, GTaskStatus.InProcess)]
        [InlineData(7, GTaskPriority.High, GTaskStatus.New)]
        [InlineData(11, GTaskPriority.High, GTaskStatus.InProcess)]

        public async Task AddTask_Test_HighPrioritySuccess(int taskCount, GTaskPriority priority, GTaskStatus status)
        {
            // Arrange
          
            var taskList = new List<TaskModel> { };
            taskList.Add(
                   new TaskModel
                   {
                       Id = 1,
                       Description = "Description",
                       DueDate = DateTime.UtcNow.Date.Date.AddDays(2),
                       EndDate = DateTime.UtcNow.Date,
                       Name = "Name",
                       Priority = priority,
                       StartDate = DateTime.UtcNow.Date,
                       Status = status
                   });

            var IqTasks = new List<GTask> { };
            foreach (int index in Enumerable.Range(2, taskCount))
            {
               

                IqTasks.Add(
                    new GTask
                {
                    Id = index,
                    Description = "Description",
                    DueDate = DateTime.UtcNow.Date.Date.AddDays(2),
                    EndDate = DateTime.UtcNow.Date,
                    Name = "Name",
                    Priority = priority,
                    StartDate = DateTime.UtcNow.Date,
                    Status = status
                    });
            }

            var service = this.CreateService(taskList, IqTasks);

            AddTaskModel newTask = new() { Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(1), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.High, StartDate = DateTime.Now.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };

            // Act
            await service.AddTask(
                newTask);

            // Assert
            this.mockDataService.Verify(x => x.AddTask(newTask), Times.Exactly(1));

        }

        [Theory]
        [InlineData(101, GTaskPriority.High, GTaskStatus.New)]
        [InlineData(101, GTaskPriority.High, GTaskStatus.InProcess)]
        [InlineData(900, GTaskPriority.High, GTaskStatus.New)]
        [InlineData(131, GTaskPriority.High, GTaskStatus.InProcess)]
        [InlineData(122, GTaskPriority.High, GTaskStatus.New)]
        [InlineData(103, GTaskPriority.High, GTaskStatus.InProcess)]
        [InlineData(1004, GTaskPriority.High, GTaskStatus.New)]
        [InlineData(10050, GTaskPriority.High, GTaskStatus.InProcess)]
        [InlineData(10666000, GTaskPriority.High, GTaskStatus.New)]
        [InlineData(1077000, GTaskPriority.High, GTaskStatus.InProcess)]
        public async Task AddTask_Test_HighPriorityFail(int taskCount, GTaskPriority priority, GTaskStatus status)
        {
            // Arrange
            var taskList = new List<TaskModel> { };

            taskList.Add(
                  new TaskModel
                  {
                      Id = 1,
                      Description = "Description",
                      DueDate = DateTime.UtcNow.Date.Date.AddDays(2),
                      EndDate = DateTime.UtcNow.Date,
                      Name = "Name",
                      Priority = priority,
                      StartDate = DateTime.UtcNow.Date,
                      Status = status
                  });

            var IqTasks = new List<GTask> { };

            foreach (int index in Enumerable.Range(2, taskCount))
            {             
                IqTasks.Add(
                    new GTask
                    {
                        Id = index,
                        Description = "Description",
                        DueDate = DateTime.UtcNow.Date.Date.AddDays(2),
                        EndDate = DateTime.UtcNow.Date,
                        Name = "Name",
                        Priority = priority,
                        StartDate = DateTime.UtcNow.Date,
                        Status = status
                    });
            }

            var service = this.CreateService(taskList, IqTasks);

            AddTaskModel newTask = new() { Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(1), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = priority, StartDate = DateTime.Now.Date, Status = status };

            // Act
            var exception = await Assert.ThrowsAsync<InvalidTaskException>(async () => await service.AddTask(newTask));

            Assert.Equal("The system should not have more than 100 High Priority tasks which have the same due date and are not finished yet at any time.", exception.Message);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(15555)]
        [InlineData(4441)]
        [InlineData(3331)]
        [InlineData(22221)]
        [InlineData(1111)]
        [InlineData(456456)]
        [InlineData(545645)]
        [InlineData(16666)]
        [InlineData(9999)]
        public async Task AddTask_TestValidDueDate(int intDays)
        {
            // Arrange
            var task = new TaskModel { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };
            var taskList = new List<TaskModel> { task };
            var service = this.CreateService(taskList);

            AddTaskModel newTask = new() { Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(intDays), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.Now.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };

                // Act
                await service.AddTask(
                newTask);

                // Assert
                this.mockDataService.Verify(x => x.AddTask(newTask), Times.Once);

        }

        [Theory]
        [InlineData("Due date is not valid", -1)]
        [InlineData("Due date is not valid", -2)]
        [InlineData("Due date is not valid", -3)]
        [InlineData("Due date is not valid", -15555)]
        [InlineData("Due date is not valid", -4441)]
        [InlineData("Due date is not valid", -3331)]
        [InlineData("Due date is not valid", -22221)]
        [InlineData("Due date is not valid", -1111)]
        [InlineData("Due date is not valid", -456456)]
        [InlineData("Due date is not valid", -545645)]
        [InlineData("Due date is not valid", -16666)]
        [InlineData("Due date is not valid", -9999)]
        public async Task AddTask_TestInvalidDueDate(string message, int intDays)
        {
            // Arrange
            var task = new TaskModel { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };
            var taskList = new List<TaskModel> { task };
            var service = this.CreateService(taskList);

            AddTaskModel newTask = new() { Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(intDays), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.Now.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };

            // Act
            var exception = await Assert.ThrowsAsync<InvalidTaskException>(async () => await service.AddTask(newTask));

            Assert.True(exception.Message.Contains(message));

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(15555)]
        [InlineData(4441)]
        [InlineData(3331)]
        [InlineData(22221)]
        [InlineData(1111)]
        [InlineData(456456)]
        [InlineData(545645)]
        [InlineData(16666)]
        [InlineData(9999)]
        public async Task UpdateTask_TestValidDueDate(int intDays)
        {
            // Arrange
            var task = new TaskModel { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };
            var taskList = new List<TaskModel> { task };
            var service = this.CreateService(taskList);

            TaskModel updateTask = new() { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(intDays), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.Now.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };

            // Act
            await service.UpdateTask(
            updateTask);

            // Assert
            this.mockDataService.Verify(x => x.UpdateTask(updateTask), Times.Once);

        }

        [Theory]
        [InlineData("Due date is not valid", -1)]
        [InlineData("Due date is not valid", -2)]
        [InlineData("Due date is not valid", -3)]
        [InlineData("Due date is not valid", -15555)]
        [InlineData("Due date is not valid", -4441)]
        [InlineData("Due date is not valid", -3331)]
        [InlineData("Due date is not valid", -22221)]
        [InlineData("Due date is not valid", -1111)]
        [InlineData("Due date is not valid", -456456)]
        [InlineData("Due date is not valid", -545645)]
        [InlineData("Due date is not valid", -16666)]
        [InlineData("Due date is not valid", -9999)]
        public async Task UpdateTask_TestInvalidDueDate(string message, int intDays)
        {
            // Arrange
            var task = new TaskModel { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.UtcNow.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };
            var taskList = new List<TaskModel> { task };
            var service = this.CreateService(taskList);

            TaskModel updateTask = new() { Id = 1, Description = "Description", DueDate = DateTime.UtcNow.Date.AddDays(intDays), EndDate = DateTime.UtcNow.Date, Name = "Name", Priority = GeicoApp.Data.Entities.GTaskPriority.Middle, StartDate = DateTime.Now.Date, Status = GeicoApp.Data.Entities.GTaskStatus.New };

            // Act
            var exception = await Assert.ThrowsAsync<InvalidTaskException>(async () => await service.UpdateTask(updateTask));

            Assert.True(exception.Message.Contains(message));

        }
    }
}
