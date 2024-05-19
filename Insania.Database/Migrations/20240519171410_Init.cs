using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dir_eyes_colors",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rgb = table.Column<string>(type: "text", nullable: true, comment: "Rgb-модель цвета"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_eyes_colors", x => x.id);
                },
                comment: "Цвета глаз");

            migrationBuilder.CreateTable(
                name: "dir_hairs_colors",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rgb = table.Column<string>(type: "text", nullable: false, comment: "Rgb-модель цвета"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_hairs_colors", x => x.id);
                },
                comment: "Цвета волос");

            migrationBuilder.CreateTable(
                name: "dir_posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scope_activity = table.Column<string>(type: "text", nullable: false, comment: "Сфера деятельности"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_posts", x => x.id);
                },
                comment: "Должности");

            migrationBuilder.CreateTable(
                name: "dir_races",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_races", x => x.id);
                },
                comment: "Расы");

            migrationBuilder.CreateTable(
                name: "dir_ranks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coefficient_accrual_honor_points = table.Column<double>(type: "double precision", nullable: false, comment: "Коэффициент начисления баллов почёта"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_ranks", x => x.id);
                },
                comment: "Звания");

            migrationBuilder.CreateTable(
                name: "dir_seasons",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence_number = table.Column<int>(type: "integer", nullable: false, comment: "Порядковый номер"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_seasons", x => x.id);
                },
                comment: "Сезоны");

            migrationBuilder.CreateTable(
                name: "dir_statuses_requests_heroes_registration",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    previous_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на предыдущий статус"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_statuses_requests_heroes_registration", x => x.id);
                    table.ForeignKey(
                        name: "FK_dir_statuses_requests_heroes_registration_dir_statuses_requ~",
                        column: x => x.previous_id,
                        principalTable: "dir_statuses_requests_heroes_registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Статусы заявок на регистрацию персонажей");

            migrationBuilder.CreateTable(
                name: "dir_types_bodies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_types_bodies", x => x.id);
                },
                comment: "Типы телосложений");

            migrationBuilder.CreateTable(
                name: "dir_types_faces",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_types_faces", x => x.id);
                },
                comment: "Типы лиц");

            migrationBuilder.CreateTable(
                name: "dir_types_files",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    path = table.Column<string>(type: "text", nullable: false, comment: "Путь"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_types_files", x => x.id);
                },
                comment: "Типы файла");

            migrationBuilder.CreateTable(
                name: "dir_types_geographical_objects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_types_geographical_objects", x => x.id);
                },
                comment: "Типы географического объекта");

            migrationBuilder.CreateTable(
                name: "dir_types_organizations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_types_organizations", x => x.id);
                },
                comment: "Типы организаций");

            migrationBuilder.CreateTable(
                name: "re_fractions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    color_on_map = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    functions = table.Column<string>(type: "text", nullable: false, comment: "Функции"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_fractions", x => x.id);
                },
                comment: "Фракции");

            migrationBuilder.CreateTable(
                name: "re_regions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    number_on_map = table.Column<string>(type: "text", nullable: false, comment: "Номер на карте"),
                    color_on_map = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_regions", x => x.id);
                },
                comment: "Регионы");

            migrationBuilder.CreateTable(
                name: "re_scripts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    is_success = table.Column<bool>(type: "boolean", nullable: true, comment: "Успешность"),
                    result_execution = table.Column<string>(type: "text", nullable: true, comment: "Результат выполнения"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_scripts", x => x.id);
                },
                comment: "Скрипты");

            migrationBuilder.CreateTable(
                name: "sys_roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_roles", x => x.Id);
                },
                comment: "Роли");

            migrationBuilder.CreateTable(
                name: "sys_users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    LinkVK = table.Column<string>(type: "text", nullable: true, comment: "Ссылка в вк"),
                    LastName = table.Column<string>(type: "text", nullable: true, comment: "Фамилия"),
                    FirstName = table.Column<string>(type: "text", nullable: true, comment: "Имя"),
                    Patronymic = table.Column<string>(type: "text", nullable: true, comment: "Отчество"),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: true, comment: "Признак заблокированного пользователя"),
                    Gender = table.Column<bool>(type: "boolean", nullable: true, comment: "Пол (истина - мужской/ложь - женский)"),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата рождения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_users", x => x.Id);
                },
                comment: "Пользователи");

            migrationBuilder.CreateTable(
                name: "sys_users_roles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_users_roles", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "dir_nations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    race_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на расу"),
                    language_for_personal_names = table.Column<string>(type: "text", nullable: false, comment: "Язык для названий"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_nations", x => x.id);
                    table.ForeignKey(
                        name: "FK_dir_nations_dir_races_race_id",
                        column: x => x.race_id,
                        principalTable: "dir_races",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Нации");

            migrationBuilder.CreateTable(
                name: "dir_months",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    season_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на сезон"),
                    sequence_number = table.Column<int>(type: "integer", nullable: false, comment: "Порядковый номер"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Английское наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dir_months", x => x.id);
                    table.ForeignKey(
                        name: "FK_dir_months_dir_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "dir_seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Месяцы");

            migrationBuilder.CreateTable(
                name: "re_files",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    extention = table.Column<string>(type: "text", nullable: false, comment: "Расширение"),
                    type_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на тип"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_files_dir_types_files_type_id",
                        column: x => x.type_id,
                        principalTable: "dir_types_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Файлы");

            migrationBuilder.CreateTable(
                name: "re_geographical_objects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    color_on_map = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    type_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на тип"),
                    parent_id = table.Column<long>(type: "bigint", nullable: true, comment: "Ссылка на родителя"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_geographical_objects", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_geographical_objects_dir_types_geographical_objects_type~",
                        column: x => x.type_id,
                        principalTable: "dir_types_geographical_objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_geographical_objects_re_geographical_objects_parent_id",
                        column: x => x.parent_id,
                        principalTable: "re_geographical_objects",
                        principalColumn: "id");
                },
                comment: "Географические объекты");

            migrationBuilder.CreateTable(
                name: "re_organizations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    type_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на тип"),
                    parent_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на родителя"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_organizations", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_organizations_dir_types_organizations_type_id",
                        column: x => x.type_id,
                        principalTable: "dir_types_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_organizations_re_organizations_parent_id",
                        column: x => x.parent_id,
                        principalTable: "re_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Организации");

            migrationBuilder.CreateTable(
                name: "re_players",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на пользователя"),
                    loyalty_points = table.Column<int>(type: "integer", nullable: false, comment: "Баллы верности"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_players", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_players_sys_users_user_id",
                        column: x => x.user_id,
                        principalTable: "sys_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Игроки");

            migrationBuilder.CreateTable(
                name: "re_countries",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    number_on_map = table.Column<string>(type: "text", nullable: false, comment: "Номер на карте"),
                    color_on_map = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    language_for_personal_names = table.Column<string>(type: "text", nullable: false, comment: "Язык для названий"),
                    organization_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на организацию"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_countries", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_countries_re_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "re_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Страны");

            migrationBuilder.CreateTable(
                name: "re_ownerships",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    number_on_map = table.Column<string>(type: "text", nullable: false, comment: "Номер на карте"),
                    color_on_map = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    organization_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на организацию"),
                    parent_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на родителя"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_ownerships", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_ownerships_re_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "re_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_ownerships_re_ownerships_parent_id",
                        column: x => x.parent_id,
                        principalTable: "re_ownerships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Владения");

            migrationBuilder.CreateTable(
                name: "re_heroes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на игрока"),
                    personal_name = table.Column<string>(type: "text", nullable: false, comment: "Личное имя"),
                    prefix_name = table.Column<string>(type: "text", nullable: true, comment: "Префикс имени"),
                    family_name = table.Column<string>(type: "text", nullable: true, comment: "Имя семьи"),
                    birth_day = table.Column<int>(type: "integer", nullable: false, comment: "День рождения"),
                    birth_month_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на месяц рождения"),
                    birth_cycle = table.Column<int>(type: "integer", nullable: false, comment: "Цикл рождения"),
                    nation_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на нацию"),
                    Gender = table.Column<bool>(type: "boolean", nullable: false, comment: "Пол (истина - мужской/ложь - женский)"),
                    height = table.Column<int>(type: "integer", nullable: false, comment: "Рост"),
                    weight = table.Column<int>(type: "integer", nullable: false, comment: "Вес"),
                    hair_color_id = table.Column<long>(type: "bigint", nullable: true, comment: "Ссылка на цвет волос"),
                    HairsColorId = table.Column<long>(type: "bigint", nullable: true),
                    eye_color_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на цвет глаз"),
                    type_body_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на тип телосложения"),
                    type_face_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на тип лица"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак активности"),
                    is_current = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак текущего"),
                    freezing_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Заморозка да"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_heroes", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_heroes_dir_eyes_colors_eye_color_id",
                        column: x => x.eye_color_id,
                        principalTable: "dir_eyes_colors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_heroes_dir_hairs_colors_HairsColorId",
                        column: x => x.HairsColorId,
                        principalTable: "dir_hairs_colors",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_re_heroes_dir_months_birth_month_id",
                        column: x => x.birth_month_id,
                        principalTable: "dir_months",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_heroes_dir_nations_nation_id",
                        column: x => x.nation_id,
                        principalTable: "dir_nations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_heroes_dir_types_bodies_type_body_id",
                        column: x => x.type_body_id,
                        principalTable: "dir_types_bodies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_heroes_dir_types_faces_type_face_id",
                        column: x => x.type_face_id,
                        principalTable: "dir_types_faces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_heroes_re_players_player_id",
                        column: x => x.player_id,
                        principalTable: "re_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Персонажи");

            migrationBuilder.CreateTable(
                name: "re_chapters",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    country_id = table.Column<long>(type: "bigint", nullable: true, comment: "Ссылка на страну"),
                    is_paramount = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак верховности"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_chapters", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_chapters_re_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "re_countries",
                        principalColumn: "id");
                },
                comment: "Капитулы");

            migrationBuilder.CreateTable(
                name: "re_areas",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    number_on_map = table.Column<string>(type: "text", nullable: false, comment: "Номер на карте"),
                    color_on_map = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    pixel_size = table.Column<int>(type: "integer", nullable: false, comment: "Размер в пикселях"),
                    country_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на страну"),
                    region_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на регион"),
                    geographical_object_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на географический объект"),
                    fraction_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на фракцию"),
                    ownership_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на владение"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_areas", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_areas_re_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "re_countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_areas_re_fractions_fraction_id",
                        column: x => x.fraction_id,
                        principalTable: "re_fractions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_areas_re_geographical_objects_geographical_object_id",
                        column: x => x.geographical_object_id,
                        principalTable: "re_geographical_objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_areas_re_ownerships_ownership_id",
                        column: x => x.ownership_id,
                        principalTable: "re_ownerships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_areas_re_regions_region_id",
                        column: x => x.region_id,
                        principalTable: "re_regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Области");

            migrationBuilder.CreateTable(
                name: "re_biographies_heroes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hero_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на персонажа"),
                    day_begin = table.Column<int>(type: "integer", nullable: false, comment: "День начала"),
                    month_begin_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на месяц начала"),
                    cycle_begin = table.Column<int>(type: "integer", nullable: false, comment: "Цикл начала"),
                    day_end = table.Column<int>(type: "integer", nullable: true, comment: "День окончания"),
                    month_end_id = table.Column<long>(type: "bigint", nullable: true, comment: "Ссылка на месяц окончания"),
                    cycle_end = table.Column<int>(type: "integer", nullable: true, comment: "Цикл окончания"),
                    text = table.Column<string>(type: "text", nullable: false, comment: "Текст"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_biographies_heroes", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_biographies_heroes_dir_months_month_begin_id",
                        column: x => x.month_begin_id,
                        principalTable: "dir_months",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_biographies_heroes_dir_months_month_end_id",
                        column: x => x.month_end_id,
                        principalTable: "dir_months",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_re_biographies_heroes_re_heroes_hero_id",
                        column: x => x.hero_id,
                        principalTable: "re_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Биографии персонажей");

            migrationBuilder.CreateTable(
                name: "un_files_heroes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hero_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на персонажа"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    file_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на файл"),
                    sequence_number = table.Column<int>(type: "integer", nullable: true, comment: "Порядковый номер")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_un_files_heroes", x => x.id);
                    table.ForeignKey(
                        name: "FK_un_files_heroes_re_files_file_id",
                        column: x => x.file_id,
                        principalTable: "re_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_un_files_heroes_re_heroes_hero_id",
                        column: x => x.hero_id,
                        principalTable: "re_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Файлы персонажей");

            migrationBuilder.CreateTable(
                name: "re_administrators",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на пользователя"),
                    post_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на должность"),
                    rank_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на звание"),
                    chapter_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на капитул"),
                    honor_points = table.Column<int>(type: "integer", nullable: false, comment: "Баллы почёта"),
                    mentor_id = table.Column<long>(type: "bigint", nullable: true, comment: "Ссылка на наставника"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_administrators", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_administrators_dir_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "dir_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_administrators_dir_ranks_rank_id",
                        column: x => x.rank_id,
                        principalTable: "dir_ranks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_administrators_re_administrators_mentor_id",
                        column: x => x.mentor_id,
                        principalTable: "re_administrators",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_re_administrators_re_chapters_chapter_id",
                        column: x => x.chapter_id,
                        principalTable: "re_chapters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_administrators_sys_users_user_id",
                        column: x => x.user_id,
                        principalTable: "sys_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Администраторы");

            migrationBuilder.CreateTable(
                name: "re_requests_heroes_registration",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hero_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на персонажа"),
                    status_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на статус"),
                    administrator_id = table.Column<long>(type: "bigint", nullable: true, comment: "Ссылка на ответственного администратора"),
                    personal_name_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по личному имени"),
                    comment_on_personal_name = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к личному имени"),
                    family_name_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по имени семьи"),
                    comment_on_family_name = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к имени семьи"),
                    race_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по расе"),
                    comment_on_race = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к расе"),
                    nation_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по нации"),
                    comment_on_nation = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к нации"),
                    birth_date_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по дате рождения"),
                    comment_on_birth_date = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к дате рождения"),
                    location_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по местоположению"),
                    comment_on_location = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к местоположению"),
                    height_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по росту"),
                    comment_on_height = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к росту"),
                    weight_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по весу"),
                    comment_on_weight = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к весу"),
                    type_body_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по типу телосложения"),
                    comment_on_type_body = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к типу телосложения"),
                    type_face_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по типу лица"),
                    comment_on_type_face = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к типу лица"),
                    hair_color_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по цвету волос"),
                    comment_on_hair_color = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к цвету волос"),
                    eye_color_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по цвету глаз"),
                    comment_on_eye_color = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к цвету глаз"),
                    image_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по изображению"),
                    comment_on_image = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к изображению"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_requests_heroes_registration", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_requests_heroes_registration_dir_statuses_requests_heroe~",
                        column: x => x.status_id,
                        principalTable: "dir_statuses_requests_heroes_registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_requests_heroes_registration_re_administrators_administr~",
                        column: x => x.administrator_id,
                        principalTable: "re_administrators",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_re_requests_heroes_registration_re_heroes_hero_id",
                        column: x => x.hero_id,
                        principalTable: "re_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Заявки на регистрацию персонажей");

            migrationBuilder.CreateTable(
                name: "re_biographies_requests_heroes_registration",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на заявку"),
                    biography_id = table.Column<long>(type: "bigint", nullable: false, comment: "Ссылка на персонажа"),
                    date_begin_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по дате начала"),
                    comment_on_date_begin = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к дате начала"),
                    date_end_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по дате окончания"),
                    comment_on_date_end = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к дате окончания"),
                    text_decision = table.Column<bool>(type: "boolean", nullable: true, comment: "Решение по тексту"),
                    comment_on_text = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к тексту"),
                    date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    user_create = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, создавший"),
                    date_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    user_update = table.Column<string>(type: "text", nullable: false, comment: "Пользователь, обновивший"),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_re_biographies_requests_heroes_registration", x => x.id);
                    table.ForeignKey(
                        name: "FK_re_biographies_requests_heroes_registration_re_biographies_~",
                        column: x => x.biography_id,
                        principalTable: "re_biographies_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_re_biographies_requests_heroes_registration_re_requests_her~",
                        column: x => x.request_id,
                        principalTable: "re_requests_heroes_registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Биографии заявок на регистрацию персонажей");

            migrationBuilder.CreateIndex(
                name: "IX_dir_months_season_id",
                table: "dir_months",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_dir_nations_race_id",
                table: "dir_nations",
                column: "race_id");

            migrationBuilder.CreateIndex(
                name: "IX_dir_statuses_requests_heroes_registration_previous_id",
                table: "dir_statuses_requests_heroes_registration",
                column: "previous_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_administrators_chapter_id",
                table: "re_administrators",
                column: "chapter_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_administrators_mentor_id",
                table: "re_administrators",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_administrators_post_id",
                table: "re_administrators",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_administrators_rank_id",
                table: "re_administrators",
                column: "rank_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_administrators_user_id",
                table: "re_administrators",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_areas_country_id",
                table: "re_areas",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_areas_fraction_id",
                table: "re_areas",
                column: "fraction_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_areas_geographical_object_id",
                table: "re_areas",
                column: "geographical_object_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_areas_ownership_id",
                table: "re_areas",
                column: "ownership_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_areas_region_id",
                table: "re_areas",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_biographies_heroes_hero_id",
                table: "re_biographies_heroes",
                column: "hero_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_biographies_heroes_month_begin_id",
                table: "re_biographies_heroes",
                column: "month_begin_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_biographies_heroes_month_end_id",
                table: "re_biographies_heroes",
                column: "month_end_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_biographies_requests_heroes_registration_biography_id",
                table: "re_biographies_requests_heroes_registration",
                column: "biography_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_biographies_requests_heroes_registration_request_id",
                table: "re_biographies_requests_heroes_registration",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_chapters_country_id",
                table: "re_chapters",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_countries_organization_id",
                table: "re_countries",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_files_type_id",
                table: "re_files",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_geographical_objects_parent_id",
                table: "re_geographical_objects",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_geographical_objects_type_id",
                table: "re_geographical_objects",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_heroes_birth_month_id",
                table: "re_heroes",
                column: "birth_month_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_heroes_eye_color_id",
                table: "re_heroes",
                column: "eye_color_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_heroes_HairsColorId",
                table: "re_heroes",
                column: "HairsColorId");

            migrationBuilder.CreateIndex(
                name: "IX_re_heroes_nation_id",
                table: "re_heroes",
                column: "nation_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_heroes_player_id",
                table: "re_heroes",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_heroes_type_body_id",
                table: "re_heroes",
                column: "type_body_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_heroes_type_face_id",
                table: "re_heroes",
                column: "type_face_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_organizations_parent_id",
                table: "re_organizations",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_organizations_type_id",
                table: "re_organizations",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_ownerships_organization_id",
                table: "re_ownerships",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_ownerships_parent_id",
                table: "re_ownerships",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_players_user_id",
                table: "re_players",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_re_requests_heroes_registration_administrator_id",
                table: "re_requests_heroes_registration",
                column: "administrator_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_requests_heroes_registration_hero_id",
                table: "re_requests_heroes_registration",
                column: "hero_id");

            migrationBuilder.CreateIndex(
                name: "IX_re_requests_heroes_registration_status_id",
                table: "re_requests_heroes_registration",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_un_files_heroes_file_id",
                table: "un_files_heroes",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "IX_un_files_heroes_hero_id",
                table: "un_files_heroes",
                column: "hero_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "re_areas");

            migrationBuilder.DropTable(
                name: "re_biographies_requests_heroes_registration");

            migrationBuilder.DropTable(
                name: "re_scripts");

            migrationBuilder.DropTable(
                name: "sys_roles");

            migrationBuilder.DropTable(
                name: "sys_users_roles");

            migrationBuilder.DropTable(
                name: "un_files_heroes");

            migrationBuilder.DropTable(
                name: "re_fractions");

            migrationBuilder.DropTable(
                name: "re_geographical_objects");

            migrationBuilder.DropTable(
                name: "re_ownerships");

            migrationBuilder.DropTable(
                name: "re_regions");

            migrationBuilder.DropTable(
                name: "re_biographies_heroes");

            migrationBuilder.DropTable(
                name: "re_requests_heroes_registration");

            migrationBuilder.DropTable(
                name: "re_files");

            migrationBuilder.DropTable(
                name: "dir_types_geographical_objects");

            migrationBuilder.DropTable(
                name: "dir_statuses_requests_heroes_registration");

            migrationBuilder.DropTable(
                name: "re_administrators");

            migrationBuilder.DropTable(
                name: "re_heroes");

            migrationBuilder.DropTable(
                name: "dir_types_files");

            migrationBuilder.DropTable(
                name: "dir_posts");

            migrationBuilder.DropTable(
                name: "dir_ranks");

            migrationBuilder.DropTable(
                name: "re_chapters");

            migrationBuilder.DropTable(
                name: "dir_eyes_colors");

            migrationBuilder.DropTable(
                name: "dir_hairs_colors");

            migrationBuilder.DropTable(
                name: "dir_months");

            migrationBuilder.DropTable(
                name: "dir_nations");

            migrationBuilder.DropTable(
                name: "dir_types_bodies");

            migrationBuilder.DropTable(
                name: "dir_types_faces");

            migrationBuilder.DropTable(
                name: "re_players");

            migrationBuilder.DropTable(
                name: "re_countries");

            migrationBuilder.DropTable(
                name: "dir_seasons");

            migrationBuilder.DropTable(
                name: "dir_races");

            migrationBuilder.DropTable(
                name: "sys_users");

            migrationBuilder.DropTable(
                name: "re_organizations");

            migrationBuilder.DropTable(
                name: "dir_types_organizations");
        }
    }
}
