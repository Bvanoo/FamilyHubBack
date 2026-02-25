using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamHubBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjoutEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventTask_CalendarEvents_CalendarEventId",
                table: "EventTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTask",
                table: "EventTask");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "EventTask");

            migrationBuilder.RenameTable(
                name: "EventTask",
                newName: "EventTasks");

            migrationBuilder.RenameColumn(
                name: "IsDone",
                table: "EventTasks",
                newName: "IsCompleted");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "EventTasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "CalendarEventId",
                table: "EventTasks",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTask_CalendarEventId",
                table: "EventTasks",
                newName: "IX_EventTasks_EventId");

            migrationBuilder.AddColumn<int>(
                name: "EventTaskId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTasks",
                table: "EventTasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EventTaskId",
                table: "AspNetUsers",
                column: "EventTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EventTasks_EventTaskId",
                table: "AspNetUsers",
                column: "EventTaskId",
                principalTable: "EventTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTasks_CalendarEvents_EventId",
                table: "EventTasks",
                column: "EventId",
                principalTable: "CalendarEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EventTasks_EventTaskId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTasks_CalendarEvents_EventId",
                table: "EventTasks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EventTaskId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTasks",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "EventTaskId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "EventTasks",
                newName: "EventTask");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "EventTask",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "EventTask",
                newName: "IsDone");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "EventTask",
                newName: "CalendarEventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTasks_EventId",
                table: "EventTask",
                newName: "IX_EventTask_CalendarEventId");

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "EventTask",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTask",
                table: "EventTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventTask_CalendarEvents_CalendarEventId",
                table: "EventTask",
                column: "CalendarEventId",
                principalTable: "CalendarEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
