using System;
using System.Collections.Generic;

namespace PracticeDucks
{


  class Hunter
  {
    public int min;
    public int max;

    public Hunter(int min, int max)
    {
      this.min = min;
      this.max = max;
    }

    public int randomHuntCount()
    {
      Random random = new Random();
      return random.Next(min, max);
    }
  }

  class Duck
  {
    public static int TOTAL_DUCKS = 0;

    public int id;
    public string kind;
    public string skill;
    public int homeID = -1;
    public Dictionary<string, string> properties = new Dictionary<string, string>();

    public Duck(string kind, string skill, string[,] props, int homeID = -1) 
    {
      this.kind = kind;
      this.skill = skill;
      for(int i = 0; i < props.GetLength(0); i++)
        properties.Add(props[0, 0], props[0, 1]);
      this.homeID = homeID;
      id = ++TOTAL_DUCKS;
      //Console.WriteLine($"Duck #{id} initialized...");
    }

    public void getInfo()
    {
      Console.WriteLine($"Утка #{id}");
      Console.WriteLine($"\tВид: {kind}");
      Console.WriteLine($"\tУмеет: {skill}");
      foreach(var prop in properties)
        Console.WriteLine($"\t{prop.Key}: {prop.Value}");
    }
  }

  class Lake
  {
    public static int TOTAL_LAKES = 0;

    public int id;
    public string title;
    //public List<int> allowKinds = new List<int>();
    public List<Duck> ducks = new List<Duck>();

    public Lake(string title)
    {
      this.title = title;
      id = ++TOTAL_LAKES;
    }

    public void addDuck(Duck duck) => ducks.Add(duck);

    public int getDuckCountBySkill(string skill)
    {
      int count = 0;
      foreach(Duck duck in ducks)
        if (String.Compare(duck.skill, skill) == 0)
          count++;
      return count;
    }

    public void getDuckInfoByID(int id)
    {
      foreach (var duck in ducks)
      {
        if (duck.id == id && duck.homeID == this.id)
        {
          duck.getInfo();
          return;
        }
      }
      Console.WriteLine($"Утка #{id} не существует в озере #{this.id}");
    }

    public void getInfo()
    {
      Console.WriteLine($"Озеро #{id}");
      Console.WriteLine($"\tКоличество уток: {getDucksCount()}");
    }

    public int getDucksCount()
    {
      return ducks.Count;
    }
  }

  class Farm
  {
    public int huntingDays;
    public string title;
    List<Hunter> hunters = new List<Hunter>();

    public Farm(int huntingDays, string title)
    {
      this.huntingDays = huntingDays;
      this.title = title;
    }

    public void addHunter(int min, int max)
    {
      hunters.Add(new Hunter(min, max));
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      string[,] props = {
        { "Имя", "Фигня" },
      };
      List<Duck> kinds = new List<Duck>
      {
        new Duck(kind: "Чирок-свистунок", skill: "плавать", props: props),
        new Duck(kind: "Капский чирок", skill: "бегать", props: props),
      };
      Lake lake = new Lake("Тоба");

      lake.addDuck(kinds[0]);
      lake.addDuck(kinds[1]);
      lake.getInfo();
    }
  }
}
