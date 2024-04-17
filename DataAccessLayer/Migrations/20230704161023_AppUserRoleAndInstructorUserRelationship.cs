using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AppUserRoleAndInstructorUserRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Instructors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Instructors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Roleid",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_ApplicationUserId1",
                table: "Instructors",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_AspNetUsers_ApplicationUserId1",
                table: "Instructors",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_AspNetUsers_ApplicationUserId1",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_ApplicationUserId1",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Roleid",
                table: "AspNetUsers");
        }
    }
}
