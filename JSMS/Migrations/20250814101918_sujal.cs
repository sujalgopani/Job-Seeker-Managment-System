using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JSMS.Migrations
{
    /// <inheritdoc />
    public partial class sujal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpResumeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpExpriance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpEducation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmpId);
                });

            migrationBuilder.CreateTable(
                name: "Founder",
                columns: table => new
                {
                    FdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FdName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fd_Company_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fd_Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Founder", x => x.FdId);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Post_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FdId = table.Column<int>(type: "int", nullable: false),
                    Post_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Post_Count = table.Column<int>(type: "int", nullable: false),
                    Post_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FounderFdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Post_Id);
                    table.ForeignKey(
                        name: "FK_Post_Founder_FdId",
                        column: x => x.FdId,
                        principalTable: "Founder",
                        principalColumn: "FdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_Founder_FounderFdId",
                        column: x => x.FounderFdId,
                        principalTable: "Founder",
                        principalColumn: "FdId");
                });

            migrationBuilder.CreateTable(
                name: "Apply",
                columns: table => new
                {
                    ApplyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    Post_Id = table.Column<int>(type: "int", nullable: false),
                    FdId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeEmpId = table.Column<int>(type: "int", nullable: true),
                    Post_Id1 = table.Column<int>(type: "int", nullable: true),
                    FounderFdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apply", x => x.ApplyId);
                    table.ForeignKey(
                        name: "FK_Apply_Employee_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employee",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apply_Employee_EmployeeEmpId",
                        column: x => x.EmployeeEmpId,
                        principalTable: "Employee",
                        principalColumn: "EmpId");
                    table.ForeignKey(
                        name: "FK_Apply_Founder_FounderFdId",
                        column: x => x.FounderFdId,
                        principalTable: "Founder",
                        principalColumn: "FdId");
                    table.ForeignKey(
                        name: "FK_Apply_Post_Post_Id",
                        column: x => x.Post_Id,
                        principalTable: "Post",
                        principalColumn: "Post_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apply_Post_Post_Id1",
                        column: x => x.Post_Id1,
                        principalTable: "Post",
                        principalColumn: "Post_Id");
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmpId", "EmpEducation", "EmpEmail", "EmpExpriance", "EmpName", "EmpResumeUrl" },
                values: new object[] { 1, "B.Tech", "rahul@test.com", "2 Years", "Rahul", "/resumes/rahul.pdf" });

            migrationBuilder.InsertData(
                table: "Founder",
                columns: new[] { "FdId", "FdName", "Fd_Company_Name", "Fd_Email" },
                values: new object[] { 1, "Rich Danny", "IT Solution", "ITSolution2000@gmail.com" });

            migrationBuilder.InsertData(
                table: "Post",
                columns: new[] { "Post_Id", "FdId", "FounderFdId", "Post_Count", "Post_Description", "Post_Name" },
                values: new object[] { 1, 1, null, 2, "C# Developer with EF Core experience", "Software Developer" });

            migrationBuilder.InsertData(
                table: "Apply",
                columns: new[] { "ApplyId", "EmpId", "EmployeeEmpId", "FdId", "FounderFdId", "Post_Id", "Post_Id1", "Status" },
                values: new object[] { 1, 1, null, 0, null, 1, null, true });

            migrationBuilder.CreateIndex(
                name: "IX_Apply_EmpId",
                table: "Apply",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_EmployeeEmpId",
                table: "Apply",
                column: "EmployeeEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_FounderFdId",
                table: "Apply",
                column: "FounderFdId");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_Post_Id",
                table: "Apply",
                column: "Post_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_Post_Id1",
                table: "Apply",
                column: "Post_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_Post_FdId",
                table: "Post",
                column: "FdId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_FounderFdId",
                table: "Post",
                column: "FounderFdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apply");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Founder");
        }
    }
}
