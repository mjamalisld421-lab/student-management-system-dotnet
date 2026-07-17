using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Courses",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false).Annotation("Sqlite:Autoincrement", true),
                Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Title = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                Credits = table.Column<int>(type: "INTEGER", nullable: false),
                Department = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Courses", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false).Annotation("Sqlite:Autoincrement", true),
                FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                Phone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                Department = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                EnrollmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                Status = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Students", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Enrollments",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false).Annotation("Sqlite:Autoincrement", true),
                StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                EnrollmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                Grade = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enrollments", x => x.Id);
                table.ForeignKey("FK_Enrollments_Courses_CourseId", x => x.CourseId, "Courses", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_Enrollments_Students_StudentId", x => x.StudentId, "Students", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex("IX_Courses_Code", "Courses", "Code", unique: true);
        migrationBuilder.CreateIndex("IX_Enrollments_CourseId", "Enrollments", "CourseId");
        migrationBuilder.CreateIndex("IX_Enrollments_StudentId_CourseId", "Enrollments", new[] { "StudentId", "CourseId" }, unique: true);
        migrationBuilder.CreateIndex("IX_Students_Email", "Students", "Email", unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Enrollments");
        migrationBuilder.DropTable("Courses");
        migrationBuilder.DropTable("Students");
    }
}
