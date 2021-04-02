/*
 * С какой вероятностью должны происходить события
 * Сколько уток сбегает в день
 */

using System;
using System.Collections.Generic;

namespace PracticeDucks
{
  class Hunter
  {
    public static int TOTAL_HUNTERS = 0;

    public int id;
    public int min;
    public int max;

    public Hunter(int min, int max)
    {
      this.min = min;
      this.max = max;
      id = ++TOTAL_HUNTERS;
    }

    public int randomHuntCount()
    {
      Random random = new Random();
      return random.Next(min, max);
    }

  }

  class Duck : ICloneable
  {
    public static int TOTAL_KINDS = 0;

    public int id;
    public string kind;
    public int kindID;
    public string skill;
    public int homeID;
    public int lastFarmID = -1;
    public string[,] properties;

    public Duck(string kind, string skill, string[,] props, int homeID = 0) 
    {
      this.kind = kind;
      this.skill = skill;
      properties = props;
      kindID = ++TOTAL_KINDS;
      this.homeID = homeID;
    }

    public string getAttributeValue(string attr)
    {
      for (int i = 0; i < properties.GetLength(0); i++)
      {
        if (String.Compare(properties[i, 0], attr) == 0) return properties[i, 1];
      }
      return "";
    }

    public void addAttribute(string attr, string value)
    {
      int propertiesLength = properties.GetLength(0);
      string[,] tempArr = new string[propertiesLength + 1, 2];
      for (int i = 0; i < propertiesLength; i++)
      {
        tempArr[i, 0] = properties[i, 0];
        tempArr[i, 1] = properties[i, 1];
      }
      tempArr[propertiesLength, 0] = attr;
      tempArr[propertiesLength, 1] = value;
      properties = tempArr;
      Console.Write($"(утке {id} {value} {attr.ToLower()}) ");
    }

    public void getInfo(bool shift = false)
    {
      Console.WriteLine($"{(shift ? "\t" : "")}Утка #{id}");
      Console.WriteLine($"{(shift ? "\t\t" : "\t")}Вид: {kind} (ID: {kindID})");
      Console.WriteLine($"{(shift ? "\t\t" : "\t")}Умеет: {skill}");
      for(int i = 0; i < properties.GetLength(0); i++)
        Console.WriteLine($"{(shift ? "\t\t" : "\t")}{properties[i, 0]}: {properties[i, 1]}");
    }

    public void getShortInfo(bool shift = false)
    {
      Console.WriteLine($"{(shift ? "\t" : "")}Утка #{id} (Вид ID: {kindID})");
    }

    public object Clone()
    {
      return MemberwiseClone();
    }

  }

  class Lake
  {
    public static int TOTAL_LAKES = 0;
    public static int TOTAL_DUCKS = 0;

    public int id;
    public string title;
    public List<int> allowKinds = new List<int>();
    public List<Duck> ducks = new List<Duck>();

    public Lake(string title, int[] kindsID)
    {
      this.title = title;
      foreach (int id in kindsID)
        allowKinds.Add(id);
      id = ++TOTAL_LAKES;
    }

    public void addDuck(Duck duck)
    {
      Duck tempDuck = (Duck)duck.Clone();
      //tempDuck.id = ducks.Count + 1;
      tempDuck.id = ++TOTAL_DUCKS;
      if(tempDuck.homeID == -1) tempDuck.homeID = id;
      ducks.Add(tempDuck);
    }

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
      for(int i = 0; i < ducks.Count; i++)
      {
        if (i == id - 1)
        {
          ducks[i].getInfo();
          return;
        }
      }
      Console.WriteLine($"Утка #{id} не существует в озере #{this.id}");
    }

    public bool getDuckByID(int id)
    {
      foreach(Duck duck in ducks)
      {
        if (duck.id == id)
          return true;
      }
      return false;
    }

    public int getCountCanJailbreak()
    {
      int count = 0;
      foreach (Duck duck in ducks)
        if (isCanJailbreak(duck.id))
          count++;
      return count;
    }

    public bool isCanJailbreak(int id)
    {
      foreach (Duck duck in ducks)
      {
        if (duck.id == id)
        {
          if (String.Compare(duck.skill, "плавать") == 0 && String.Compare(duck.getAttributeValue("Груз"), "") == 0) return true;
          if (String.Compare(duck.skill, "летать") == 0 && String.Compare(duck.getAttributeValue("Крылья"), "") == 0) return true;
        }
      }
      return false;
    }

    public Duck getDuckObjectByID(int id)
    {
      foreach (Duck duck in ducks)
        if (duck.id == id)
          return duck;
      return null;
    }

    public void removeDuckByID(int id)
    {
      for(int i = 0; i < ducks.Count; i++)
      {
        if (ducks[i].id == id)
        {
          ducks.Remove(ducks[i]);
          TOTAL_DUCKS--;
          return;
        }
      }
    }

    public void getInfo()
    {
      Console.WriteLine($"Озеро {title} ({id})");
      Console.WriteLine($"\tКоличество уток: {getDucksCount()}");
    }

    public void getFullInfo()
    {
      getInfo();
      for (int i = 0; i < ducks.Count; i++)
        ducks[i].getInfo(true);
    }

    public int getDucksCount()
    {
      return ducks.Count;
    }

    /*public void getDuckCountBySkill()
    {
      Console.WriteLine($"Озеро {title} (ID: {id})");
      Dictionary<string, int> skills = new Dictionary<string, int>();
      foreach(Duck duck in ducks)
      {
        if (skills.ContainsKey(duck.skill))
        {
          skills.TryGetValue(duck.skill, out int count);
          skills.Remove(duck.skill);
          skills.Add(duck.skill, count + 1);
        }
        else skills.Add(duck.skill, 1);
      }
      foreach (var skill in skills)
        Console.WriteLine($"\tУмеет {skill.Key}: {skill.Value}");
    }*/

    public int getDuckMaxID()
    {
      int max = 0;
      foreach(Duck duck in ducks)
      {
        if (duck.id > max) max = duck.id;
      }
      return max;
    }

  }

  class Farm
  {
    public static int TOTAL_DUCKS = 0;
    public static int TOTAL_FARMS = 0;

    public int id;
    public string title;
    public Lake lake;
    public List<Hunter> hunters = new List<Hunter>();

    public Farm(string title, int[,] hunters)
    {
      this.title = title;
      id = ++TOTAL_FARMS;
      lake = new Lake($"{this.title}", new int[] { });
      for (int i = 0; i < hunters.GetLength(0); i++)
        this.hunters.Add(new Hunter(hunters[i, 0], hunters[i, 1]));
    }

    public void addDuck(Duck duck)
    {
      if (duck.lastFarmID != -1)
      {
        if(String.Compare(duck.skill, "плавать") == 0) duck.addAttribute("Груз", "установлен");
        else if(String.Compare(duck.skill, "летать") == 0) duck.addAttribute("Крылья", "подрезаны");
      }
      duck.lastFarmID = id;
      lake.ducks.Add(duck);
      TOTAL_DUCKS++;
    }

    public void getInfo()
    {
      Console.WriteLine($"Ферма {title} (ID: {id}):");
      Console.WriteLine($"Уток на ферме: {lake.ducks.Count}");
      foreach(Duck duck in lake.ducks)
        duck.getShortInfo(true);
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      const int COUNT_DAYS = 9;
      const int COUNT_DUCKS = 97;

      List<Duck> kinds = new List<Duck>
      {
        new Duck(kind: "Чирок-свистунок", skill: "плавать", homeID: -1, props: new string[,] { { "Имя", "Joan" }, { "Вес", "12" }, { "Пол", "мужской" }, { "Размер клюва", "14.1" } } ),
        new Duck(kind: "Капский чирок", skill: "бегать", props: new string[,] { { "Имя", "Pauline" }, { "Вес", "11.2" }, { "Любимое блюдо", "бутерброд" }, { "Высота", "44.2" } } ),
        new Duck(kind: "Шилохвость", skill: "бегать", props: new string[,] { { "Имя", "Charles" }, { "Вес", "5.6" }, { "Окрас крыльев", "серо-зеленый" }, { "Высота", "37.5" } } ),

        new Duck(kind: "Чирок-свистунок", skill: "летать", props: new string[,] { { "Имя", "Barbara" }, { "Вес", "5.6" }, { "Любимое блюдо", "хлопья" }, { "Цвет", "красный" } } ),
        new Duck(kind: "Гоголи", skill: "бегать", props: new string[,] { { "Имя", "William" }, { "Вес", "3.4" }, { "Любимое блюдо", "варенье" }, { "Форма крыльев", "овальная" } } ),
        new Duck(kind: "Мадагаскарская кряква", skill: "крякать", props: new string[,] { { "Имя", "Johnny" }, { "Вес", "4.7" }, { "Форма крыльев", "треугольная" }, { "Цвет глаз", "карие" } } ),
        
        new Duck(kind: "Лайсанская кряква", skill: "плавать", props: new string[,] { { "Имя", "Frederick" }, { "Вес", "5.0" }, { "Цвет глаз", "зеленый" }, { "Сила", "сильная" } } ),
        new Duck(kind: "Чирок-свистунок", skill: "рыбачить", props: new string[,] { { "Имя", "Raymond" }, { "Вес", "2.1" }, { "Цвет", "желтый" }, { "Форма хвоста", "обычная" } } ),
        new Duck(kind: "Чернети", skill: "ползать", props: new string[,] { { "Имя", "Sylvia" }, { "Вес", "4.1" }, { "Цвет", "оранжевый" }, { "Любимое блюдо", "яичница" } } ),
      };

      List<Lake> lakes = new List<Lake>
      {
        new Lake("Тоба", new int[] { 1, 2, 3 }),
        new Lake("Танганьика", new int[] { 4, 5, 6 }),
        new Lake("Пос", new int[] { 7, 8, 9 }),
      };

      List<Farm> farms = new List<Farm>
      {
        new Farm("MUBAYEZ", new int[,] { { 1, 9 }, { 3, 9 } }),
        new Farm("MALINKA", new int[,] { { 2, 4 }, { 1, 5 } }),
      };

      Random rnd = new Random();
      for (int i = 0; i < COUNT_DUCKS; i++)
      {
        bool process = true;
        while (process)
        {
          int lakeID = rnd.Next(0, lakes.Count);
          int kindID = rnd.Next(0, kinds.Count);
          for (int j = 0; j < lakes[lakeID].allowKinds.Count; j++)
          {
            if (lakes[lakeID].allowKinds[j] - 1 == kindID)
            {
              lakes[lakeID].addDuck(kinds[kindID]);
              process = false;
              break;
            }
          }
        }
      }

      for (int i = 0; i < lakes.Count; i++)
        lakes[i].getFullInfo();

      Console.WriteLine("Нажмите кнопку для перевода в симуляцию...");
      Console.ReadKey();

      int currentDay = 1;
      do
      {
        Console.Clear();
        Console.WriteLine($"Сейчас {currentDay} день охоты, осталось {COUNT_DAYS - currentDay} дней.\n");

        Console.WriteLine($"Поймано уток по всем фермам: {Farm.TOTAL_DUCKS}");

        int rndFarmID = rnd.Next(0, farms.Count);
        Farm currentFarmID = farms[rndFarmID];
        Console.WriteLine($"Сегодня охотятся охотники с фермы {farms[rndFarmID].title} (ID: {farms[rndFarmID].id})");

        int rndLakeID = rnd.Next(0, lakes.Count);
        Lake currentLakeID = lakes[rndLakeID];
        if (currentLakeID.getDucksCount() == 0) continue;
        Console.WriteLine($"Охотники пришли на озеро {currentLakeID.title} (ID: {currentLakeID.id})\n");

        //currentLakeID.getInfo(); // Озеро было

        int[] rndHuntersCount = new int[farms[rndFarmID].hunters.Count];
        for(int i = 0; i < farms[rndFarmID].hunters.Count; i++)
        {
          rndHuntersCount[i] = farms[rndFarmID].hunters[i].randomHuntCount();
          Console.Write($"Охотник #{i + 1} хочет поймать {rndHuntersCount[i]} уток.\n\tИ он ловит уток с ID: ");
          for(int j = 0; j < rndHuntersCount[i] && currentLakeID.ducks.Count > 0; j++)
          {
            int rndDuckID;
            while (true)
            {
              rndDuckID = rnd.Next(1, currentLakeID.getDuckMaxID() + 1);
              if (currentLakeID.getDuckByID(rndDuckID) || currentLakeID.ducks.Count == 0) break;
            }
            currentFarmID.addDuck(currentLakeID.getDuckObjectByID(rndDuckID));
            currentLakeID.removeDuckByID(rndDuckID);
            Console.Write($"{rndDuckID} ");
          }
          Console.WriteLine();
        }
        currentFarmID.getInfo();
        Console.WriteLine();
        
        

        for(int i = 0; i < farms.Count; i++)
        {
          Console.WriteLine($"Побеги с фермы {farms[i].title} (ID: {farms[i].id}):");
          Lake currentFarmLake = farms[i].lake;
          int countCanJailbreak = currentFarmLake.getCountCanJailbreak();
          if (countCanJailbreak == 0)
          {
            Console.WriteLine("Ни одна утка не сбежала...\n");
            continue;
          }
          int rndJailBreakDucks = rnd.Next(1, countCanJailbreak);
          for (int j = 0; j < rndJailBreakDucks; j++)
          {
            int randomDuckID;
            while (true)
            {
              randomDuckID = rnd.Next(1, currentFarmLake.getDuckMaxID() + 1);
              if ((currentFarmLake.getDuckByID(randomDuckID) && currentFarmLake.isCanJailbreak(randomDuckID)) || currentFarmLake.ducks.Count == 0) break;
            }
            Console.Write($"Сбегает утка с ID: {randomDuckID}");
            Duck duck = currentFarmLake.getDuckObjectByID(randomDuckID);
            if (duck.homeID != 0)
            {
              lakes[duck.homeID].addDuck(duck);
              Console.WriteLine($" в озеро ID: {duck.homeID}");
            }
            else
            {
              int rndJBLakeID = rnd.Next(0, lakes.Count);
              lakes[rndJBLakeID].addDuck(duck);
              Console.WriteLine($", не зная где она живет, в озеро ID: {rndJBLakeID + 1}");
            }
            currentFarmLake.removeDuckByID(randomDuckID);
            Farm.TOTAL_DUCKS--;
          }
          farms[i].getInfo();
          Console.WriteLine();

          //Console.WriteLine($"С фермы {farms[i].title} (ID: {farms[i].id}) сбегают {rndJailBreakDucks} уток.");
        }



        Console.Write("НАЖМИТЕ ЛЮБУЮ КНОПКУ ДЛЯ СЛЕДУЮЩЕГО ДЕЙСТВИЯ...");
        Console.ReadLine();
      } while ((COUNT_DAYS - currentDay++) > 0 && Farm.TOTAL_DUCKS != COUNT_DUCKS);

      Console.Clear();
      Console.WriteLine("Симуляция закончена...");
    }
  }
}