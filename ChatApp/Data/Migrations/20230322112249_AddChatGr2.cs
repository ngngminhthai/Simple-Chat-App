using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Data.Migrations
{
    public partial class AddChatGr2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_GroupChats_GroupChatId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_IndividualChats_IndividualChatId",
                table: "Chats");

            migrationBuilder.AlterColumn<int>(
                name: "IndividualChatId",
                table: "Chats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GroupChatId",
                table: "Chats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_GroupChats_GroupChatId",
                table: "Chats",
                column: "GroupChatId",
                principalTable: "GroupChats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_IndividualChats_IndividualChatId",
                table: "Chats",
                column: "IndividualChatId",
                principalTable: "IndividualChats",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_GroupChats_GroupChatId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_IndividualChats_IndividualChatId",
                table: "Chats");

            migrationBuilder.AlterColumn<int>(
                name: "IndividualChatId",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupChatId",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_GroupChats_GroupChatId",
                table: "Chats",
                column: "GroupChatId",
                principalTable: "GroupChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_IndividualChats_IndividualChatId",
                table: "Chats",
                column: "IndividualChatId",
                principalTable: "IndividualChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
