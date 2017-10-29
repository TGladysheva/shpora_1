﻿using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
	public class ObjectComparison
	{
		[Test]
		[Description("Проверка текущего царя")]
		[Category("ToRefactor")]
		public void CheckCurrentTsar()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

		    actualTsar.ShouldBeEquivalentTo(expectedTsar, options => options
		        .AllowingInfiniteRecursion()
		        .Excluding(f => f.SelectedMemberInfo.DeclaringType.Name==typeof(Person).Name
                && f.SelectedMemberInfo.Name=="Id"));
		}
        //При использовании подхода выше при фейле теста явно пишется, какие поля не совпали.
        //При добавлении новых полей в классе придется подправить тест только если
        //эти поля имеют особое правило сравнения или не должны сравниваться (как поле Id, например).
        //
		[Test]
		[Description("Альтернативное решение. Какие у него недостатки?")]
		public void CheckCurrentTsar_WithCustomEquality()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
			new Person("Vasili III of Russia", 28, 170, 60, null));

			// Какие недостатки у такого подхода? 
			Assert.True(AreEqual(actualTsar, expectedTsar));

		}
        //Недостатки теста выше:
        // * Функция сравнения не должна быть определена в классе тестов, т.к. если сравнение понадобится где-то еще,
        // то это будет во-первых дублирование кода, а во-вторых можно просто забыть поменять ее в тесте, если она изменится.
        // * При фейле теста видим результат, что ожидалось True, но получили False. Т.е. что конкретно не так мы не увидим
        // и придется анализировать все данные самим.

		private bool AreEqual(Person actual, Person expected)
		{
			if (actual == expected) return true;
			if (actual == null || expected == null) return false;
			return
			actual.Name == expected.Name
			&& actual.Age == expected.Age
			&& actual.Height == expected.Height
			&& actual.Weight == expected.Weight
			&& AreEqual(actual.Parent, expected.Parent);
		}
	}

	public class TsarRegistry
	{
		public static Person GetCurrentTsar()
		{
			return new Person(
				"Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));
		}
	}

    public class MyClass
    {
        public int Id;

        public MyClass(int i)
        {
            Id = i;
        }
    }
	public class Person
	{
		public static int IdCounter = 0;
		public int Age, Height, Weight;
		public string Name;
		public Person Parent;
		public int Id;

		public Person(string name, int age, int height, int weight, Person parent)
		{
			Id = IdCounter++;
			Name = name;
			Age = age;
			Height = height;
			Weight = weight;
			Parent = parent;
		}
	}
}
