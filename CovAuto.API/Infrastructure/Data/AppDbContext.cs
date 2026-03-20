using CovAuto.API.Domain.Entities;
using CovAuto.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CovAuto.API.Infrastructure.Data;

/// <summary>
/// Hoofd database context. Bevat alle tabellen en seed data.
/// </summary>
public class AppDbContext : DbContext
{
    // Fixed BCrypt hash for "Demo1234!" - hardcoded to ensure migration stability
    private const string DemoPasswordHash = "$2a$11$CtVfTc19Zo6wqxhu6FaEp.XC83OAZastaj4jl.OVrpVwcYVykck7e";

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ServiceTeam> ServiceTeams => Set<ServiceTeam>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configureer relaties
        modelBuilder.Entity<User>()
            .HasOne(u => u.ServiceTeam)
            .WithMany(t => t.Members)
            .HasForeignKey(u => u.ServiceTeamId)
            .IsRequired(false);

        modelBuilder.Entity<WorkOrder>()
            .HasOne(w => w.ServiceTeam)
            .WithMany(t => t.WorkOrders)
            .HasForeignKey(w => w.ServiceTeamId);

        // Sla enums op als strings in de database (beter leesbaar)
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<WorkOrder>()
            .Property(w => w.Status)
            .HasConversion<string>();

        modelBuilder.Entity<WorkOrder>()
            .Property(w => w.Priority)
            .HasConversion<string>();

        // Voeg seed data toe
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // --- Serviceteams ---
        modelBuilder.Entity<ServiceTeam>().HasData(
            new ServiceTeam { Id = 1, Name = "Noord Service", Description = "Regio Noord-Holland", PlannerName = "Linda van Noord" },
            new ServiceTeam { Id = 2, Name = "Zuid Service", Description = "Regio Zuid-Holland", PlannerName = "Ahmed El Farsi" }
        );

        // --- Gebruikers (wachtwoord: Demo1234!) ---
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "planner.noord", PasswordHash = DemoPasswordHash, FullName = "Linda van Noord",  Role = UserRole.Planner, ServiceTeamId = null },
            new User { Id = 2, Username = "planner.zuid",  PasswordHash = DemoPasswordHash, FullName = "Ahmed El Farsi",   Role = UserRole.Planner, ServiceTeamId = null },
            new User { Id = 3, Username = "monteur.jan",   PasswordHash = DemoPasswordHash, FullName = "Jan de Vries",     Role = UserRole.Monteur, ServiceTeamId = 1 },
            new User { Id = 4, Username = "monteur.fatma", PasswordHash = DemoPasswordHash, FullName = "Fatma Yilmaz",     Role = UserRole.Monteur, ServiceTeamId = 1 },
            new User { Id = 5, Username = "monteur.sven",  PasswordHash = DemoPasswordHash, FullName = "Sven Bakker",      Role = UserRole.Monteur, ServiceTeamId = 2 },
            new User { Id = 6, Username = "monteur.ayse",  PasswordHash = DemoPasswordHash, FullName = "Ayse Demir",       Role = UserRole.Monteur, ServiceTeamId = 2 }
        );

        // --- Werkorders ---
        var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<WorkOrder>().HasData(
            // Team Noord (teamId = 1)
            new WorkOrder { Id = 1,  Title = "Storing CV-ketel repareren",            Description = "CV-ketel geeft foutcode E5.",                    EstimatedHours = 3.0, Status = WorkOrderStatus.Nieuw,        Priority = WorkOrderPriority.Hoog,    CreatedAt = baseDate.AddDays(0),  ScheduledFor = baseDate.AddDays(2),  CustomerName = "Familie Jansen",       Address = "Hoofdstraat 12, Amsterdam",    ServiceTeamId = 1 },
            new WorkOrder { Id = 2,  Title = "Vervangen groepenkast",                 Description = "Verouderde groepenkast vervangen.",              EstimatedHours = 6.0, Status = WorkOrderStatus.Gepland,      Priority = WorkOrderPriority.Normaal, CreatedAt = baseDate.AddDays(1),  ScheduledFor = baseDate.AddDays(5),  CustomerName = "Dhr. Peters",          Address = "Kalverstraat 88, Amsterdam",   ServiceTeamId = 1 },
            new WorkOrder { Id = 3,  Title = "Onderhoud ventilatiesysteem",           Description = "Jaarlijks onderhoud WTW-installatie.",           EstimatedHours = 4.5, Status = WorkOrderStatus.InUitvoering, Priority = WorkOrderPriority.Normaal, CreatedAt = baseDate.AddDays(2),  ScheduledFor = baseDate.AddDays(3),  CustomerName = "Kantoor Bakker B.V.",  Address = "Industrieweg 5, Haarlem",      ServiceTeamId = 1 },
            new WorkOrder { Id = 4,  Title = "Inspectie zonnepanelen",               Description = "Jaarlijkse inspectie en schoonmaak.",            EstimatedHours = 2.0, Status = WorkOrderStatus.Voltooid,     Priority = WorkOrderPriority.Laag,    CreatedAt = baseDate.AddDays(3),  ScheduledFor = baseDate.AddDays(4),  CustomerName = "Fam. de Groot",        Address = "Zonnebloemstraat 7, Alkmaar",  ServiceTeamId = 1 },
            new WorkOrder { Id = 5,  Title = "Lekkage waterleidingen verhelpen",     Description = "Lekkage onder de keuken.",                       EstimatedHours = 2.5, Status = WorkOrderStatus.Nieuw,        Priority = WorkOrderPriority.Hoog,    CreatedAt = baseDate.AddDays(4),  ScheduledFor = baseDate.AddDays(6),  CustomerName = "Mevr. Smit",           Address = "Rozenstraat 3, Zaandam",       ServiceTeamId = 1 },
            new WorkOrder { Id = 6,  Title = "Installatie warmtepomp",               Description = "Nieuwe warmtepomp plaatsen en aansluiten.",      EstimatedHours = 8.0, Status = WorkOrderStatus.Gepland,      Priority = WorkOrderPriority.Normaal, CreatedAt = baseDate.AddDays(5),  ScheduledFor = baseDate.AddDays(10), CustomerName = "Fam. Visser",          Address = "Merenweg 21, Purmerend",       ServiceTeamId = 1 },
            new WorkOrder { Id = 7,  Title = "Storing elektra woning",               Description = "Geen stroom op verdieping.",                     EstimatedHours = 3.5, Status = WorkOrderStatus.Nieuw,        Priority = WorkOrderPriority.Kritiek, CreatedAt = baseDate.AddDays(6),  ScheduledFor = baseDate.AddDays(7),  CustomerName = "Dhr. Nguyen",          Address = "Tulpstraat 15, Amsterdam",     ServiceTeamId = 1 },
            new WorkOrder { Id = 8,  Title = "Vervangen radiatorkranen",             Description = "Alle radiatorkranen vervangen.",                 EstimatedHours = 5.0, Status = WorkOrderStatus.Gepland,      Priority = WorkOrderPriority.Normaal, CreatedAt = baseDate.AddDays(7),  ScheduledFor = baseDate.AddDays(12), CustomerName = "VVE Rozenhof",         Address = "Rozenhof 1-20, Haarlem",       ServiceTeamId = 1 },

            // Team Zuid (teamId = 2)
            new WorkOrder { Id = 9,  Title = "Onderhoud airconditioning",            Description = "Filters reinigen en koelmiddel controleren.",    EstimatedHours = 3.0, Status = WorkOrderStatus.Gepland,      Priority = WorkOrderPriority.Normaal, CreatedAt = baseDate.AddDays(1),  ScheduledFor = baseDate.AddDays(4),  CustomerName = "Hotel Rotterdam",      Address = "Hotelplein 1, Rotterdam",      ServiceTeamId = 2 },
            new WorkOrder { Id = 10, Title = "Reparatie dakgoot",                    Description = "Dakgoot verstopt en lekkend.",                   EstimatedHours = 2.0, Status = WorkOrderStatus.Voltooid,     Priority = WorkOrderPriority.Hoog,    CreatedAt = baseDate.AddDays(2),  ScheduledFor = baseDate.AddDays(3),  CustomerName = "Fam. Hassan",          Address = "Bergweg 42, Rotterdam",        ServiceTeamId = 2 },
            new WorkOrder { Id = 11, Title = "Plaatsen laadpaal elektrisch voertuig",Description = "Laadpaal installeren in garage.",                EstimatedHours = 4.0, Status = WorkOrderStatus.Nieuw,        Priority = WorkOrderPriority.Normaal, CreatedAt = baseDate.AddDays(3),  ScheduledFor = baseDate.AddDays(8),  CustomerName = "Dhr. van Dijk",        Address = "Parkweg 9, Dordrecht",         ServiceTeamId = 2 },
            new WorkOrder { Id = 12, Title = "Storing cv-ketel flatgebouw",          Description = "Storing in centrale verwarmingsinstallatie.",    EstimatedHours = 6.0, Status = WorkOrderStatus.InUitvoering, Priority = WorkOrderPriority.Kritiek, CreatedAt = baseDate.AddDays(4),  ScheduledFor = baseDate.AddDays(5),  CustomerName = "Woningcorp De Haag",   Address = "Haagweg 100, Den Haag",        ServiceTeamId = 2 },
            new WorkOrder { Id = 13, Title = "Inspectie gasleiding",                 Description = "Jaarlijkse veiligheidscontrole.",                EstimatedHours = 1.5, Status = WorkOrderStatus.Voltooid,     Priority = WorkOrderPriority.Hoog,    CreatedAt = baseDate.AddDays(5),  ScheduledFor = baseDate.AddDays(6),  CustomerName = "Supermarkt Koops",     Address = "Winkelcentrum Zuid, Delft",    ServiceTeamId = 2 },
            new WorkOrder { Id = 14, Title = "Reparatie waterpijp",                  Description = "Gebarsten waterleiding in kelder.",              EstimatedHours = 3.5, Status = WorkOrderStatus.Gepland,      Priority = WorkOrderPriority.Hoog,    CreatedAt = baseDate.AddDays(6),  ScheduledFor = baseDate.AddDays(9),  CustomerName = "Fam. de Bruijn",       Address = "Kelderlaan 5, Zoetermeer",     ServiceTeamId = 2 },
            new WorkOrder { Id = 15, Title = "Installatie domotica systeem",         Description = "Smart home systeem installeren.",                EstimatedHours = 7.0, Status = WorkOrderStatus.Nieuw,        Priority = WorkOrderPriority.Laag,    CreatedAt = baseDate.AddDays(7),  ScheduledFor = baseDate.AddDays(14), CustomerName = "Fam. Vermeer",         Address = "Vermeerstraat 18, Leiden",     ServiceTeamId = 2 }
        );
    }
}
