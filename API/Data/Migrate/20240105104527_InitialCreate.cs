using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrate
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenerateOtp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Otp = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerateOtp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KycDoc",
                columns: table => new
                {
                    KycId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegId = table.Column<int>(type: "int", nullable: false),
                    Pan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocPathPan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocPanType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocPan = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DocPathPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocPhotoType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocPhoto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DocPathSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocSignType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocSign = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycDoc", x => x.KycId);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectedBy = table.Column<int>(type: "int", nullable: false),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenerateOtp");

            migrationBuilder.DropTable(
                name: "KycDoc");

            migrationBuilder.DropTable(
                name: "Login");
        }
    }
}
