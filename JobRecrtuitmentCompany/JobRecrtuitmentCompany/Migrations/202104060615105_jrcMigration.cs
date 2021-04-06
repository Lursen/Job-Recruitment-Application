namespace JobRecrtuitmentCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jrcMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        Portfolio = c.String(),
                        Company = c.String(),
                        Requisites = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Vacancies",
                c => new
                    {
                        VacancyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Salary = c.Int(nullable: false),
                        Type = c.String(),
                        Requirements = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VacancyId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VacancyEmployees",
                c => new
                    {
                        Vacancy_VacancyId = c.Int(nullable: false),
                        Employee_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_VacancyId, t.Employee_UserId })
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_VacancyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Employee_UserId, cascadeDelete: false)
                .Index(t => t.Vacancy_VacancyId)
                .Index(t => t.Employee_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacancies", "UserId", "dbo.Users");
            DropForeignKey("dbo.VacancyEmployees", "Employee_UserId", "dbo.Users");
            DropForeignKey("dbo.VacancyEmployees", "Vacancy_VacancyId", "dbo.Vacancies");
            DropIndex("dbo.VacancyEmployees", new[] { "Employee_UserId" });
            DropIndex("dbo.VacancyEmployees", new[] { "Vacancy_VacancyId" });
            DropIndex("dbo.Vacancies", new[] { "UserId" });
            DropTable("dbo.VacancyEmployees");
            DropTable("dbo.Vacancies");
            DropTable("dbo.Users");
        }
    }
}
