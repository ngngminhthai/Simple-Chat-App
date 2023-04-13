using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Data.Migrations
{
    public partial class AddChatGr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChats_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndividualChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserOneId = table.Column<int>(type: "int", nullable: false),
                    UserTwoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualChats_Users_UserOneId",
                        column: x => x.UserOneId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IndividualChats_Users_UserTwoId",
                        column: x => x.UserTwoId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    GroupChatId = table.Column<int>(type: "int", nullable: false),
                    IndividualChatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_GroupChats_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_IndividualChats_IndividualChatId",
                        column: x => x.IndividualChatId,
                        principalTable: "IndividualChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_GroupChatId",
                table: "Chats",
                column: "GroupChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_IndividualChatId",
                table: "Chats",
                column: "IndividualChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SenderId",
                table: "Chats",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChats_CreatorId",
                table: "GroupChats",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualChats_UserOneId",
                table: "IndividualChats",
                column: "UserOneId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualChats_UserTwoId",
                table: "IndividualChats",
                column: "UserTwoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "GroupChats");

            migrationBuilder.DropTable(
                name: "IndividualChats");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
