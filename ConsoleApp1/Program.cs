// Корабли

public class Spaceship {
  public string Name {get;}
  public Engine Engine {get;} 
  public int Hull {get; set;}
  public Deflectors Deflectors {get;}

  public  Spaceship(string name, Engine engine, int hull, Deflectors deflectors) {
    this.Name = name;
    this.Engine = engine;
    this.Hull = hull;
    this.Deflectors = deflectors; 
  }

  public void TakeDamage(int damage) {
    Hull -= damage;
    if (Hull <= 0) {
      Destroy();
    }
  }

  private void Destroy() {
    // Логика уничтожения
  }
  
  public bool IsDestroyed() {
    return Hull <= 0; 
  }
}

public class Deflectors {
  public int Class {get;} // 1, 2, 3
  public bool HasPhoton {get;}

  public Deflectors(int deflectorClass, bool hasPhotonDeflectors) {
    Class = deflectorClass;
    HasPhoton = hasPhotonDeflectors;
  }
}

public enum EngineType {
  ImpulseC, ImpulseE, JumpAlpha, JumpOmega, JumpGamma
}

public class Engine {
  public EngineType Type {get;}
  public double FuelUse {get;}

  public Engine(EngineType type, double fuelUse) {
    Type = type;
    FuelUse = fuelUse; 
  }
}


// Среды

public abstract class SpaceEnv {
  public string Name {get;}

  protected SpaceEnv(string name) {
    this.Name = name;
  }
  
  public abstract bool CanPass(Spaceship ship);
}

public class EmptySpace : SpaceEnv {

  public EmptySpace() : base("Empty space") {}
  
  public override bool CanPass(Spaceship ship) {
    return ship.Engine.Type == EngineType.ImpulseC || ship.Engine.Type == EngineType.ImpulseE;
  }

}

public class Nebula : SpaceEnv {

  public Nebula() : base("Nebula") {}

  public override bool CanPass(Spaceship ship) {
    return ship.Engine.Type == EngineType.ImpulseE; 
  }

}

public class Wormhole : SpaceEnv {

  public int Length {get;}

  public Wormhole(int length) : base("Wormhole") {
    Length = length;
  }

  public override bool CanPass(Spaceship ship) {
    return ship.Engine.Type == EngineType.JumpAlpha ||
           ship.Engine.Type == EngineType.JumpOmega || 
           ship.Engine.Type == EngineType.JumpGamma;
  }

}

public class AntimatterClouds : SpaceEnv {

  public AntimatterClouds() : base("Antimatter clouds") {}

  public override bool CanPass(Spaceship ship) {
    return ship.Deflectors.HasPhoton;
  }

}


// Препятствия

public abstract class Obstacle {

  protected Obstacle(string name) {
    this.Name = name;
  }

  public string Name {get;}

  public abstract int GetDamage(Spaceship ship);

}

public class Meteorite : Obstacle {

  private const int BASE_DAMAGE = 20;
  
  public Meteorite() : base("Meteorite") {}

  public override int GetDamage(Spaceship ship) {
    return BASE_DAMAGE * GetDamageMultiplier(ship);
  }

  private int GetDamageMultiplier(Spaceship ship) {
    if (ship.Deflectors.Class == 1) return 3;
    if (ship.Deflectors.Class == 2) return 2; 
    return 1;
  }

}

public class WormholeInstability : Obstacle {

  private const int BASE_DAMAGE = 50;

  public WormholeInstability() : base("Wormhole instability") {}

  public override int GetDamage(Spaceship ship) {
    if (ship.Deflectors.HasPhoton) {
      return 0; 
    } else {
      return BASE_DAMAGE;
    }
  }

}

// Маршруты

public class SpaceRoute {

  public List<RouteLeg> Legs {get;}

  public SpaceRoute(List<RouteLeg> legs) {
    Legs = legs; 
  }

  public Result Pass(Spaceship ship) {
    // Проходим по маршруту
    foreach (RouteLeg leg in Legs) {
      // Проверка проходимости среды
      if (!leg.Environment.CanPass(ship)) {
        return Result.Fail($"Ship {ship.Name} cannot pass {leg.Environment.Name}");
      }

      // Получение урона от препятствий
      int damage = 0;
      foreach (Obstacle obs in leg.Obstacles) {
        damage += obs.GetDamage(ship);
      }
      ship.TakeDamage(damage);

      // Проверка, не уничтожен ли корабль
      if (ship.IsDestroyed()) {
        return Result.Fail($"Ship {ship.Name} was destroyed"); 
      }
    }

    // Успех
    return Result.Success();
  }

}

public class RouteLeg {
  public double Length {get;}
  public SpaceEnv Environment {get;}
  public List<Obstacle> Obstacles {get;}

  public RouteLeg(double length, SpaceEnv env, List<Obstacle> obstacles) {
    Length = length;
    Environment = env;
    Obstacles = obstacles;
  }
}

public class Result {
  public bool IsSuccess {get;}
  public string Message {get;}

  public static Result Success() {
    return new Result(true, "");
  }

  public static Result Fail(string message) {
    return new Result(false, message);
  }

  private Result(bool isSuccess, string message) {
    IsSuccess = isSuccess;
    Message = message;
  }
}

// Пример использования

public class Program
{

  public static void Main()
  {
    int a = 4;
    var route = new SpaceRoute(
      new List<RouteLeg>
      {
        new RouteLeg(100, new EmptySpace(), new List<Obstacle>
        {
          new Meteorite(), new Meteorite()
        }),
        new RouteLeg(500, new Wormhole(300), new List<Obstacle>
        {
          new WormholeInstability(), new WormholeInstability()
        })
      }
    ); 

    var scoutShip = new Spaceship("Scout", new Engine(EngineType.ImpulseC, 10), 10, new Deflectors(1, false));
    var result = route.Pass(scoutShip);
    
    if (result.IsSuccess) {
      Console.WriteLine("Успех!"); 
    } else {
      Console.WriteLine("Неудача: " + result.Message);
    }
  }
}