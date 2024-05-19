using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TaskSystem;
using TaskSystem.Interfaces;
using UnityEngine;
using UnityEngine.TestTools;

namespace TaskSystem_UnitTests
{
	[TestFixture]
	public class TaskManagerTests
	{
		#region Adding Tasks

		[Test]
		public void AddTask_AddsTaskToInternalTaskCollection()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			ITask task = Substitute.For<ITask>();

			// Act
			taskManager.AddTask(task);

			// Assert
			taskManager.Tasks.Should().Contain(task);
		}

		[Test]
		public void AddTask_DoesNotAddTask_WhenTaskIsNull()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			
			// Act
			taskManager.AddTask(null);
			
			// Assert
			taskManager.Tasks.Count.Should().Be(0);
		}

		[Test]
		public void AddTask_LogsWarning_WhenTaskIsNull()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			
			// Act
			taskManager.AddTask(null);
			
			// Assert
			LogAssert.Expect(LogType.Warning, "Unable to add task. Task is null");
		}

		[Test]
		public void AddTask_IncreasesTaskCount()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			
			// Act
			taskManager.AddTask(Substitute.For<ITask>());
			
			// Assert
			taskManager.TaskCount.Should().Be(1);
		}

		[Test]
		public void AddTask_InsertsTaskWithHigherPriority_BeforeTaskWithLowerPriority()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			ITask higherPriorityTask = Substitute.For<ITask>();
			ITask lowerPriorityTask = Substitute.For<ITask>();

			higherPriorityTask.Priority.Returns(10);
			lowerPriorityTask.Priority.Returns(1);

			// Act
			taskManager.AddTask(lowerPriorityTask);
			taskManager.AddTask(higherPriorityTask);
			
			// Assert
			taskManager.Tasks[0].Should().Be(higherPriorityTask);
			taskManager.Tasks[1].Should().Be(lowerPriorityTask);
		}

		[Test]
		public void AddTask_InsertsLowerPriorityTask_AfterHigherPriorityTasks()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			ITask higherPriorityTask = Substitute.For<ITask>();
			ITask lowerPriorityTask = Substitute.For<ITask>();

			higherPriorityTask.Priority.Returns(10);
			lowerPriorityTask.Priority.Returns(1);

			// Act
			taskManager.AddTask(higherPriorityTask);
			taskManager.AddTask(lowerPriorityTask);
			
			// Assert
			taskManager.Tasks[0].Should().Be(higherPriorityTask);
			taskManager.Tasks[1].Should().Be(lowerPriorityTask);
		}
		#endregion

		#region Removing Tasks

		[Test]
		public void RemoveTask_RemovesTaskInternally_WhenTaskExists()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			ITask task = Substitute.For<ITask>();
			taskManager.AddTask(task);
			
			// Act
			taskManager.Tasks.Should().Contain(task);
			taskManager.RemoveTask(task);
			
			// Assert
			taskManager.Tasks.Should().NotContain(task);
		}

		[Test]
		public void RemoveTask_DoesNotRemoveTask_WhenTaskDoesNotExist()
		{
			// Arrange
			TaskManager<ITask> taskManager = new();
			taskManager.AddTask(Substitute.For<ITask>());
			
			// Act
			taskManager.Tasks.Count.Should().Be(1);
			taskManager.RemoveTask(Substitute.For<ITask>());
			
			// Assert
			taskManager.Tasks.Count.Should().Be(1);
		}
		
		#endregion
	}
}