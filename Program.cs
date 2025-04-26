using System;
using System.Collections.Generic;
using System.Linq;

abstract class EmergencyUnit
{
    public string Name { get; set; }
    public int Speed { get; set; } 
    public int BusyRounds { get; set; } = 0;

    public EmergencyUnit(string name, int speed)
    {
        Name = name;
        Speed = speed;
    }

    public bool IsAvailable => BusyRounds == 0;

    public abstract bool CanHandle(string incidentType);
    public abstract void RespondToIncident(Incident incident);
}

class Police : EmergencyUnit
{
    public Police(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType == "Crime";
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"Police Unit '{Name}' is responding to a {incident.Type} at {incident.Location}.");
    }
}

class Firefighter : EmergencyUnit
{
    public Firefighter(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType == "Fire";
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"Firefighter Unit '{Name}' is responding to a {incident.Type} at {incident.Location}.");
    }
}

class Ambulance : EmergencyUnit
{
    public Ambulance(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType == "Medical";
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"Ambulance Unit '{Name}' is responding to a {incident.Type} at {incident.Location}.");
    }
}

class Hazmat : EmergencyUnit
{
    public Hazmat(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType == "Chemical";
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"Hazmat Unit '{Name}' is responding to a {incident.Type} at {incident.Location}.");
    }
}

class CyberUnit : EmergencyUnit
{
    public CyberUnit(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType == "Cyber";
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"Cyber Unit '{Name}' is responding to a {incident.Type} at {incident.Location}.");
    }
}

class Incident
{
    public string Type { get; set; }
    public string Location { get; set; }
    public string Difficulty { get; set; }

    public Incident(string type, string location, string difficulty)
    {
        Type = type;
        Location = location;
        Difficulty = difficulty;
    }

    public int DifficultyBonus => Difficulty switch
    {
        "Easy" => 0,
        "Medium" => 2,
        "Hard" => 5,
        _ => 0
    };
}

class Program
{
    static Random rand = new Random();

    static void Main()
    {
        List<EmergencyUnit> units = new List<EmergencyUnit>
        {
            new Police("Sami", 90),
            new Police("Eyo",85),
            new Firefighter("Biruk", 80),
            new Firefighter("Dagim", 78),
            new Ambulance("Chernet", 85),
            new Ambulance("Fatuma", 88),
            new Hazmat("Guri", 75),
            new CyberUnit("Osman", 95)
        };

        string[] incidentTypes = { "Fire", "Crime", "Medical", "Chemical", "Cyber" };
        string[] locations = { "Bole", "CMC", "Lebu", "Mexico", "Ayertena" };
        string[] difficulties = { "Easy", "Medium", "Hard" };

        int score = 0;
        int rounds = 5;

        Console.WriteLine("Advanced Emergency Response Simulation\n");

        for (int i = 1; i <= rounds; i++)
        {
            Console.WriteLine($"\n Round {i} ");

            foreach (var unit in units)
            {
                if (unit.BusyRounds > 0)
                    unit.BusyRounds--;
            }

            string type = incidentTypes[rand.Next(incidentTypes.Length)];
            string location = locations[rand.Next(locations.Length)];
            string difficulty = difficulties[rand.Next(difficulties.Length)];
            Incident incident = new Incident(type, location, difficulty);

            Console.WriteLine($"Incident: {incident.Type} at {incident.Location} (Difficulty: {incident.Difficulty})");

            var availableUnits = units
                .Where(u => u.IsAvailable && u.CanHandle(incident.Type))
                .ToList();

            if (availableUnits.Any())
            {
                var responder = availableUnits.OrderByDescending(u => u.Speed).First();

                responder.RespondToIncident(incident);
                responder.BusyRounds = 1;

                int responseTime = 100 - responder.Speed; // Lower is faster
                int timeBonus = responseTime < 10 ? 5 : responseTime < 20 ? 3 : 0;
                int roundScore = 10 + timeBonus + incident.DifficultyBonus;

                Console.WriteLine($"Response Time: {responseTime} seconds");
                Console.WriteLine($"Round Score: +{roundScore}");
                score += roundScore;
            }
            else
            {
                Console.WriteLine("No available unit can handle this incident.");
                Console.WriteLine("Round Score: -5");
                score -= 5;
            }

            Console.WriteLine($"Current Score: {score}");
        }

        Console.WriteLine("\n=== Simulation Complete ===");
        Console.WriteLine($"Final Score: {score}");
    }
}
