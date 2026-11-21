using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyWorkout.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MuscleGroup = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Equipment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Difficulty = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: true),
                    Source = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "user"),
                    HeightCm = table.Column<int>(type: "integer", nullable: true),
                    WeightKg = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Level = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Goal = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PrivacyConsentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DurationMin = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workouts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutExerciseId = table.Column<int>(type: "integer", nullable: false),
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_WorkoutExercises_WorkoutExerciseId",
                        column: x => x.WorkoutExerciseId,
                        principalTable: "WorkoutExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Description", "Difficulty", "Equipment", "MuscleGroup", "Name", "Rating", "Source" },
                values: new object[,]
                {
                    { 1, "Leh na rovné lavici, chodidla na zemi. Činku spouštěj kontrolovaně ke spodní části hrudníku a vytlač ji zpět nad ramena.", "Mírně pokročilý", "Velká činka", "Hrudník", "Bench press", null, null },
                    { 2, "Lavice skloněná asi na 30 stupňů. Jednoručky spouštěj k horní části hrudníku a tlač je nahoru nad ramena.", "Začátečník", "Jednoručky", "Hrudník", "Tlaky s jednoručkami na šikmé lavici", null, null },
                    { 3, "Dlaně o něco šíř než ramena, tělo v jedné linii od hlavy po paty. Spouštěj hrudník k zemi a vytlač se zpět.", "Začátečník", "Vlastní váha", "Hrudník", "Kliky", null, null },
                    { 4, "Opora na bradlech s propnutými pažemi. Pokrč lokty, spusť se dolů a vytlač se zpět nahoru.", "Mírně pokročilý", "Vlastní váha", "Triceps", "Kliky na bradlech", null, null },
                    { 5, "Leh na lavici, činka nad hlavou. Pokrčením loktů ji spusť k čelu a propnutím loktů vrať zpět, lokty drž na místě.", "Mírně pokročilý", "Velká činka", "Triceps", "Francouzský tlak", null, null },
                    { 6, "Činka na horní části zad, chodidla na šířku ramen. Jdi dolů do dřepu s rovnými zády a vytlač se zpět nahoru.", "Mírně pokročilý", "Velká činka", "Nohy", "Dřep", null, null },
                    { 7, "Kolena lehce pokrčená, záda rovná. Předkloň se v kyčlích a spouštěj činku podél nohou, dokud cítíš protažení zadní strany stehen.", "Mírně pokročilý", "Velká činka", "Nohy", "Rumunský mrtvý tah", null, null },
                    { 8, "Udělej dlouhý krok vpřed a pokrč obě kolena, zadní koleno míří k zemi. Odraz se zpět do stoje a vystřídej nohy.", "Začátečník", "Jednoručky", "Nohy", "Výpady", null, null },
                    { 9, "Činka na zemi nad středem chodidla. S rovnými zády ji zvedni do stoje prací nohou a kyčlí a stejnou cestou ji polož zpět.", "Pokročilý", "Velká činka", "Záda", "Mrtvý tah", null, null },
                    { 10, "Vis na hrazdě nadhmatem. Přitáhni se bradou nad hrazdu a kontrolovaně se spusť do visu.", "Mírně pokročilý", "Vlastní váha", "Záda", "Shyby", null, null },
                    { 11, "Předklon s rovnými zády, činka v natažených pažích. Přitahuj ji k břichu a lopatky stahuj k sobě.", "Mírně pokročilý", "Velká činka", "Záda", "Přítahy v předklonu", null, null },
                    { 12, "Vsedě uchop tyč kladky na šířku ramen a stahuj ji k horní části hrudníku, pak ji pomalu pusť zpět.", "Začátečník", "Stroj", "Záda", "Stahování kladky", null, null },
                    { 13, "Ve stoje drž činku u horní části hrudníku. Vytlač ji nad hlavu do propnutých paží a spusť zpět.", "Mírně pokročilý", "Velká činka", "Ramena", "Tlaky nad hlavu", null, null },
                    { 14, "Ve stoje zvedej jednoručky lehce pokrčenými pažemi do stran do výšky ramen a pomalu je spouštěj.", "Začátečník", "Jednoručky", "Ramena", "Upažování s jednoručkami", null, null },
                    { 15, "Ve stoje, lokty u těla. Ohýbej paže a zvedej jednoručky k ramenům, pak je pomalu spusť.", "Začátečník", "Jednoručky", "Biceps", "Bicepsový zdvih", null, null },
                    { 16, "Leh na zádech, kolena pokrčená. Zvedni hlavu a ramena od země stažením břicha a pomalu se vrať zpět.", "Začátečník", "Vlastní váha", "Břicho", "Zkracovačky", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name",
                table: "Exercises",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sets_WorkoutExerciseId",
                table: "Sets",
                column: "WorkoutExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
